using System;
using System.Collections.Generic;
using System.Linq;
using RogueDefense.Logic.Enemy;
using RogueDefense.Logic.Save;
using RogueDefense.Logic.UI.Lobby.Settings;

namespace RogueDefense.Logic;

public class PP
{
    public static void UpdateLobbyPPMultDisplay()
    {
        ((Label)Network.Lobby.Instance.GetNode("PPMult")).Text =
            "pp Multiplier: " + GetGameSettingsPPMult().ToString("0.000");
    }

    public static float GetGameSettingsPPMult()
    {
        float result = 1f;
        result /= GameSettings.totalDmgMult > 1f ? GameSettings.totalDmgMult : Mathf.Sqrt(GameSettings.totalDmgMult);
        result /= GameSettings.totalFireRateMult > 1f
            ? Mathf.Pow(GameSettings.totalFireRateMult, 1.25f)
            : Mathf.Sqrt(GameSettings.totalFireRateMult);
        if (GameSettings.healthDrain)
            result *= 1.6f;
        return result;
    }

    public static float currentPP = 0f;

    public static float GetWavePP(int gen, int modifierCount, int immunityCount, int enemyCount)
    {
        float settingsMult = GetGameSettingsPPMult();
        
        float genMult;
        if (gen <= 25)
            genMult = Mathf.Pow(gen / 25f, 1.5f);
        else
            genMult = gen / 25f;
        if (EnemySpawner.IsBossWave(gen))
            genMult += 1f;

        float countMult = MathF.Sqrt(enemyCount);

        float modifierMult = 1 + modifierCount * 0.1f;

        float immunityMult = 1 + immunityCount * 0.15f;

        return settingsMult * genMult * countMult * modifierMult * immunityMult;
    }


    public static bool TryRecordPP()
    {
        if (currentPP < UserData.topPP.Last())
            return false;

        List<float> newTopPP = UserData.topPP.Concat([currentPP]).ToList();
        newTopPP.Sort((x, y) => y.CompareTo(x));

        UserData.topPP = newTopPP.Take(3).ToArray();
        return true;
    }

    public static float GetTotalPP()
    {
        float result = 0f;
        float mult = 1f;
        for (int i = 0; i < UserData.topPP.Length; i++)
        {
            result += UserData.topPP[i] * mult;
            mult /= 2f;
        }

        return result;
    }
}