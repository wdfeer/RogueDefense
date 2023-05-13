using System;
using System.Linq;
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
                int.TryParse(file.GetLine(), out gameCount);
                int.TryParse(file.GetLine(), out killCount);
                for (int i = 0; i < contributedUpgradePoints.Length; i++)
                {
                    contributedUpgradePoints[i] = (int)file.Get64();
                    SpareUpgradePoints -= contributedUpgradePoints[i];
                }

                file.Close();
                return true;
            }
            catch (Exception exception)
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
            file.StoreLine(gameCount.ToString());
            file.StoreLine(killCount.ToString());
            for (int i = 0; i < contributedUpgradePoints.Length; i++)
            {
                file.Store64((ulong)contributedUpgradePoints[i]);
            }

            file.Close();
        }
        public static string name = "";
        public static string lastIp = "";
        public static bool showCombatText = false;
        public static int highscoreSingleplayer = 0;
        public static int highscoreMultiplayer = 0;
        public static int gameCount = 0;
        public static int killCount = 0;
        public static int[] contributedUpgradePoints = new int[] { 0, 0, 0 };
        private static int spareUpgradePoints = 10;
        public static int SpareUpgradePoints
        {
            get => spareUpgradePoints; set
            {
                onUpgradePointCountChanged(value);
                spareUpgradePoints = value;
            }
        }
        public static Action<int> onUpgradePointCountChanged = (value) => { };

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