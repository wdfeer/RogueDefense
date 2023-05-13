using Godot;
using RogueDefense;
using System;

public class PlusMinusButton : Button
{
    [Export]
    public int effect = 1;
    public override void _Pressed()
    {
        GetParent<UpgradeContainer>().ChangeStat(effect);
    }

    public override void _Process(float delta)
    {
        Disabled = effect > UserSaveData.SpareUpgradePoints ||
            (GetParent<UpgradeContainer>().points == UpgradeContainer.MIN_POINTS && effect < 0);
    }
}
