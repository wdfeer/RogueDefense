using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

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
        public static readonly UpgradeType MaxHp = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Max Hp");
        public static readonly UpgradeType DamageReduction = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Damage Reduction") { valueMult = 0.6f };
        public static readonly UpgradeType Damage = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Damage") { chanceMult = 1.1f };
        public static readonly UpgradeType FireRate = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Fire Rate") { chanceMult = 1.2f };
        public static readonly UpgradeType Multishot = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Multishot") { chanceMult = 1.2f };
        public static readonly UpgradeType CritChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Crit Chance");
        public static readonly UpgradeType CritDamage = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Crit Damage");
        public static readonly UpgradeType BleedChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Bleed Chance")
        {
            getBaseRandomValue = () =>
            (Player.localInstance.upgradeManager.bleedChance > 0.2f ?
                (0.2f / (Player.localInstance.upgradeManager.bleedChance / 0.2f)) : 0.25f) * (0.75f + GD.Randf() * 0.45f)
        };
        public static readonly UpgradeType ViralChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Viral Chance") { chanceMult = 0.5f, valueMult = 0.8f };
        public static readonly UpgradeType ColdChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Slow Chance") { valueMult = 0.13f, canBeRolled = () => Game.instance.generation > 29 && Player.localInstance.upgradeManager.coldChance < 0.13f };
        public static readonly UpgradeType AbilityStrength = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Ability Strength") { valueMult = 2f, chanceMult = 0.75f };
        public static readonly UpgradeType AbilityDuration = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Ability Duration") { valueMult = 1.15f, chanceMult = 0.5f };
        public static readonly UpgradeType FirstHitCritDamage = new UpgradeType(x => $"+{(int)x}x Total Crit Dmg on First Hit")
        {
            chanceMult = 0.2f,
            getBaseRandomValue = () => 15f
        };
        public static readonly UpgradeType NthShotMultishot = new UpgradeType(x => $"Every 4th shot has +{MathHelper.ToPercentAndRound(x)}% Total Multishot")
        {
            chanceMult = 0.2f,
            getBaseRandomValue = () => 1f
        };
        public static readonly UpgradeType PlusDamageMinusFireRate = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Damage, -{MathHelper.ToPercentAndRound(x / 2)}% Fire Rate")
        {
            chanceMult = 0.2f,
            getBaseRandomValue = () => 0.9f
        };
        public static readonly UpgradeType MaxHpPerKill = new UpgradeType(x => $"On Kill: +{MathHelper.ToPercentAndRound(x)}% Max Hp")
        {
            chanceMult = 0.33f,
            canBeRolled = () => Game.instance.generation < 10,
            getBaseRandomValue = () => 0.02f
        };
        public static readonly UpgradeType Turret = new UpgradeType(x => $"Summon a turret")
        {
            chanceMult = 0.1f,
            canBeRolled = () => Game.instance.generation > (NetworkManager.Singleplayer ? 30 : 36) &&
                PlayerHooks.GetHooks<TurretPlayer>(Player.localInstance).TurretCount < 2
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
            FirstHitCritDamage,
            NthShotMultishot,
            PlusDamageMinusFireRate,
            MaxHpPerKill,
            Turret
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




        public static UpgradeType GetRandomType(IEnumerable<UpgradeType> blacklist = null)
        {
            UpgradeType[] possible = AllTypes.Where(x => x.canBeRolled()).ToArray();
            if (blacklist != null)
                possible = possible.Except(blacklist).ToArray();
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