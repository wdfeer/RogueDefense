using Godot;
using RogueDefense;
using System;
using System.Linq;

public class UsernameInput : LineEdit
{
    public override void _Ready()
    {
        Connect("text_changed", this, "OnTextChanged");
        ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() =>
        {
            if (RogueDefense.SaveData.name == "")
                GenerateRandomName();
            else
                Text = RogueDefense.SaveData.name;
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
        newText = String.Concat(newText.Where(x => ALLOWED_CHARACTERS.Contains(x))).Replace(' ', '_');
        if (newText.Length > 0)
            RogueDefense.SaveData.name = newText;
    }
}
