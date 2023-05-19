using Godot;
using RogueDefense;
using System;

public class TopPlays : VBoxContainer
{
    public override void _Ready()
    {
        UpdatePlays();
    }

    public void UpdatePlays()
    {
        for (int i = 0; i < 3; i++)
        {
            Label label = (Label)GetNode($"TopPlay{i}/Label");
            label.Text = SaveData.topPP[i].ToString("0.000") + " pp";
        }
    }
}
