using Godot;
using System;

public partial class CustomButton : Button
{
	public Action onClick = () => { };
	public override void _Pressed()
	{
		onClick();
	}
}
