using System;
using RogueDefense.Logic.Enemy.Statuses;
using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.Enemy;

public partial class Enemy
{
    public static float oneTimeHpMult = 1f;
    public static float oneTimeArmorMult = 1f;
    public static float oneTimeDamageMult = 1f;
    public static int oneTimeCountIncrease = 0;

    protected virtual void ModifyGen(ref int gen, int index)
    {
        gen = Math.Max(1, gen - index * 2);
    }

    void ScaleStats(int gen, int index)
    {
        ModifyGen(ref gen, index);
        ScaleMaxHp(gen);
        ScaleDamage(gen);
        ScaleArmor(gen);

        ResetImmunities(gen, index);

        ResetShieldOrbs(gen);
        ResetModifiers(gen);
    }

    void ResetImmunities(int gen, int index)
    {
        if (index != 0 && gen > 10 && statsRng.Randf() < 0.3f)
        {
            switch (statsRng.RandiRange(0, 2))
            {
                case 0 when gen > 30:
                    cold.immune = true;
                    break;
                case 1:
                    viral.immune = true;
                    break;
                case 2:
                    corrosive.immune = true;
                    break;
                default:
                    break;
            }
        }

        ModifyImmunities(ref statuses);
    }

    private ShieldOrbGenerator shieldOrbGenerator;

    void ResetShieldOrbs(int gen)
    {
        if (!ShieldOrbsAllowed) return;

        bool exploding = gen > 19;

        if (gen % 10 == 9)
        {
            shieldOrbGenerator.CreateOrbs(5, false);
            return;
        }

        if (gen % 2 == 0 && GD.Randf() < 0.5f)
            shieldOrbGenerator.CreateOrbs(1 + Mathf.RoundToInt(GD.Randf() * 4), exploding: exploding);
        else shieldOrbGenerator.count = 0;
    }

    public EnemyModifiers modifiers = new();

    void ResetModifiers(int gen)
    {
        if (!bleed.immune && !corrosive.immune)
        {
            float rand = statsRng.Randf();
            if (rand < 0.1f)
                modifiers = new EnemyModifiers(damageCap: EnemyModifiers.GetDamageCap(gen));
            else if (shieldOrbGenerator.count > 0 && gen > 15 && rand < 0.2f)
                modifiers = new EnemyModifiers(minDamage: EnemyModifiers.GetMinDamage(gen));
            modifiers.SetLabels(this);
        }
    }

    void ScaleMaxHp(int gen)
    {
        float baseMaxHp = 5f;
        float power;
        if (NetworkManager.Singleplayer)
            power = (gen <= 40f ? gen : 40f + Mathf.Pow(gen - 40, 0.75f)) / 20f;
        else
        {
            power = gen / 17.5f;
            baseMaxHp *= NetworkManager.PlayerCount;
        }

        maxHp = Mathf.Round(baseMaxHp * Mathf.Pow(1f + gen, power) * (0.8f + statsRng.Randf() * 0.4f)) * oneTimeHpMult;

        ModifyMaxHp(ref maxHp);

        Hp = maxHp;

        oneTimeHpMult = 1f;
    }

    void ScaleDamage(int gen)
    {
        damage = 11.5f * Mathf.Sqrt(1f + gen) * oneTimeDamageMult;
        if (NetworkManager.Singleplayer)
            damage *= 0.8f;

        ModifyDamage(ref damage);

        oneTimeDamageMult = 1f;
    }

    void ScaleArmor(int gen)
    {
        if (gen > 9)
            armor = (NetworkManager.Singleplayer ? 30f : (gen > 55 ? 150f : 75f)) * (gen - 9f) * oneTimeArmorMult;
        else
            armor = 0f;

        ModifyArmor(ref armor);

        oneTimeArmorMult = 1f;

        ResetArmorDisplay();
    }


    protected virtual void ModifyMaxHp(ref float maxHp)
    {
    }

    protected virtual void ModifyDamage(ref float damage)
    {
    }

    protected virtual void ModifyArmor(ref float armor)
    {
    }

    protected virtual void ModifyImmunities(ref Status[] statuses)
    {
    }
}