namespace RogueDefense.Logic.Enemy.Variants;

public partial class RegularEnemy : Enemy
{
    public override float GetBaseSpeed()
        => 1.15f;


    private bool noArmor = false;
    protected override void ModifyMaxHp(ref float maxHp)
    {
        noArmor = statsRng.Randf() < 0.3f;

        if (noArmor)
            maxHp *= 1.5f;
    }
    protected override void ModifyArmor(ref float armor)
    {
        if (noArmor)
            armor *= 0;
    }
}