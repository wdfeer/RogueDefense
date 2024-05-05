using Godot;
using RogueDefense;
using RogueDefense.Logic;
using System;

public partial class BuffText : Label
{
    public override void _Process(double delta)
    {
        Text = "";

        UpdateMultishotPerShotText();
    }
    void UpdateMultishotPerShotText()
    {
        var hook = PlayerHooks.GetLocalHooks<MultishotPerShotPlayer>();
        if (hook.CurrentBuff > 0)
            Text += $"+{MathHelper.ToPercentAndRound(hook.CurrentBuff)}% Multishot\n";
    }
}
