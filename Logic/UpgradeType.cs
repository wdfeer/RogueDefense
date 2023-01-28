using System;
using System.Linq;
using Godot;

namespace RogueDefense
{
    public class UpgradeType
    {
        public UpgradeType(Func<float, string> upgradeTextGetter, float chanceMult = 1f)
        {
            this.getUpgradeText = upgradeTextGetter;
            this.chanceMult = chanceMult;
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
        public static readonly UpgradeType Damage = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Damage");
        public static readonly UpgradeType FireRate = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Fire Rate");
        public static readonly UpgradeType Multishot = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Multishot");
        public static readonly UpgradeType CritChance = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Crit Chance");
        public static readonly UpgradeType CritDamage = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Crit Damage");
        public static readonly UpgradeType BleedChance = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Bleed Chance") { valueMult = 0.7f };
        public static readonly UpgradeType AbilityStrength = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Ability Strength") { valueMult = 2.25f };
        public static readonly UpgradeType AbilityDuration = new UpgradeType(x => $"+{ToPercentAndRound(x)}% Ability Duration") { chanceMult = 0.4f };
        const float BASE_UPGRADE_VALUE = 0.8f;
        public static readonly UpgradeType BaseDamage = new UpgradeType(x => $"+{ToPercentAndRound(x)}% BASE Damage") { canBeRolled = () => Game.instance.generation % 5 == 0, chanceMult = 10f, getBaseRandomValue = () => BASE_UPGRADE_VALUE };
        public static readonly UpgradeType BaseFireRate = new UpgradeType(x => $"+{ToPercentAndRound(x)}% BASE Fire Rate") { canBeRolled = () => Game.instance.generation % 5 == 0, chanceMult = 10f, getBaseRandomValue = () => BASE_UPGRADE_VALUE };
        public static readonly UpgradeType BaseMultishot = new UpgradeType(x => $"+{ToPercentAndRound(x)}% BASE Multishot") { canBeRolled = () => Game.instance.generation % 5 == 0, chanceMult = 10f, getBaseRandomValue = () => BASE_UPGRADE_VALUE };
        public static readonly UpgradeType BaseCritMultiplier = new UpgradeType(x => $"+{ToPercentAndRound(x)}% BASE Crit Damage") { canBeRolled = () => Game.instance.generation % 5 == 0, chanceMult = 10f, getBaseRandomValue = () => BASE_UPGRADE_VALUE };
        public static readonly UpgradeType FirstHitCritDamage = new UpgradeType(x => $"+{(int)x}x Total Crit Dmg on First Hit")
        {
            chanceMult = 0.25f,
            getBaseRandomValue = () => 15f
        };
        public static readonly UpgradeType NthShotMultishot = new UpgradeType(x => $"Every 4th shot has +{ToPercentAndRound(x)}% Total Multishot")
        {
            chanceMult = 0.25f,
            getBaseRandomValue = () => 1f
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
            AbilityStrength,
            AbilityDuration,
            BaseDamage,
            BaseFireRate,
            BaseMultishot,
            BaseCritMultiplier,
            FirstHitCritDamage,
            NthShotMultishot
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