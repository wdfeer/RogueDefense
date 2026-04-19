using System;
using System.Collections.Generic;
using System.Linq;
using RogueDefense.Logic.Enemy.Statuses;
using RogueDefense.Logic.Network;
using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Save;
using RogueDefense.Logic.UI.InGame;
using UserData = RogueDefense.Logic.Save.UserData;

namespace RogueDefense.Logic.Enemy;

public abstract partial class Enemy : Area2D
{
	public static List<Enemy> enemies = [];
	public abstract float GetBaseSpeed();
	EnemySound sound;
	public override void _Ready()
	{
		shieldOrbGenerator = GetNode<ShieldOrbGenerator>("ShieldOrbGenerator");
		hurtAnimator = GetNode<AnimationPlayer>("HurtAnimator");

		if (!enemies.Contains(this))
			enemies.Add(this);

		BodyEntered += OnBodyEntered;

		statuses = [burn, bleed, corrosive, viral, cold];
		foreach (Status status in statuses)
		{
			status.enemy = this;
		}

		ScaleStats(Game.Wave, enemies.FindIndex(x => x == this));

		CallDeferred("InitializeSound");
	}
	void InitializeSound()
	{
		sound = new EnemySound(this);
	}
	public static RandomNumberGenerator statsRng = new();
	public static void ResetRngSeed()
	{
		if (NetworkManager.Singleplayer)
			statsRng.Randomize();
		else
		{
			List<char> firstChars = Client.instance.others.Select(x => x.name[0]).ToList();
			firstChars.Add(UserData.name[0]);

			int seed = firstChars.Aggregate(0, (a, b) => a + b);
			statsRng.Seed = (ulong)seed;
		}
	}
	protected virtual bool ShieldOrbsAllowed => true;
	protected virtual void OnDeath() { }

	public float maxHp;
	private float hp;
	public float Hp
	{
		get => hp; set => hp = value;
	}
	public float armor;
	public float dynamicArmorMult = 1f;
	public float GetArmorDamageMultiplier(float armor) => 300f / (300f + armor * dynamicArmorMult);
	public void ResetArmorDisplay()
	{
		((ArmorBar)GetNode("ArmorBar")).SetDisplay(1f - GetArmorDamageMultiplier(armor));
	}

	public float dynamicDamageMult = 1f;
	AnimationPlayer hurtAnimator;
	public void Damage(float damage, bool unhideable, Color textColor, Vector2? textVelocity = null, bool ignoreArmor = false)
	{
		if (Dead) return;

		damage *= dynamicDamageMult;
		if (damage < minDamage)
			damage = 0;
		if (!ignoreArmor)
			damage *= GetArmorDamageMultiplier(armor);
		if (damageCap > 0 && damage > damageCap)
			damage = damageCap;
		Hp -= damage;

		PlayerManager.my.hooks.ForEach(x => x.OnAnyHit(damage));

		if (UserData.clientSettings.ShowCombatText || unhideable)
		{
			CombatTextDisplay.instance.AddCombatText(new CombatText()
			{
				position = GlobalPosition + new Vector2(-80 + GD.Randf() * 80, -100),
				direction = textVelocity ?? Vector2.Up,
				modulate = textColor,
				text = MathHelper.ToShortenedString(damage)
			});
		}

		if (Hp <= 0)
		{
			Die();
			return;
		}

		hurtAnimator.Play("Hurt");
	}
	public bool Dead => Hp <= 0;
	public void Die(bool netUpdate = true)
	{
		Hp = 0;
		OnDeath();
		Game.instance.OnEnemyDeath(this, netUpdate);
		sound.PlayDeathSound();
	}
	bool attacking = false;
	public float damage = 10f;
	public float attackInterval = 1f;
	float attackTimer = 0f;
	public float dynamicSpeedMult = 1f;
	public override void _Process(double delta)
	{
		if (Dead) return;

		if (attacking)
		{
			attackTimer += (float)delta;
			if (attackTimer > attackInterval)
			{
				attackTimer = 0f;
				DefenseObjective.instance.Damage(damage);
			}
		}
		dynamicSpeedMult = 1f;
		dynamicDamageMult = 1f;
		dynamicArmorMult = 1f;

		foreach (var s in statuses)
		{
			s.TryProcess((float)delta);
		}

		ResetArmorDisplay();
	}
	public override void _PhysicsProcess(double delta)
	{
		if (!Dead && !attacking)
		{
			var direction = GlobalPosition.DirectionTo(DefenseObjective.instance.GlobalPosition);
			var velocity = direction * GetBaseSpeed() * dynamicSpeedMult * (float)delta * 60;
			GlobalPosition += velocity;
		}
	}
	public void OnBodyEntered(Node body)
	{
		if (body == DefenseObjective.instance)
			attacking = true;
	}


	public float damageCap = -1f;
	static float GetDamageCap(int gen)
		=> gen > 20 ? (gen > 60 ? 0.03f : 0.08f) : 0.151f;
	public void SetDamageCap(float maxHpDamageCap)
	{
		Label label = (Label)GetNode("BottomInfo");
		label.Visible = true;
		label.Text = $"Damage Cap per Hit: {MathHelper.ToPercentAndRound(maxHpDamageCap)}%";
		damageCap = maxHp * maxHpDamageCap;
	}


	public float minDamage = -1f;
	static float GetMinDamage(int gen)
		=> gen > 10 ? (gen > 25 ? 0.03f : 0.06f) : 0.099f;
	public void SetMinDamage(float value)
	{
		Label label = (Label)GetNode("BottomInfo");
		label.Visible = true;
		label.Text = $"Minimum Damage per Hit: {MathHelper.ToPercentAndRound(value)}%";
		minDamage = maxHp * value;
	}
	
	
        // TODO: refactor and standardize this
	public Bleed bleed = new();
	public Burn burn = new();
	public Viral viral = new();
	public Cold cold = new();
	public Corrosive corrosive = new();
	public Status[] statuses;
	public void AddBurn(float totalDmg, float duration) => burn.Add(totalDmg / 5f, duration);
	public void AddBleed(float totalDmg, float duration) => bleed.Add(totalDmg / 5f, duration);
	public void AddViral(float duration) => viral.Add(duration);
	public void AddCold(float duration) => cold.Add(duration);



	public virtual bool Targetable => !Dead;
}
