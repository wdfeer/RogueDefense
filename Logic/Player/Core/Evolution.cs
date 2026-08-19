using System.Linq;
using System.Text;

namespace RogueDefense.Logic.Player.Core;

[GlobalClass]
public partial class Evolution : Resource
{
    public static Evolution[] evolutions = ResourceLoader.ListDirectory("res://Data/Evolutions")
        .Select((path) => ResourceLoader.Load<Evolution>(path)).ToArray();

    [Export] public string name;
    [Export] public float damage = 1;
    [Export] public float fireRate = 1;
    [Export] public float multishot = 1;
    [Export] public float spread = 1;

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"{name}:");
        if (damage != 1f)
            builder.AppendLine($"{damage:1.0}x Damage");
        if (fireRate != 1f)
            builder.AppendLine($"{damage:1.0}x Fire Rate");
        if (multishot != 1f)
            builder.AppendLine($"{damage:1.0}x Multishot");
        if (spread != 1f)
            builder.AppendLine($"{spread:1.0}x Spread");
        return builder.ToString();
    }
}