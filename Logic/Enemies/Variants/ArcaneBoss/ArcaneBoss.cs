using Godot;
using System.Linq;
using RogueDefense.Logic.Enemies.Statuses;
using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.Enemies.Variants.ArcaneBoss;
public partial class ArcaneBoss : Enemy
{
	public override float GetBaseSpeed()
		=> 0.1f;

	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp += 10 ^ Game.GetStage() * NetworkManager.PlayerCount;
	}
	protected override void ModifyArmor(ref float armor)
	{
		armor = 600;
	}
	protected override void ModifyImmunities(ref Status[] statuses)
	{
		foreach (var status in statuses)
		{
			status.immune = false;
		}
	}
	protected override bool ShieldOrbsAllowed => false;


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


	public float charge = 0;
	const float MIN_CHARGE = 12;
	public override void _Process(double delta)
	{
		base._Process(delta);

		CollisionLayer = (uint)(Vulnerable ? 2 : 0);
		GetNode<Sprite2D>("ShieldSprite").Visible = !Vulnerable;

		GetNode<Node2D>("Sprite2D").Rotate(Mathf.Pi / 6 * (float)delta);

		if (Vulnerable)
		{
			GetNode<Control>("HpBar").Visible = true;
			GetNode<Control>("Statuses").Visible = true;
		}

		Sprite2D energy = GetNode<Sprite2D>("EnergySprite");
		energy.Modulate = energy.Modulate with { A = charge / MIN_CHARGE };

		charge += (float)delta;
		if (charge >= MIN_CHARGE)
		{
			charge = 0;
			Shoot();
		}

		QueueRedraw();
	}

	[Export]
	PackedScene arcaneBulletScene;
	void Shoot()
	{
		Vector2 target = DefenseObjective.instance.GlobalPosition;

		for (int i = 0; i < 10; i++)
		{
			ArcaneBullet bullet = arcaneBulletScene.Instantiate<ArcaneBullet>();
			AddSibling(bullet);

			bullet.GlobalPosition = GlobalPosition;
			bullet.velocity = GlobalPosition.DirectionTo(target).Rotated((GD.Randf() - 0.5f) * 0.12f) * (200 + i * 40);

			bullet.damage = damage / 3;
			bullet.lifespan = 3;
		}
	}
}
