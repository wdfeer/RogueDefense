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
            if (x < 1e3f)
                return x.ToString("0.0");
            else if (x < 1e6f)
                return (x / 1e3f).ToString("0.0") + "K";
            else if (x < 1e9f)
                return (x / 1e6f).ToString("0.0") + "M";
            else
                return (x / 1e9f).ToString("0.0") + "B";
        }
    }
}
