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
    public static Upgrade RandomUpgrade(bool based = false)
    {
        UpgradeType type = UpgradeType.GetRandomType();
        float value = type.GetRandomValue();
        value = Mathf.Round(value * 10000f) / 10000f;

        return new Upgrade(type, value);
    }

    public override string ToString()
    {
        return type.getUpgradeText(value);
    }
}