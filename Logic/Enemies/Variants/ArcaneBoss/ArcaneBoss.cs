using Godot;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.Statuses;
using System;
using System.Linq;

public partial class ArcaneBoss : Enemy
{
	public override float GetBaseSpeed()
		=> 0.1f;

	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp += 100000 * NetworkManager.PlayerCount;
	}
	protected override void ModifyArmor(ref float armor)
	{
		armor = 600;
	}


	public ArcaneBossNode[] nodes = new ArcaneBossNode[4];
	public bool Vulnerable => nodes.All(x => !IsInstanceValid(x) || x.Dead);
	public override bool Targetable => base.Targetable && Vulnerable;
	public override void _Ready()
	{
		base._Ready();

		for (int i = 0; i < 4; i++)
		{
			nodes[i] = GetNode("Sprite2D").GetChild<ArcaneBossNode>(i);
		}
	}


	public override void _Process(double delta)
	{
		base._Process(delta);

		CollisionLayer = (uint)(Vulnerable ? 2 : 0);

		GetNode<Node2D>("Sprite2D").Rotate(Mathf.Pi / 4 * (float)delta);

		if (Vulnerable)
		{
			GetNode<Control>("HpBar").Visible = true;
			GetNode<Control>("Statuses").Visible = true;
		}
	}
}
