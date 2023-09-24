using Godot;
using RogueDefense.Logic.Statuses;
using System;

public partial class ColdContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return Enemy.instance.cold;
    }
}
