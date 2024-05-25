using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

namespace RogueDefense.Logic;

public partial class Projectiles : Node2D
{
    public List<Projectile> proj = new List<Projectile>();
    public Bullet SpawnBullet(Vector2 gposition)
    {
        Bullet b = new Bullet();
        proj.Add(b);
        b.position = gposition;
        return b;
    }
    public override void _Draw()
    {
        int count = proj.Count;
        for (int i = 0; i < count; i++)
        {
            proj[i].Draw(this);
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        int count = proj.Count;
        float deltaTime = (float)delta;
        for (int i = 0; i < count; i++)
        {
            proj[i].PhysicsProcess(deltaTime);
        }

        DeleteQueuedProjectiles();
    }
    public void DeleteQueuedProjectiles()
    {
        proj.RemoveAll(p => p.queuedForDeletion);
    }
    public void ClearProjectiles()
    {
        proj = new List<Projectile>();
    }
}
