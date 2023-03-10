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
        public static readonly UpgradeType Damage = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Damage") { chanceMult = 1.1f, valueMult = 1.2f };
        public static readonly UpgradeType FireRate = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Fire Rate");
        public static readonly UpgradeType Multishot = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Multishot");
        public static readonly UpgradeType CritChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Crit Chance")
        {
            canBeRolled = () => Player.localInstance.upgradeManager.critChance < 1.25f
        };
        public static readonly UpgradeType CritDamage = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Crit Damage");
        public static readonly UpgradeType BleedChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Bleed Chance")
        {
            getBaseRandomValue = () =>
            (Player.localInstance.upgradeManager.bleedChance > 0.2f ?
                (0.2f / (Player.localInstance.upgradeManager.bleedChance / 0.2f)) : 0.25f) * (0.65f + GD.Randf() * 0.35f),
            canBeRolled = () => Player.localInstance.upgradeManager.bleedChance < 0.5f,
        };
        public static readonly UpgradeType CorrosiveChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Corrosive Chance") { chanceMult = 0.6f, valueMult = 0.37f, canBeRolled = () => Game.instance.generation > 15 };
        public static readonly UpgradeType ViralChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Viral Chance") { chanceMult = 0.5f, valueMult = 0.8f };
        public static readonly UpgradeType ColdChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Slow Chance") { valueMult = 0.13f, canBeRolled = () => Game.instance.generation > 24 && Player.localInstance.upgradeManager.coldChance < 0.13f };
        public static readonly UpgradeType AbilityStrength = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Ability Strength") { valueMult = 2f, chanceMult = 0.75f, canBeRolled = () => !Player.localInstance.abilityManager.ability1.ConstantValues };
        public static readonly UpgradeType AbilityDuration = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Ability Duration") { valueMult = 1.15f, chanceMult = 0.5f, canBeRolled = () => !Player.localInstance.abilityManager.ability1.ConstantValues };
        public static readonly UpgradeType NthShotMultishot = new UpgradeType(x => $"Every 4th shot has +{MathHelper.ToPercentAndRound(x)}% Total Multishot")
        {
            chanceMult = 0.2f,
            getBaseRandomValue = () => 1f
        };
        public static readonly UpgradeType PlusDamageMinusFireRate = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Damage, -{MathHelper.ToPercentAndRound(x / 2)}% Fire Rate")
        {
            chanceMult = 0.2f,
            canBeRolled = () => Player.localInstance.shootManager.shootInterval < 1.4f,
            getBaseRandomValue = () => 0.9f
        };
        public static readonly UpgradeType MaxHpPerKill = new UpgradeType(x => $"On Kill: +{MathHelper.ToPercentAndRound(x)}% Max Hp")
        {
            chanceMult = 0.33f,
            canBeRolled = () => Game.instance.generation < 10,
            getBaseRandomValue = () => 0.02f
        };
        public static readonly UpgradeType Turret = new UpgradeType(x => $"Summon a Turret")
        {
            chanceMult = 0.1f,
            canBeRolled = () => Game.instance.generation > (NetworkManager.Singleplayer ? 30 : 36) &&
                PlayerHooks.GetHooks<TurretPlayer>(Player.localInstance).TurretCount < 2
        };
        public static readonly UpgradeType DamagePerUniqueStatus = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Total Damage per Unique Status Effect")
        {
            chanceMult = 0.2f,
            canBeRolled = () => Game.instance.generation > 45 && PlayerHooks.GetLocalHooks<DamagePerUniqueStatusPlayer>().damageIncreasePerUniqueStatus < 0.35f,
            valueMult = 0.3f
        };
        public static readonly UpgradeType FireRateMinusMaxHp = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Fire Rate, -{MathHelper.ToPercentAndRound(x / 2)}% Max Hp")
        {
            chanceMult = 0.2f,
            canBeRolled = () => Game.instance.generation > 10 && Player.localInstance.hpManager.maxHp > 75f,
            valueMult = 1.75f
        };
        public static readonly UpgradeType LowEnemyHpDamage = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Total Damage if Enemy HP is lower than 50%")
        {
            chanceMult = 0.2f,
            canBeRolled = () => Game.instance.generation > 35,
            valueMult = 1.6f
        };
        public static readonly UpgradeType MultishotPerShot = new UpgradeType(x => $"On Shot: +{MathHelper.ToPercentAndRound(x)}% Total Multishot, stacks up to {MultishotPerShotPlayer.MAX_STACK} times")
        {
            chanceMult = 0.2f,
            getBaseRandomValue = () => 0.01f
        };
        public static readonly UpgradeType FirstShotTotalDamage = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Total Damage on First Shot")
        {
            chanceMult = 0.2f,
            getBaseRandomValue = () => 2f
        };
        public static readonly UpgradeType FirstHitCritDamage = new UpgradeType(x => $"+{(int)x}x Total Crit Dmg on First Hit")
        {
            chanceMult = 0.25f,
            getBaseRandomValue = () => 15f,
            canBeRolled = () => PlayerHooks.GetLocalHooks<FirstShotPlayer>().damageMult > 1f
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
            Turret,
            CorrosiveChance,
            DamagePerUniqueStatus,
            FireRateMinusMaxHp,
            LowEnemyHpDamage,
            MultishotPerShot,
            FirstShotTotalDamage
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