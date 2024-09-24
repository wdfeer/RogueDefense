using Godot;
using System.Collections.Generic;
using System.Linq;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.Network;
using RogueDefense.Logic.PlayerHooks;
using RogueDefense.Logic.PlayerHooks.Upgrades;
using RogueDefense.Logic.PlayerProjectile;

namespace RogueDefense.Logic.PlayerCore;

public partial class Player
{
    public bool IsLocal => id == Client.myId;
    public int id;
    public string Name => IsLocal ? SaveData.name : Client.instance.GetUserData(id).name;

    public ShootManager shootManager;
    public UpgradeManager upgradeManager;
    public AbilityManager abilityManager;
    public int[] augmentPoints;
    public Player(int id, int[] augmentPoints)
    {
        this.id = id;
        this.augmentPoints = augmentPoints;

        InitializeHooks();

        PlayerManager.players.Add(id, this);
        shootManager = new ShootManager(this);
        upgradeManager = new UpgradeManager(this);
        abilityManager = new AbilityManager(this);
        
        SpawnTurret();
    }

    public void _Process(double delta)
    {
        hooks.ForEach(x => x.PreUpdate((float)delta));
        upgradeManager.Process((float)delta);
        hooks.ForEach(x => x.PostUpgradeUpdate((float)delta));
        shootManager.Process((float)delta);
        hooks.ForEach(x => x.PostUpdate((float)delta));

        if (target == null || !GodotObject.IsInstanceValid(target) || target.Dead)
            FindTarget();
    }
    
    public void _PhysicsProcess(double delta)
    {
        if (this == PlayerManager.my)
            UpdateMovement(delta);
    }

    public void OnWaveEnd()
    {
        shootManager.ClearBullets(x => x is FusedBullet);
        hooks.ForEach(x => x.OnWaveEnd());
        upgradeManager.UpdateUpgrades();
        upgradeManager.UpdateUpgradeText();
        shootManager.shootCount = 0;
    }
}
