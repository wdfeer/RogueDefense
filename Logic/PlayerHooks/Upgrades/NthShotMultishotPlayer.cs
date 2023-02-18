using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class NthShotMultishotPlayer : PlayerHooks
    {
        int shots = 0;
        public override void PreShoot(ShootManager shooter)
        {
            if (shots >= 4)
            {
                float mult = Player.upgradeManager.GetTotalUpgradeMultiplier(UpgradeType.NthShotMultishot);
                shooter.multishot *= mult;

                shots = 0;
            }
            shots++;
        }
    }
}