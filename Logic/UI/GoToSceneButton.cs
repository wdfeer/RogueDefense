using Godot;
using System;

public class GoToSceneButton : Button
{
    [Export]
    public PackedScene goToScene;
    public override void _Pressed()
    {
        GetTree().ChangeSceneTo(goToScene);
    }
}
