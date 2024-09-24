using Godot;

namespace RogueDefense.Logic.UI.MainMenu;

public partial class AugmentsButton : Button
{
    public override void _Pressed()
    {
        var upgradePanel = GetNode<Panel>("/root/Control/AugmentPanel");
        upgradePanel.Visible = !upgradePanel.Visible;

        foreach (AugmentScreen.AugmentContainer upgrader in upgradePanel.GetNode("VBoxContainer").GetChildren())
        {
            upgrader.Save();
        }
        SaveData.Save();
    }
}