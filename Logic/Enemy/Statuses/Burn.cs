namespace RogueDefense.Logic.Enemy.Statuses;

public class Burn : DamagingStatus
{
    public override void Process(float delta)
    {
        base.Process(delta);

        if (Count >= 10)
            enemy.dynamicArmorMult *= 0.5f;
    }
}