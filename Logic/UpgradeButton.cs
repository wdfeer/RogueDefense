using Godot;
using System;

public class UpgradeButton : Button
{
	public Action onClick = () => { };
	public override void _Pressed()
	{
		onClick();
	}
}
