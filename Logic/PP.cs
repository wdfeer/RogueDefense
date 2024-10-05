using System;
using System.Collections.Generic;
using System.Linq;
using RogueDefense.Logic.Save;
using RogueDefense.Logic.UI.Lobby.Settings;

namespace RogueDefense.Logic;

public class PP
{
    public static void UpdateLobbyPPMultDisplay()
    {
        ((Label)Network.Lobby.Instance.GetNode("PPMult")).Text = "pp Multiplier: " + GetGameSettingsPPMult().ToString("0.000");
    }
    private static float GetGameSettingsPPMult()
    {
        float result = 1f;
        result /= GameSettings.totalDmgMult > 1f ? GameSettings.totalDmgMult : Mathf.Sqrt(GameSettings.totalDmgMult);
        result /= GameSettings.totalFireRateMult > 1f ? Mathf.Pow(GameSettings.totalFireRateMult, 1.25f) : Mathf.Sqrt(GameSettings.totalFireRateMult);
        if (GameSettings.healthDrain)
            result *= 1.6f;
        return result;
    }
    public static float currentPP = 0f;
    public static float GetWavePP(int gen, float hpRatio, int enemyCount)
    {
        float genMult;
        if (gen <= 25)
            genMult = Mathf.Pow(gen / 25f, 1.5f);
        else
            genMult = gen / 25f;

        return GetGameSettingsPPMult() * hpRatio * genMult * MathF.Sqrt(enemyCount);
    }


    public static bool TryRecordPP()
    {
        if (currentPP < SaveManager.user.topPP.Last())
            return false;

        List<float> newTopPP = SaveManager.user.topPP.Concat(new[] { currentPP }).ToList();
        newTopPP.Sort((x, y) => y.CompareTo(x));

        SaveManager.user.topPP = newTopPP.Take(3).ToArray();
        return true;
    }

    public static float GetTotalPP()
    {
        float result = 0f;
        float mult = 1f;
        foreach (var pp in SaveManager.user.topPP)
        {
            result += pp * mult;
            mult /= 2f;
        }
        return result;
    }
}