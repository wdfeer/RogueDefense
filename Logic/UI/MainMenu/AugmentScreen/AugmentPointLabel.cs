using Godot;

namespace RogueDefense.Logic.UI.MainMenu.AugmentScreen;

public partial class AugmentPointLabel : Label
{
    public override void _Ready()
    {
        SaveData.updateAugmentPointCounter = UpdateText;
        UpdateText(SaveData.SpareAugmentPoints);
    }
    void UpdateText(int points)
    {
        if (IsInstanceValid(this))
            Text = points.ToString() + " Points";
    }
}