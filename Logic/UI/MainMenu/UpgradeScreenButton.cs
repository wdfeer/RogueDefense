using Godot;
using RogueDefense;
using System;

public class UpgradeScreenButton : TextureButton
{
    public override void _Pressed()
    {
        var upgradePanel = GetNode<Panel>("/root/Control/UpgradePanel");
        upgradePanel.Visible = !upgradePanel.Visible;

        foreach (UpgradeContainer upgrader in upgradePanel.GetNode("VBoxContainer").GetChildren())
        {
            upgrader.Save();
        }
        UserSaveData.Save();
    }
}
