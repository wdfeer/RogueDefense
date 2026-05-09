using RogueDefense.Logic.Save;
using RogueDefense.Logic.UI.MainMenu.AugmentScreen;

namespace RogueDefense.Logic.UI.MainMenu;

public partial class AugmentsButton : Button
{
    public override void _Pressed()
    {
        var upgradePanel = GetNode<Panel>("/root/Control/AugmentPanel");
        upgradePanel.Visible = !upgradePanel.Visible;

        foreach (var node in upgradePanel.GetNode("VBoxContainer").GetChildren())
        {
            var upgrader = (AugmentContainer)node;
            upgrader.Save();
        }

        SaveManager.Save();
    }
}