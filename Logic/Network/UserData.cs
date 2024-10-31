using System.Linq;

namespace RogueDefense.Logic.Network;

public class UserData
{
    public int id;
    public string name;
    public int ability;
    public int[] augmentPoints;
    public static string AugmentPointsAsString(int[] augments) => string.Join("/", augments.Select(x => x.ToString()));
    public static int[] AugmentPointsFromString(string str) => str.Split("/").Select(x => int.Parse(x)).ToArray();
    public UserData(int id, string name, int ability, int[] augments)
    {
        this.id = id;
        this.name = name;
        this.ability = ability;
        augmentPoints = augments;
    }
}