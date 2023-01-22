using Godot;
using RogueDefense;
using System;

public class UsernameInput : LineEdit
{
    public override void _Ready()
    {
        Connect("text_changed", this, "OnTextChanged");
    }

    public void OnTextChanged(string newText)
    {
        Player.myName = newText;
    }
}
