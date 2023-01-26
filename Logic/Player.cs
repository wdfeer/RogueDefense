using Godot;
using RogueDefense.Logic;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RogueDefense
{
    public class Player : Node2D
    {
        public static Player localInstance;
        public static string myName = "Player123";

        [Export]
        public PackedScene bulletScene;

        public List<PlayerHooks> hooks = new List<PlayerHooks>();

        public PlayerHpManager hpManager;
        public PlayerShootManager shootManager;
        public PlayerUpgradeManager upgradeManager;
        public AbilityManager abilityManager;
        public override void _Ready()
        {
            localInstance = this;

            hpManager = new PlayerHpManager(this);
            shootManager = new PlayerShootManager(this);
            upgradeManager = new PlayerUpgradeManager(this);
            abilityManager = new AbilityManager(this);
        }
        public override void _Process(float delta)
        {
            hooks.ForEach(x => x.PreUpdate(delta));
            hooks.ForEach(x => x.Update(delta));
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
        public bool dead = false;
        public void Death(bool local = true)
        {
            if (local && !NetworkManager.Singleplayer)
            {
                Client.instance.SendMessage(MessageType.Death);
            }

            Game.instance.GetTree().Paused = true;
            ((Panel)Game.instance.GetNode("DeathScreen")).Show();
            ((Label)Game.instance.GetNode("DeathScreen/ScoreLabel")).Text = $"Level {Game.instance.generation + 1} reached";
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
}
