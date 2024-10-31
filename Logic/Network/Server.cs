using System.Collections.Generic;
using System.Linq;

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
	private void SendPacket(int id, string data)
	{
		peers[id].PutUtf8String(data);
	}

	private void Broadcast(string data, int ignore = -1) => users.ToList().ForEach(x =>
	{
		if (x.Key != ignore)
			SendPacket(x.Key, data);
	});
	private void OnConnect(int id)
	{
		string msg = $"{(char)MessageType.FetchLobby}{id}";
		if (users.Count > 0)
		{
			msg += " " + string.Join(" ", users.Select(x => $"{x.Key};{x.Value.name};{x.Value.ability};{UserData.AugmentPointsAsString(x.Value.augmentPoints)}"));
		}
		GD.Print($"Sending FetchLobby message: {msg}");
		SendPacket(id, msg);
		users.Add(id, new UserData(id, "", -1, null));
		GD.Print($"Client {id} connected");
	}
	private void OnDisconnect(int id)
	{
		users.Remove(id);
		peers[id] = null;
		GD.Print($"Client {id} disconnected");

		SendMessage(MessageType.Unregister, new string[] { id.ToString() });
	}
	private void ReceiveData(int id, string data)
	{
		GD.Print($"Server is broadcasting packet with from {id}: {data}");

		Broadcast(data, id);

		string[] args = data[1..].Split(" ");
		AfterBroadcastMessage(id, (MessageType)data[0], args);
	}
	private void AfterBroadcastMessage(int from, MessageType type, string[] args)
	{
		switch (type)
		{
			case MessageType.Register:
				users[from] = new UserData(from, args[1], args[2].ToInt(), UserData.AugmentPointsFromString(args[3]));
				GD.Print($"Registered user {args[1]} with ability {args[2]} as {from}");
				return;
			case MessageType.SetAbility:
				GD.Print($"Got a SetAbility message from {from}");
				users[from] = new UserData(from, users[from].name, args[1].ToInt(), users[from].augmentPoints);
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
			msg += string.Join(" ", args);
		Broadcast(msg);
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

			StreamPeerTcp.Status status = client.GetStatus();
			switch (status)
			{
				case StreamPeerTcp.Status.None or StreamPeerTcp.Status.Error:
					OnDisconnect(i);
					continue;
				case StreamPeerTcp.Status.Connecting:
					GD.Print($"Client {i} is still connecting, skipping data check");
					continue;
			}

			int byteCount = client.GetAvailableBytes();
			if (byteCount > 0)
			{
				GD.Print($"Server: {byteCount} bytes are available from {i}");
				ReceiveData(i, client.GetUtf8String());
			}
		}
	}
}