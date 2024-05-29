using Godot;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.Statuses;
using System;
using System.Linq;

public partial class ArcaneBoss : Enemy
{
	public override float GetBaseSpeed()
		=> 0.1f;


	public ArcaneBossNode[] nodes = new ArcaneBossNode[4];
	public override void _Ready()
	{
		for (int i = 0; i < 4; i++)
		{
			var node = new ArcaneBossNode();
			GetNode<Node2D>("Sprite2D").AddChild(node);

			nodes[i] = node;
		}
	}


	public override void _Process(double delta)
	{
		GetNode<Node2D>("Sprite2D").Rotate(Mathf.Pi / 2 * (float)delta);
	}
}
