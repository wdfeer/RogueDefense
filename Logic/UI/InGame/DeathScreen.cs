using Godot;

public partial class DeathScreen : Panel
{
    public static DeathScreen instance;
    public override void _Ready()
    {
        instance = this;
    }
}
