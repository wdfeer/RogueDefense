using System;
using System.Collections.Generic;

namespace RogueDefense.Logic;

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

    public static T GetRandomElement<T>(IList<T> list)
    {
        return list[(int)(GD.Randi() % list.Count)];
    }

    public static byte BoolArrayToByte(bool[] boolArray)
    {
        if (boolArray.Length != 8)
            throw new ArgumentException("The array must contain exactly 8 elements.");

        byte result = 0;

        for (int i = 0; i < 8; i++)
        {
            if (boolArray[i])
            {
                result |= (byte)(1 << i);
            }
        }

        return result;
    }

    public static bool[] ByteToBoolArray(byte b)
    {
        bool[] boolArray = new bool[8];

        for (int i = 0; i < 8; i++)
        {
            boolArray[i] = (b & (1 << i)) != 0;
        }

        return boolArray;
    }

}
