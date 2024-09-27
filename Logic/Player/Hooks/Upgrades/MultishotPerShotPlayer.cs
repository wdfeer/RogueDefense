using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.Player.Hooks.Upgrades;

public class MultishotPerShotPlayer : PlayerHooks
{
    public float MultishotPerShot => player.upgradeManager.SumAllUpgradeValues(UpgradeType.MultishotPerShot);
    public float CurrentBuff => MultishotPerShot * Mathf.Min((int)player.shootManager.shootCount, MAX_STACK);
    public const int MAX_STACK = 60;

    public MultishotPerShotPlayer(Core.Player player) : base(player)
    {
        Game.instance.GetNode<UI.InGame.BuffText>("BuffText").textGetters.Add(GetBuffText);
    }
    string GetBuffText()
    {
        if (CurrentBuff > 0)
            return $"+{MathHelper.ToPercentAndRound(CurrentBuff)}% Multishot\n";
        return "";
    }

    public override void PreShoot(ShootManager shooter)
    {
        if (MultishotPerShot <= 0) return;
        shooter.multishot *= 1f + CurrentBuff;
    }
}