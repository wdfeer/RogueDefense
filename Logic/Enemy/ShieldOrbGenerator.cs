using Godot;
using System;

public partial class ShieldOrbGenerator : Node2D
{
    [Export]
    public PackedScene shieldOrb;
    public override void _Ready()
    {
        if ((Game.Wave % 2 == 0 && GD.Randf() < 0.5f) || Game.Wave % 5 == 0)
            CreateOrbs(1 + Mathf.RoundToInt(GD.Randf() * 4));
        else count = 0;
    }
    public ShieldOrb[] orbs = new ShieldOrb[0];
    public int count = 0;
    void CreateOrbs(int count)
    {
        this.count = count;

        if (orbs.Length > 0)
            foreach (ShieldOrb o in orbs)
            {
                if (IsInstanceValid(o))
                    o.QueueFree();
            }

        Vector2 pos = new Vector2(0, 160);
        float angleStep = Mathf.Pi * 2 / count;
        orbs = new ShieldOrb[count];
        for (int i = 0; i < count; i++)
        {
            ShieldOrb orb = shieldOrb.Instantiate<ShieldOrb>();
            ((ShieldOrbButton)orb.GetNode("Button")).enemy = (Enemy)GetParent();
            AddChild(orb);
            orbs[i] = orb;

            orb.Position = pos;
            pos = pos.Rotated(angleStep);
        }
    }
    public override void _Process(double delta)
    {
        for (int i = 0; i < orbs.Length; i++)
        {
            if (IsInstanceValid(orbs[i]))
                orbs[i].Position = orbs[i].Position.Rotated(Mathf.Pi * 0.4f * (float)delta);
        }
    }
}
