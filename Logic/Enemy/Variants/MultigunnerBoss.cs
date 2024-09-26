namespace RogueDefense.Logic.Enemy.Variants;

public partial class MultigunnerBoss : Multigunner
{
    protected override int GunCount => base.GunCount + 4;
    protected override void ModifyMaxHp(ref float maxHp)
    {
        maxHp *= 2f;
    }

    protected override float HpCriticalModifier => (Hp / maxHp < 0.75f) ? 2.5f : 1;
}
