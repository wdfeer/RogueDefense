using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class MultishotPerShotPlayer : PlayerHooks
    {
        public float multishotPerShot = 0;
        public float CurrentBuff => multishotPerShot * (player.shootManager.shootCount > MAX_STACK ? MAX_STACK : player.shootManager.shootCount);
        public static int MAX_STACK = 60;

        public MultishotPerShotPlayer(Player player) : base(player)
        {
        }

        public override void PreShoot(ShootManager shooter)
        {
            if (multishotPerShot <= 0) return;
            shooter.multishot *= 1f + CurrentBuff;
        }
    }
}