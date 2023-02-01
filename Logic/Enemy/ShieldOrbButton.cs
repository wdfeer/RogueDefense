using Godot;
using RogueDefense;
using RogueDefense.Logic;
using System;

public class ShieldOrbButton : TextureButton
{
    public override void _Pressed()
    {
        if (ShieldOrb.damageConsumed > 0)
        {
            float oldArmor = Enemy.instance.armor;
            Enemy.instance.armor = 0;

            float damage = ShieldOrb.damageConsumed;
            int critLevel = MathHelper.RandomRound(Player.localInstance.upgradeManager.critChance);
            if (critLevel > 0)
                damage *= Player.localInstance.upgradeManager.critDamageMult;
            Enemy.instance.Damage(damage, true, Bullet.GetCritColor(critLevel).Darkened(0.5f), new Vector2(0, -1.5f));

            Enemy.instance.armor = oldArmor;
            ShieldOrb.damageConsumed = 0;
        }
        GetParent().QueueFree();
    }
}
