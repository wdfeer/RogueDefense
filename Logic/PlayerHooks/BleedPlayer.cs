using RogueDefense.Logic;

namespace RogueDefense
{
    public class BleedPlayer : PlayerHooks
    {
        public override void OnHitWithBullet(Bullet b, float postCritDmg)
        {
            int bleedCount = MathHelper.RandomRound(Player.upgradeManager.bleedChance);
            int duration = 5;
            if (bleedCount > 0)
                Game.instance.enemy.AddBleed(postCritDmg * bleedCount, duration);
        }
    }
}