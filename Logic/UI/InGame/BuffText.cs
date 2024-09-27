using System;
using System.Collections.Generic;

namespace RogueDefense.Logic.UI.InGame;

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