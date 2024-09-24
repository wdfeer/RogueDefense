using Godot;
using RogueDefense.Logic.Enemies;
using System.Collections.Generic;
using System.Linq;
using RogueDefense.Logic.PlayerHooks.Upgrades;
using RogueDefense.Logic.UI.Lobby.Settings;

namespace RogueDefense.Logic.PlayerCore;

public class UpgradeManager
{
    public static UpgradeManager local;
    public static Dictionary<int, UpgradeManager> others = new Dictionary<int, UpgradeManager>();
    readonly Player player;
    public UpgradeManager(Player player)
    {
        this.player = player;
        Game.instance.ToSignal(Game.instance.GetTree().CreateTimer(0.01f), "timeout").OnCompleted(() =>
        {
            UpdateUpgrades();
            UpdateUpgradeText();
        });
    }

    public void Process(float delta)
    {
        UpdateUpgrades();
    }

    public List<Upgrade> upgrades = new List<Upgrade>();
    public static void AddUpgrade(Upgrade upgrade, int from)
    {
        if (upgrade.risky)
        {
            Enemy.oneTimeHpMult += 1f;
            Enemy.oneTimeArmorMult += 1f;
            Enemy.oneTimeCountIncrease += Game.GetStage();
        }

        if (upgrade.type == UpgradeType.Turret)
        {
            PlayerManager.players[from].SpawnTurret();
        }
        else
        {
            var upgradeManager = PlayerManager.players[from].upgradeManager;
            upgradeManager.upgrades.Add(upgrade);
            foreach (var item in PlayerManager.players)
            {
                item.Value.upgradeManager.UpdateUpgrades();
                PlayerManager.my.upgradeManager.UpdateUpgradeText();
            }
        }
    }

    public float critChance = 0f;
    public float baseCritMult = 2f;
    public float critDamageMult = 2f;
    public float bleedChance = 0f;
    public float viralChance = 0f;
    public float coldChance = 0f;
    public void UpdateMaxHp()
    {
        float hpMult = 1f + SumEveryonesUpgradeValues(UpgradeType.MaxHp) + MaxHpPerKillPlayer.GetTotalIncrease();
        DefenseObjective.instance.maxHp = DefenseObjective.BASE_MAX_HP * hpMult;
    }
    public void UpdateDamageReduction()
    {
        float damageTakenMult = PlayerManager.players.Select(x => x.Value.upgradeManager.GetReversedMultiplier(UpgradeType.DamageReduction)).Aggregate(1f, (a, b) => a * b);
        DefenseObjective.instance.damageMult = damageTakenMult;
    }
    public void UpdateEvasion()
    {
        float chanceToTakeDamage = PlayerManager.players.Select(x => x.Value.upgradeManager.GetReversedMultiplier(UpgradeType.Evasion)).Aggregate(1f, (a, b) => a * b);
        float evasionChance = 1 - chanceToTakeDamage;
        DefenseObjective.instance.evasionChance = evasionChance;
    }
    public void UpdateUpgrades()
    {
        if (player.IsLocal)
        {
            UpdateDamageReduction();
            UpdateEvasion();
        }

        float fireRateMult = (GetTotalUpgradeMultiplier(UpgradeType.FireRate) + SumAllUpgradeValues(UpgradeType.FireRateMinusMultishot) - SumAllUpgradeValues(UpgradeType.PlusDamageMinusFireRate) / 2) * GameSettings.totalFireRateMult;
        if (fireRateMult <= 0)
            fireRateMult = 0.001f;
        player.shootManager.shootInterval = player.shootManager.baseShootInterval / fireRateMult;

        float damageMult = GetTotalUpgradeMultiplier(UpgradeType.Damage) + SumAllUpgradeValues(UpgradeType.PlusDamageMinusFireRate);
        player.shootManager.damage = player.shootManager.baseDamage * damageMult * GameSettings.totalDmgMult * GetTotalUpgradeMultiplier(UpgradeType.RecoilDamage);

        float multishotMult = GetTotalUpgradeMultiplier(UpgradeType.Multishot) - SumAllUpgradeValues(UpgradeType.FireRateMinusMultishot) / 2f;
        player.shootManager.multishot = player.shootManager.baseMultishot * multishotMult;

        critChance = SumAllUpgradeValues(UpgradeType.CritChance);
        baseCritMult = 2f * UI.MainMenu.AugmentScreen.AugmentContainer.GetStatMult(4);
        critDamageMult = baseCritMult * GetTotalUpgradeMultiplier(UpgradeType.CritDamage);

        player.abilityManager.strengthMult = GetTotalUpgradeMultiplier(UpgradeType.AbilityStrength);
        player.abilityManager.durationMult = GetTotalUpgradeMultiplier(UpgradeType.AbilityDuration);
        if (player.IsLocal) player.abilityManager.ResetAbilityText();

        bleedChance = SumAllUpgradeValues(UpgradeType.BleedChance);
        viralChance = SumAllUpgradeValues(UpgradeType.ViralChance);
        coldChance = SumAllUpgradeValues(UpgradeType.ColdChance);
        PlayerHooks.PlayerHooks.GetHooks<StatusPlayer>(player).corrosiveChance = SumAllUpgradeValues(UpgradeType.CorrosiveChance);

        player.shootManager.shootSpeed = ShootManager.BASE_SHOOT_SPEED;

        dynamicUpgradeModifier = 1f;
    }

