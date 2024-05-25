using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

public partial class ShieldOrbButton : TextureButton
{
    public override void _Pressed()
    {
        DamageEnemies();

        GetParent<ShieldOrb>().TryExplode();

        GetParent().QueueFree();
    }

    void DamageEnemies()
    {
        if (ShieldOrb.damageConsumed > 0)
        {
            for (int i = 0; i < Enemy.enemies.Count; i++)
            {
                Enemy enemy = Enemy.enemies[i];

                if (enemy == null || !IsInstanceValid(enemy) || enemy.Dead)
                    continue;

                float oldArmor = enemy.armor;
                enemy.armor = 0;

                float damage = ShieldOrb.damageConsumed;
                int critLevel = MathHelper.RandomRound(Player.my.upgradeManager.critChance);
                if (critLevel > 0)
                    damage *= Player.my.upgradeManager.critDamageMult;
                enemy.Damage(damage, true, Colors.Black, new Vector2(0, -1.5f));

                enemy.armor = oldArmor;
            }
            ShieldOrb.damageConsumed = 0;
        }
    }
}
