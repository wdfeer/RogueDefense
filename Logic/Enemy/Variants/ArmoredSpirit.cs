using Godot;
using RogueDefense.Logic.Statuses;
using System;
using System.Linq;

public partial class ArmoredSpirit : Enemy
{
	public override float GetBaseSpeed()
		=> 0.75f;

	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp *= 1.25f;
		maxHp += 50f;
	}
	protected override void ModifyArmor(ref float armor)
	{
		armor *= 3f;
		armor += 500f;
	}
	protected override void ModifyImmunities(ref Status[] statuses)
	{
		Bleed bleed = (Bleed)statuses.FirstOrDefault(x => x is Bleed, null);
		if (bleed != null)
		{
			bleed.immune = true;
		}
	}
}
