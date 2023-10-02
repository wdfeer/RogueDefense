using Godot;
using RogueDefense.Logic.Statuses;
using System;

public partial class BleedContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return Enemy.instance.bleed;
    }
}