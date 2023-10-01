using Godot;
using System;

public partial class PlayerData : PanelContainer
{
	public HBoxContainer Container => (HBoxContainer)GetNode("./Container");
	public Label NameLabel => GetNode("./Container/Name") as Label;
	public void SetPlayerName(string name)
	{
		NameLabel.Text = name;
		CallDeferred("SetNodeName", new[] { name });
	}
	private void SetNodeName(string name)
	{
		Name = name;
	}
	public Label AbilityLabel => GetNode("./Container/AbilityLabel") as Label;
	public void SetAbilityText(string text)
	{
		AbilityLabel.Text = text;
	}
}
