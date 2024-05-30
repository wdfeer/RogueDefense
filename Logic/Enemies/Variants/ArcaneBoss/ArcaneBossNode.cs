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
		maxHp += 50000 * NetworkManager.PlayerCount;
	}
	protected override void ModifyArmor(ref float armor)
	{
		armor = 0;
	}


	public override void _Process(double delta)
	{
		base._Process(delta);

		GlobalRotation = 0;
	}
}
