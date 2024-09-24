using Godot;
using RogueDefense.Logic.Enemies.Statuses;
using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.Enemies.Variants.ArcaneBoss;
public partial class ArcaneBossNode : Enemy
{
	[Export]
	public float animationRotationDegrees = 0;
	public override void _Ready()
	{
		base._Ready();

		GetNode<Node2D>("OutgoingEnergy").Rotate(Mathf.DegToRad(animationRotationDegrees));
	}

	public override float GetBaseSpeed()
		=> 0;

	protected override void ModifyGen(ref int gen, int index) { }
	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp += 10 ^ Game.GetStage() * NetworkManager.PlayerCount / 4;
	}
	protected override void ModifyArmor(ref float armor)
	{
		armor = 0;
	}
	protected override void ModifyImmunities(ref Status[] statuses)
	{
		foreach (var status in statuses)
		{
			status.immune = false;
		}
	}
	protected override bool ShieldOrbsAllowed => false;


	public override void _Process(double delta)
	{
		base._Process(delta);

		GetNode<Node2D>("OutgoingEnergy").Rotation += GlobalRotation;
		GlobalRotation = 0;

		GetNode<ArcaneBoss>("../..").charge += (float)delta;
	}
}
