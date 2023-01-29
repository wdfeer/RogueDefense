using Godot;
using System;

public class ColdContainer : HBoxContainer
{
    public override void _Process(float delta)
    {
        int colds = Game.instance.enemy.colds.Count;
        if (colds > 0)
        {
            Visible = true;
            ((Label)GetNode("Counter")).Text = colds.ToString();
        }
        else Visible = false;
    }
}
