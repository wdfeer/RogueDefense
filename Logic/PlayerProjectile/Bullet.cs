using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;
using System;
using System.Security.Cryptography.X509Certificates;

public class Bullet : Projectile
{
	private Texture2D texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://Assets/Images/outlined_circle.svg"));
	public Color modulate = Colors.White;
	public override void Draw(CanvasItem drawer)
	{
		drawer.DrawTexture(texture, position, modulate);
	}
}
