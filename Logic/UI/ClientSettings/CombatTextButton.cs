using Godot;
using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class CombatTextButton : CheckBox
{
    public override void _Ready()
    {
        ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() => ButtonPressed = UserData.ShowCombatText);
    }
    public override void _Toggled(bool buttonPressed)
    {
        UserData.ShowCombatText = buttonPressed;
    }
}