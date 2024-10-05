using System;
using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.Save;

public struct UserData
{
    public UserData() { }
    
    public string name = "";
    public string lastIp = "";

    public int highscoreSingleplayer = 0;
    public int highscoreMultiplayer = 0;
    public int gameCount = 0;
    public int killCount = 0;
    public float[] topPP = { 0f, 0f, 0f };
    public int[] augmentAllotment = { 0, 0, 0, 0, 0 };
    private int spareAugmentPoints = 10;

    public int SpareAugmentPoints
    {
        get => spareAugmentPoints; set
        {
            updateAugmentPointCounter(value);
            spareAugmentPoints = value;
        }
    }

    public Action<int> updateAugmentPointCounter = _ => { };
    
    public void UpdateHighscore()
    {
        int lvl = Game.Wave;
        if (NetworkManager.Singleplayer && lvl > highscoreSingleplayer)
            highscoreSingleplayer = lvl;
        else if (lvl > highscoreMultiplayer) highscoreMultiplayer = lvl;
    }
    
    public const string PATH = "user://data.json";
}