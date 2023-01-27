using Godot;
using RogueDefense;
using System;

public class UsernameInput : LineEdit
{
    public override void _Ready()
    {
        Connect("text_changed", this, "OnTextChanged");


        int RandomDigit() { return new Random().Next() % 10; }

        string randomName = $"Player{RandomDigit()}{RandomDigit()}{RandomDigit()}";
        OnTextChanged(randomName);
        this.Text = randomName;
    }

    public void OnTextChanged(string newText)
    {
        Player.myName = newText;
    }
}
