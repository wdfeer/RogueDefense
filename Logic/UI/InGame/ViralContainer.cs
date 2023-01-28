using Godot;
using System;

public class ViralContainer : HBoxContainer
{
    public override void _Process(float delta)
    {
        int virals = Game.instance.enemy.virals.Count;
        if (virals > 0)
        {
            Visible = true;
            ((Label)GetNode("Counter")).Text = virals.ToString();
        }
        else Visible = false;
    }
}
