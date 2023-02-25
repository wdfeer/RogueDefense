using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using static Client;

public class Server : Node
{
    public static Server instance = new Server();
    public static WebSocketServer server;
    public const int PORT = 7777;
    public void Start()
    {
        server = new WebSocketServer();
        server.Connect("client_connected", this, "Connected");
        server.Connect("client_disconnected", this, "Disconnected");
        server.Connect("client_close_request", this, "CloseRequest");
        server.Connect("data_received", this, "OnData");

        var err = server.Listen(PORT);
        GD.Print($"Server is listening on port {PORT}");
        if (err != Error.Ok || !server.IsListening())
        {
            GD.PrintErr($"Unable to start server ({err})");
            SetProcess(false);
        }
    }
    public Dictionary<int, (string name, int ability)> users = new Dictionary<int, (string name, int ability)>();
    public void SendPacket(int id, byte[] data) => server.GetPeer(id).PutPacket(data);
    public void Broadcast(byte[] data, int ignore = -1) => users.ToList().ForEach(x =>
    {
        if (x.Key != ignore)
            SendPacket(x.Key, data);
    });
    public void Broadcast(string data, int ignore = -1) => Broadcast(data.ToUTF8(), ignore);
    public void Connected(int id, string protocol)
    {
        string data = $"{(char)MessageType.FetchLobby}{id.ToString()}";
        if (users.Count > 0)
            data += " " + String.Join(" ", users.Select(x => $"{x.Key.ToString()};{x.Value.name};{x.Value.ability}"));
        SendPacket(id, data.ToUTF8());
        users.Add(id, ("", -1));
        GD.Print($"Client {id} connected with protocol: {protocol}");
    }
    public void Disconnected(int id, bool wasCleanClose = false)
    {
        users.Remove(id);
        GD.Print($"Client {id} disconnected, clean: {wasCleanClose}");

        SendMessage(MessageType.Unregister, new string[] { id.ToString() });
    }
    public void CloseRequest(int id, int code, string reason)
    {
        users.Remove(id);
        GD.Print($"Client {id} disconnecting with code {code}, reason: {reason}");
    }
    public void OnData(int id)
    {
        byte[] data = server.GetPeer(id).GetPacket();
        GD.Print($"Server: got packet from {id}: {data.GetStringFromUTF8()} ... broadcasting");
        Broadcast(data, id);

        string str = data.GetStringFromUTF8();
        string[] args = str.Substring(1).Split(" ");
        PostBroadcastMessage(id, (MessageType)str[0], args);
    }
    public void PostBroadcastMessage(int from, MessageType type, string[] args)
    {
        switch (type)
        {
            case MessageType.Register:
                users[from] = (args[1], args[2].ToInt());
                GD.Print($"Registered user {args[1]} with ability {args[2]} as {from}");
                return;
            case MessageType.SetAbility:
                users[from] = (users[from].name, args[1].ToInt());
                GD.Print($"Set ability {users[from].ability} for {users[from].name}");
                return;
            default:
                return;
        }
    }
    public void SendMessage(MessageType type, string[] args = null)
    {
        string msg = $"{(char)type}";
        if (args != null)
            msg += String.Join(" ", args);
        Broadcast(msg);
    }


    public void Poll() // important to always keep polling
    {
        server.Poll();
    }
}