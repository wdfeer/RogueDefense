using Godot;
using System;

public partial class PlayerData : PanelContainer
{
	public HBoxContainer Container => (HBoxContainer)GetNode("./Container");
	public Label NameLabel => (GetNode("./Container/Name") as Label);
	public new void SetName(string name)
	{
		NameLabel.Text = name;
		this.Name = name;
	}
	public Label AbilityLabel => (GetNode("./Container/AbilityLabel") as Label);
	public void SetAbilityText(string text)
	{
		AbilityLabel.Text = text;
	}
}
