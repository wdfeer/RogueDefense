using Godot;
using RogueDefense.Logic.Statuses;
using System;

public class ViralContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return Enemy.instance.viral;
    }
}
