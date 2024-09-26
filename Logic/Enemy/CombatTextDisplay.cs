using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic.UI.InGame;

namespace RogueDefense.Logic.Enemy;

public partial class CombatTextDisplay : Node2D
{
	public static CombatTextDisplay instance;

	public override void _Ready()
	{
		instance = this;
	}
	public void AddCombatText(CombatText combatText)
	{
		texts.Add(combatText);
	}
	private List<CombatText> texts = new();
	public override void _Draw()
	{
		for (int i = 0; i < texts.Count; i++)
		{
			texts[i].Draw(this);
		}
	}
	public override void _Process(double delta)
	{
		for (int i = 0; i < texts.Count; i++)
		{
			texts[i].Process((float)delta);
		}

		DeleteQueued();

		QueueRedraw();
	}
	void DeleteQueued()
	{
		texts = texts.Where(x => !x.queuedForDeletion).ToList();
	}
}