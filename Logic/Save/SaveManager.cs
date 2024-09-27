using System;
using RogueDefense.Logic.Network;
using static RogueDefense.Logic.Save.UserData;

namespace RogueDefense.Logic.Save;

public static class SaveManager
{
    private const string SAVE_PATH = "user://user.txt";
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
        else if (lvl > highscoreMultiplayer) highscoreMultiplayer = lvl;
    }
}