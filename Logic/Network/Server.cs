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
            GD.PrintErr("Unable to start server");
            SetProcess(false);
        }
    }
    public Dictionary<int, string> names = new Dictionary<int, string>();
    public void SendPacket(int id, byte[] data) => server.GetPeer(id).PutPacket(data);
    public void Broadcast(byte[] data, int ignore = -1) => names.ToList().ForEach(x =>
    {
        if (x.Key != ignore)
            SendPacket(x.Key, data);
    });
    public void Connected(int id, string protocol)
    {
        string data = $"{(char)Client.MessageType.FetchLobby}{id.ToString()}";
        if (names.Count > 0)
            data += " " + String.Join(" ", names.Select(x => $"{x.Key.ToString()};{x.Value}"));
        SendPacket(id, data.ToUTF8());
        names.Add(id, "");
        GD.Print($"Client {id} connected with protocol: {protocol}");
    }
    public void Disconnected(int id, bool wasCleanClose = false)
    {
        names.Remove(id);
        GD.Print($"Client {id} disconnected, clean: {wasCleanClose}");
    }
    public void CloseRequest(int id, int code, string reason)
    {
        names.Remove(id);
        GD.Print($"Client {id} disconnecting with code {code}, reason: {reason}");
    }
    public void OnData(int id)
    {
        byte[] data = server.GetPeer(id).GetPacket();
        GD.Print($"Server: got packet from {id}: {data.GetStringFromUTF8()} ... broadcasting");
        Broadcast(data, id);

        string str = data.GetStringFromUTF8();
        if (str[0] == (char)MessageType.Register)
        {
            string name = str.Substring(1).Split(" ")[1];
            names[id] = name;
            GD.Print($"Registered name {name} for {id}");
        }
    }

    public void Poll()
    {
        server.Poll();
    }
}