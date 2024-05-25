using Godot;
using RogueDefense;
using RogueDefense.Logic.PlayerCore;

public partial class AugmentContainer : HBoxContainer
{
	[Export]
	public string augmentText;
	public Label Label => GetNode<Label>("Label");

	public static float GetStatMult(int augmentIndex)
		=> 1f + Player.my.augmentPoints[augmentIndex] * STAT_PER_POINT[augmentIndex];
	public static readonly float[] STAT_PER_POINT = new float[] {
		0.1f, 0.07f, 0.08f, 0.12f, 0.12f
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
		SaveData.SpareAugmentPoints -= effect;
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
		points = SaveData.augmentAllotment[GetAugmentIndex()];
		UpdateStatBasedOnPoints();
		UpdateLabelText();
	}

	int GetAugmentIndex() =>
		int.Parse(Name);
	public void Save()
	{
		SaveData.augmentAllotment[GetAugmentIndex()] = points;
	}
}
