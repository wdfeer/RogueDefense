using RogueDefense.Logic.Save;
using UserData = RogueDefense.Logic.Save.UserData;

namespace RogueDefense.Logic.UI.InGame;

public partial class DeathScreen : Panel
{
	public static DeathScreen instance;
	[Export] public Label scoreLabel;
	[Export] public Label augmentLabel;

	public override void _Ready()
	{
		instance = this;
	}

	public void Activate()
	{
		GetTree().Paused = true;
		Show();
		
		scoreLabel.Text = $"{Game.GetStage() - 1} Stages cleared\n{PP.currentPP.ToString("0.000")} pp";

		var oldThreshold = AugmentBalance.GetNextPPThreshold(UserData.MaxPP);
		if (AugmentBalance.GetNextPPThreshold(PP.currentPP) > oldThreshold)
		{
			augmentLabel.Visible = true;
			augmentLabel.Text =
				$"{oldThreshold}pp Achieved: +1 Augment Point";
		}
	}
}
