using Godot;
using RogueDefense;
using System;

public class AugmentContainer : HBoxContainer
{
    [Export]
    public string augmentText;
    public Label Label => GetNode<Label>("Label");

    public static float GetMyMult(int augmentIndex)
        => 1f + Player.my.augmentPoints[augmentIndex] * STAT_PER_POINT[augmentIndex];
    public static readonly float[] STAT_PER_POINT = new float[] {
        0.1f, 0.06f, 0.07f, 0.12f
    };
    public const int MIN_POINTS = -2;
    public float stat = 1f;
    public int points = 0;
    public override void _Ready()
    {
        Load();
    }
    void UpdateLabelText()
    {
        Label.Text = augmentText + stat.ToString("0.00");
    }

    public void ChangeStat(int effect)
    {
        UserSaveData.SpareAugmentPoints -= effect;
        points += effect;
        UpdateStatBasedOnPoints();
        UpdateLabelText();
    }
    void UpdateStatBasedOnPoints()
    {
        stat = 1f + points * STAT_PER_POINT[GetAugmentIndex()];
    }

    public void Load()
    {
        points = UserSaveData.augmentAllotment[GetAugmentIndex()];
        UpdateStatBasedOnPoints();
        UpdateLabelText();
    }

    int GetAugmentIndex() =>
        int.Parse(Name);
    public void Save()
    {
        UserSaveData.augmentAllotment[GetAugmentIndex()] = points;
    }
}
