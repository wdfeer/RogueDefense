using Godot;
using RogueDefense;
using System;

public class PointLabel : Label
{
    public override void _Ready()
    {
        UserSaveData.onUpgradePointCountChanged = UpdateText;
        UpdateText(UserSaveData.SpareUpgradePoints);
    }
    void UpdateText(int points)
    {
        Text = points.ToString() + " Points";
    }
}
