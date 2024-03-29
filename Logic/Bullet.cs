using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;
using System;

public partial class Bullet : MovingKinematicBody2D
{
	public Player owner;
	public override void _Ready()
	{
		base._Ready();
		ParticleEmitter.Modulate = Colors.Purple;
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
	public void SetHitMultiplier(float hitMult)
	{
		this.hitMult = MathHelper.RandomRound(hitMult);
		if (this.hitMult > 1)
		{
			((Label)GetNode("HitMult")).Text = this.hitMult.ToString();
		}
	}
	public int hitMult = 1;
	public float damage = 1;
	public bool killShieldOrbs = false;
	public void EnemyCollision()
	{
		for (int i = 0; i < hitMult; i++)
		{
			if (Game.instance.enemy == null)
				break;
			float dmg = this.damage;
			int critLevel = GetCritLevel();
			float critMult = owner.upgradeManager.critDamageMult;
			owner.hooks.ForEach(x => x.ModifyHitWithBullet(this, ref dmg, ref critLevel, ref critMult));
			ModifyHit(ref dmg, ref critLevel, ref critMult);
			if (critLevel > 0)
				dmg *= critMult * critLevel;
			owner.hooks.ForEach(x => x.OnHitWithBullet(this, dmg));
			OnHit(dmg);
			Game.instance.enemy.Damage(dmg, UnhideableDamageNumbers, GetCritColor(critLevel));
		}
		QueueFree();
	}
	public void ShieldOrbCollision(ShieldOrb orb)
	{
		if (killShieldOrbs)
			orb.QueueFree();
		else
		{
			ShieldOrb.damageConsumed += damage * hitMult;
			QueueFree();
		}
	}
	protected virtual bool UnhideableDamageNumbers => false;
	protected virtual void ModifyHit(ref float dmg, ref int critlevel, ref float critMult) { }
	protected virtual void OnHit(float totalDmg) { }
	private int GetCritLevel()
		=> MathHelper.RandomRound(owner.upgradeManager.critChance);
	public static Color GetCritColor(int critLevel)
	{
		switch (critLevel)
		{
			case 0:
				return Color.Color8(255, 255, 255);
			case 1:
				return Color.Color8(153, 153, 0);
			case 2:
				return Color.Color8(204, 133, 0);
			default:
				return Color.Color8(204, 0, 0);
		}
	}
	public GpuParticles2D ParticleEmitter => (GetNode("GpuParticles2D") as GpuParticles2D);
	public void StartParticleEffect()
	{
		ParticleEmitter.Emitting = true;
	}


	public bool fused = false;
}
