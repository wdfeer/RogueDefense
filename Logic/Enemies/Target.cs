using Godot;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense.Logic.Enemies;

public partial class Target : Sprite2D
{
	public Enemy enemy;
	public override void _Ready()
	{
		enemy = (Enemy)GetParent();
	}
	public override void _Process(double delta)
	{
		Visible = enemy == PlayerManager.my.target;
		if (!Visible)
			return;

		Rotate((float)delta);
	}
}