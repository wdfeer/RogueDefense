using System;
using System.Linq;
using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.MainMenu;

public partial class UsernameInput : LineEdit
{
    public override void _Ready()
    {
        Connect("text_changed", new Callable(this, "OnTextChanged"));
        ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() =>
        {
            if (UserData.name == "")
                GenerateRandomName();
            else
                Text = UserData.name;
        });
    }
    void GenerateRandomName()
    {
        int RandomDigit() { return new Random().Next() % 10; }

        string randomName = $"Player{RandomDigit()}{RandomDigit()}{RandomDigit()}";
        OnTextChanged(randomName);
        Text = randomName;
    }
    public static readonly char[] ALLOWED_CHARACTERS = Enumerable.Range('\x1', 127).Select(x => (char)x).ToArray();
    public void OnTextChanged(string newText)
    {
        newText = string.Concat(newText.Where(x => ALLOWED_CHARACTERS.Contains(x))).Replace(' ', '_');
        if (newText.Length > 0)
            UserData.name = newText;
    }
}