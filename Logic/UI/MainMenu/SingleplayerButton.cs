using Godot;
using System;

public class SingleplayerButton : Button
{
    [Export]
    public PackedScene singleplayerScene;
    public override void _Pressed()
    {
        GetTree().ChangeSceneTo(singleplayerScene);
    }
}
