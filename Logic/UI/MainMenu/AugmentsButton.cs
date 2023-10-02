using Godot;
using RogueDefense;
using System;

public partial class AugmentsButton : TextureButton
{
    public override void _Pressed()
    {
        var upgradePanel = GetNode<Panel>("/root/Control/AugmentPanel");
        upgradePanel.Visible = !upgradePanel.Visible;

        foreach (AugmentContainer upgrader in upgradePanel.GetNode("VBoxContainer").GetChildren())
        {
            upgrader.Save();
        }
        SaveData.Save();
    }
}