    public float dynamicUpgradeModifier = 1f;
    private IEnumerable<float> GetAllUpgradeValues(UpgradeType type)
        => upgrades.Where(x => x.type.Equals(type)).Select(x => x.Value);
    public float SumAllUpgradeValues(UpgradeType type)
        => GetAllUpgradeValues(type).Aggregate(0f, (a, b) => a + b) * dynamicUpgradeModifier;
    public float GetTotalUpgradeMultiplier(UpgradeType type)
        => 1f + SumAllUpgradeValues(type);
    public float SumEveryonesUpgradeValues(UpgradeType type)
        => PlayerManager.players.Select(x => x.Value.upgradeManager.SumAllUpgradeValues(type)).Aggregate(0f, (a, b) => a + b);
    public float GetReversedMultiplier(UpgradeType type) // Returns the product of all (1 - upgradeValue) on the player
        => GetAllUpgradeValues(type).Select(x => 1 - x).Aggregate(1f, (a, b) => a * b) / dynamicUpgradeModifier;

    public void UpdateUpgradeText()
    {
        if (player != PlayerManager.my) return;

        var upgradeText = Game.instance.GetNode("UpgradeScreen/UpgradeText") as Label;
        upgradeText.Text = $"Max HP: {DefenseObjective.instance.maxHp.ToString("0.0")}\n";
        if (DefenseObjective.instance.damageMult != 1f)
            upgradeText.Text += $"Damage Reduction: {MathHelper.ToPercentAndRound(1 - DefenseObjective.instance.damageMult)}%\n";
        if (DefenseObjective.instance.evasionChance > 0f)
            upgradeText.Text += $"Evasion: {MathHelper.ToPercentAndRound(DefenseObjective.instance.evasionChance)}%\n";
        upgradeText.Text += $@"
Damage: {player.shootManager.damage.ToString("0.00")}
Fire Rate: {(1f / player.shootManager.shootInterval).ToString("0.00")}
Multishot: {player.shootManager.multishot.ToString("0.00")}

Critical Chance: {(critChance * 100f).ToString("0.0")}%
Critical Multiplier: {critDamageMult.ToString("0.00")}x
";
        if (bleedChance > 0f) upgradeText.Text += $"\nBleeding Chance: {(bleedChance * 100f).ToString("0.0")}%";
        float corrosiveChance = PlayerHooks.PlayerHooks.GetLocalHooks<StatusPlayer>().corrosiveChance;
        if (corrosiveChance > 0f) upgradeText.Text += $"\nCorrosive Chance: {(corrosiveChance * 100f).ToString("0.0")}%"; if (viralChance > 0f) upgradeText.Text += $"\nViral Chance: {(viralChance * 100f).ToString("0.0")}%";
        if (coldChance > 0f) upgradeText.Text += $"\nCold Chance: {(coldChance * 100f).ToString("0.0")}% ";
    }
}
