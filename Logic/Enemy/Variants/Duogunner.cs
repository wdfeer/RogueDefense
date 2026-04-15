using System;
using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Player.Turret;

namespace RogueDefense.Logic.Enemy.Variants;

public partial class Duogunner : Enemy
{
    [Export] PackedScene gunScene;
    [Export] PackedScene bulletScene;

    public override void _Ready()
    {
        base._Ready();

        GetNode<Sprite2D>("Sprite2D").Rotate(GD.Randf() * Mathf.Pi);
    }

    [Export] Node2D[] spawnPoints;

    void SpawnGuns(int count)
    {
        spawnPoints = new Node2D[count];
        for (int i = 0; i < count; i++)
        {
            Node2D gun = gunScene.Instantiate<Node2D>();
            GetNode<Sprite2D>("Sprite2D").AddChild(gun);
            gun.GlobalPosition = GlobalPosition;
            gun.Rotation = Mathf.Pi * 2 * i / count;
            spawnPoints[i] = gun.GetNode<Node2D>("SpawnPoint");
        }
    }


    protected override bool ShieldOrbsAllowed => false;

    public override float GetBaseSpeed()
        => 0.3f;

    protected override void ModifyDamage(ref float damage)
    {
        damage *= 0.25f;
    }

    protected override void ModifyMaxHp(ref float maxHp)
    {
        maxHp *= 0.75f;
    }

    protected override void ModifyArmor(ref float armor)
    {
        armor *= 1.25f;

        if (armor < 300)
            armor = 300f;
    }


    protected virtual float HpCriticalModifier => (Hp / maxHp < 0.5f) ? 1.5f : 1;

    float shootTimer = GD.Randf();
    float ShootInterval => 1f / HpCriticalModifier;

    public override void _Process(double delta)
    {
        base._Process(delta);

        GetNode<Sprite2D>("Sprite2D").SetRotation(GlobalPosition.AngleTo(DefenseObjective.instance.GlobalPosition));

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