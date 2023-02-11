using Godot;
using RogueDefense;
using System;
using System.Collections.Generic;
using System.Linq;

public struct Upgrade
{
    public UpgradeType type;
    public float value;
    public Upgrade(UpgradeType type, float value)
    {
        this.type = type;
        this.value = value;
    }
    public static Upgrade[] RandomUniqueUpgrades(int count)
    {
        List<UpgradeType> alreadyRolled = new List<UpgradeType>();
        Upgrade[] upgrades = new Upgrade[count];
        for (int i = 0; i < count; i++)
        {
            upgrades[i] = RandomUpgrade(alreadyRolled);
            alreadyRolled.Add(upgrades[i].type);
        }


        if (Player.localInstance.abilityManager.ability1 is UpgradeBuffAbility ability && ability.active)
        {
            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].value *= 1 + UpgradeBuffAbility.UPGRADE_VALUE_INCREASE;
            }
            ability.active = false;
        }

        return upgrades;
    }
    public static Upgrade RandomUpgrade(IEnumerable<UpgradeType> blacklist = null)
    {
        UpgradeType type = UpgradeType.GetRandomType(blacklist);
        float value = type.GetRandomValue();
        value = Mathf.Round(value * 10000f) / 10000f;
        return new Upgrade(type, value);
    }

    public override string ToString()
    {
        return type.getUpgradeText(value);
    }
}