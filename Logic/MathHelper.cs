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
    }
}
