using Godot;
using RogueDefense.Logic.Statuses;
using System;
using System.Linq;

public partial class FirstBoss : Enemy
{
	public override float GetBaseSpeed()
		=> (Hp / maxHp) < 0.5f ? 2f : 1f;

	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp *= 1.5f;
		maxHp += 60f;
	}
	protected override void ModifyImmunities(ref Status[] statuses)
	{
		Viral viral = (Viral)statuses.FirstOrDefault(x => x is Viral, null);
		if (viral != null)
		{
			viral.immune = false;
		}
	}
}
