using Godot;
using Godot.Collections;

namespace RogueDefense.Logic.Player.Projectile;

public class FusedBullet : Bullet
{
    public FusedBullet(Array<Texture2D> textures) : base(textures)
    {
        texture = textures[3];

        penetration = 8;
        modulate = Colors.HotPink;
    }
    protected override int Radius => base.Radius * 4;
    public override bool KillShieldOrbs => true;


    float time = 0;
    public override void PhysicsProcess(float delta)
    {
        base.PhysicsProcess(delta);

        time += delta;
        modulate = Colors.HotPink.Lerp(Colors.LightBlue, Mathf.Abs(Mathf.Sin(time * 2)));
    }
}
