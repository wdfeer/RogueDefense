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
            ActiveAbility ability1 = GetRandomAbility(ability1Button);
            player.hooks.Add(ability1);

            TimerManager.AddTimer(ResetAbilityText, 0.01f);
        }
        ActiveAbility GetRandomAbility(CustomButton button)
        {
            switch (new Random().Next(0, 4))
            {
                case 0:
                    return new FireRateAbility(button);
                case 1:
                    return new DamageAbility(button);
                case 2:
                    return new ShurikenAbility(button);
                default:
                    return new ArmorStripAbility(button);

            }
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
