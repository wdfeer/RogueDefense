using Godot;
using System;
using System.Linq;

public partial class ConnectingLabel : Label
{
    public override void _Ready()
    {
        CreateTimer();
    }
    public SceneTreeTimer timer;
    public void TimerTimeout()
    {
        UpdateText();
        CreateTimer();
    }
    public void CreateTimer()
    {
        timer = GetTree().CreateTimer(0.5f);
        timer.Connect("timeout", new Callable(this, "TimerTimeout"));
    }
    int dotCount = 0;
    public void UpdateText()
    {
        if (!Visible) return;

        dotCount++;
        if (dotCount > 3)
            dotCount = 0;
        Text = "Connecting";
        for (int i = 0; i < dotCount; i++)
        {
            Text += ".";
        }
    }

    public override void _ExitTree()
    {
        timer.Disconnect("timeout", new Callable(this, "TimerTimeout"));
    }
}
