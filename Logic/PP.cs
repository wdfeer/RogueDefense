using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace RogueDefense.Logic
{
    public class PP
    {
        public static void UpdateLobbyPPMultDisplay()
        {
            ((Label)Lobby.Instance.GetNode("PPMult")).Text = "pp Multiplier: " + GetGameSettingsPPMult().ToString("0.000");
        }
        public static float GetGameSettingsPPMult()
        {
            float result = 1f;
            result /= GameSettings.totalDmgMult;
            result /= Mathf.Pow(GameSettings.totalFireRateMult, 1.25f);
            if (GameSettings.healthDrain)
                result *= 2f;
            return result;
        }
        public static float currentPP = 0f;
        public static float GetKillPP(int gen, float hpRatio)
        {
            float genMult;
            if (gen <= 25)
                genMult = Mathf.Pow(gen / 25f, 1.5f);
            else
                genMult = (gen / 25f);

            return GetGameSettingsPPMult() * hpRatio * genMult;
        }


        public static bool TryRecordPP()
        {
            if (currentPP < SaveData.topPP.Last())
                return false;

            List<float> newTopPP = SaveData.topPP.Concat(new float[] { currentPP }).ToList();
            newTopPP.Sort((x, y) => y.CompareTo(x));

            SaveData.topPP = newTopPP.Take(3).ToArray();
            return true;
        }
    }
}