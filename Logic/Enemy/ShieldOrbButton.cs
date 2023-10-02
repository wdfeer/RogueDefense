using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;
using System;

public partial class ShieldOrbButton : TextureButton
{
    public override void _Pressed()
    {
        if (ShieldOrb.damageConsumed > 0)
        {
            float oldArmor = Enemy.instance.armor;
            Enemy.instance.armor = 0;

            float damage = ShieldOrb.damageConsumed;
            int critLevel = MathHelper.RandomRound(Player.my.upgradeManager.critChance);
            if (critLevel > 0)
                damage *= Player.my.upgradeManager.critDamageMult;
            Enemy.instance.Damage(damage, true, Bullet.GetCritColor(critLevel), new Vector2(0, -1.5f));

            Enemy.instance.armor = oldArmor;
            ShieldOrb.damageConsumed = 0;
        }
        GetParent().QueueFree();
    }
}
