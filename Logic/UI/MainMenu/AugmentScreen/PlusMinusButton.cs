using Godot;
using RogueDefense;
using System;

public class PlusMinusButton : Button
{
    [Export]
    public int effect = 1;
    public override void _Pressed()
    {
        GetParent<AugmentContainer>().ChangeStat(effect);
    }

    public override void _Process(float delta)
    {
        Disabled = effect > SaveData.SpareAugmentPoints ||
            (GetParent<AugmentContainer>().points == AugmentContainer.MIN_POINTS && effect < 0);
    }
}
