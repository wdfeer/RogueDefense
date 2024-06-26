using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace RogueDefense.Logic.PlayerCore;

public partial class UpgradeType
{
    public UpgradeType(Func<float, string> upgradeTextGetter)
    {
        getUpgradeText = upgradeTextGetter;
    }
    public Func<float, string> getUpgradeText;
    public bool status = false;
    public float chanceMult = 1f;
    public float valueMult = 1f;
    public float GetRandomValue()
        => getBaseRandomValue() * valueMult;
    Func<float> getBaseRandomValue = () => GD.Randf() * 0.055f + 0.2f;
    public Func<bool> canBeRolled = () => true;

    public int uniqueId;

    public static readonly UpgradeType MaxHp = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)} Max Hp")
    {
        getBaseRandomValue = () => GD.Randf() * 0.05f + 0.01f * (10 + Game.Wave / 2)
    };
    public static readonly UpgradeType DamageReduction = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Damage Reduction") { valueMult = 0.455f };
    public static readonly UpgradeType Evasion = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Evasion")
    {
        chanceMult = 0.5f,
        valueMult = 0.52f,
        canBeRolled = () => Game.Wave > 15
    };
    public static readonly UpgradeType Damage = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Damage") { chanceMult = 1.1f, valueMult = 1.2f };
    public static readonly UpgradeType FireRate = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Fire Rate");
    public static readonly UpgradeType Multishot = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Multishot");
    public static readonly UpgradeType CritChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Crit Chance")
    {
        canBeRolled = () => Player.my.upgradeManager.critChance < 1.25f
    };
    public static readonly UpgradeType CritDamage = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Crit Damage")
    {
        canBeRolled = () => Player.my.upgradeManager.critChance > 0
    };
    public static readonly UpgradeType BleedChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Bleed Chance")
    {
        status = true,
        getBaseRandomValue = () => 0.16f,
        canBeRolled = () => Player.my.upgradeManager.bleedChance < 0.25f * AugmentContainer.GetStatMult(3),
    };
    public static readonly UpgradeType CorrosiveChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Corrosive Chance")
    {
        status = true,
        chanceMult = 0.6f,
        valueMult = 0.37f,
        canBeRolled = () => Game.Wave > 15
    };
    public static readonly UpgradeType ViralChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Viral Chance")
    {
        status = true,
        chanceMult = 0.5f,
        valueMult = 0.7f
    };
    public static readonly UpgradeType ColdChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Slow Chance")
    {
        status = true,
        valueMult = 0.13f,
        canBeRolled = () => Game.Wave > 24 && Player.my.upgradeManager.coldChance < 0.13f
    };
    public static readonly UpgradeType AbilityStrength = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Ability Strength") { valueMult = 2f, chanceMult = 0.75f, canBeRolled = () => !Player.my.abilityManager.ability1.ConstantValues };
    public static readonly UpgradeType AbilityDuration = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Ability Duration") { valueMult = 1.15f, chanceMult = 0.5f, canBeRolled = () => !Player.my.abilityManager.ability1.ConstantValues };
    public static readonly UpgradeType NthShotMultishot = new UpgradeType(x => $"Every 4th shot has +{MathHelper.ToPercentAndRound(x)}% Total Multishot")
    {
        chanceMult = 0.2f,
        getBaseRandomValue = () => 1f,
        canBeRolled = () => Player.my.upgradeManager.SumAllUpgradeValues(NthShotMultishot) < 1.5f
    };
    public static readonly UpgradeType PlusDamageMinusFireRate = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Damage, -{MathHelper.ToPercentAndRound(x / 2)}% Fire Rate")
    {
        chanceMult = 0.2f,
        canBeRolled = () => Player.my.shootManager.shootInterval < 1.4f,
        getBaseRandomValue = () => 0.9f
    };
    public static readonly UpgradeType MaxHpPerKill = new UpgradeType(x => $"On Kill: +{MathHelper.ToPercentAndRound(x)} Max Hp")
    {
        chanceMult = 0.33f,
        canBeRolled = () => Game.Wave < 12 || GD.Randf() < 0.3f,
        getBaseRandomValue = () => 0.01f
    };
    public static readonly UpgradeType Turret = new UpgradeType(x => $"Summon a Turret")
    {
        chanceMult = 0.25f,
        canBeRolled = () => Game.Wave > (NetworkManager.Singleplayer ? 22 : 36) &&
            (float)Player.my.turrets.Count / NetworkManager.PlayerCount < 1.9f
    };
    public static readonly UpgradeType DamagePerUniqueStatus = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Total Damage per Unique Status Effect")
    {
        chanceMult = 0.2f,
        canBeRolled = () => Game.Wave > 45 && PlayerHooks.GetLocalHooks<DamagePerUniqueStatusPlayer>().IncreasePerStatus < 0.35f,
        valueMult = 0.3f
    };
    public static readonly UpgradeType FireRateMinusMultishot = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Fire Rate, -{MathHelper.ToPercentAndRound(x / 2)}% Multishot")
    {
        chanceMult = 0.2f,
        canBeRolled = () => Game.Wave > 10 && Player.my.shootManager.multishot > 1f,
        valueMult = 2.2f
    };
    public static readonly UpgradeType LowEnemyHpDamage = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Total Damage if Enemy HP is lower than 50%")
    {
        chanceMult = 0.2f,
        canBeRolled = () => Game.Wave > 35,
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
        canBeRolled = () => PlayerHooks.GetLocalHooks<FirstShotPlayer>().DamageMult > 1f
    };
    public static readonly UpgradeType TotalDamageVsArmor = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Total Damage vs Armored Enemy")
    {
        chanceMult = 0.33f,
        getBaseRandomValue = () => (20 + SaveData.augmentAllotment[0]) / 100f,
        canBeRolled = () => Game.Wave > 15 && PlayerHooks.GetLocalHooks<DamageVsArmorPlayer>().Mult < 1.1f
    };
    public static readonly UpgradeType ExplosionChance = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Explosion Chance")
    {
        chanceMult = 0.25f,
        getBaseRandomValue = () => 0.33f,
        canBeRolled = () => Game.Wave > 30 && PlayerHooks.GetLocalHooks<ExplosionPlayer>().Chance < 0.5f
    };
    public static readonly UpgradeType ExplosionRadius = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Explosion Radius")
    {
        chanceMult = 0.35f,
        getBaseRandomValue = () => 0.3f,
        canBeRolled = () => PlayerHooks.GetLocalHooks<ExplosionPlayer>().Chance > 0 && PlayerHooks.GetLocalHooks<ExplosionPlayer>().RadiusMult < 1.5f
    };
    public static readonly UpgradeType RecoverySpeed = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)}% Recovery Speed")
    {
        chanceMult = 0.22f,
        getBaseRandomValue = () => 0.5f,
        canBeRolled = () => Game.Wave > 12 && Player.my.upgradeManager.SumAllUpgradeValues(RecoverySpeed) < 1f
    };
    public static readonly UpgradeType CritChanceOnStunned = new UpgradeType(x => $"On Stunned: +{MathHelper.ToPercentAndRound(x)}% Critical Chance for {CritChanceOnStunnedPlayer.DURATION} Seconds")
    {
        chanceMult = 0.15f,
        getBaseRandomValue = () => 0.5f,
        canBeRolled = () => Game.Wave > 18
    };
    public static readonly UpgradeType RecoilDamage = new UpgradeType(x => $"+{MathHelper.ToPercentAndRound(x)} Recoil\n+{MathHelper.ToPercentAndRound(x)}% Total Damage")
    {
        chanceMult = 0.15f,
        getBaseRandomValue = () => 0.2f,
        canBeRolled = () => Player.my.shootManager.Recoil == 0
    };
    public static UpgradeType[] AllTypes = new UpgradeType[] {
        MaxHp,
        DamageReduction,
        Evasion,
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
        FireRateMinusMultishot,
        LowEnemyHpDamage,
        MultishotPerShot,
        FirstShotTotalDamage,
        TotalDamageVsArmor,
        ExplosionChance,
        ExplosionRadius,
        RecoverySpeed,
        CritChanceOnStunned,
        RecoilDamage
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
        => obj is UpgradeType && ((UpgradeType)obj).uniqueId == uniqueId;



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