using System;
using System.Linq;
using RogueDefense.Logic.Enemy.Statuses;
using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.Enemy.Variants;

public partial class ArmoredSpiritBoss : Enemy
{
	public override float GetBaseSpeed()
		=> 0.5f;

	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp *= 2;
		maxHp += NetworkManager.Singleplayer ? 75f : 100f;
	}
	protected override void ModifyArmor(ref float armor)
	{
		armor *= 2.5f;
		armor += NetworkManager.Singleplayer ? 360f : 500f;
	}
	protected override void ModifyImmunities(ref Status[] statuses)
	{
		Bleed bleed = (Bleed)statuses.FirstOrDefault(x => x is Bleed, null);
		if (bleed != null)
		{
			bleed.immune = true;
		}
	}
	protected override bool ShieldOrbsAllowed => false;

	protected virtual float RotationSpeed => MathF.PI / 3;
	public override void _Process(double delta)
	{
		base._Process(delta);

		GetNode<Sprite2D>("Sprite2D").Rotate(RotationSpeed * (float)delta);
	}
}