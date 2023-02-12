using Godot;
using System;

public class NotificationPopup : PopupPanel
{
    public static NotificationPopup instance;
    public Rect2 bounds;
    public override void _Ready()
    {
        instance = this;
        bounds = GetRect();
    }
    Label Label => (Label)GetNode("Label");
    public static void Notify(string text, float duration)
    {
        instance.Label.Text = text;
        instance.Popup(duration);
    }
    void Popup(float duration)
    {
        Popup_(bounds);

        ToSignal(GetTree().CreateTimer(duration), "timeout").OnCompleted(() =>
        {
            Hide();
        });
    }
}
