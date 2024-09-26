using System.Collections.Generic;
using RogueDefense.Logic.Player.Hooks;
using RogueDefense.Logic.Player.Hooks.Upgrades;

namespace RogueDefense.Logic.Player.Core;

public partial class Player
{
    public List<PlayerHooks> hooks;

    private void InitializeHooks()
    {
        hooks = new List<PlayerHooks>()
        {
            new DpsCounterPlayer(this), new StatusPlayer(this), new FirstShotPlayer(this), new FirstHitPlayer(this),
            new NthShotMultishotPlayer(this), new MaxHpPerKillPlayer(this), new DamagePerUniqueStatusPlayer(this),
            new LowEnemyHpDamagePlayer(this), new MultishotPerShotPlayer(this), new DamageVsArmorPlayer(this),
            new ExplosionPlayer(this), new CritChanceOnStunnedPlayer(this)
        };
        
        if (IsLocal) hooks.Add(new HpResetter(this));
    }
}