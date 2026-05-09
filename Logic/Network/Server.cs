using System;
using System.Collections.Generic;
using System.Linq;
using RogueDefense.Logic.Network.Messages;

namespace RogueDefense.Logic.Network;

public partial class Server : Node
{
    public static Server instance = new();
    private static TcpServer server;
    private static readonly StreamPeerTcp[] peers = new StreamPeerTcp[16];
    public const ushort PORT = 7777;

    public void Start()
    {
        server = new TcpServer();

        var err = server.Listen(PORT);
        if (err == Error.Ok)
        {
            GD.Print($"Server is listening on port {PORT}");
        }
        else
        {
            GD.PrintErr($"Unable to start server ({err})");
            SetProcess(false);
        }
    }

    public void Stop()
    {
        server?.Stop();
        server = null;
    }

    private readonly Dictionary<int, UserData> users = new();

    private void SendPacket(int id, MessageType type, Resource data)
    {
        peers[id].Put8((sbyte)type);
        peers[id].PutVar(data);
    }

    public void Broadcast(MessageType type, Resource message, int ignore = -1) => users.ToList().ForEach(x =>
    {
        if (x.Key != ignore)
            SendPacket(x.Key, type, message);
    });

    private void OnConnect(int id)
    {
        FetchLobbyMessage msg = new FetchLobbyMessage()
        {
            id = id
        };
        msg.others = users.Select(it => new RegisterMessage()
        {
            id = it.Key,
            name = it.Value.name,
            ability = it.Value.ability,
            augmentPoints = it.Value.augmentPoints
        }).ToArray();

        GD.Print($"Sending FetchLobby message: {msg}");
        SendPacket(id, MessageType.FetchLobby, msg);
        users.Add(id, new UserData(id, "", -1, null));
        GD.Print($"Client {id} connected");
    }

    private void OnDisconnect(int id)
    {
        users.Remove(id);
        peers[id] = null;
        GD.Print($"Client {id} disconnected");

        Broadcast(MessageType.Unregister, new UnregisterMessage() { id = id });
    }

    private void ReceiveData(int id, MessageType type, Resource message)
    {
        GD.Print($"Server is broadcasting packet with from {id} of type {type}.");

        Broadcast(type, message, id);

        AfterBroadcastMessage(id, type, message);
    }

    private void AfterBroadcastMessage(int from, MessageType type, Resource message)
    {
        switch (type)
        {
            case MessageType.Register:
                RegisterMessage msg = (RegisterMessage)message;
                users[from] = new UserData(msg.id, msg.name, msg.ability, msg.augmentPoints);
                GD.Print($"Registered user \"{msg.name}\" {{{msg.id}}}.");
                return;
            default:
                return;
        }
    }

    public void Poll() // important to always keep polling
    {
        if (server.IsConnectionAvailable())
        {
            var connection = server.TakeConnection();
            int id = -1;
            for (int i = 0; i < peers.Length; i++)
            {
                if (peers[i] == null)
                {
                    peers[i] = connection;
                    id = i;
                    break;
                }
            }

            OnConnect(id);
        }

        for (int i = 0; i < peers.Length; i++)
        {
            StreamPeerTcp client = peers[i];
            if (client == null)
                continue;

            StreamPeerSocket.Status status = client.GetStatus();
            switch (status)
            {
                case StreamPeerSocket.Status.None or StreamPeerSocket.Status.Error:
                    OnDisconnect(i);
                    continue;
                case StreamPeerSocket.Status.Connecting:
                    GD.Print($"Client {i} is still connecting, skipping data check");
                    continue;
            }

            int byteCount = client.GetAvailableBytes();
            if (byteCount > 0)
            {
                GD.Print($"Server: {byteCount} bytes are available from {i}");
                MessageType type = (MessageType)client.Get8();
                Resource message = (Resource)client.GetVar();
                ReceiveData(i, type, message);
            }
        }
    }
}