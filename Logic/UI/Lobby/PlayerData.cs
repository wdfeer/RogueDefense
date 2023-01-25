using Godot;
using System;

public class PlayerData : PanelContainer
{
    public new void SetName(string name)
    {
        (GetNode("./Container/Name") as Label).Text = name;
        this.Name = name;
    }
}
