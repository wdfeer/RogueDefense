using System;
using System.Linq;
using Godot;

namespace RogueDefense
{
    public class UpgradeType
    {
        public UpgradeType(int id, Func<float, string> upgradeTextGetter, float chanceMult = 1f)
        {
            this.uniqueId = id;
            this.getUpgradeText = upgradeTextGetter;
            this.chanceMult = chanceMult;
        }
        public Func<float, string> getUpgradeText;
        public float chanceMult = 1f;
        public float valueMult = 1f;
        public float GetRandomValue()
            => getBaseRandomValue() * valueMult;
        Func<float> getBaseRandomValue = () => GD.Randf() * 0.05f + 0.2f;
        public Func<bool> canBeRolled = () => true;

        public int uniqueId;
        public static UpgradeType MaxHp => new UpgradeType(0, x => $"+{ToPercentAndRound(x)}% Max Hp");
        public static UpgradeType DamageReduction => new UpgradeType(1, x => $"+{ToPercentAndRound(x)}% Damage Reduction") { valueMult = 0.5f };
        public static UpgradeType Damage => new UpgradeType(2, x => $"+{ToPercentAndRound(x)}% Damage");
        public static UpgradeType FireRate => new UpgradeType(3, x => $"+{ToPercentAndRound(x)}% Fire Rate");
        public static UpgradeType Multishot => new UpgradeType(4, x => $"+{ToPercentAndRound(x)}% Multishot");
        public static UpgradeType CritChance => new UpgradeType(5, x => $"+{ToPercentAndRound(x)}% Crit Chance");
        public static UpgradeType CritDamage => new UpgradeType(6, x => $"+{ToPercentAndRound(x)}% Crit Damage");
        public static UpgradeType BleedChance => new UpgradeType(7, x => $"+{ToPercentAndRound(x)}% Bleed Chance");
        public static UpgradeType AbilityStrength => new UpgradeType(8, x => $"+{ToPercentAndRound(x)}% Ability Strength") { valueMult = 2f };
        public static UpgradeType BaseDamage => new UpgradeType(9, x => $"+{ToPercentAndRound(x)}% BASE Damage") { canBeRolled = () => Game.instance.generation % 5 == 0, chanceMult = 10f, getBaseRandomValue = () => 0.5f };
        public static UpgradeType BaseFireRate => new UpgradeType(10, x => $"+{ToPercentAndRound(x)}% BASE Fire Rate") { canBeRolled = () => Game.instance.generation % 5 == 0, chanceMult = 10f, getBaseRandomValue = () => 0.5f };
        public static UpgradeType BaseMultishot => new UpgradeType(11, x => $"+{ToPercentAndRound(x)}% BASE Multishot") { canBeRolled = () => Game.instance.generation % 5 == 0, chanceMult = 10f, getBaseRandomValue = () => 0.5f };
        public static UpgradeType BaseCritMultiplier => new UpgradeType(12, x => $"+{ToPercentAndRound(x)}% BASE Crit Damage") { canBeRolled = () => Game.instance.generation % 5 == 0, chanceMult = 10f, getBaseRandomValue = () => 0.5f };

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
            BaseDamage,
            BaseFireRate,
            BaseMultishot,
            BaseCritMultiplier
        };

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