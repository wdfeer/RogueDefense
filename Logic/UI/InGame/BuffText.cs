using System;
using System.Collections.Generic;
using Godot;
using RogueDefense;
using RogueDefense.Logic;

public partial class BuffText : Label
{
    public override void _Process(double delta)
    {
        Text = "";

        foreach (var getText in textGetters)
        {
            Text += getText();
        }
    }
    public List<Func<string>> textGetters = new List<Func<string>>();
}
