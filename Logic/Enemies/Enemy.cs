
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic.PlayerCore;
using RogueDefense.Logic.Statuses;

namespace RogueDefense.Logic.Enemies;

public abstract partial class Enemy : Area2D
{
	public static List<Enemy> enemies = new List<Enemy>();
	public abstract float GetBaseSpeed();
	EnemySound sound;
	public override void _Ready()
	{
		shieldOrbGenerator = GetNode<ShieldOrbGenerator>("ShieldOrbGenerator");
		hurtAnimator = GetNode<AnimationPlayer>("HurtAnimator");

		if (!enemies.Contains(this))
			enemies.Add(this);

		BodyEntered += OnBodyEntered;

		statuses = new Status[] { bleed, corrosive, viral, cold };
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
	public static RandomNumberGenerator statsRng = new RandomNumberGenerator();
	public static void ResetRngSeed()
	{
		if (NetworkManager.Singleplayer)
			statsRng.Randomize();
		else
		{
			List<char> firstChars = Client.instance.others.Select(x => x.name[0]).ToList();
			firstChars.Add(SaveData.name[0]);

			int seed = firstChars.Aggregate(0, (a, b) => a + b);
			statsRng.Seed = (ulong)seed;
		}
	}
	public static float oneTimeHpMult = 1f;
	public static float oneTimeArmorMult = 1f;
	public static float oneTimeDamageMult = 1f;
	public static int oneTimeCountIncrease = 0;
	protected virtual void ModifyGen(ref int gen, int index)
	{
		gen = Math.Max(1, gen - index * 2);
	}
	void ScaleStats(int gen, int index)
	{
		ModifyGen(ref gen, index);
		ScaleMaxHp(gen);
		ScaleDamage(gen);
		ScaleArmor(gen);

		ResetImmunities(gen, index);

		if (index == 0)
		{
			ResetShieldOrbs(gen);
			ResetEffects(gen);
		}
	}
	void ResetImmunities(int gen, int index)
	{
		if (index != 0 && gen > 10 && statsRng.Randf() < 0.3f)
		{
			switch (statsRng.RandiRange(0, 2))
			{
				case 0 when gen > 30:
					cold.immune = true;
					break;
				case 1:
					viral.immune = true;
					break;
				case 2:
					corrosive.immune = true;
					break;
				default:
					break;
			}
		}

		ModifyImmunities(ref statuses);
	}
	ShieldOrbGenerator shieldOrbGenerator;
	void ResetEffects(int gen)
	{
		if (!bleed.immune && !corrosive.immune)
		{
			float rand = statsRng.Randf();
			if (rand < 0.1f)
				SetDamageCap(GetDamageCap(gen));
			else if (shieldOrbGenerator.count > 0 && gen > 15 && gen < 35 && rand < 0.2f)
				SetMinDamage(GetMinDamage(gen));
		}
	}
	void ResetShieldOrbs(int gen)
	{
		if (!ShieldOrbsAllowed) return;

		bool exploding = gen > 19;

		if (gen % 10 == 9)
		{
			shieldOrbGenerator.CreateOrbs(5, false);
			return;
		}

		if (gen % 2 == 0 && GD.Randf() < 0.5f)
			shieldOrbGenerator.CreateOrbs(1 + Mathf.RoundToInt(GD.Randf() * 4), exploding: exploding);
		else shieldOrbGenerator.count = 0;
	}
	void ScaleMaxHp(int gen)
	{
		float baseMaxHp = 5f;
		float power;
		if (NetworkManager.Singleplayer)
			power = (gen <= 40f ? gen : 40f + Mathf.Pow(gen - 40, 0.75f)) / 20f;
		else
		{
			power = gen / 17.5f;
			baseMaxHp *= NetworkManager.PlayerCount;
		}
		maxHp = Mathf.Round(baseMaxHp * Mathf.Pow(1f + gen, power) * (0.8f + statsRng.Randf() * 0.4f)) * oneTimeHpMult;

		ModifyMaxHp(ref maxHp);

		Hp = maxHp;

		oneTimeHpMult = 1f;
	}
	void ScaleDamage(int gen)
	{
		damage = 12.5f * Mathf.Sqrt(1f + gen) * oneTimeDamageMult;

		ModifyDamage(ref damage);

		oneTimeDamageMult = 1f;
	}
	void ScaleArmor(int gen)
	{
		if (gen > 9)
			armor = (NetworkManager.Singleplayer ? 30f : (gen > 55 ? 150f : 75f)) * (gen - 9f) * oneTimeArmorMult;
		else
			armor = 0f;

		ModifyArmor(ref armor);

		oneTimeArmorMult = 1f;

		ResetArmorDisplay();
	}


	protected virtual void ModifyMaxHp(ref float maxHp) { }
	protected virtual void ModifyDamage(ref float damage) { }
	protected virtual void ModifyArmor(ref float armor) { }
	protected virtual void ModifyImmunities(ref Status[] statuses) { }
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

		Player.my.hooks.ForEach(x => x.OnAnyHit(damage));

		if (SaveData.showCombatText || unhideable)
		{
			CombatTextDisplay.instance.AddCombatText(new CombatText()
			{
				position = GlobalPosition + new Vector2(-80 + GD.Randf() * 80, -100),
				direction = textVelocity == null ? Vector2.Up : (Vector2)textVelocity,
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

		bleed.TryProcess((float)delta);
		viral.TryProcess((float)delta);
		cold.TryProcess((float)delta);
		corrosive.TryProcess((float)delta);

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
	float GetDamageCap(int gen)
		=> gen > 20 ? (gen > 60 ? 0.03f : 0.08f) : 0.151f;
	public void SetDamageCap(float maxHpDamageCap)
	{
		Label label = (Label)GetNode("BottomInfo");
		label.Visible = true;
		label.Text = $"Damage Cap per Hit: {MathHelper.ToPercentAndRound(maxHpDamageCap)}%";
		damageCap = maxHp * maxHpDamageCap;
	}


	public float minDamage = -1f;
	float GetMinDamage(int gen)
		=> gen > 10 ? (gen > 25 ? 0.03f : 0.06f) : 0.099f;
	public void SetMinDamage(float minDamage)
	{
		Label label = (Label)GetNode("BottomInfo");
		label.Visible = true;
		label.Text = $"Minimum Damage per Hit: {MathHelper.ToPercentAndRound(minDamage)}%";
		this.minDamage = maxHp * minDamage;
	}



	public Bleed bleed = new Bleed();
	public Viral viral = new Viral();
	public Cold cold = new Cold();
	public Corrosive corrosive = new Corrosive();
	public Status[] statuses;
	public void AddBleed(float totalDmg, float duration) => bleed.Add(totalDmg / 5f, duration);
	public void AddViral(float duration) => viral.Add(duration);
	public void AddCold(float duration) => cold.Add(duration);



	public virtual bool Targetable => !Dead;
}
