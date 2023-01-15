using Godot;
using RogueDefense.Logic;
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
        public AbilityManager abilityManager;
        public override void _Ready()
        {
            hpManager = new PlayerHpManager(this);
            shootManager = new PlayerShootManager(this);
            upgradeManager = new PlayerUpgradeManager(this);
            abilityManager = new AbilityManager(this);
        }
        public override void _Process(float delta)
        {
            shootManager.Process(delta);
            upgradeManager.Process(delta);
            abilityManager.Process(delta);
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
                OnDeath();
            }
        }
        bool dead = false;
        private void OnDeath()
        {
            if (!dead)
            {
                GD.Print($"I am dead!!!");
                dead = true;
            }
        }
    }
    public class PlayerShootManager
    {
        readonly Player player;
        public PlayerShootManager(Player player)
        {
            this.player = player;
        }
        public float baseDamage = 1;
        public float damage = 1;
        public float baseShootInterval = 1;
        public float shootInterval = 1;
        float timeSinceLastShot = 0;
        public float baseMultishot = 1f;
        public float multishot = 1f;
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
        private const float SPREAD_DEGREES = 16f;
        private void CreateBullet()
        {
            int bulletCount = MathHelper.RandomRound(multishot);
            for (int i = 0; i < bulletCount; i++)
            {
                Bullet bullet = player.bulletScene.Instance() as Bullet;
                bullet.velocity = new Godot.Vector2(2.5f, 0).Rotated(Mathf.Deg2Rad(GD.Randf() * SPREAD_DEGREES - SPREAD_DEGREES / 2f));
                bullet.Position = player.Position + new Godot.Vector2(20, 0);
                bullet.damage = damage;
                Game.instance.AddChild(bullet);
                bullets.Add(bullet);
            }
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
            UpdateUpgrades();
            UpdateUpgradeText();
        }

        public void Process(float delta)
        {
            UpdateUpgrades();
        }

        List<Upgrade> upgrades = new List<Upgrade>();
        public void AddUpgrade(Upgrade upgrade)
        {
            upgrades.Add(upgrade);
            UpdateUpgrades();
            UpdateUpgradeText();
        }

        public float critChance = 0f;
        public const float BASE_CRIT_DAMAGE = 2f;
        public float critDamage = BASE_CRIT_DAMAGE;
        void UpdateUpgrades()
        {
            float baseDamageMult = GetTotalUpgradeMultiplier(UpgradeType.BaseDamage);
            player.shootManager.baseDamage = 1f * baseDamageMult;

            float baseFireRateMult = GetTotalUpgradeMultiplier(UpgradeType.BaseFireRate);
            player.shootManager.baseShootInterval = 1f / baseFireRateMult;

            float baseMultishotMult = GetTotalUpgradeMultiplier(UpgradeType.BaseMultishot);
            player.shootManager.baseMultishot = 1f * baseMultishotMult;


            float hpMult = GetTotalUpgradeMultiplier(UpgradeType.MaxHp);
            player.hpManager.maxHp = PlayerHpManager.BASE_MAX_HP * hpMult;

            float damageDividor = GetTotalUpgradeMultiplier(UpgradeType.DamageDivision);
            player.hpManager.damageMult = 1f / damageDividor;

            float fireRateMult = GetTotalUpgradeMultiplier(UpgradeType.FireRate);
            player.shootManager.shootInterval = player.shootManager.baseShootInterval / fireRateMult;

            float damageMult = GetTotalUpgradeMultiplier(UpgradeType.Damage);
            player.shootManager.damage = player.shootManager.baseDamage * damageMult;

            float multishotMult = GetTotalUpgradeMultiplier(UpgradeType.Multishot);
            player.shootManager.multishot = player.shootManager.baseMultishot * multishotMult;

            critChance = GetAllUpgradeValues(UpgradeType.CritChance).Aggregate(0f, (a, b) => a + b);
            critDamage = BASE_CRIT_DAMAGE * GetTotalUpgradeMultiplier(UpgradeType.CritDamage);
        }
        IEnumerable<float> GetAllUpgradeValues(UpgradeType type)
    => upgrades.Where(x => x.type == type).Select(x => x.value);
        float GetTotalUpgradeMultiplier(UpgradeType type)
            => 1f + GetAllUpgradeValues(type).Aggregate(0f, (a, b) => a + b);

        void UpdateUpgradeText()
        {
            var upgradeText = player.GetNode("/root/Game/UpgradeScreen/UpgradeText") as Label;
            upgradeText.Text = $"Max HP: {player.hpManager.maxHp.ToString("0.0")}\n";
            if (player.hpManager.damageMult != 1f)
                upgradeText.Text += $"Damage Reduction: {(1 - player.hpManager.damageMult) * 100f}%\n";
            upgradeText.Text += $@"
Damage: {player.shootManager.damage.ToString("0.00")}
Fire Rate: {(1f / player.shootManager.shootInterval).ToString("0.00")}
Multishot: {player.shootManager.multishot.ToString("0.00")}

Critical Chance: {(critChance * 100f).ToString("0.0")}%
Critical Multiplier: {critDamage.ToString("0.00")}";
        }
    }
}
