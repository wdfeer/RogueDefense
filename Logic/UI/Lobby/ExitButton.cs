using Godot;
using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.UI.Lobby;

public partial class ExitButton : Button
{
    public override void _Pressed()
    {
        NetworkManager.NetStop();
        GetTree().ChangeSceneToFile("Scenes/MainMenu/MainMenu.tscn");
    }
}