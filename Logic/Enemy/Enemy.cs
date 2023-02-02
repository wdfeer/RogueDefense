
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense;
using RogueDefense.Logic;
using RogueDefense.Logic.Statuses;

public class Enemy : MovingKinematicBody2D
{
    public static Enemy instance;
    public const float BASE_SPEED = 1.15f;
    public override void _Ready()
    {
        instance = this;

        var gen = Game.instance.generation;

        SetMaxHp(gen);

        damage = 10f * Mathf.Sqrt(1f + gen);

        if (gen > 10f)
            armor = 50f * (gen - 10f);
        else armor = 0f;
        ResetArmorDisplay();

        bleed.immune = gen >= 20 && gen % 10 == 0;

        slowingField = (SlowingField)GetNode("SlowingField");
        if (statsRng.Randf() < 0.2f)
            slowingField.Enable();

        if (!bleed.immune)
        {
            float rand = statsRng.Randf();
            if (rand < 0.1f)
                SetDamageCap(GetDamageCap(gen));
            else if (gen > 9 && rand < 0.16f)
                SetMinDamage(GetMinDamage(gen));
        }
    }
    public static RandomNumberGenerator statsRng = new RandomNumberGenerator();
    public static void ResetRngSeed()
    {
        if (NetworkManager.Singleplayer)
            statsRng.Randomize();
        else
        {
            List<char> firstChars = Client.instance.others.Select(x => x.name[0]).ToList();
            firstChars.Add(RogueDefense.UserSaveData.name[0]);

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
            power = (gen <= 27f ? gen : 27f) / 15f;
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
    public float GetArmorDamageMultiplier(float armor) => 300f / (300f + armor);
    public void ResetArmorDisplay()
    {
        ArmorBar.instance.SetDisplay(1f - GetArmorDamageMultiplier(armor));
    }
    [Export]
    public PackedScene combatText;
    public float dynamicDamageMult = 1f;
    public void Damage(float damage, bool unhideable, Color textColor, Vector2? combatTextDirection = null, bool ignoreArmor = false)
    {
        damage *= dynamicDamageMult;
        if (damage < minDamage)
            damage = 0;
        if (!ignoreArmor)
            damage *= GetArmorDamageMultiplier(armor);
        if (damageCap > 0 && damage > damageCap)
            damage = damageCap;
        Hp -= damage;

        Player.localInstance.hooks.ForEach(x => x.OnAnyHit(damage));

        if (UserSaveData.showCombatText || unhideable)
        {
            CombatText dmgText = combatText.Instance() as CombatText;
            GetNode("/root/Game").AddChild(dmgText);
            if (combatTextDirection != null)
                dmgText.direction = (Vector2)combatTextDirection;
            dmgText.Modulate = textColor;
            dmgText.Text = damage.ToString("0.0");
            dmgText.SetGlobalPosition(GlobalPosition + new Vector2(-80 + GD.Randf() * 80, -120));
        }

        if (Hp <= 0)
        {
            Game.instance.DeleteEnemy(true);
        }
    }
    bool attacking = false;
    public float damage = 10f;
    public float attackInterval = 1f;
    float attackTimer = 0f;
    public float dynamicSpeedMult = 1f;
    public override void _Process(float delta)
    {
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
        dynamicSpeedMult = 1f;
        dynamicDamageMult = 1f;

        bleed.TryProcess(delta);
        viral.TryProcess(delta);
        cold.TryProcess(delta);

        velocity = new Vector2(-BASE_SPEED * dynamicSpeedMult, 0);
    }

    protected override void OnCollision(KinematicCollision2D collision)
    {
        if (collision.Collider == Game.instance.myPlayer)
        {
            attacking = true;
        }
    }


    public float damageCap = -1f;
    float GetDamageCap(int gen)
        => gen > 30 ? (gen > 60 ? 0.03f : 0.06f) : 0.101f;
    public void SetDamageCap(float maxHpDamageCap)
    {
        Label label = (Label)GetNode("BottomInfo");
        label.Visible = true;
        label.Text = $"Damage Cap per Hit: {MathHelper.ToPercentAndRound(maxHpDamageCap)}%";
        damageCap = maxHp * maxHpDamageCap;
    }


    public float minDamage = -1f;
    float GetMinDamage(int gen)
        => GetDamageCap(gen) * 0.49f;
    public void SetMinDamage(float minDamage)
    {
        Label label = (Label)GetNode("BottomInfo");
        label.Visible = true;
        label.Text = $"Minimum Damage per Hit: {MathHelper.ToPercentAndRound(minDamage)}%";
        this.minDamage = maxHp * minDamage;
    }


    public SlowingField slowingField;


    public Bleed bleed = new Bleed();
    public Viral viral = new Viral();
    public Cold cold = new Cold();
    public void AddBleed(float totalDmg, float duration) => bleed.Add(totalDmg / 5f, duration);
    public void AddViral(float duration) => viral.Add(duration);
    public void AddCold(float duration) => cold.Add(duration);
}
