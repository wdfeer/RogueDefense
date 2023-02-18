using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class MultishotPerShotPlayer : PlayerHooks
    {
        public override void OnKill()
        {
            shots = 0;
        }
        int shots = 0;
        public static int MAX_STACK = 50;
        public override void PreShoot(ShootManager shooter)
        {
            float increase = Player.upgradeManager.SumAllUpgradeValues(UpgradeType.MultishotPerShot);
            shooter.multishot += increase * shots;

            if (shots < MAX_STACK)
                shots++;
        }
    }
}