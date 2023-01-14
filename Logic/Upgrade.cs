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
        return new Upgrade(GetRandomUpgradeType(), GD.Randf() * 0.1f + 0.1f);
    }


    public override string ToString()
    {
        return $"+{(int)(value * 100f)}% {type}";
    }

    public enum UpgradeType
    {
        MaxHp,
        Damage,
        FireRate,
        Multishot
    }
}