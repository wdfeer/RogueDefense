using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RogueDefense.Logic.Enemy;
using RogueDefense.Logic.Network.Messages;
using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Player.Hooks;
using RogueDefense.Logic.Save;
using RogueDefense.Logic.UI.Lobby.Settings;
using RogueDefense.Logic.UI.MainMenu;

namespace RogueDefense.Logic.Network;

public partial class Client : Node
{
	public static string host;
	public static ushort port;
	public static Client instance = new();
	public static StreamPeerTcp client;
	public static int myId = -1;

	public void Start()
	{
		client = new();

		GD.Print($"Trying to connect to {host}:{port}");
		var err = client.ConnectToHost(host, port);
		if (err != Error.Ok)
		{
			GD.PrintErr("Unable to start client");
			SetProcess(false);
		}
	}

	public void Stop()
	{
		if (client != null)
			client.DisconnectFromHost();
		client = null;
	}

	public List<UserData> others = [];
	public UserData GetUserData(int id) => others.Find(x => x.id == id);
	private void RemoveUserData(int id) => others.Remove(GetUserData(id));

	public static void ChangeSceneToLobby()
	{
		GD.Print("This client connected! Loading lobby...");
		if (NetworkManager.mode == NetMode.Client)
			UI.JoinScene.JoinScene.TryChangeToLobbyScene();
	}

	private void ReceiveMessage(MessageType type, Resource message)
	{
		GD.Print($"Client received message of type {type}.");
		((IMessage)message).ClientHandle(this);
	}


	public void RegisterUser(RegisterMessage data)
	{
		UserData d = new UserData(data.id, data.name, data.ability, data.augmentPoints);
		others.Add(d);
		if (Lobby.Instance != null)
		{
			Lobby.Instance.AddUser(d);
		}
	}

	public static void RegisterSelf()
	{
		instance.SendMessage(MessageType.Register, new RegisterMessage()
		{
			id = myId,
			name = Save.UserData.name,
			ability = AbilityChooser.chosen,
			augmentPoints = Save.UserData.augmentAllotment,
		});
	}

	public void UnregisterUser(int id)
	{
		RemoveUserData(id);
		if (Lobby.Instance != null)
		{
			Lobby.Instance.RemoveUser(id);
		}
	}

	static void Broadcast(MessageType type, Resource message)
	{
		Debug.Assert(client != null);
		StreamHelper.WriteIntoStream(client, type, message);
	}

	public void SendMessage(MessageType type, Resource message)
	{
		GD.Print($"Sending message of type {type} to Server.");
		Broadcast(type, message);
	}


	public void Poll() // always keep polling
	{
		client.Poll();
		if (client.GetStatus() != StreamPeerSocket.Status.Connected)
			return;

		int byteCount = client.GetAvailableBytes();
		if (byteCount > 0)
		{
			GD.Print($"Client: {byteCount} bytes are available.");
			var tuple = StreamHelper.ReadFromStream(client);
			ReceiveMessage(tuple.Item1, tuple.Item2);
		}
	}
}
