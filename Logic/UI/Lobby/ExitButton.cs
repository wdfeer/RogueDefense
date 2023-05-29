using Godot;
using System;

namespace RogueDefense.Logic.UI.Lobby
{
    public class ExitButton : TextureButton
    {
        public override void _Pressed()
        {
            NetworkManager.NetStop();
            GetTree().ChangeScene("Scenes/MainMenu/MainMenu.tscn");
        }
    }
}