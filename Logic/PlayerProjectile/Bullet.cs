using Godot;
using Godot.Collections;
using RogueDefense.Logic;

public class Bullet : Projectile
{
	public Bullet(Array<Texture2D> textures) : base()
	{
		texture = textures[0];
	}
	protected Texture2D texture;
	protected override int Radius => 16;
	public Color modulate = Colors.White;
	public override void Draw(CanvasItem drawer)
	{
		Rect2 rect = new Rect2() { Position = position - new Vector2(Radius, Radius), Size = new Vector2(Diameter, Diameter) };
		drawer.DrawTextureRect(texture, rect, false, modulate);

		if (hitMult > 1)
			drawer.DrawString(ThemeDB.FallbackFont, position + new Vector2(-Radius / 2, Radius / 3), hitMult.ToString(), HorizontalAlignment.Center, Radius, Radius);
	}
}
