using Godot;
using RogueDefense;
using System;

public class PointLabel : Label
{
    public override void _Ready()
    {
        UserSaveData.updateAugmentPointCounter = UpdateText;
        UpdateText(UserSaveData.SpareAugmentPoints);
    }
    void UpdateText(int points)
    {
        Text = points.ToString() + " Points";
    }
}
