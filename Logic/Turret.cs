using Godot;
using System;

public partial class Turret : CharacterBody2D
{
	public const int SPEED = 9;
	[Export]
	public AnimationPlayer animationPlayer;
	[Export]
	public Node2D bulletSpawnpoint;
	public override void _Process(double delta)
	{
		if (target != null && IsInstanceValid(target))
		{
			(GetNode("TurretSprite") as Sprite2D).LookAt(target.GlobalPosition);
		}
	}
	public Enemy target;
	public void SetLabel(string text)
	{
		Label label = (Label)GetNode("Label");
		label.Text = text;
	}

	GpuParticles2D Particles => GetNode<GpuParticles2D>("GPUParticles2D");
	public void EnableParticles(float duration)
	{
		Particles.Emitting = true;
		ToSignal(GetTree().CreateTimer(duration, false), "timeout").OnCompleted(() => Particles.Emitting = false);
	}
}
