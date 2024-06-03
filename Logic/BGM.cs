using Godot;
using RogueDefense.Logic;
using System.Collections.Generic;

namespace RogueDefense.Logic;

public partial class BGM : AudioStreamPlayer
{
	readonly List<AudioStream> streams = new List<AudioStream>();
	public override void _Ready()
	{
		const string MUSIC_PATH = "res://Assets/Music";
		using var dir = DirAccess.Open(MUSIC_PATH);
		if (dir != null)
		{
			dir.ListDirBegin();
			string fileName = dir.GetNext();
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
			GD.Print("An error occurred when trying to access the path.");
		}

		PlayRandom();
		Finished += PlayRandom;
	}
	void PlayRandom()
	{
		Stream = MathHelper.GetRandomElement(streams);
		Play();
	}
}
