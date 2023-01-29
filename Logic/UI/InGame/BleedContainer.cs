using Godot;
using System;

public class BleedContainer : HBoxContainer
{
    Label Counter => ((Label)GetNode("Counter"));
    public override void _Process(float delta)
    {
        bool bleedImmune = Game.instance.enemy.bleedImmune;
        if (bleedImmune)
        {
            Visible = true;
            Counter.Text = "IMMUNE";
            return;
        }

        int bleeds = Game.instance.enemy.bleeds.Count;
        if (bleeds > 0)
        {
            Visible = true;
            Counter.Text = bleeds.ToString();
        }
        else Visible = false;
    }
}
