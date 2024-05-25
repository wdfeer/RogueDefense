using System;
using Godot;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense.Logic;

public abstract class Projectile
{
    public Player owner;
    public void SetHitMultiplier(float hitMult)
    {
        this.hitMult = MathHelper.RandomRound(hitMult);
    }
    public int hitMult = 1;
    public float damage = 1;
    public virtual bool KillShieldOrbs => false;

    public void ShieldOrbCollision(ShieldOrb orb)
    {
        if (KillShieldOrbs)
            orb.QueueFree();
        else
        {
            ShieldOrb.damageConsumed += damage * hitMult;
            QueueFree();
        }
    }
    protected virtual bool UnhideableDamageNumbers => false;
    protected virtual void ModifyHit(ref float dmg, ref int critlevel, ref float critMult) { }
    protected virtual void OnHit(Enemy enemy, float totalDmg) { }
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


    private bool emittingParticles = false;
    public void StartParticleEffect()
    {
        emittingParticles = true;
    }


    public bool fused = false;

    public Vector2 position = Vector2.Zero;
    public Vector2 velocity = Vector2.Zero;
    private void QueueFree() => queuedForDeletion = true;
    public bool queuedForDeletion = false;


    public abstract void Draw(CanvasItem drawer);
    public virtual void PhysicsProcess(float delta)
    {
        position += velocity * delta;
        CheckCollision();
    }
    private const float RADIUS = 50;
    private void CheckCollision()
    {
        var spaceState = DefenseObjective.instance.GetWorld2D().DirectSpaceState;
        var query = new PhysicsShapeQueryParameters2D() { CollisionMask = 2, Shape = new CircleShape2D() { Radius = RADIUS } };
        var result = spaceState.IntersectShape(query, 1);
        object collider = result[0]["collider"];
        if (collider is Enemy enemy)
            EnemyCollision(enemy);
    }

    public void EnemyCollision(Enemy enemy)
    {
        for (int i = 0; i < hitMult; i++)
        {
            if (enemy.Dead)
                break;
            float dmg = this.damage;
            int critLevel = GetCritLevel();
            float critMult = owner.upgradeManager.critDamageMult;
            owner.hooks.ForEach(x => x.ModifyHitEnemyWithProj(enemy, this, ref dmg, ref critLevel, ref critMult));
            ModifyHit(ref dmg, ref critLevel, ref critMult);
            if (critLevel > 0)
                dmg *= critMult * critLevel;
            owner.hooks.ForEach(x => x.OnHitWithProj(enemy, this, dmg));
            OnHit(enemy, dmg);
            enemy.Damage(dmg, UnhideableDamageNumbers, GetCritColor(critLevel));
        }
        QueueFree();
    }
}
