using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Upgrade;

namespace RogueDefense
{
    public class Player : Node2D
    {
        [Export]
        public PackedScene bulletScene;

        public PlayerHpManager hpManager;
        public PlayerShootManager shootManager;
        public PlayerUpgradeManager upgradeManager;
        public override void _Ready()
        {
            hpManager = new PlayerHpManager(this);
            shootManager = new PlayerShootManager(this);
            upgradeManager = new PlayerUpgradeManager(this);
        }
        public override void _Process(float delta)
        {
            shootManager.Process(delta);    
        }

        public void Damage(float damage) { hpManager.Damage(damage); }
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
                hpBar.Value = (float)hp / maxHp;
            }
        }
        public const float BASE_MAX_HP = 100;
        public float maxHp = BASE_MAX_HP;
        private readonly ProgressBar hpBar;

        public void Damage(float value)
        {
            Hp -= value;
            if (Hp <= 0)
            {
                OnDeath();
            }
        }
        private void OnDeath()
        {
            GD.Print($"I am dead!!!");
        }
    }
    public class PlayerShootManager
    {
        readonly Player player;
        public PlayerShootManager(Player player)
        {
            this.player = player;
        }
        public const float BASE_DAMAGE = 1;
        public float damage = BASE_DAMAGE;
        public const float BASE_SHOOT_INTERVAL = 1;
        public float shootInterval = BASE_SHOOT_INTERVAL;
        float timeSinceLastShot = 0;
        public void Process(float delta)
        {
            timeSinceLastShot += delta;
            if (timeSinceLastShot > shootInterval)
            {
                timeSinceLastShot = 0;
                CreateBullet();
            }
        }
        private List<Bullet> bullets = new List<Bullet>();
        private void CreateBullet()
        {
            Bullet bullet = player.bulletScene.Instance() as Bullet;
            bullet.velocity = new Godot.Vector2(2.5f, 0);
            bullet.Position = player.Position + new Godot.Vector2(20, 0);
            bullet.damage = damage;
            Game.instance.AddChild(bullet);
            bullets.Add(bullet);
        }
        public void ClearBullets()
        {
            foreach (Bullet bull in bullets)
            {
                if (Godot.Object.IsInstanceValid(bull))
                {
                    bull.QueueFree();
                }
            }
            bullets = new List<Bullet>();
        }
    }
    public class PlayerUpgradeManager
    {
        readonly Player player;
        public PlayerUpgradeManager(Player player)
        {
            this.player = player;
        }

        List<Upgrade> upgrades = new List<Upgrade>();
        public void AddUpgrade(Upgrade upgrade)
        {
            upgrades.Add(upgrade);
            UpdateUpgrades();
        }
        void UpdateUpgrades()
        {
            float hpMult = GetTotalUpgradeMultiplier(UpgradeType.MaxHp);
            player.hpManager.maxHp = PlayerHpManager.BASE_MAX_HP * hpMult;
            player.hpManager.Hp = player.hpManager.maxHp;

            float fireRateMult = GetTotalUpgradeMultiplier(UpgradeType.FireRate);
            player.shootManager.shootInterval = PlayerShootManager.BASE_SHOOT_INTERVAL / fireRateMult;

            float damageMult = GetTotalUpgradeMultiplier(UpgradeType.Damage);
            player.shootManager.damage = PlayerShootManager.BASE_DAMAGE * damageMult;

            var upgradeText = Game.instance.GetNode("./UpgradeScreen/UpgradeText") as RichTextLabel;
            upgradeText.Text = $@"Max HP: {player.hpManager.maxHp.ToString("0.0")}

Damage: {player.shootManager.damage.ToString("0.00")}
Fire Rate: {(1f / player.shootManager.shootInterval).ToString("0.00")}";
        }
        float GetTotalUpgradeMultiplier(UpgradeType type)
        {
            return 1f + upgrades.Where(x => x.type == type).Select(x => x.value).Aggregate(0f, (a, b) => a + b);
        }
    }
}
