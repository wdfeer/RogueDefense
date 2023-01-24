using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

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
    public List<int> ids = new List<int>();
    public void SendPacket(int id, byte[] data) => server.GetPeer(id).PutPacket(data);
    public void Broadcast(byte[] data, int ignore = -1) => ids.ForEach(x =>
    {
        if (x != ignore)
            SendPacket(x, data);
    });
    public void Connected(int id, string protocol)
    {
        ids.Add(id);
        GD.Print($"Client {id} connected with protocol: {protocol}");
        string data = $"{Client.MessageType.FetchLobby}{id.ToString()}";
        if (ids.Any())
            data += " " + String.Join(" ", ids.Select(x => $"{x.ToString()};{Client.instance.users.Find(y => y.id == x)}"));
        SendPacket(id, data.ToUTF8());
    }
    public void Disconnected(int id, bool wasCleanClose = false)
    {
        ids.Remove(id);
        GD.Print($"Client {id} disconnected, clean: {wasCleanClose}");
    }
    public void CloseRequest(int id, int code, string reason)
    {
        ids.Remove(id);
        GD.Print($"Client {id} disconnecting with code {code}, reason: {reason}");
    }
    public void OnData(int id)
    {
        var data = server.GetPeer(id).GetPacket();
        GD.Print($"Got packet from {id}: {data.GetStringFromUTF8()} ... broadcasting");
        Broadcast(data, id);
    }

    public void Poll()
    {
        server.Poll();
    }
}