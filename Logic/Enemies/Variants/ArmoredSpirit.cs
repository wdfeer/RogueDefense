using System;
using System.Linq;
using Godot;
using RogueDefense.Logic.Enemies.Statuses;

namespace RogueDefense.Logic.Enemies.Variants;

public partial class ArmoredSpirit : Enemy
{
	public override float GetBaseSpeed()
		=> 0.6f;

	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp /= GetBaseSpeed();
		maxHp += 100f;
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
	protected override bool ShieldOrbsAllowed => false;

	protected virtual float RotationSpeed => MathF.PI / 3;
	public override void _Process(double delta)
	{
		base._Process(delta);

		GetNode<Sprite2D>("Sprite2D").Rotate(RotationSpeed * (float)delta);
	}
}