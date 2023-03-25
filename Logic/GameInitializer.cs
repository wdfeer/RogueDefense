using Godot;
using RogueDefense;
using System;
using System.Collections.Generic;

public class GameInitializer : Node
{
    public override void _Ready()
    {
        Player.my = null;
        Player.players = new Dictionary<int, Player>();

        RogueDefense.UserSaveData.Load();

        UpgradeType.Initialize();
    }
}
