using System;
using Godot;
using RogueDefense.Logic.Network;

namespace RogueDefense.Logic;

public static class SaveData
{
    public const string SAVE_PATH = "user://user.txt";
    public static bool Load()
    {
        try
        {
            FileAccess file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);

            name = file.GetLine();
            lastIp = file.GetLine();
            clientSettings = MathHelper.ByteToBoolArray(file.Get8());
            int.TryParse(file.GetLine(), out highscoreSingleplayer);
            int.TryParse(file.GetLine(), out highscoreMultiplayer);
            int.TryParse(file.GetLine(), out gameCount);
            int.TryParse(file.GetLine(), out killCount);

            for (int i = 0; i < topPP.Length; i++)
            {
                topPP[i] = file.GetFloat();
            }

            LoadAugments(file);

            file.Close();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    static void LoadAugments(FileAccess file)
    {
        int stages = Math.Max(highscoreSingleplayer, highscoreMultiplayer) / 10;
        SpareAugmentPoints = stages * (stages + 1) / 2;
        for (int i = 0; i < augmentAllotment.Length; i++)
        {
            augmentAllotment[i] = (int)file.Get64();
            SpareAugmentPoints -= augmentAllotment[i];
        }
        if (SpareAugmentPoints < 0)
        {
            augmentAllotment = new int[augmentAllotment.Length];
            SpareAugmentPoints = stages * (stages + 1) / 2;
        }
    }
    public static void Save()
    {
        FileAccess file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);

        file.StoreLine(name);
        file.StoreLine(lastIp);
        file.Store8(MathHelper.BoolArrayToByte(clientSettings));
        file.StoreLine(highscoreSingleplayer.ToString());
        file.StoreLine(highscoreMultiplayer.ToString());
        file.StoreLine(gameCount.ToString());
        file.StoreLine(killCount.ToString());
        for (int i = 0; i < topPP.Length; i++)
        {
            file.StoreFloat(topPP[i]);
        }
        for (int i = 0; i < augmentAllotment.Length; i++)
        {
            file.Store64((ulong)augmentAllotment[i]);
        }

        file.Close();
    }
    public static void UpdateHighscore()
    {
        int lvl = Game.Wave;
        if (NetworkManager.Singleplayer && lvl > highscoreSingleplayer)
            highscoreSingleplayer = lvl;
        else if (lvl > highscoreMultiplayer)
            highscoreMultiplayer = lvl;
    }


    public static string name = "";
    public static string lastIp = "";

    public static bool[] clientSettings = new bool[8] { true, true, true, false, true, true, false, false };
    public static bool ShowCombatText
    {
        get { return clientSettings[0]; }
        set { clientSettings[0] = value; }
    }
    public static bool ShowHpBar
    {
        get { return clientSettings[1]; }
        set { clientSettings[1] = value; }
    }
    public static bool ShowAvgDPS
    {
        get { return clientSettings[2]; }
        set { clientSettings[2] = value; }
    }
    public static bool ShowFPS
    {
        get { return clientSettings[3]; }
        set { clientSettings[3] = value; }
    }
    public static bool Music
    {
        get { return clientSettings[4]; }
        set { clientSettings[4] = value; }
    }
    public static bool Sound
    {
        get { return clientSettings[5]; }
        set { clientSettings[5] = value; }
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
    public static Action<int> updateAugmentPointCounter = (value) => { };
}