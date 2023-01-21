using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueDefense
{
    public class AbilityManager
    {
        Player player;

        public float strengthMult = 1f;
        public float durationMult = 1f;
        public float cooldownMult = 1f;
        public AbilityManager(Player player)
        {
            this.player = player;
            var ability1Button = player.GetNode("/root/Game/AbilityContainer/AbilityButton1") as CustomButton;
            ActiveAbility ability1;
            if (GD.Randf() > 0.5f)
                ability1 = new FireRateAbility(ability1Button);
            else
                ability1 = new DamageAbility(ability1Button);
            player.hooks.Add(ability1);

            TimerManager.AddTimer(ResetAbilityText, 0.01f);
        }
        public void ResetAbilityText()
        {
            player.hooks.ForEach(x =>
            {
                if (x is ActiveAbility)
                    (x as ActiveAbility).ResetText();
            });
        }
    }
}
