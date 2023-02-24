using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class MultishotPerShotPlayer : PlayerHooks
    {
        public float multishotPerShot = 0;
        public float CurrentBuff => multishotPerShot * (Player.shootManager.shootCount > MAX_STACK ? MAX_STACK : Player.shootManager.shootCount);
        public static int MAX_STACK = 60;
        public override void PreShoot(ShootManager shooter)
        {
            if (multishotPerShot <= 0) return;
            shooter.multishot *= 1f + CurrentBuff;
        }
    }
}