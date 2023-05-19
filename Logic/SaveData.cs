using System;
using System.Linq;
using Godot;

namespace RogueDefense
{
    public static class SaveData
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

                for (int i = 0; i < topPP.Length; i++)
                {
                    topPP[i] = file.GetFloat();
                }

                SpareAugmentPoints = 10;
                for (int i = 0; i < augmentAllotment.Length; i++)
                {
                    augmentAllotment[i] = (int)file.Get64();
                    SpareAugmentPoints -= augmentAllotment[i];
                }

                file.Close();
                return true;
            }
            catch (Exception)
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
            int lvl = Game.instance.generation;
            if (NetworkManager.Singleplayer && lvl > highscoreSingleplayer)
                highscoreSingleplayer = lvl;
            else if (lvl > highscoreMultiplayer)
                highscoreMultiplayer = lvl;
        }


        public static string name = "";
        public static string lastIp = "";
        public static bool showCombatText = false;
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
                try
                {
                    updateAugmentPointCounter(value);
                }
                catch (Exception) { }
                spareAugmentPoints = value;
            }
        }
        public static Action<int> updateAugmentPointCounter = (value) => { };
    }
}