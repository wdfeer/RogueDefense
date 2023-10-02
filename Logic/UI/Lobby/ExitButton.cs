using Godot;
using System;

namespace RogueDefense.Logic.UI.Lobby
{
    public partial class ExitButton : TextureButton
    {
        public override void _Pressed()
        {
            NetworkManager.NetStop();
            GetTree().ChangeSceneToFile("Scenes/MainMenu/MainMenu.tscn");
        }
    }
}