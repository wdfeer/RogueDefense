using System;
using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Player.Turret;

namespace RogueDefense.Logic.Enemy.Variants;

public partial class Duogunner : Enemy
{
    [Export] PackedScene bulletScene;
    [Export] Node2D[] spawnPoints;

    protected override bool ShieldOrbsAllowed => false;

    public override float GetBaseSpeed()
        => 0.3f;

    protected override void ModifyDamage(ref float damage)
    {
        damage *= 0.3f;
    }

    protected override void ModifyMaxHp(ref float maxHp)
    {
        maxHp *= 0.75f;
    }

    protected override void ModifyArmor(ref float armor)
    {
        armor *= 1.5f;

        if (armor < 300)
            armor = 300f;
    }


    protected virtual float HpCriticalModifier => (Hp / maxHp < 0.5f) ? 1.5f : 1;

    float shootTimer = GD.Randf();
    float ShootInterval => (Game.Wave > 20 ? 1.2f : 1.5f) / HpCriticalModifier;

    public override void _Process(double delta)
    {
        base._Process(delta);

        var target = PlayerManager.my.controlledTurret.GlobalPosition;
        var diff = target - GlobalPosition;
        GetNode<Sprite2D>("Sprite2D").GlobalRotation =
            MathF.Atan2(diff.Y, diff.X);

        shootTimer += (float)delta;
        if (shootTimer > ShootInterval)
        {
            shootTimer = 0;
            Shoot();
        }
    }

    float ShootSpeed => 80 * HpCriticalModifier;

    void Shoot()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            EnemyBullet bullet = bulletScene.Instantiate<EnemyBullet>();
            AddSibling(bullet);

            Vector2 pos = spawnPoints[i].GlobalPosition;
            bullet.GlobalPosition = pos;

            Vector2 velocity = GlobalPosition.DirectionTo(pos) * ShootSpeed;
            bullet.velocity = velocity;

            bullet.damage = damage / 2;
            bullet.lifespan = 8 / HpCriticalModifier;
        }
    }
}