using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RogueDefense
{
    public class Player : Node2D
    {
        [Export]
        public PackedScene bulletScene;

        PlayerHpManager hpManager;
        PlayerShootManager shootManager;
        public override void _Ready()
        {
            hpManager = new PlayerHpManager(this);
            shootManager = new PlayerShootManager(this);
        }
        public void Damage(int damage) { hpManager.Damage(damage); }
        

        public override void _Process(float delta)
        {
            shootManager.Process(delta);    
        }
    }
    class PlayerHpManager
    {
        readonly Player player;
        public PlayerHpManager(Player player)
        {
            this.player = player;
            hpBar = player.GetNode("./HpBar") as ProgressBar;
            Hp = maxHp;
        }
        private int hp;
        public int Hp
        {
            get => hp;
            set
            {
                hp = value;
                hpBar.Value = (float)hp / maxHp;
            }
        }
        public int maxHp = 1000;
        private readonly ProgressBar hpBar;

        public void Damage(int value)
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
    class PlayerShootManager
    {
        readonly Player player;
        public PlayerShootManager(Player player)
        {
            this.player = player;
        }
        float shootInterval = 1;
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
        private void CreateBullet()
        {
            Bullet bullet = player.bulletScene.Instance() as Bullet;
            bullet.velocity = new Godot.Vector2(2.5f, 0);
            bullet.Position = player.Position;
            Game.instance.AddChild(bullet);
        }
    }
}
