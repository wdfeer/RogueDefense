using Godot;
using RogueDefense;
using RogueDefense.Logic;
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

            string text = $"{SaveData.topPP[i].ToString("0.000")} pp  *  {MathHelper.ToPercentAndRound(Mathf.Pow(0.5f, i))}%";

            label.Text = text;
        }

        ((Label)GetNode("TotalPP")).Text = $"= {PP.GetTotalPP().ToString("0.000")} pp";
    }
}
