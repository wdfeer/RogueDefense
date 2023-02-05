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
        List<UpgradeType> rolled = new List<UpgradeType>();
        Upgrade[] upgrades = new Upgrade[count];
        for (int i = 0; i < count; i++)
        {
            upgrades[i] = RandomUpgrade(rolled);
            rolled.Add(upgrades[i].type);
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