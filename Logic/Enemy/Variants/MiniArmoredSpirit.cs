using System;
using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.Enemy.Variants;

public partial class MiniArmoredSpirit : ArmoredSpiritBoss
{
	public override float GetBaseSpeed()
		=> 0.9f;

	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp *= 0.7f;
	}

	protected override void ModifyDamage(ref float damage)
	{
		damage *= NetworkManager.Singleplayer ? 0.5f : 0.8f;
	}

	protected override float RotationSpeed => MathF.PI / 2;
}