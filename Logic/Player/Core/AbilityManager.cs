using System;
using System.Linq;
using RogueDefense.Logic.Network;
using RogueDefense.Logic.Player.Hooks;
using RogueDefense.Logic.Player.Hooks.ActiveAbilities;
using RogueDefense.Logic.UI.MainMenu;

namespace RogueDefense.Logic.Player.Core;

public class AbilityManager
{
    public ActiveAbility ability1;
    Player player;

    public float strengthMult = 1f;
    public float durationMult = 1f;
    public float cooldownMult = 1f;
    public AbilityManager(Player player)
    {
        this.player = player;
        if (player.IsLocal)
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
    public static readonly Type[] abilityTypes = {
        typeof(DamageAbility),
        typeof(FireRateAbility),
        typeof(ShurikenAbility),
        typeof(ViralChanceAbility),
        typeof(FuseBulletsAbility),
        typeof(ArmorStripAbility),
        typeof(UpgradeBuffAbility),
        typeof(CritChanceAbility),
        typeof(DamageDealtTakenAbility),
        typeof(DamageReductionAbility)
    };
    public static ActiveAbility CreateAbilityInstance(int index, Player player, Button button = null)
    {
        if (index < 0) index = new Random().Next(0, abilityTypes.Length);
        return (ActiveAbility)Activator.CreateInstance(abilityTypes[index], player, button);
    }
    public void ResetAbilityText()
    {
        ability1.ResetText();
    }
}
