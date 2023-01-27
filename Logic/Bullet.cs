using Godot;
using RogueDefense;
using RogueDefense.Logic;
using System;

public class Bullet : MovingKinematicBody2D
{
    public Player owner;
    public override void _Ready()
    {
        base._Ready();
        owner = Player.localInstance;
    }
    public override void _Process(float delta)
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
    int hitMult = 1;
    public float damage = 1;
    protected override void OnCollision(KinematicCollision2D collision)
    {
        if (collision.Collider == Game.instance.enemy)
        {
            for (int i = 0; i < hitMult; i++)
            {
                if (Game.instance.enemy == null)
                    break;
                float damage = this.damage;
                int critLevel = MathHelper.RandomRound(owner.upgradeManager.critChance);
                float critMult = owner.upgradeManager.critDamage;
                owner.hooks.ForEach(x => x.ModifyHitWithBullet(this, ref damage, ref critLevel, ref critMult));
                if (critLevel > 0)
                    damage *= critMult * critLevel;
                Game.instance.enemy.Damage(damage, GetCritColor(critLevel));
            }
            QueueFree();
        }
    }

    private Color GetCritColor(int critLevel)
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
}
