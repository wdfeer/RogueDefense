using Godot;
using RogueDefense.Logic.Statuses;
using System;

public abstract partial class StatusContainer : HBoxContainer
{
    public Label Counter => ((Label)GetNode("Counter"));
    public abstract Status GetStatus();
    public override void _Process(double delta)
    {
        if (GetStatus().immune)
        {
            Visible = true;
            Counter.Text = "IMMUNE";
            return;
        }

        int count = GetStatus().Count;
        if (count > 0)
        {
            Visible = true;
            Counter.Text = count.ToString();
        }
        else Visible = false;
    }
}
