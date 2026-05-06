using RogueDefense.Logic.Network;

namespace RogueDefense.Logic;

public partial class DefenseObjective
{
    private void SetHealthDrainTimer()
    {
        ToSignal(GetTree().CreateTimer(1, false), "timeout").OnCompleted(() =>
        {
            DrainHealth();
            SetHealthDrainTimer();
        });
    }

    private void DrainHealth()
    {
        float dps = 6;
        if (!NetworkManager.Singleplayer) dps *= 1.5f;
        if (Game.Wave > 20) dps *= Mathf.Pow(2f, Game.Wave / 20f + 1f);
        Damage(dps);
    }
}