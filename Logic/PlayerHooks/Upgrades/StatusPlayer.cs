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
                for (int i = 0; i < bleedCount; i++)
                {
                    Game.instance.enemy.AddBleed(postCritDmg, STATUS_DURATION);
                }

            int viralCount = MathHelper.RandomRound(Player.upgradeManager.viralChance);
            if (viralCount > 0)
                for (int i = 0; i < viralCount; i++)
                {
                    Game.instance.enemy.AddViral(STATUS_DURATION);
                }

            int coldCount = MathHelper.RandomRound(Player.upgradeManager.coldChance);
            if (coldCount > 0)
                for (int i = 0; i < coldCount; i++)
                {
                    Game.instance.enemy.AddCold(STATUS_DURATION);
                }
        }
    }
}