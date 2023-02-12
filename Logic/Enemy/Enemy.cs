
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense;
using RogueDefense.Logic;
using RogueDefense.Logic.Statuses;

public class Enemy : KinematicBody2D
{
    public static Enemy instance;
    public const float BASE_SPEED = 1.15f;
    public override void _Ready()
    {
        instance = this;

        lastPos = GlobalPosition;

        statuses = new Status[] { bleed, corrosive, viral, cold };

        ScaleStats(Game.instance.generation);
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
    void ScaleStats(int gen)
    {
        ScaleMaxHp(gen);

        damage = 10f * Mathf.Sqrt(1f + gen);

        if (gen > 10f)
            armor = (NetworkManager.Singleplayer ? 30f : (gen > 55 ? 150f : 75f)) * (gen - 10f);
        else armor = 0f;
        ResetArmorDisplay();

        if (gen >= (NetworkManager.Singleplayer ? 75 : 24) && gen % (NetworkManager.Singleplayer ? 25 : 12) == 0)
        {
            bleed.immune = true;
            corrosive.immune = true;
        }
        else
        {
            bleed.immune = gen >= 20 && gen % 10 == 0;
            corrosive.immune = gen >= 30 && (gen + 5) % 10 == 0;
            viral.immune = !bleed.immune && gen >= 10 && statsRng.Randf() < 0.1f;
            cold.immune = !corrosive.immune && gen >= 40 && statsRng.Randf() < 0.1f;
        }

        if (statsRng.Randf() < 0.15f)
            ((SlowingField)GetNode("SlowingField")).Enable();

        if (!bleed.immune && !corrosive.immune)
        {
            ShieldOrbGenerator GetShieldOrbGenerator() => GetNode("ShieldOrbGenerator") as ShieldOrbGenerator;

            float rand = statsRng.Randf();
            if (rand < 0.1f)
                SetDamageCap(GetDamageCap(gen));
            else if (GetShieldOrbGenerator().count > 0 && gen < 40 && rand < 0.2f)
                SetMinDamage(GetMinDamage(gen));
        }
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
            baseMaxHp *= Client.instance.others.Count + 1f;
        }
        maxHp = Mathf.Round(baseMaxHp * Mathf.Pow(1f + gen, power) * (0.8f + statsRng.Randf() * 0.4f));
        Hp = maxHp;
    }

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
    public Vector2 lastPos;
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
        dynamicArmorMult = 1f;

        bleed.TryProcess(delta);
        viral.TryProcess(delta);
        cold.TryProcess(delta);
        corrosive.TryProcess(delta);

        ResetArmorDisplay();

        if (!attacking)
        {
            lastPos += new Vector2(-BASE_SPEED * dynamicSpeedMult, 0);
            GlobalPosition = lastPos;
            var collision = MoveAndCollide(Vector2.Zero);
            if (collision != null) OnCollision(collision);
        }
    }
    void OnCollision(KinematicCollision2D collision)
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
        => gen > 10 ? (gen > 25 ? 0.03f : 0.06f) : 0.099f;
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
    public Corrosive corrosive = new Corrosive();
    public Status[] statuses;
    public void AddBleed(float totalDmg, float duration) => bleed.Add(totalDmg / 5f, duration);
    public void AddViral(float duration) => viral.Add(duration);
    public void AddCold(float duration) => cold.Add(duration);
}
