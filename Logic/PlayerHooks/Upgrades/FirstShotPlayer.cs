using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class FirstShotPlayer : PlayerHooks
    {
        public float damageMult = 1f;
        bool affectingThisShot = false;
        public override void PreShoot(ShootManager shooter)
        {
            if (damageMult <= 1f || shooter.shootCount > 0)
            {
                affectingThisShot = false;
                return;
            }

            shooter.damage *= damageMult;
            affectingThisShot = true;
        }
        public override void PostShoot(Bullet bullet)
        {
            if (affectingThisShot)
            {
                bullet.ParticleEmitter.Modulate = Colors.White;
                bullet.StartParticleEffect();
            }
        }
    }
}