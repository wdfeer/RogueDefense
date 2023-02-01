using Godot;
using RogueDefense;
using System;

public class UsernameInput : LineEdit
{
    public override void _Ready()
    {
        Connect("text_changed", this, "OnTextChanged");
        ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() =>
        {
            if (RogueDefense.UserData.name == "default")
                GenerateRandomName();
            else
                Text = RogueDefense.UserData.name;
        });
    }
    void GenerateRandomName()
    {
        int RandomDigit() { return new Random().Next() % 10; }

        string randomName = $"Player{RandomDigit()}{RandomDigit()}{RandomDigit()}";
        OnTextChanged(randomName);
        Text = randomName;
    }

    public void OnTextChanged(string newText)
    {
        RogueDefense.UserData.name = newText;
    }
}
