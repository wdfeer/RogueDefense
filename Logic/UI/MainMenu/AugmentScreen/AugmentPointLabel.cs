using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.MainMenu.AugmentScreen;

public partial class AugmentPointLabel : Label
{
    public override void _Ready()
    {
        SaveManager.user.updateAugmentPointCounter = UpdateText;
        UpdateText(SaveManager.user.SpareAugmentPoints);
    }
    void UpdateText(int points)
    {
        if (IsInstanceValid(this))
            Text = points.ToString() + " Points";
    }
}