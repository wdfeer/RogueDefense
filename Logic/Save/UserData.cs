using System;

namespace RogueDefense.Logic.Save;

public static class UserData
{
    public static string name = "";
    public static string lastIp = "";
    public static bool[] clientSettings = { true, true, true, false, true, true, false, false };

    public static bool ShowCombatText
    {
        get => clientSettings[0];
        set => clientSettings[0] = value;
    }

    public static bool ShowHpBar
    {
        get => clientSettings[1];
        set => clientSettings[1] = value;
    }

    public static bool ShowAvgDPS
    {
        get => clientSettings[2];
        set => clientSettings[2] = value;
    }

    public static bool ShowFPS
    {
        get => clientSettings[3];
        set => clientSettings[3] = value;
    }

    public static bool Music
    {
        get => clientSettings[4];
        set => clientSettings[4] = value;
    }

    public static bool Sound
    {
        get => clientSettings[5];
        set => clientSettings[5] = value;
    }

    public static int highscoreSingleplayer = 0;
    public static int highscoreMultiplayer = 0;
    public static int gameCount = 0;
    public static int killCount = 0;
    public static float[] topPP = new float[] { 0f, 0f, 0f };
    public static int[] augmentAllotment = new int[] { 0, 0, 0, 0, 0 };
    private static int spareAugmentPoints = 10;

    public static int SpareAugmentPoints
    {
        get => spareAugmentPoints; set
        {
            updateAugmentPointCounter(value);
            spareAugmentPoints = value;
        }
    }

    public static Action<int> updateAugmentPointCounter = _ => { };
}