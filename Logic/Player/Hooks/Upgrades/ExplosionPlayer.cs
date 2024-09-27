using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Player.Projectile;

namespace RogueDefense.Logic.Player.Hooks.Upgrades;

public class ExplosionPlayer : PlayerHooks
{
    public ExplosionPlayer(Core.Player player) : base(player)
    {
    }

    public float Chance => player.upgradeManager.SumAllUpgradeValues(UpgradeType.ExplosionChance);
    const int BASE_RADIUS = 100;
    public float RadiusMult => player.upgradeManager.GetTotalUpgradeMultiplier(UpgradeType.ExplosionRadius);
    public override void OnHitWithProj(Enemy.Enemy enemy, Projectile.Projectile p, float postCritDmg)
    {
        if (p is Explosion || GD.Randf() > Chance)
            return;

        Explosion explosion = new Explosion(player.shootManager.projectileManager.textures)
        {
            owner = player,
            position = p.position,
            damage = p.damage,
            radius = BASE_RADIUS * RadiusMult
        };
        player.shootManager.projectileManager.projDeffered.Add(explosion);
    }
}