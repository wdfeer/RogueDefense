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

        public List<PlayerHooks> hooks = new List<PlayerHooks>() { new HpResetter(), new DpsCounterPlayer(), new StatusPlayer(), new FirstHitPlayer(), new NthShotMultishotPlayer(), new MaxHpPerKillPlayer(), new TurretPlayer(), new DamagePerUniqueStatusPlayer(), new LowEnemyHpDamagePlayer(), new MultishotPerShotPlayer() };

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
            upgradeManager.Process(delta);
            hooks.ForEach(x => x.PostUpgradeUpdate(delta));
            hpManager.Process(delta);
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
            set => hp = value;
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
            if (UserSaveData.killCount > 25)
            {
                float dps = 12;
                if (!NetworkManager.Singleplayer) dps *= 2f;
                if (Game.instance.generation > 40) dps *= 2f;
                Damage(delta * dps);
            }

            float hpOfMaxHp = hp / maxHp;
            hpBar.Value = hpOfMaxHp;
            if (hpOfMaxHp < 0.5f) hpBar.Modulate = Colors.Red;
            else hpBar.Modulate = Colors.White;
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
