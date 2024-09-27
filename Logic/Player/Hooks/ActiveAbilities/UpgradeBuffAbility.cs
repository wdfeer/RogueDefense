namespace RogueDefense.Logic.Player.Hooks.ActiveAbilities;

public class UpgradeBuffAbility : ActiveAbility
{
    public UpgradeBuffAbility(Core.Player player, Button button) : base(player, button)
    {
    }
    public override void Activate()
    {
        buffLeft = BASE_DURATION;
        player.turrets.ForEach((t) => t.EnableParticles(BASE_DURATION));
    }
    private float buffLeft = 0;
    public const float BASE_DURATION = 8f;
    public const float UPGRADE_INCREASE = 1f;
    public override void PreUpdate(float delta)
    {
        base.PreUpdate(delta);

        if (buffLeft > 0)
        {
            player.upgradeManager.dynamicUpgradeModifier += UPGRADE_INCREASE;
            buffLeft -= delta;
        }
    }

    public override float BaseCooldown => 90f;
    protected override string GetAbilityText()
        => $@"+{MathHelper.ToPercentAndRound(UPGRADE_INCREASE)}% Value on all previous upgrades
Duration: {BASE_DURATION:0.00} s
Cooldown: {Cooldown:0.00} s";
    public override bool ConstantValues => true;
    public override bool Shared => false;
}