using Godot;
using RogueDefense.Logic.PlayerCore;
using System;
using System.Collections.Generic;

public partial class Lobby : Control
{
	[Export]
	public PackedScene userDataScene;
	public static Lobby Instance => GDScript.IsInstanceValid(instance) ? instance : null;
	private static Lobby instance;
	public override void _Ready()
	{
		instance = this;


		if (NetworkManager.mode == NetMode.Client)
		{
			(GetNode("StartButton") as Button).Disabled = true;
			foreach (var data in Client.instance.others)
			{
				AddUser(data);
			}
		}
		else
			NetworkManager.NetStart(); // clients' NetworkManager is already started by JoinButton
	}
	Dictionary<int, PlayerData> userDisplayNodes = new Dictionary<int, PlayerData>();
	public void AddUser(UserData data)
	{
		var node = userDataScene.Instantiate<PlayerData>();
		node.SetPlayerName(data.name);
		node.SetAbilityText(AbilityManager.GetAbilityName(data.ability));
		userDisplayNodes.Add(data.id, node);
		GetNode("PlayerList").AddChild(node);
	}
	public void RemoveUser(int id)
	{
		if (userDisplayNodes.ContainsKey(id))
		{
			userDisplayNodes[id].QueueFree();
			userDisplayNodes.Remove(id);
		}
	}
}
