
using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense;
using RogueDefense.Logic;

public class Enemy : MovingKinematicBody2D
{
    public override void _Ready()
    {
        velocity = new Vector2(-1.15f, 0);
        var gen = Game.instance.generation;
        SetMaxHp(gen);
        damage = 10f * Mathf.Sqrt(1f + gen);
    }
    void SetMaxHp(int gen)
    {
        float baseMaxHp = 5f;
        if (!NetworkManager.Singleplayer)
            baseMaxHp *= Client.instance.others.Count + 1f;
        float power = gen <= 40f ? gen / 10f : (40f + Mathf.Sqrt(gen - 40f)) / 10f;
        maxHp = baseMaxHp * Mathf.Pow(1f + gen, power);
        Hp = maxHp;
    }

    public float maxHp;
    private float hp;
    public float Hp
    {
        get => hp; set
        {
            hp = value;
            (GetNode("./HpBar") as ProgressBar).Value = hp / maxHp;
            (GetNode("./HpBar/HpText") as Label).Text = $"{hp.ToString("0.0")} / {maxHp.ToString("0.0")}";
        }
    }
    [Export]
    public PackedScene combatText;
    public void Damage(float damage, Color textColor)
    {
        Hp -= damage;

        Player.localInstance.hooks.ForEach(x => x.OnAnyHit(damage));

        Label dmgText = combatText.Instance() as CombatText;
        GetNode("/root/Game").AddChild(dmgText);
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
    }

    protected override void OnCollision(KinematicCollision2D collision)
    {
        if (collision.Collider == Game.instance.myPlayer)
        {
            attacking = true;
        }
    }


    public void AddBleed(float totalDmg, float duration)
    {
        int ticks = MathHelper.RandomRound(duration / BLEED_INTERVAL);
        bleeds.Add((totalDmg / ticks, ticks));
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
            Damage(damage, Colors.WebGray);

            bleeds = bleeds.Select(x => (x.dpt, x.ticksLeft - 1)).Where(x => x.Item2 > 0).ToList();

            bleedTimer %= BLEED_INTERVAL;
        }
    }
}
