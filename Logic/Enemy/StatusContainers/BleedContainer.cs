using Godot;
using RogueDefense.Logic.Statuses;
using System;

public class BleedContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return Enemy.instance.bleed;
    }
}