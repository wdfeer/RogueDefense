using Godot;
using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Player.Hooks;
using RogueDefense.Logic.Player.Hooks.Upgrades;

namespace RogueDefense.Logic.Player.Turret;

public partial class Turret : CharacterBody2D
{
	public const int SPEED = 9;
	[Export]
	public AnimationPlayer animationPlayer;
	[Export]
	public Node2D bulletSpawnpoint;
	public override void _Process(double delta)
	{
		if (StunTimer > 0)
		{
			StunTimer -= (float)delta;
			return;
		}

		if (target != null && IsInstanceValid(target))
		{
			(GetNode("TurretSprite") as Sprite2D).LookAt(target.GlobalPosition);
		}
	}
	public Enemy.Enemy target;
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


	public Core.Player owner;
	private float StunTimer
	{
		get => stunTimer; set
		{
			stunTimer = value;
			GetNode<Control>("StunIndicator").Visible = stunTimer > 0;
			GetNode<Label>("StunIndicator/Label").Text = stunTimer.ToString("0.0");
		}
	}
	private float stunTimer = 0;
	public bool Stunned => StunTimer > 0;


	public void Stun(float duration)
	{
		StunTimer += duration / owner.upgradeManager.GetTotalUpgradeMultiplier(UpgradeType.RecoverySpeed);

		PlayerHooks.GetHooks<CritChanceOnStunnedPlayer>(owner).Activate();
	}
}
