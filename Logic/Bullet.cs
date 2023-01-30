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
    public bool killShieldOrbs = false;
    protected override void OnCollision(KinematicCollision2D collision)
    {
        if (collision.Collider == Game.instance.enemy)
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
                Game.instance.enemy.Damage(dmg, GetCritColor(critLevel));
            }
            QueueFree();
        }
        else if (collision.Collider is ShieldOrb)
        {
            if (killShieldOrbs)
                ((ShieldOrb)collision.Collider).QueueFree();
            else
            {
                ShieldOrb.damageConsumed += damage * hitMult;
                QueueFree();
            }
        }
    }
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
}
