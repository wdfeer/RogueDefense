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
    Label Label => (Label)GetNode("Label");
    public static void Notify(string text, float duration)
    {
        instance.Label.Text = text;
        instance.Activate(duration);
    }
    SceneTreeTimer timer;
    void Activate(float duration)
    {
        Popup((Rect2I)bounds);

        if (timer != null) timer.Dispose();
        timer = GetTree().CreateTimer(duration);
        ToSignal(timer, "timeout").OnCompleted(() =>
        {
            Hide();
        });
    }
}