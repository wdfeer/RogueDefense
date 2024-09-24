using Godot;
using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.UI.Lobby.Settings;

public partial class SettingsPanel : Panel
{
    public override void _Ready()
    {
        if (NetworkManager.mode == NetMode.Client)
            ((Panel)GetNode("ShadowingPanel")).Visible = true;
    }
}