using Godot;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.Statuses;

public abstract partial class StatusContainer : HBoxContainer
{
    public Label Counter => (Label)GetNode("Counter");
    public Enemy enemy;
    public override void _Ready()
    {
        enemy = (Enemy)GetNode("../..");
    }
    public abstract Status GetStatus();
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