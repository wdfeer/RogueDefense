using System;
using RogueDefense.Logic.Enemy.Statuses;

namespace RogueDefense.Logic.Enemy;

public partial class StatusContainer : HBoxContainer
{
    [Export] public StatusType status;
    
    public Label Counter => (Label)GetNode("Counter");
    public Enemy enemy;
    public override void _Ready()
    {
        enemy = (Enemy)GetNode("../..");
    }

    private Status GetStatus()
    {
        // TODO: refactor after making a generic StatusDict in Enemy.cs
        switch (status)
        {
            case StatusType.Burn:
                return enemy.burn;
            case StatusType.Bleed:
                return enemy.bleed;
            case StatusType.Viral:
                return enemy.viral;
            case StatusType.Corrosive:
                return enemy.corrosive;
            case StatusType.Cold:
                return enemy.cold;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public override void _Process(double delta)
    {
        if (GetStatus().immune)
        {
            Visible = true;
            Counter.Text = "IMMUNE";
            return;
        }

        int count = GetStatus().Count;
        if (count > 0)
        {
            Visible = true;
            Counter.Text = count.ToString();
        }
        else Visible = false;
    }
}