using Godot;
using RogueDefense.Logic.Statuses;
using System;

public partial class ViralContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.viral;
    }
}
