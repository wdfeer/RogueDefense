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
        public float multishotPerShot = 0;
        public int shots = 0;
        public float CurrentBuff => multishotPerShot * shots;
        public static int MAX_STACK = 60;
        public override void PreShoot(ShootManager shooter)
        {
            shooter.multishot *= 1f + CurrentBuff;

            if (shots < MAX_STACK)
                shots++;
        }
    }
}