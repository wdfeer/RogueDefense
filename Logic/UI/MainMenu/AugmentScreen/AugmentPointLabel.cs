using Godot;
using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.MainMenu.AugmentScreen;

public partial class AugmentPointLabel : Label
{
    public override void _Ready()
    {
        UserData.updateAugmentPointCounter = UpdateText;
        UpdateText(UserData.SpareAugmentPoints);
    }
    void UpdateText(int points)
    {
        if (IsInstanceValid(this))
            Text = points.ToString() + " Points";
    }
}