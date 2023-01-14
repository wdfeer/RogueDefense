using Godot;
using RogueDefense.Logic;
using System;

public class Bullet : MovingKinematicBody2D
{
    public override void _Process(float delta)
    {
        base._Process(delta);
    }

    public float damage = 1;
    protected override void OnCollision(KinematicCollision2D collision)
    {
        if (collision.Collider == Game.instance.enemy)
        {
            int critLevel = MathHelper.RandomRound(Game.instance.player.upgradeManager.critChance);
            if (critLevel > 0)
                damage *= Game.instance.player.upgradeManager.critDamage * critLevel;
            Game.instance.enemy.Damage(damage);
            QueueFree();
        }
    }
}
