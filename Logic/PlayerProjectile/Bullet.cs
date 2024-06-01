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
	readonly Font font = ThemeDB.FallbackFont;
	public override void Draw(CanvasItem drawer)
	{
		Rect2 rect = new Rect2() { Position = position - new Vector2(Radius, Radius), Size = new Vector2(Diameter, Diameter) };
		drawer.DrawTextureRect(texture, rect, false, modulate);

		if (hitMult > 1)
		{
			string str = hitMult.ToString();
			int fontSize = Radius / str.Length;
			drawer.DrawString(font, position + new Vector2(-Radius / 2, fontSize / 3), str, HorizontalAlignment.Center, Radius, fontSize);
		}
	}
}
