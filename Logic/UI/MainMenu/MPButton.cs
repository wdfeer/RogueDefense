using Godot;
using RogueDefense;
using System;

public class MPButton : GoToSceneButton
{
    [Export]
    public PackedScene networkManager;
    [Export]
    public bool host;

    public override void _Pressed()
    {
        base._Pressed();
        TimerManager.AddTimer(() =>
        {
            NetworkManager net = networkManager.Instance() as NetworkManager;
            Game.instance.AddChild(net);
            net.InitializeNetMode(host ? NetMode.Server : NetMode.Client);
        }, 0.01f);
    }
}
