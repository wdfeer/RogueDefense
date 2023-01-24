using Godot;
using RogueDefense;
using System;

public class MPButton : GoToSceneButton
{
    [Export]
    public bool host;

    public override void _Pressed()
    {
        NetworkManager.mode = host ? NetMode.Server : NetMode.Client;
        base._Pressed();
    }
}
