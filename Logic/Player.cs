using Godot;
using System.Collections.Generic;
using System.Linq;

namespace RogueDefense
{
    public class Player
    {
        public static Player my;
        public bool local => id == Client.myId;
        public static Dictionary<int, Player> players = new Dictionary<int, Player>();
        public int id;
        public string Name => local ? UserSaveData.name : Client.instance.GetUserData(id).name;

        public List<PlayerHooks> hooks = new List<PlayerHooks>() { new HpResetter(), new DpsCounterPlayer(), new StatusPlayer(), new FirstShotPlayer(), new NthShotMultishotPlayer(), new MaxHpPerKillPlayer(), new DamagePerUniqueStatusPlayer(), new LowEnemyHpDamagePlayer(), new MultishotPerShotPlayer() };
        public ShootManager shootManager;
        public UpgradeManager upgradeManager;
        public AbilityManager abilityManager;
        public Player(int id)
        {
            this.id = id;
            Player.players.Add(id, this);
            shootManager = new ShootManager(this);
            upgradeManager = new UpgradeManager(this);
            abilityManager = new AbilityManager(this);
            SpawnTurret();
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
