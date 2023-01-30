using System;
using System.Linq;
using Godot;

namespace RogueDefense
{
    public class UpgradeType
    {
        public UpgradeType(Func<float, string> upgradeTextGetter)
        {
            this.getUpgradeText = upgradeTextGetter;
        }
        public Func<float, string> getUpgradeText;
        public float chanceMult = 1f;
        public float valueMult = 1f;
        public float GetRandomValue()
            => getBaseRandomValue() * valueMult;
        Func<float> getBaseRandomValue = () => GD.Randf() * 0.055f + 0.2f;
        public Func<bool> canBeRolled = () => true;

        public int uniqueId;
        public static readonly UpgradeType MaxHp = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Max Hp");
        public static readonly UpgradeType DamageReduction = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Damage Reduction") { valueMult = 0.6f };
        public static readonly UpgradeType Damage = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Damage") { chanceMult = 1.1f };
        public static readonly UpgradeType FireRate = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Fire Rate") { chanceMult = 1.25f };
        public static readonly UpgradeType Multishot = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Multishot") { chanceMult = 1.2f };
        public static readonly UpgradeType CritChance = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Crit Chance");
        public static readonly UpgradeType CritDamage = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Crit Damage");
        public static readonly UpgradeType BleedChance = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Bleed Chance")
        {
            getBaseRandomValue = () =>
            (Player.localInstance.upgradeManager.bleedChance > 0.125f ?
                (0.2f / (Player.localInstance.upgradeManager.bleedChance / 0.125f)) : 0.2f) * (0.8f + GD.Randf() * 0.4f)
        };
        public static readonly UpgradeType ViralChance = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Viral Chance") { chanceMult = 0.5f };
        public static readonly UpgradeType ColdChance = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Slow Chance") { valueMult = 0.1f, canBeRolled = () => Player.localInstance.upgradeManager.coldChance < 0.1f };
        public static readonly UpgradeType AbilityStrength = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Ability Strength") { valueMult = 2.25f, chanceMult = 0.8f };
        public static readonly UpgradeType AbilityDuration = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Ability Duration") { valueMult = 1.25f, chanceMult = 0.4f };
        const float BASE_UPGRADE_VALUE = 0.75f;
        const int BASE_UPGRADE_APPEARANCE_STEP = 6;
        public static readonly UpgradeType BaseDamage = new UpgradeType(x => $"+{ToPercentAndRound(x)}% BASE Damage") { canBeRolled = () => Game.instance.generation % BASE_UPGRADE_APPEARANCE_STEP == 0, chanceMult = 10f, getBaseRandomValue = () => BASE_UPGRADE_VALUE };
        public static readonly UpgradeType BaseFireRate = new UpgradeType(x => $"+{ToPercentAndRound(x)}% BASE Fire Rate") { canBeRolled = () => Game.instance.generation % BASE_UPGRADE_APPEARANCE_STEP == 0, chanceMult = 8f, getBaseRandomValue = () => BASE_UPGRADE_VALUE };
        public static readonly UpgradeType BaseMultishot = new UpgradeType(x => $"+{ToPercentAndRound(x)}% BASE Multishot") { canBeRolled = () => Game.instance.generation % BASE_UPGRADE_APPEARANCE_STEP == 0, chanceMult = 8f, getBaseRandomValue = () => BASE_UPGRADE_VALUE };
        public static readonly UpgradeType BaseCritMultiplier = new UpgradeType(x => $"+{ToPercentAndRound(x)}% BASE Crit Damage") { canBeRolled = () => Game.instance.generation % BASE_UPGRADE_APPEARANCE_STEP == 0, chanceMult = 10f, getBaseRandomValue = () => BASE_UPGRADE_VALUE };
        public static readonly UpgradeType FirstHitCritDamage = new UpgradeType(x => $"+{(int)x}x Total Crit Dmg on First Hit")
        {
            chanceMult = 0.2f,
            getBaseRandomValue = () => 15f
        };
        public static readonly UpgradeType NthShotMultishot = new UpgradeType(x => $"Every 4th shot has +{ToPercentAndRound(x)}% Total Multishot")
        {
            chanceMult = 0.2f,
            getBaseRandomValue = () => 1f
        };
        public static readonly UpgradeType PlusDamageMinusFireRate = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Damage, -{ToPercentAndRound(x / 2)}% Fire Rate")
        {
            chanceMult = 0.2f,
            getBaseRandomValue = () => 0.8f
        };
        public static readonly UpgradeType MaxHpPerKill = new UpgradeType(x => $"On Kill: +{ToPercentAndRound(x)}% Max Hp")
        {
            chanceMult = 0.15f,
            getBaseRandomValue = () => 0.02f
        };
        public static UpgradeType[] AllTypes = new UpgradeType[] {
            MaxHp,
            DamageReduction,
            Damage,
            FireRate,
            Multishot,
            CritChance,
            CritDamage,
            BleedChance,
            ViralChance,
            ColdChance,
            AbilityStrength,
            AbilityDuration,
            BaseDamage,
            BaseFireRate,
            BaseMultishot,
            BaseCritMultiplier,
            FirstHitCritDamage,
            NthShotMultishot,
            PlusDamageMinusFireRate,
            MaxHpPerKill
        };
        public static void Initialize()
        {
            for (int i = 0; i < AllTypes.Length; i++)
            {
                AllTypes[i].uniqueId = i;
            }
        }


        public override int GetHashCode() => uniqueId;
        public override bool Equals(object obj)
            => obj is UpgradeType && ((UpgradeType)obj).uniqueId == this.uniqueId;


        static int ToPercentAndRound(float f)
        {
            return Mathf.RoundToInt(f * 100f);
        }

        public static UpgradeType GetRandomType()
        {
            UpgradeType[] possible = AllTypes.Where(x => x.canBeRolled()).ToArray();
            float chanceRange = possible.Aggregate(0f, (a, b) => a + b.chanceMult);
            float rand = GD.Randf() * chanceRange;

            float current = 0f;
            for (int i = 0; i < possible.Length; i++)
            {
                current += possible[i].chanceMult;
                if (current > rand)
                    return possible[i];
            }
            throw new Exception("Failed to produce a random UpgradeType!");
        }
    }
}