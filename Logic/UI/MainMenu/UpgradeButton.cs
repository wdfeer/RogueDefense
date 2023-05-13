using Godot;
using System;

public class UpgradeButton : TextureButton
{
    public override void _Pressed()
    {
        var upgradePanel = GetNode<Panel>("/root/Control/UpgradePanel");
        upgradePanel.Visible = !upgradePanel.Visible;
    }
}
