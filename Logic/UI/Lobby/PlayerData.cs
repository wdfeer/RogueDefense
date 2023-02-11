using Godot;
using System;

public class PlayerData : PanelContainer
{
    public new void SetName(string name)
    {
        (GetNode("./Container/Name") as Label).Text = name;
        this.Name = name;
    }
    public void SetAbilityText(string text)
    {
        (GetNode("./Container/AbilityLabel") as Label).Text = text;
    }
}
