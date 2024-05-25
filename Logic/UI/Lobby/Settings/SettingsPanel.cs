using Godot;

public partial class SettingsPanel : Panel
{
    public override void _Ready()
    {
        if (NetworkManager.mode == NetMode.Client)
            ((Panel)GetNode("ShadowingPanel")).Visible = true;
    }
}
