namespace RogueDefense.Logic.UI.InGame;

public partial class NotificationPopup : PopupPanel
{
	public static NotificationPopup instance;
	public Rect2 bounds;

	public override void _Ready()
	{
		instance = this;
		bounds = GetVisibleRect();
	}

	private Label Label => (Label)GetNode("Label");
	private AnimationPlayer animator => (AnimationPlayer)GetNode("AnimationPlayer");

	public static void Notify(string text, float duration)
	{
		instance.Label.Text = text;
		instance.Activate(duration);
	}

	void Activate(float duration)
	{
		Popup((Rect2I)bounds);
		animator.Play("show");
	}
}
