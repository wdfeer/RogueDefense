using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;
using System.Linq;

namespace RogueDefense
{
    public partial class MaxHpPerKillPlayer : PlayerHooks
    {
        public float increase = 0f; // Used in UpgradeManager

        public MaxHpPerKillPlayer(Player player) : base(player)
        {
        }

        public static float GetTotalIncrease()
            => Player.players
                .Select(pair => PlayerHooks.GetHooks<MaxHpPerKillPlayer>(pair.Value).increase)
                .Aggregate(0f, (a, b) => a + b);
        public override void OnKill()
        {
            increase += player.upgradeManager.SumAllUpgradeValues(UpgradeType.MaxHpPerKill);
        }
    }
}