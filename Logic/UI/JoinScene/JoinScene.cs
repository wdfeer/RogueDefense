using Godot;
using System;

public partial class JoinScene : CenterContainer
{
	[Export]
	public PackedScene lobbyScene;
	public static JoinScene instance;
	public override void _Ready()
	{
		instance = this;
	}

	public static void TryChangeToLobbyScene()
	{
		if (instance == null || !IsInstanceValid(instance)) return;
		instance.GetTree().ChangeSceneToPacked(instance.lobbyScene);
	}
}
