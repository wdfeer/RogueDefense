using Godot;
using RogueDefense;
using System;

public class PointLabel : Label
{
    public override void _Ready()
    {
        SaveData.updateAugmentPointCounter = UpdateText;
        UpdateText(SaveData.SpareAugmentPoints);
    }
    void UpdateText(int points)
    {
        Text = points.ToString() + " Points";
    }
}
