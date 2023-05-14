using Godot;
using RogueDefense;
using System;

public class AugmentsButton : TextureButton
{
    public override void _Pressed()
    {
        var upgradePanel = GetNode<Panel>("/root/Control/AugmentPanel");
        upgradePanel.Visible = !upgradePanel.Visible;

        foreach (AugmentContainer upgrader in upgradePanel.GetNode("VBoxContainer").GetChildren())
        {
            upgrader.Save();
        }
        UserSaveData.Save();
    }
}
