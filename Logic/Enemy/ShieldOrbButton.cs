using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;
using System;

public partial class ShieldOrbButton : TextureButton
{
    public Enemy enemy;
    public override void _Pressed()
    {
        if (ShieldOrb.damageConsumed > 0)
        {
            float oldArmor = enemy.armor;
            enemy.armor = 0;

            float damage = ShieldOrb.damageConsumed;
            int critLevel = MathHelper.RandomRound(Player.my.upgradeManager.critChance);
            if (critLevel > 0)
                damage *= Player.my.upgradeManager.critDamageMult;
            enemy.Damage(damage, true, Bullet.GetCritColor(critLevel), new Vector2(0, -1.5f));

            enemy.armor = oldArmor;
            ShieldOrb.damageConsumed = 0;
        }
        GetParent().QueueFree();
    }
}
