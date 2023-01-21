using Godot;
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
        UpgradeType GetRandomUpgradeType(bool b)
        {
            int[] values = Enum.GetValues(typeof(UpgradeType)) as int[];
            values = (b ? values.Where(x => x > 500) : values.Where(x => x < 500)).ToArray();
            return (UpgradeType)values[(int)(GD.Randf() * values.Length)];
        }
        UpgradeType type = GetRandomUpgradeType(based);
        float value = based ? 1f : (GD.Randf() * 0.05f + 0.2f);
        value *= GetUpgradeValueMultiplier(type);

        return new Upgrade(type, value);
    }
    static float GetUpgradeValueMultiplier(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.DamageReduction:
                return 0.5f;
            case UpgradeType.AbilityStrength:
                return 2f;
        }
        return 1f;
    }

    public override string ToString()
    {
        return $"+{Mathf.RoundToInt(value * 100f)}% {type}";
    }
    public enum UpgradeType
    {
        MaxHp,
        DamageReduction,
        Damage,
        FireRate,
        Multishot,
        CritChance,
        CritDamage,

        AbilityStrength,

        BaseDamage = 501,
        BaseFireRate = 502,
        BaseMultishot = 503,
        BaseCritMultiplier = 504
    }
}