using System;

namespace RogueDefense.Logic.Player.Core;

public partial class UpgradeType
{
    public UpgradeType(Func<float, string> upgradeTextGetter)
    {
        getUpgradeText = upgradeTextGetter;
    }
    public Func<float, string> getUpgradeText;
    public bool status;
    public float chanceMult = 1f;
    public float valueMult = 1f;
    public float GetRandomValue()
        => getBaseRandomValue() * valueMult;
    Func<float> getBaseRandomValue = () => GD.Randf() * 0.055f + 0.2f;
    public Func<bool> canBeRolled = () => true;
    public int uniqueId;
    
    public override int GetHashCode() => uniqueId;
    public override bool Equals(object obj)
        => obj is UpgradeType && ((UpgradeType)obj).uniqueId == uniqueId;
}