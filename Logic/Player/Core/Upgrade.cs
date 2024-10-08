using System.Collections.Generic;
using RogueDefense.Logic.UI.MainMenu.AugmentScreen;

namespace RogueDefense.Logic.Player.Core;

public struct Upgrade
{
    public UpgradeType type;
    public float baseValue;
    public float Value => baseValue * valueMult;
    public float valueMult;
    public bool risky;
    public Upgrade(UpgradeType type, float baseValue)
    {
        this.type = type;
        this.baseValue = baseValue;
        valueMult = 1f;
        risky = false;
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

        return upgrades;
    }
    public static Upgrade RandomUpgrade(IEnumerable<UpgradeType> blacklist = null)
    {
        UpgradeType type = UpgradeType.GetRandomType(blacklist);
        float value = type.GetRandomValue();
        value = Mathf.Round(value * 10000f) / 10000f;
        if (type.status)
            value *= AugmentContainer.GetStatMult(3);
        return new Upgrade(type, value);
    }

    public override string ToString()
    {
        return type.getUpgradeText(Value);
    }
}