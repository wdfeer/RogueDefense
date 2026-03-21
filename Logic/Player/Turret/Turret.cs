using System;
using System.Collections.Generic;
using System.Linq;
using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Player.Hooks;
using RogueDefense.Logic.Player.Hooks.Upgrades;

namespace RogueDefense.Logic.Player.Turret;

public partial class Turret : CharacterBody2D
{
    public const int SPEED = 9;
    [Export] public AnimationPlayer animationPlayer;
    [Export] public Node2D bulletSpawnpoint;

    // used for interpolating received position
    private Queue<Tuple<ulong, Vector2>> netPositions = new();

    // how far back we render (in ms)
    private const ulong interpolationDelay = 100;

    public void UpdatePositionFromNetwork(Vector2 newPos)
    {
        netPositions.Enqueue(Tuple.Create(Time.GetTicksMsec(), newPos));
    }

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

        if (netPositions.Count < 2)
            return;

        ulong renderTime = Time.GetTicksMsec() - interpolationDelay;

        // Ensure we have at least two points around renderTime
        while (netPositions.Count >= 2 && netPositions.ElementAt(1).Item1 <= renderTime)
        {
            netPositions.Dequeue();
        }

        var first = netPositions.Peek();

        if (netPositions.Count < 2)
        {
            GlobalPosition = first.Item2;
            return;
        }

        var second = netPositions.ElementAt(1);

        float t = (float)(renderTime - first.Item1) / (second.Item1 - first.Item1);
        GlobalPosition = first.Item2.Lerp(second.Item2, Mathf.Clamp(t, 0f, 1f));
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
        get => stunTimer;
        set
        {
            stunTimer = value;
            GetNode<Control>("StunIndicator").Visible = stunTimer > 0;
            GetNode<Label>("StunIndicator/Label").Text = stunTimer.ToString("0.0");
        }
    }

    private float stunTimer;
    public bool Stunned => StunTimer > 0;


    public void Stun(float duration)
    {
        StunTimer += duration / owner.upgradeManager.GetTotalUpgradeMultiplier(UpgradeType.RecoverySpeed);

        PlayerHooks.GetHooks<CritChanceOnStunnedPlayer>(owner).Activate();
    }
}