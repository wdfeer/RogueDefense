using Godot;

public partial class GoToSceneButton : Button
{
    [Export]
    public PackedScene goToScene;
    public override void _Pressed()
    {
        GetTree().ChangeSceneToPacked(goToScene);
    }
}
