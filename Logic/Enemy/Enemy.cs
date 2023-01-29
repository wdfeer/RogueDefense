
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense;
using RogueDefense.Logic;

public class Enemy : MovingKinematicBody2D
{
    public static Enemy instance;
    public const float BASE_SPEED = 1.15f;
    public override void _Ready()
    {
        instance = this;

        ResetRngSeed();
        var gen = Game.instance.generation;

        SetMaxHp(gen);

        damage = 10f * Mathf.Sqrt(1f + gen);

        if (gen > 10f)
            armor = 50f * (gen - 10f);
        else armor = 0f;
        ResetArmorDisplay();

        bleedImmune = gen >= 20 && gen % 10 == 0;
    }
    RandomNumberGenerator statsRng = new RandomNumberGenerator();
    void ResetRngSeed()
    {
        if (NetworkManager.Singleplayer)
            statsRng.Randomize();
        else
        {
            List<char> firstChars = Client.instance.others.Select(x => x.name[0]).ToList();
            firstChars.Add(Player.myName[0]);

            int seed = firstChars.Aggregate(0, (a, b) => a + b);
        }
    }
    void SetMaxHp(int gen)
    {
        float baseMaxHp = 5f;
        if (!NetworkManager.Singleplayer && gen > 2)
            baseMaxHp *= Client.instance.others.Count + 1f;
        float power;
        if (NetworkManager.Singleplayer)
            power = (gen <= 40f ? gen : (40f + Mathf.Sqrt(gen - 40f))) / 15f;
        else
            power = gen / 15f;
        maxHp = Mathf.Round(baseMaxHp * Mathf.Pow(1f + gen * 0.8f, power) * (0.8f + statsRng.Randf() * 0.4f));
        Hp = maxHp;
    }

    public float maxHp;
    private float hp;
    public float Hp
    {
        get => hp; set => hp = value;
    }
    public float armor;
    public float ArmorDamageMultiplier => 300f / (300f + armor);
    public void ResetArmorDisplay()
    {
        ArmorBar.instance.SetDisplay(1f - ArmorDamageMultiplier);
    }
    [Export]
    public PackedScene combatText;
    public void Damage(float damage, Color textColor, Vector2? combatTextDirection = null)
    {
        damage *= ArmorDamageMultiplier * GetViralDmgMult();
        Hp -= damage;

        Player.localInstance.hooks.ForEach(x => x.OnAnyHit(damage));

        CombatText dmgText = combatText.Instance() as CombatText;
        GetNode("/root/Game").AddChild(dmgText);
        if (combatTextDirection != null)
            dmgText.direction = (Vector2)combatTextDirection;
        dmgText.Modulate = textColor;
        dmgText.Text = damage.ToString("0.0");
        dmgText.SetGlobalPosition(GlobalPosition + new Vector2(-80 + GD.Randf() * 80, -120));
        if (Hp <= 0)
        {
            Game.instance.DeleteEnemy(true);
        }
    }
    bool attacking = false;
    public float damage = 10f;
    public float attackInterval = 1f;
    float attackTimer = 0f;
    public override void _Process(float delta)
    {
        velocity = new Vector2(-BASE_SPEED, 0);
        base._Process(delta);
        if (attacking)
        {
            attackTimer += delta;
            if (attackTimer > attackInterval)
            {
                attackTimer = 0f;
                Game.instance.myPlayer.hpManager.Damage(damage);
            }
        }

        ProcessBleeds(delta);
        ProcessVirals(delta);
        ProcessColds(delta);
    }

    protected override void OnCollision(KinematicCollision2D collision)
    {
        if (collision.Collider == Game.instance.myPlayer)
        {
            attacking = true;
        }
    }


    public bool bleedImmune = false;
    public void AddBleed(float totalDmg, float duration)
    {
        if (bleedImmune)
            return;
        int ticks = MathHelper.RandomRound(duration / BLEED_INTERVAL);
        bleeds.Add((totalDmg / 5, ticks));
    }
    public List<(float dpt, int ticksLeft)> bleeds = new List<(float dpt, int ticksLeft)>();
    const float BLEED_INTERVAL = 1f;
    float bleedTimer = 0f;
    public void ProcessBleeds(float delta)
    {
        if (!bleeds.Any())
        {
            bleedTimer = BLEED_INTERVAL;
            return;
        }

        bleedTimer += delta;
        if (bleedTimer >= BLEED_INTERVAL)
        {
            float damage = bleeds.Aggregate(0f, (a, b) => a + b.dpt);

            float oldArmor = armor;
            armor = 0;
            Damage(damage, Colors.WebGray, new Vector2(0f, -0.6f));
            armor = oldArmor;

            bleeds = bleeds.Select(x => (x.dpt, x.ticksLeft - 1)).Where(x => x.Item2 > 0).ToList();

            bleedTimer %= BLEED_INTERVAL;
        }
    }


    public float GetViralDmgMult()
        => 1f + (virals.Count > 10 ? ((Mathf.Pow(virals.Count - 10, 0.7f)) + 10) : virals.Count) / 10f;
    public void AddViral(float duration)
        => virals.Add(duration);
    public List<float> virals = new List<float>();
    public void ProcessVirals(float delta)
    {
        virals = virals.Select(x => x - delta).Where(x => x > 0).ToList();
    }


    public float GetColdSpeedMult()
        => 1f / Mathf.Pow(colds.Count + 1f, 0.33f);
    public void AddCold(float duration)
    {
        if (colds.Count < 1000) colds.Add(duration);
    }
    public List<float> colds = new List<float>();
    public void ProcessColds(float delta)
    {
        colds = colds.Select(x => x - delta).Where(x => x > 0).ToList();
        velocity *= GetColdSpeedMult();
    }
}
