using Godot;
using System.Collections.Generic;
using System.Linq;

namespace RogueDefense
{
    public class Player
    {
        public static Player my;
        public bool Local => id == Client.myId;
        public static Dictionary<int, Player> players;
        public int id;
        public string Name => Local ? UserSaveData.name : Client.instance.GetUserData(id).name;

        public List<PlayerHooks> hooks;
        public ShootManager shootManager;
        public UpgradeManager upgradeManager;
        public AbilityManager abilityManager;
        public Player(int id)
        {
            hooks = new List<PlayerHooks>() { new DpsCounterPlayer(this), new StatusPlayer(this), new FirstShotPlayer(this), new FirstHitPlayer(this), new NthShotMultishotPlayer(this), new MaxHpPerKillPlayer(this), new DamagePerUniqueStatusPlayer(this), new LowEnemyHpDamagePlayer(this), new MultishotPerShotPlayer(this) };

            this.id = id;
            Player.players.Add(id, this);
            shootManager = new ShootManager(this);
            upgradeManager = new UpgradeManager(this);
            abilityManager = new AbilityManager(this);
            SpawnTurret();

            if (Local) hooks.Add(new HpResetter(this));
        }
        public void _Process(float delta)
        {
            hooks.ForEach(x => x.PreUpdate(delta));
            upgradeManager.Process(delta);
            hooks.ForEach(x => x.PostUpgradeUpdate(delta));
            shootManager.Process(delta);
            hooks.ForEach(x => x.PostUpdate(delta));
        }

        public List<Turret> turrets = new List<Turret>();
        public void SpawnTurret()
        {
            Turret turret = DefenseObjective.instance.turretScene.Instance<Turret>();
            DefenseObjective.instance.AddChild(turret);
            turret.Position += new Vector2(-50f + GD.Randf() * 200f, (GD.Randf() - 0.5f) * 300);
            turrets.Add(turret);

            shootManager.bulletSpawns.Add(turret.GlobalPosition);

            turret.SetLabel(string.Concat(Name.Take(3)).ToUpper());
        }
    }
}
