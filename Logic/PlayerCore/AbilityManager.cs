using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueDefense.Logic.PlayerCore
{
    public partial class AbilityManager
    {
        public ActiveAbility ability1;
        Player player;

        public float strengthMult = 1f;
        public float durationMult = 1f;
        public float cooldownMult = 1f;
        public AbilityManager(Player player)
        {
            this.player = player;
            if (player.Local)
            {
                var ability1Button = Game.instance.GetNode("AbilityButton") as Button;
                ability1 = GetAbility(ability1Button);


                Game.instance.ToSignal(Game.instance.GetTree().CreateTimer(0.01f), "timeout").OnCompleted(ResetAbilityText);
            }
            else
            {
                ability1 = CreateAbilityInstance(Client.instance.GetUserData(player.id).ability, player);
            }
            player.hooks.Add(ability1);
            int ability1Index = ability1.GetAbilityIndex();
            for (int i = 0; i < abilityTypes.Length; i++)
            {
                if (i == ability1Index)
                    continue;

                player.hooks.Add(CreateAbilityInstance(i, player));
            }
        }

        ActiveAbility GetAbility(Button button)
        {
            return CreateAbilityInstance(AbilityChooser.chosen, player, button);
        }
        public static string GetAbilityName(int index)
        {
            if (index == -1) return "Random";
            return abilityTypes[index].ToString().Split(".").Last();
        }
        public static readonly Type[] abilityTypes = new Type[] {
            typeof(DamageAbility),
            typeof(FireRateAbility),
            typeof(ShurikenAbility),
            typeof(ViralChanceAbility),
            typeof(FuseBulletsAbility),
            typeof(ArmorStripAbility),
            typeof(UpgradeBuffAbility),
            typeof(CritChanceAbility),
            typeof(DmgDealtDmgTakenAbility),
            typeof(DamageReductionAbility)
        };
        public static ActiveAbility CreateAbilityInstance(int index, Player player, Button button = null)
        {
            if (index < 0) index = new Random().Next(0, abilityTypes.Length);
            return (ActiveAbility)Activator.CreateInstance(abilityTypes[index], new object[] { player, button });
        }
        public void ResetAbilityText()
        {
            ability1.ResetText();
        }
    }
}
