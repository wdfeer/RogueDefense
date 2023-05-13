using Godot;
using RogueDefense;
using System;

public class UpgradeContainer : HBoxContainer
{
    [Export]
    public string upgradeText;
    public Label Label => GetNode<Label>("Label");

    public static readonly float[] STAT_PER_POINT = new float[] {
        0.1f, 0.06f, 0.07f
    };
    public const int MIN_POINTS = 0;
    public float stat = 1f;
    public int points = 0;
    public override void _Ready()
    {
        Load();
    }
    void UpdateLabelText()
    {
        Label.Text = upgradeText + stat.ToString("0.00");
    }

    public void ChangeStat(int effect)
    {
        UserSaveData.SpareUpgradePoints -= effect;
        points += effect;
        UpdateStatBasedOnPoints();
        UpdateLabelText();
    }
    void UpdateStatBasedOnPoints()
    {
        stat = 1f + points * STAT_PER_POINT[GetUpgradeIndex()];
    }

    public void Load()
    {
        points = UserSaveData.upgradePointDistribution[GetUpgradeIndex()];
        UpdateStatBasedOnPoints();
        UpdateLabelText();
    }

    int GetUpgradeIndex() =>
        int.Parse(Name);
    public void Save()
    {
        UserSaveData.upgradePointDistribution[GetUpgradeIndex()] = points;
    }
}
