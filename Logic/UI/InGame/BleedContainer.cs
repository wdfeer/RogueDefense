using Godot;
using System;

public class BleedContainer : HBoxContainer
{
    public override void _Process(float delta)
    {
        int bleeds = Game.instance.enemy.bleeds.Count;
        if (bleeds > 0)
        {
            Visible = true;
            ((Label)GetNode("Counter")).Text = bleeds.ToString();
        }
        else Visible = false;
    }
}
