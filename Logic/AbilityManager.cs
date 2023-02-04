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
            ActiveAbility ability1 = GetAbility(ability1Button);
            player.hooks.Add(ability1);

            TimerManager.AddTimer(ResetAbilityText, 0.01f);
        }

        ActiveAbility GetAbility(CustomButton button)
        {
            if (AbilityChooser.chosen == -1)
                return GetRandomAbility(button);
            else return CreateAbilityInstance(AbilityChooser.chosen, button);
        }
        ActiveAbility GetRandomAbility(CustomButton button)
        {
            int index = new Random().Next(0, abilityTypes.Length);
            return CreateAbilityInstance(index, button);
        }
        public static readonly Type[] abilityTypes = new Type[] {
            typeof(DamageAbility),
            typeof(FireRateAbility),
            typeof(ShurikenAbility),
            typeof(ViralChanceAbility),
            typeof(FuseBulletsAbility),
            typeof(ArmorStripAbility),
        };
        ActiveAbility CreateAbilityInstance(int index, CustomButton button)
            => (ActiveAbility)Activator.CreateInstance(abilityTypes[index], new object[] { button });
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
