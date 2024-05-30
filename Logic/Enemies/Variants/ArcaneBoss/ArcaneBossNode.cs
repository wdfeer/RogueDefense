using Godot;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.Statuses;
using System;
using System.Linq;

public partial class ArcaneBossNode : Enemy
{
	public override float GetBaseSpeed()
		=> 0;

	protected override void ModifyMaxHp(ref float maxHp)
	{
		base.ModifyMaxHp(ref maxHp);
	}
}
