using Godot;
using System.Collections.Generic;
using System.Linq;

namespace RogueDefense.Logic.PlayerCore
{
    public partial class Player
    {
        public static Player my;
        public bool Local => id == Client.myId;
        public static Dictionary<int, Player> players;
        public int id;
        public string Name => Local ? SaveData.name : Client.instance.GetUserData(id).name;

        public List<PlayerHooks> hooks;
        public ShootManager shootManager;
        public UpgradeManager upgradeManager;
        public AbilityManager abilityManager;
        public int[] augmentPoints;
        public Player(int id, int[] upgradePointDistribution)
        {
            augmentPoints = upgradePointDistribution;

            hooks = new List<PlayerHooks>() { new DpsCounterPlayer(this), new StatusPlayer(this), new FirstShotPlayer(this), new FirstHitPlayer(this), new NthShotMultishotPlayer(this), new MaxHpPerKillPlayer(this), new DamagePerUniqueStatusPlayer(this), new LowEnemyHpDamagePlayer(this), new MultishotPerShotPlayer(this) };

            this.id = id;
            players.Add(id, this);
            shootManager = new ShootManager(this);
            upgradeManager = new UpgradeManager(this);
            abilityManager = new AbilityManager(this);
            SpawnTurret();

            if (Local) hooks.Add(new HpResetter(this));
        }
        public void _Process(double delta)
        {
            hooks.ForEach(x => x.PreUpdate((float)delta));
            upgradeManager.Process((float)delta);
            hooks.ForEach(x => x.PostUpgradeUpdate((float)delta));
            shootManager.Process((float)delta);
            hooks.ForEach(x => x.PostUpdate((float)delta));

            if (target == null || target.Hp < 0)
                FindTarget();
        }
        public void _PhysicsProcess(double delta)
        {
            if (this == my)
                UpdateMovement(delta);
        }
        void UpdateMovement(double delta)
        {
            Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
            controlledTurret.GlobalPosition += inputDirection * Turret.SPEED;

            if (NetworkManager.Singleplayer)
                return;
            Vector2 pos = controlledTurret.GlobalPosition;
            SendPositionUpdateMessage(Client.myId, turrets.FindIndex(x => x == controlledTurret), pos.X, pos.Y);
        }
        static void SendPositionUpdateMessage(int client, int turretIndex, float x, float y)
        {
            Client.instance.SendMessage(MessageType.PositionUpdated, new string[] { client.ToString(), turretIndex.ToString(), x.ToString(), y.ToString() });
        }
        public List<Turret> turrets = new List<Turret>();
        public Turret controlledTurret;
        public Enemy target;
        void FindTarget()
        {
            for (int i = 0; i < Enemy.enemies.Count; i++)
            {
                Enemy enemy = Enemy.enemies[i];
                if (enemy.Hp > 0)
                {
                    SetTarget(i);
                    return;
                }
            }
        }
        public void SetTarget(int enemyIndex, bool netUpdate = true)
        {
            Enemy enemy = Enemy.enemies[enemyIndex];
            target = enemy;
            foreach (Turret turret in turrets)
            {
                turret.target = enemy;
            }

            if (netUpdate)
            {
                Client.instance.SendMessage(MessageType.TargetSelected, new string[] { Client.myId.ToString(), enemyIndex.ToString() });
            }
        }
        public void SpawnTurret()
        {
            controlledTurret = DefenseObjective.instance.turretScene.Instantiate<Turret>();
            DefenseObjective.instance.AddChild(controlledTurret);
            controlledTurret.Position += new Vector2(-50f + GD.Randf() * 200f, (GD.Randf() - 0.5f) * 300);
            turrets.Add(controlledTurret);

            shootManager.bulletSpawns.Add(controlledTurret.bulletSpawnpoint);

            controlledTurret.SetLabel(string.Concat(Name.Take(3)).ToUpper());

            controlledTurret.target = target;
        }

        public void OnEnemyKill()
        {
            shootManager.ClearBullets();
            hooks.ForEach(x => x.OnKill());
            upgradeManager.UpdateUpgrades();
            upgradeManager.UpdateUpgradeText();
            shootManager.shootCount = 0;
        }
    }
}
