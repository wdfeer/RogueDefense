using Godot;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RogueDefense
{
    public class Player : Node2D
    {
        public static Player localInstance;

        [Export]
        public PackedScene bulletScene;
        [Export]
        public PackedScene turretScene;

        public List<PlayerHooks> hooks = new List<PlayerHooks>() { new DpsCounterPlayer(), new StatusPlayer(), new FirstHitPlayer(), new NthShotMultishotPlayer(), new MaxHpPerKillPlayer(), new TurretPlayer() };

        public PlayerHpManager hpManager;
        public ShootManager shootManager;
        public UpgradeManager upgradeManager;
        public AbilityManager abilityManager;
        public override void _Ready()
        {
            localInstance = this;

            hpManager = new PlayerHpManager(this);
            shootManager = new ShootManager(this);
            upgradeManager = new UpgradeManager(this);
            abilityManager = new AbilityManager(this);
        }
        public override void _Process(float delta)
        {
            hooks.ForEach(x => x.PreUpdate(delta));
            hooks.ForEach(x => x.Update(delta));
            hpManager.Process(delta);
            upgradeManager.Process(delta);
            hooks.ForEach(x => x.PostUpgradeUpdate(delta));
            shootManager.Process(delta);
            hooks.ForEach(x => x.PostUpdate(delta));
        }
    }
    public class PlayerHpManager
    {
        readonly Player player;
        public PlayerHpManager(Player player)
        {
            this.player = player;
            hpBar = player.GetNode("./HpBar") as ProgressBar;
            Hp = maxHp;
        }
        private float hp;
        public float Hp
        {
            get => hp;
            set
            {
                hp = value;
                hpBar.Value = hp / maxHp;
            }
        }
        public const float BASE_MAX_HP = 100;
        public float maxHp = BASE_MAX_HP;
        private readonly ProgressBar hpBar;

        public float damageMult = 1f;
        public void Damage(float dmg)
        {
            Hp -= dmg * damageMult;
            if (Hp <= 0)
            {
                Death();
            }
        }
        public void Process(float delta)
        {
            Damage(delta);
        }
        public bool dead = false;
        public void Death(bool local = true)
        {
            if (local && !NetworkManager.Singleplayer)
            {
                Client.instance.SendMessage(MessageType.Death);
            }

            UserSaveData.gameCount++;
            UserSaveData.Save();

            Game.instance.GetTree().Paused = true;
            DeathScreen.instance.Show();
            (DeathScreen.instance.GetNode("ScoreLabel") as Label).Text = $"Level {Game.instance.generation} reached";
        }
    }
}
