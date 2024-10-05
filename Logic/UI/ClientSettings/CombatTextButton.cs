using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class CombatTextButton : CheckBox
{
    public override void _Ready()
    {
        ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() => ButtonPressed = UserData.clientSettings.ShowCombatText);
    }
    public override void _Toggled(bool buttonPressed)
    {
        UserData.clientSettings.ShowCombatText = buttonPressed;
    }
}