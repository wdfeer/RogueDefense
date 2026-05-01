namespace RogueDefense.Logic.Enemy;

public record EnemyModifiers(float damageCap = -1f, float minDamage = -1f)
{
    public static float GetDamageCap(int gen)
        => gen > 20 ? (gen > 60 ? 0.03f : 0.08f) : 0.151f; 
    public static float GetMinDamage(int gen)
        => gen > 10 ? (gen > 25 ? 0.03f : 0.06f) : 0.099f;

    public void SetLabels(Enemy enemy)
    {
        Label label = (Label)enemy.GetNode("BottomInfo");
        if (damageCap > 0f)
        {
            label.Visible = true;
            label.Text += $"Damage Cap per Hit: {MathHelper.ToPercentAndRound(damageCap)}%\n";
        }

        if (minDamage > 0f)
        {
            label.Visible = true;
            label.Text += $"Minimum Damage per Hit: {MathHelper.ToPercentAndRound(minDamage)}%\n";
        }
    }
}