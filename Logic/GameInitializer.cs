using Godot;
using RogueDefense;
using System;

public class GameInitializer : Node
{
    public override void _Ready()
    {
        UpgradeType.Initialize();
    }
}
