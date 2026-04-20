using System;
using System.Linq;
using static RogueDefense.Logic.Save.UserData;

namespace RogueDefense.Logic.Save;

public static class AugmentBalance
{
    public static int GetNextPPThreshold(float pp)
    {
        // TODO: make it keep going forever instead of four hardcoded threshholds
        int[] thresholds = [10, 20, 30, 40, 50, 60, 80, 100, 130, 160, 200, 250, 300, 400, 500, 1000];
        foreach (var val in thresholds)
            if (pp < val)
                return val;

        return int.MaxValue;
    }

    public static int GetTotalAugmentCount()
    {
        int stages = Math.Max(highscoreSingleplayer, highscoreMultiplayer) / 10;
        int fromStages = stages;

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