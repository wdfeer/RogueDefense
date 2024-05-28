using System.Linq;
using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class RecoveryPlayer : PlayerHooks
{
    public RecoveryPlayer(Player player) : base(player)
    {
    }

    public float recoverySpeed = 1f;
}