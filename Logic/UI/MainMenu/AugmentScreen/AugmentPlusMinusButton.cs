using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.MainMenu.AugmentScreen;

public partial class AugmentPlusMinusButton : Button
{
    [Export]
    public int effect = 1;
    public override void _Pressed()
    {
        GetParent<AugmentContainer>().ChangeStat(effect);
    }

    public override void _Process(double delta)
    {
        Disabled = effect > UserData.SpareAugmentPoints ||
                   (GetParent<AugmentContainer>().points == AugmentContainer.MIN_POINTS && effect < 0);
    }
}