using Godot;
using RogueDefense.Logic.Statuses;
using System;

public class CorrosiveContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return Enemy.instance.corrosive;
    }
}
