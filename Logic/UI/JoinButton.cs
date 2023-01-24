using Godot;
using System;

public class JoinButton : GoToSceneButton
{
    [Export]
    public PackedScene netManagerScene;
    public override void _Pressed()
    {
        NetworkManager.mode = NetMode.Client;
        Client.address = (GetNode("../IP Input/LineEdit") as LineEdit).Text;
        Client.port = int.Parse((GetNode("../Port Input/LineEdit") as LineEdit).Text);
        base._Pressed();
    }
}
