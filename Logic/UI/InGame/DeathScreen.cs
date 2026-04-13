namespace RogueDefense.Logic.UI.InGame;

public partial class DeathScreen : Panel
{
	public static DeathScreen instance;
	[Export]
	public Label scoreLabel;
	public override void _Ready()
	{
		instance = this;
	}
}
