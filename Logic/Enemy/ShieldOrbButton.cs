using Godot;
using System;

public class ShieldOrbButton : TextureButton
{
    public override void _Pressed()
    {
        if (ShieldOrb.damageConsumed > 0)
        {
            float oldArmor = Enemy.instance.armor;
            Enemy.instance.armor = 0;
            Enemy.instance.Damage(ShieldOrb.damageConsumed, Colors.Black, new Vector2(0, -1.5f));
            Enemy.instance.armor = oldArmor;
            ShieldOrb.damageConsumed = 0;
        }
        GetParent().QueueFree();
    }
}
