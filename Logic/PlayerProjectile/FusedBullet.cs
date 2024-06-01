using Godot;
using Godot.Collections;

namespace RogueDefense.Logic.PlayerProjectile;

public class FusedBullet : Bullet
{
    public FusedBullet(Array<Texture2D> textures) : base(textures)
    {
        texture = textures[3];

        penetration = 8;
    }
    protected override int Radius => base.Radius * 4;

    public override bool KillShieldOrbs => true;
}
