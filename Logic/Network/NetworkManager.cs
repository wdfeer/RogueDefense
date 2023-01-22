using Godot;
using System;

public class NetworkManager : Node
{
    public static NetMode mode = NetMode.Singleplayer;
    public static NetworkManager instance;

    public override void _Ready()
    {
        instance = this;
        GetTree().Connect("network_peer_connected", this, "PeerConnected");
    }
    private NetworkedMultiplayerENet net;
    public void Initialize()
    {
        switch (mode)
        {
            case NetMode.Server:
                net = new NetworkedMultiplayerENet();
                net.CreateServer(7777, 7);
                GetTree().NetworkPeer = net;
                break;
            case NetMode.Client:
                net = new NetworkedMultiplayerENet();
                net.CreateClient("127.0.0.1", 7777);
                GetTree().NetworkPeer = net;
                break;
            default:
                break;
        }
    }
    public void PeerConnected(int id)
    {
        GD.Print($"Connection received, id: {id}");
    }

    public override void _Process(float delta)
    {

    }


}
public enum NetMode
{
    Singleplayer,
    Server,
    Client
}