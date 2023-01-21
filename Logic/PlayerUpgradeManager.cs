using Godot;
using System.Collections.Generic;
using System.Linq;
using static Upgrade;

namespace RogueDefense
{
    public class PlayerUpgradeManager
    {
        readonly Player player;
        public PlayerUpgradeManager(Player player)
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

        List<Upgrade> upgrades = new List<Upgrade>();
        public void AddUpgrade(Upgrade upgrade)
        {
            upgrades.Add(upgrade);
            UpdateUpgrades();
            UpdateUpgradeText();
        }

        public float critChance = 0f;
        public float baseCritMult = 2f;
        public float critDamage = 2f;
        void UpdateUpgrades()
        {
            float baseDamageMult = GetTotalUpgradeMultiplier(UpgradeType.BaseDamage);
            player.shootManager.baseDamage = 1f * baseDamageMult;

            float baseFireRateMult = GetTotalUpgradeMultiplier(UpgradeType.BaseFireRate);
            player.shootManager.baseShootInterval = 1f / baseFireRateMult;

            float baseMultishotMult = GetTotalUpgradeMultiplier(UpgradeType.BaseMultishot);
            player.shootManager.baseMultishot = 1f * baseMultishotMult;

            float baseCritDmgMult = GetTotalUpgradeMultiplier(UpgradeType.BaseCritMultiplier);
            this.baseCritMult = 2f * baseCritDmgMult;

            float hpMult = GetTotalUpgradeMultiplier(UpgradeType.MaxHp);
            player.hpManager.maxHp = PlayerHpManager.BASE_MAX_HP * hpMult;

            float damageTakenMult = GetAllUpgradeValues(UpgradeType.DamageReduction).Select(x => 1 - x).Aggregate(1f, (a, b) => a * b);
            player.hpManager.damageMult = damageTakenMult;

            float fireRateMult = GetTotalUpgradeMultiplier(UpgradeType.FireRate);
            player.shootManager.shootInterval = player.shootManager.baseShootInterval / fireRateMult;

            float damageMult = GetTotalUpgradeMultiplier(UpgradeType.Damage);
            player.shootManager.damage = player.shootManager.baseDamage * damageMult;

            float multishotMult = GetTotalUpgradeMultiplier(UpgradeType.Multishot);
            player.shootManager.multishot = player.shootManager.baseMultishot * multishotMult;

            critChance = GetAllUpgradeValues(UpgradeType.CritChance).Aggregate(0f, (a, b) => a + b);
            critDamage = this.baseCritMult * GetTotalUpgradeMultiplier(UpgradeType.CritDamage);

            player.abilityManager.strengthMult = GetTotalUpgradeMultiplier(UpgradeType.AbilityStrength);
        }
        IEnumerable<float> GetAllUpgradeValues(UpgradeType type)
    => upgrades.Where(x => x.type == type).Select(x => x.value);
        float GetTotalUpgradeMultiplier(UpgradeType type)
            => 1f + GetAllUpgradeValues(type).Aggregate(0f, (a, b) => a + b);

        void UpdateUpgradeText()
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
Critical Multiplier: {critDamage.ToString("0.00")}";
        }
    }
}
