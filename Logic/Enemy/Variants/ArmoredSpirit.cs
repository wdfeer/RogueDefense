using Godot;
using System;

public partial class ArmoredSpirit : Enemy
{
	public override float GetBaseSpeed()
		=> 0.75f;

	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp += 50;
	}
	protected override void ModifyArmor(ref float armor)
	{
		armor *= 2f;
		armor += 300f;
	}

}
