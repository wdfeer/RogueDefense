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

            file.Close();
        }
        public static string name = "default";
        public static string lastIp = "";
        public static bool showCombatText = false;
    }
}