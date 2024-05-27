using Godot;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.PlayerCore;

public partial class Target : Sprite2D
{
	public Enemy enemy;
	public override void _Ready()
	{
		enemy = (Enemy)GetParent();
	}
	public override void _Process(double delta)
	{
		Visible = enemy == Player.my.target;
		if (!Visible)
			return;

		Rotate((float)delta);
	}
}
