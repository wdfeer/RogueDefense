using Godot;
using System;

public class JoinButton : GoToSceneButton
{
    [Export]
    public PackedScene netManagerScene;
    public override void _Pressed()
    {
        NetworkManager.mode = NetMode.Client;
        NetworkManager.connectingAddress = (GetNode("../IP Input/LineEdit") as LineEdit).Text;
        NetworkManager.connectingPort = int.Parse((GetNode("../Port Input/LineEdit") as LineEdit).Text);
        base._Pressed();
    }
}
