using Godot;
using System;

public struct Upgrade
{
	public UpgradeType type;
	public float value;
    public Upgrade(UpgradeType type, float value)
    {
        this.type = type;
        this.value = value;
    }
    public static Upgrade RandomUpgrade()
    {
        UpgradeType GetRandomUpgradeType()
        {
            Array values = Enum.GetValues(typeof(UpgradeType));
            return (UpgradeType)values.GetValue((int)(GD.Randf() * values.Length));
        }
        return new Upgrade(GetRandomUpgradeType(), GD.Randf() * 0.05f + 0.2f);
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
        Multishot
    }
}