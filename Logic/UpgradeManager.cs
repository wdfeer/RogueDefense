using Godot;
using System.Collections.Generic;
using System.Linq;
using static Upgrade;

namespace RogueDefense
{
    public class UpgradeManager
    {
        readonly Player player;
        public UpgradeManager(Player player)
        {
            this.player = player;
            TimerManager.AddTimer(() =>
            {
                UpdateUpgrades();
                UpdateUpgradeText();
            }, 0.01f);
        }

        public void Process(float delta)
        {
            UpdateUpgrades();
        }

        public List<Upgrade> upgrades = new List<Upgrade>();
        public void AddUpgrade(Upgrade upgrade)
        {
            if (upgrade.type == UpgradeType.Turret)
            {
                PlayerHooks.GetHooks<TurretPlayer>(player).SpawnTurret();
                return;
            }

            upgrades.Add(upgrade);
            UpdateUpgrades();
            UpdateUpgradeText();
        }

        public float critChance = 0f;
        public float baseCritMult = 2f;
        public float critDamageMult = 2f;
        public float bleedChance = 0f;
        public float viralChance = 0f;
        public float coldChance = 0f;
        public void UpdateUpgrades()
        {
            float baseDamageMult = GetTotalUpgradeMultiplier(UpgradeType.BaseDamage);
            player.shootManager.baseDamage = 1f * baseDamageMult;

            float baseFireRateMult = GetTotalUpgradeMultiplier(UpgradeType.BaseFireRate);
            player.shootManager.baseShootInterval = 1f / baseFireRateMult;

            float baseMultishotMult = GetTotalUpgradeMultiplier(UpgradeType.BaseMultishot);
            player.shootManager.baseMultishot = 1f * baseMultishotMult;

            float baseCritDmgMult = GetTotalUpgradeMultiplier(UpgradeType.BaseCritMultiplier);
            this.baseCritMult = 2f * baseCritDmgMult;

            float hpMult = GetTotalUpgradeMultiplier(UpgradeType.MaxHp) + PlayerHooks.GetHooks<MaxHpPerKillPlayer>(player).increase;
            player.hpManager.maxHp = PlayerHpManager.BASE_MAX_HP * hpMult;

            float damageTakenMult = GetAllUpgradeValues(UpgradeType.DamageReduction).Select(x => 1 - x).Aggregate(1f, (a, b) => a * b);
            player.hpManager.damageMult = damageTakenMult;

            float fireRateMult = GetTotalUpgradeMultiplier(UpgradeType.FireRate) - SumAllUpgradeValues(UpgradeType.PlusDamageMinusFireRate) / 2;
            if (fireRateMult <= 0)
                fireRateMult = 0.0001f;
            player.shootManager.shootInterval = player.shootManager.baseShootInterval / fireRateMult;

            float damageMult = GetTotalUpgradeMultiplier(UpgradeType.Damage) + SumAllUpgradeValues(UpgradeType.PlusDamageMinusFireRate);
            player.shootManager.damage = player.shootManager.baseDamage * damageMult;

            float multishotMult = GetTotalUpgradeMultiplier(UpgradeType.Multishot);
            player.shootManager.multishot = player.shootManager.baseMultishot * multishotMult;

            critChance = SumAllUpgradeValues(UpgradeType.CritChance);
            critDamageMult = this.baseCritMult * GetTotalUpgradeMultiplier(UpgradeType.CritDamage);

            player.abilityManager.strengthMult = GetTotalUpgradeMultiplier(UpgradeType.AbilityStrength);
            player.abilityManager.durationMult = GetTotalUpgradeMultiplier(UpgradeType.AbilityDuration);
            player.abilityManager.ResetAbilityText();

            bleedChance = SumAllUpgradeValues(UpgradeType.BleedChance);
            viralChance = SumAllUpgradeValues(UpgradeType.ViralChance);
            coldChance = SumAllUpgradeValues(UpgradeType.ColdChance);
        }
        public IEnumerable<float> GetAllUpgradeValues(UpgradeType type)
            => upgrades.Where(x => x.type.Equals(type)).Select(x => x.value);
        public float SumAllUpgradeValues(UpgradeType type)
            => GetAllUpgradeValues(type).Aggregate(0f, (a, b) => a + b);
        public float GetTotalUpgradeMultiplier(UpgradeType type)
            => 1f + SumAllUpgradeValues(type);

        public void UpdateUpgradeText()
        {
            var upgradeText = player.GetNode("/root/Game/UpgradeScreen/UpgradeText") as Label;
            upgradeText.Text = $"Max HP: {player.hpManager.maxHp.ToString("0.0")}\n";
            if (player.hpManager.damageMult != 1f)
                upgradeText.Text += $"Damage Reduction: {(1 - player.hpManager.damageMult) * 100f}%\n";
            upgradeText.Text += $@"
Damage: {player.shootManager.damage.ToString("0.00")}
Fire Rate: {(1f / player.shootManager.shootInterval).ToString("0.00")}
Multishot: {player.shootManager.multishot.ToString("0.00")}

Critical Chance: {(critChance * 100f).ToString("0.0")}%
Critical Multiplier: {critDamageMult.ToString("0.00")}

Bleeding Chance: {(bleedChance * 100f).ToString("0.0")}%
Viral Chance: {(viralChance * 100f).ToString("0.0")}%
Cold Chance: {(coldChance * 100f).ToString("0.0")}%";
        }
    }
}
