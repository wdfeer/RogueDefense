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
            Game.instance.enemy.Damage(damage, GetCritColor(critLevel));
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
