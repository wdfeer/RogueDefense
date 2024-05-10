
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;
using RogueDefense.Logic.Statuses;

public abstract partial class Enemy : Area2D
{
	public static List<Enemy> enemies = new List<Enemy>();
	public abstract float GetBaseSpeed();
	public override void _Ready()
	{
		shieldOrbGenerator = GetNode<ShieldOrbGenerator>("ShieldOrbGenerator");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		if (!enemies.Contains(this))
			enemies.Add(this);

		BodyEntered += OnBodyEntered;

		statuses = new Status[] { bleed, corrosive, viral, cold };
		foreach (Status status in statuses)
		{
			status.enemy = this;
		}

		ScaleStats(Game.Wave, enemies.FindIndex(x => x == this));
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
	void ScaleStats(int gen, int index)
	{
		gen = Math.Max(1, gen - index * 3);
		ScaleMaxHp(gen);
		ScaleDamage(gen);
		ScaleArmor(gen);

		ResetImmunities(gen, index);

		if (index == 0)
		{
			ResetShieldOrbs(gen);
			ResetEffects(gen);

			if (statsRng.Randf() < 0.15f && gen % 10 != 9)
				ActivateEffectField();
		}
	}
	void ResetImmunities(int gen, int index)
	{
		if (index == 0)
		{
			corrosive.immune = gen >= 30 && (gen + 5) % 10 == 0;
			viral.immune = !bleed.immune && gen >= 10 && statsRng.Randf() < 0.1f;
			cold.immune = !corrosive.immune && gen >= 40 && statsRng.Randf() < 0.1f;
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
			else if (shieldOrbGenerator.count > 0 && gen < 40 && rand < 0.2f)
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
	void ActivateEffectField()
	{
		((EffectField)GetNode("EffectField")).Enable(GD.Randf() < 0.5f ? EffectField.EffectFieldMode.Slow : EffectField.EffectFieldMode.Diffuse);
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

	[Export]
	public PackedScene combatText;
	public float dynamicDamageMult = 1f;
	AnimationPlayer animationPlayer;
	public void Damage(float damage, bool unhideable, Color textColor, Vector2? combatTextDirection = null, bool ignoreArmor = false)
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
			CombatText dmgText = combatText.Instantiate<CombatText>();
			GetNode("/root/Game").AddChild(dmgText);
			if (combatTextDirection != null)
				dmgText.direction = (Vector2)combatTextDirection;
			dmgText.Modulate = textColor;
			dmgText.Text = damage.ToString("0.0");
			dmgText.SetGlobalPosition(GlobalPosition + new Vector2(-80 + GD.Randf() * 80, -120));
		}

		if (Hp <= 0)
		{
			Die();
			return;
		}

		animationPlayer.Play("Hurt");
	}
	public bool Dead => Hp <= 0;
	public void Die(bool netUpdate = true)
	{
		Hp = 0;
		OnDeath();
		Game.instance.OnEnemyDeath(this, netUpdate);
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

		if (!attacking)
		{
			GlobalPosition += new Vector2(-GetBaseSpeed() * dynamicSpeedMult * (float)delta * 60, 0);
		}
	}
	public void OnBodyEntered(Node body)
	{
		if (body is Bullet bullet)
		{
			bullet.EnemyCollision(this);
		}
		else if (body == DefenseObjective.instance)
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


	public EffectField slowingField;


	public Bleed bleed = new Bleed();
	public Viral viral = new Viral();
	public Cold cold = new Cold();
	public Corrosive corrosive = new Corrosive();
	public Status[] statuses;
	public void AddBleed(float totalDmg, float duration) => bleed.Add(totalDmg / 5f, duration);
	public void AddViral(float duration) => viral.Add(duration);
	public void AddCold(float duration) => cold.Add(duration);
}
