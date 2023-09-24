using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class DmgDealtDmgTakenAbility : ActiveAbility
    {
        public DmgDealtDmgTakenAbility(Player player, CustomButton button) : base(player, button)
        {
        }

        public override void Activate()
        {
            buffLeft = Duration * 5f;
        }
        public float buffLeft = 0;
        public override void PostUpgradeUpdate(float delta)
        {
            if (buffLeft > 0 && player.Local)
            {
                buffLeft -= delta;
                DefenseObjective.instance.damageMult *= 1f + DamageTaken;
            }
        }
        public override void PostShoot(Bullet bullet)
        {
            if (buffLeft > 0)
            {
                bullet.damage *= 1f + 2f * Strength;
                bullet.StartParticleEffect();
                bullet.ParticleEmitter.Modulate = new Color(1f, 0f, 1f);
            }
        }
        public float DamageTaken => 0.5f * Strength;
        protected override string GetAbilityText()
            => $@"+{(int)(200f * Strength)}% Total Damage but
+{MathHelper.ToPercentAndRound(DamageTaken)}% Damage Taken
Duration: {(5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}