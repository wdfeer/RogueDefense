using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Godot;
using Godot.Collections;

namespace RogueDefense.Logic;

public partial class ProjectileManager : Node2D
{
    [Export]
    public Array<Texture2D> textures;

    public List<Projectile> proj = new List<Projectile>();
    public Bullet SpawnBullet(Vector2 gposition)
    {
        Bullet b = new Bullet(textures);
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
    public override void _Process(double delta)
    {
        QueueRedraw();
    }
    public override void _PhysicsProcess(double delta)
    {
        float deltaTime = (float)delta;
        for (int i = 0; i < proj.Count; i++)
            proj[i].PhysicsProcess(deltaTime);

        DeleteQueuedProjectiles();
    }
    public void DeleteQueuedProjectiles()
    {
        proj.RemoveAll(p => p.queuedForDeletion);
    }
    public void ClearProjectiles(Func<Projectile, bool> except)
    {
        proj = proj.Where(except).ToList();
    }
    public void ClearProjectiles()
    {
        proj = new List<Projectile>();
    }
}
