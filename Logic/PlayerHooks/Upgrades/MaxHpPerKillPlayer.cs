using RogueDefense.Logic;

namespace RogueDefense
{
    public class MaxHpPerKillPlayer : PlayerHooks
    {
        public float increase = 0f; // Used in UpgradeManager
        public override void OnKill()
        {
            increase += Player.upgradeManager.SumAllUpgradeValues(UpgradeType.MaxHpPerKill);
        }
    }
}