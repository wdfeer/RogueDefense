using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using RogueDefense.Logic.PlayerCore;
using RogueDefense.Logic.Enemies;

namespace RogueDefense.Logic;

public abstract class Projectile
{
    public Projectile()
    {
        shape = new CircleShape2D() { Radius = Radius };
        hitEnemies = new List<Rid>();
    }

    public Player owner;
    public void SetHitMultiplier(float hitMult)
    {
        this.hitMult = MathHelper.RandomRound(hitMult);
    }
    public int penetration = 0;
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


    public void QueueFree() => queuedForDeletion = true;
    public bool queuedForDeletion = false;


    protected abstract int Radius { get; }
    protected int Diameter => 2 * Radius;
    private CircleShape2D shape;
    protected virtual void CheckCollision()
    {
        var spaceState = owner.shootManager.projectileManager.GetWorld2D().DirectSpaceState;
        var query = new PhysicsShapeQueryParameters2D() { Shape = shape, Transform = new Transform2D() { Origin = position, X = Vector2.Right, Y = Vector2.Down }, CollideWithAreas = true, CollisionMask = 2, Exclude = new Array<Rid>(hitEnemies), Motion = velocity };
        Array<Dictionary> result = spaceState.IntersectShape(query, penetration + 1);

        for (int i = 0; i < result.Count; i++)
        {
            Variant collider = result[i]["collider"];
            Variant rid = result[i]["rid"];
            if (collider.Obj is Enemy enemy)
                EnemyCollision(enemy, (Rid)rid.Obj);
            else if (collider.Obj is ShieldOrb shieldOrb)
                ShieldOrbCollision(shieldOrb);
        }
    }

    readonly List<Rid> hitEnemies;
    public void EnemyCollision(Enemy enemy, Rid rid)
    {
        if (hitEnemies.Contains(rid))
            return;

        for (int i = 0; i < hitMult; i++)
        {
            if (enemy.Dead)
                break;
            float dmg = damage;
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

        penetration--;
        hitEnemies.Add(rid);
        if (penetration < 0)
            QueueFree();
    }
}
