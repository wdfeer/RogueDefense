using System;
using System.Linq;
using static RogueDefense.Logic.Save.UserData;

namespace RogueDefense.Logic.Save;

public static class Augmenter
{
    public static int GetNextPPThreshold(float pp)
    {
        // TODO: make it keep going forever instead of four hardcoded threshholds
        switch (pp)
        {
            case <10:
                return 10;
            case <25:
                return 25;
            case <50:
                return 50;
            case <100:
                return 100;
        }

        return int.MaxValue;
    }
    public static int GetTotalAugmentCount()
    {
        int stages = Math.Max(highscoreSingleplayer, highscoreMultiplayer) / 10;
        var fromStages = stages * (stages + 1) / 2;

        int fromPP = 0;
        int i = GetNextPPThreshold(0);
        var ppAchieved = topPP.Max();
        while (i < ppAchieved)
        {
            i = GetNextPPThreshold(i);
            fromPP++;
        }
        
        return fromStages + fromPP;
    }
}