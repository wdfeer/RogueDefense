using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;
using System;
using System.Security.Cryptography.X509Certificates;

public class Bullet : Projectile
{
	private Texture2D texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://Assets/Images/outlined_circle.svg"));
	protected override int Radius => 32;
	public Color modulate = Colors.White;
	public override void Draw(CanvasItem drawer)
	{
		Rect2 rect = new Rect2() { Position = position - new Vector2(Radius, Radius), Size = new Vector2(Diameter, Diameter) };
		drawer.DrawTextureRect(texture, rect, false, modulate);
	}
}
