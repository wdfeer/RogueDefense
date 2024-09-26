using Godot;

namespace RogueDefense.Logic.Player.Hooks.ActiveAbilities;

public class ViralChanceAbility : ActiveAbility
{
    public ViralChanceAbility(Core.Player player, Button button) : base(player, button)
    {
    }


    public override void Activate()
    {
        buffLeft = Duration * BASE_DURATION;
    }
    public float buffLeft = 0;
    float unaffectedViralChance = 0;
    public override void PostUpgradeUpdate(float delta)
    {
        unaffectedViralChance = player.upgradeManager.viralChance;
        if (buffLeft > 0)
        {
            buffLeft -= delta;
            player.upgradeManager.viralChance += FlatBonus;
            player.upgradeManager.viralChance *= 1 + MultBonus;
            foreach (Enemy.Enemy enemy in Enemy.Enemy.enemies)
            {
                enemy.viral.immune = false;
            }
        }
    }
    public const float BASE_DURATION = 4f;


    public float FlatBonus => 0.2f * Strength;
    public float MultBonus => 1f * Mathf.Pow(Strength, 1.5f);
    float TotalBonus => (unaffectedViralChance + FlatBonus) * MultBonus;
    public override float BaseCooldown => 16f;
    protected override string GetAbilityText()
        => $@"+{MathHelper.ToPercentAndRound(TotalBonus)}% Viral Chance
Erase Viral Immunities
Duration: {(BASE_DURATION * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
}