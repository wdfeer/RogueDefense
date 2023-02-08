using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueDefense.Logic
{
    internal static class MathHelper
    {
        public static int RandomRound(float value)
        {
            int result = Mathf.FloorToInt(value);
            if (GD.Randf() < value % 1)
            {
                result++;
            }
            return result;
        }
        public static int ToPercentAndRound(float f)
        {
            return Mathf.RoundToInt(f * 100f);
        }

        public static string ToShortenedString(float x)
        {
            if (x < 1000)
                return x.ToString("0.0");
            else if (x < 1000000)
                return (x / 1000f).ToString("0.0") + "K";
            else
                return (x / 1000000f).ToString("0.0") + "M";
        }
    }
}
