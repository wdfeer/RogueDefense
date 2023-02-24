using Godot;
using RogueDefense;
using RogueDefense.Logic;
using System;

public class BuffText : Label
{
    public override void _Process(float delta)
    {
        Text = "";

        var hook = PlayerHooks.GetLocalHooks<MultishotPerShotPlayer>();
        if (hook.CurrentBuff > 0)
            Text += $"+{MathHelper.ToPercentAndRound(hook.CurrentBuff)}% Multishot";
    }
}
