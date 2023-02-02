using System;
using Godot;

namespace RogueDefense
{
    public static class UserSaveData
    {
        public const string SAVE_PATH = "user://user.txt";
        public static bool Load()
        {
            try
            {
                Godot.File file = new File();
                file.Open(SAVE_PATH, Godot.File.ModeFlags.Read);

                name = file.GetLine();
                lastIp = file.GetLine();
                showCombatText = file.GetLine() == "1";
                int.TryParse(file.GetLine(), out highscoreSingleplayer);
                int.TryParse(file.GetLine(), out highscoreMultiplayer);

                file.Close();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        public static void Save()
        {
            File file = new File();
            file.Open(SAVE_PATH, File.ModeFlags.Write);

            file.StoreLine(name);
            file.StoreLine(lastIp);
            file.StoreLine(showCombatText ? "1" : "0");
            file.StoreLine(highscoreSingleplayer.ToString());
            file.StoreLine(highscoreMultiplayer.ToString());

            file.Close();
        }
        public static string name = "";
        public static string lastIp = "";
        public static bool showCombatText = false;
        public static int highscoreSingleplayer = 0;
        public static int highscoreMultiplayer = 0;
        public static void UpdateHighscore()
        {
            int lvl = Game.instance.generation;
            if (NetworkManager.Singleplayer && lvl > highscoreSingleplayer)
                highscoreSingleplayer = lvl;
            else if (lvl > highscoreMultiplayer)
                highscoreMultiplayer = lvl;
        }
    }
}