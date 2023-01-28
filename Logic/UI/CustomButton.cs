using Godot;
using System;

public class CustomButton : Button
{
    public Action onClick = () => { };
    public override void _Pressed()
    {
        onClick();
    }
}
