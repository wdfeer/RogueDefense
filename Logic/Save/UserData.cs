using System;

namespace RogueDefense.Logic.Save;

public static class UserData
{
    public static string name = "";
    public static string lastIp = "";
    public static ClientSettings clientSettings = new();

    public static int highscoreSingleplayer = 0;
    public static int highscoreMultiplayer = 0;
    public static int gameCount = 0;
    public static int killCount = 0;
    public static float[] topPP = { 0f, 0f, 0f };
    public static int[] augmentAllotment = { 0, 0, 0, 0, 0 };
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