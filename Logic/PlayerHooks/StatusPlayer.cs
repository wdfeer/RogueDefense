using RogueDefense.Logic;

namespace RogueDefense
{
    public class StatusPlayer : PlayerHooks
    {
        public const float STATUS_DURATION = 5f;
        public override void OnHitWithBullet(Bullet b, float postCritDmg)
        {
            int bleedCount = MathHelper.RandomRound(Player.upgradeManager.bleedChance);
            if (bleedCount > 0)
                Game.instance.enemy.AddBleed(postCritDmg * bleedCount, STATUS_DURATION);

            int viralCount = MathHelper.RandomRound(Player.upgradeManager.viralChance);
            if (viralCount > 0)
                Game.instance.enemy.AddViral(STATUS_DURATION);
        }
    }
}