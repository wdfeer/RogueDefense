using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class ExplosionPlayer : PlayerHooks
{
    public ExplosionPlayer(Player player) : base(player)
    {
    }

    public float chance = 0;
    const int BASE_RADIUS = 100;
    public float radiusMult = 1;
    public override void OnHitWithProj(Enemy enemy, Projectile p, float postCritDmg)
    {
        if (p is Explosion || GD.Randf() > chance)
            return;

        Explosion explosion = new Explosion(player.shootManager.projectileManager.textures)
        {
            owner = player,
            position = p.position,
            damage = p.damage,
            radius = BASE_RADIUS * radiusMult
        };
        player.shootManager.projectileManager.projDeffered.Add(explosion);
    }
}