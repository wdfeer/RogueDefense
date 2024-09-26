namespace RogueDefense.Logic.Player.Hooks.Upgrades;

public class StatusPlayer : PlayerHooks
{
    public const float STATUS_DURATION = 5f;

    public float corrosiveChance = 0f;

    public StatusPlayer(Core.Player player) : base(player)
    {
    }

    public override void OnHitWithProj(Enemy.Enemy enemy, Projectile.Projectile p, float postCritDmg)
    {
        int bleedCount = MathHelper.RandomRound(player.upgradeManager.bleedChance);
        if (bleedCount > 0)
            for (int i = 0; i < bleedCount; i++)
            {
                enemy.AddBleed(postCritDmg, STATUS_DURATION);
            }

        int viralCount = MathHelper.RandomRound(player.upgradeManager.viralChance);
        if (viralCount > 0)
            for (int i = 0; i < viralCount; i++)
            {
                enemy.AddViral(STATUS_DURATION);
            }

        int coldCount = MathHelper.RandomRound(player.upgradeManager.coldChance);
        if (coldCount > 0)
            for (int i = 0; i < coldCount; i++)
            {
                enemy.AddCold(STATUS_DURATION);
            }

        int corrosiveCount = MathHelper.RandomRound(corrosiveChance);
        if (corrosiveCount > 0)
            for (int i = 0; i < corrosiveCount; i++)
            {
                enemy.corrosive.Add(STATUS_DURATION);
            }
    }
}