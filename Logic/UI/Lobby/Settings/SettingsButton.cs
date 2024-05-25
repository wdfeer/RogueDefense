using Godot;

public partial class SettingsButton : Button
{
    public static Panel panel;
    public override void _Ready()
    {
        panel = GetNode("../SettingsPanel") as Panel;
    }
    public override void _Pressed()
    {
        panel.Visible = !panel.Visible;
    }
}
