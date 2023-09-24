using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class StatusPlayer : PlayerHooks
    {
        public const float STATUS_DURATION = 5f;

        public float corrosiveChance = 0f;

        public StatusPlayer(Player player) : base(player)
        {
        }

        public override void OnHitWithBullet(Bullet b, float postCritDmg)
        {
            int bleedCount = MathHelper.RandomRound(player.upgradeManager.bleedChance);
            if (bleedCount > 0)
                for (int i = 0; i < bleedCount; i++)
                {
                    Game.instance.enemy.AddBleed(postCritDmg, STATUS_DURATION);
                }

            int viralCount = MathHelper.RandomRound(player.upgradeManager.viralChance);
            if (viralCount > 0)
                for (int i = 0; i < viralCount; i++)
                {
                    Game.instance.enemy.AddViral(STATUS_DURATION);
                }

            int coldCount = MathHelper.RandomRound(player.upgradeManager.coldChance);
            if (coldCount > 0)
                for (int i = 0; i < coldCount; i++)
                {
                    Game.instance.enemy.AddCold(STATUS_DURATION);
                }

            int corrosiveCount = MathHelper.RandomRound(corrosiveChance);
            if (corrosiveCount > 0)
                for (int i = 0; i < corrosiveCount; i++)
                {
                    Game.instance.enemy.corrosive.Add(STATUS_DURATION);
                }
        }
    }
}