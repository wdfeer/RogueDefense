using System;

namespace RogueDefense.Logic.Enemies.Variants;

public partial class MiniArmoredSpirit : ArmoredSpirit
{
	public override float GetBaseSpeed()
		=> 1f;

	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp *= 0.7f;
	}

	protected override float RotationSpeed => MathF.PI / 2;
}