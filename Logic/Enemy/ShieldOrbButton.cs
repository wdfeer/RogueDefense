using Godot;
using System;

public class ShieldOrbButton : TextureButton
{
    public override void _Pressed()
    {
        GetParent().QueueFree();
    }
}
