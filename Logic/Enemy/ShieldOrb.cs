using Godot;
using RogueDefense;
using System;

public partial class ShieldOrb : Area2D
{
	public static float damageConsumed = 0f;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}
	public void OnBodyEntered(Node body)
	{
		if (body is Bullet bullet)
		{
			bullet.ShieldOrbCollision(this);
		}
		else if (body is DefenseObjective defObjective && !tappable)
		{
			defObjective.Damage(10f * Game.instance.GetCurrentStage());
		}
	}

	bool tappable = true;
	public void SetTappability(bool tappable)
	{
		this.tappable = tappable;

		((Label)GetNode("Label")).Visible = tappable;

		ShieldOrbButton button = (ShieldOrbButton)GetNode("Button");
		button.Disabled = !tappable;

		button.Modulate = tappable ? Colors.White : Colors.Red;
	}
}
