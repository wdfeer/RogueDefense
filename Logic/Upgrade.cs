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
        float value = based ? 1f : (GD.Randf() * 0.05f + 0.2f);
        return new Upgrade(GetRandomUpgradeType(based), value);
    }


    public override string ToString()
    {
        return $"+{Mathf.RoundToInt(value * 100f)}% {type}";
    }
    public enum UpgradeType
    {
        MaxHp,
        DamageDivision,
        Damage,
        FireRate,
        Multishot,
        CritChance,
        CritDamage,

        BaseDamage = 501,
        BaseFireRate = 502,
        BaseMultishot = 503,
        BaseCritMultiplier = 504
    }
}