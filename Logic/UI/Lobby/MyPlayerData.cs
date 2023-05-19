using Godot;
using RogueDefense;
using System;

public class MyPlayerData : PlayerData
{
    public override void _Ready()
    {
        SetName(RogueDefense.SaveData.name);
        AbilityLabel.Visible = false;
        Container.AddChild(new AbilityChooser());
    }
}
