using Godot;
using System;

public class LobbyPlayerData : PanelContainer
{
    public new void SetName(string name)
    {
        (GetNode("./Container/Name") as Label).Text = name;
        this.Name = name;
    }
}
