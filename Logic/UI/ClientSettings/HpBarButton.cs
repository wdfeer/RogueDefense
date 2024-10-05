using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class HpBarButton : CheckBox
{
    public override void _Ready()
    {
        ToSignal(GetTree().CreateTimer(0.001f), "timeout")
            .OnCompleted(() => ButtonPressed = UserData.clientSettings.ShowHpBar);
    }
    public override void _Toggled(bool buttonPressed)
    {
        UserData.clientSettings.ShowHpBar = buttonPressed;
    }
}