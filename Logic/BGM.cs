using System;
using Godot;
using System.Collections.Generic;

namespace RogueDefense.Logic;

public partial class BGM : AudioStreamPlayer
{
	private const string MUSIC_PATH = "res://Assets/Music";

	private readonly List<AudioStream> streams = new();
	public override void _Ready()
	{
		using var dir = DirAccess.Open(MUSIC_PATH);
		if (dir != null)
		{
			dir.ListDirBegin();
			var fileName = dir.GetNext();
			while (fileName != "")
			{
				if (!dir.CurrentIsDir() && fileName.EndsWith(".mp3"))
				{
					streams.Add(GD.Load<AudioStream>(MUSIC_PATH + "/" + fileName));
				}
				fileName = dir.GetNext();
			}
		}
		else
		{
			GD.PrintErr("An error occurred when trying to access the music directory.");
		}
	}
	
	private int interval = new Random().Next(10, 31);
	private float timer = 0;
	public override void _Process(double delta)
	{
		if (Playing) return;

		timer += (float)delta;
		if (timer > interval)
		{
			PlayRandom();

			timer = 0;
			interval = new Random().Next(30, 61);
		}
	}
	
	void PlayRandom()
	{
		Stream = MathHelper.GetRandomElement(streams);
		Play();
	}
}
