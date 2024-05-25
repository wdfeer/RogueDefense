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


    protected bool emittingParticles = false;
    public void StartParticleEffect()
    {
        emittingParticles = true;
    }


    public bool fused = false;


    public abstract void Draw(CanvasItem drawer);


    public Vector2 position = Vector2.Zero;
    public Vector2 velocity = Vector2.Zero;
    public Rect2 playArea = new Rect2(-612, -300, new Vector2I(2050, 1200));
    public virtual void PhysicsProcess(float delta)
    {
        position += velocity * delta * 60;
        CheckCollision();

        if (!playArea.HasPoint(position))
            QueueFree();
    }


    private void QueueFree() => queuedForDeletion = true;
    public bool queuedForDeletion = false;


    protected abstract int Radius { get; }
    protected int Diameter => 2 * Radius;
    private void CheckCollision()
    {
        var spaceState = Player.my.controlledTurret.GetWorld2D().DirectSpaceState;
        var query = new PhysicsShapeQueryParameters2D() { Shape = new CircleShape2D() { Radius = Radius }, Transform = new Transform2D() { Origin = position }, CollideWithAreas = true, CollisionMask = 2 };
        var result = spaceState.IntersectShape(query, 1);
        if (result.Count == 0)
            return;
        Variant collider = result[0]["collider"];
        if (collider.Obj is Enemy enemy)
            EnemyCollision(enemy);
        else if (collider.Obj is ShieldOrb shieldOrb)
            ShieldOrbCollision(shieldOrb);
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
