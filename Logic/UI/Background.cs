namespace RogueDefense.Logic.UI;

public partial class Background : Sprite2D
{
	[Export]
	public CompressedTexture2D[] backgrounds;
	public override void _Ready()
	{
		Texture = backgrounds[0];
	}
	public void UpdateBackground(int stage)
	{
		if (backgrounds.Length < stage)
			return;

		Texture = backgrounds[stage - 1];
	}
}