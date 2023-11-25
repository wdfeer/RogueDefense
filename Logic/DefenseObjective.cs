using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RogueDefense
{
    public partial class DefenseObjective : Node2D
    {
        public static DefenseObjective instance;

        [Export]
        public PackedScene bulletScene;
        [Export]
        public PackedScene turretScene;

        public override void _Ready()
        {
            instance = this;

            hpBar = GetNode<ProgressBar>("./HpBar");
            hpBar.Visible = SaveData.showHpBar;

            sprite = GetNode<Sprite2D>("./Sprite2D");

            Hp = maxHp;
        }


        private float hp;
        public float Hp
        {
            get => hp;
            set => hp = value;
        }
        public float HpRatio => Hp / maxHp;
        public const float BASE_MAX_HP = 100;
        public float maxHp = BASE_MAX_HP;
        private ProgressBar hpBar;

        private Sprite2D sprite;
        private int GetSpriteFrame()
        {
            int result = 4 - (int)(HpRatio / 0.2f);
            return result >= 0 ? result : 0;
        }

        public float damageMult = 1f;
        public float evasionChance = 0f;
        public void Damage(float dmg)
        {
            if (GD.Randf() < evasionChance)
                return;

            Hp -= dmg * damageMult;
            if (Hp <= 0)
            {
                Death();
            }
        }
        public override void _Process(double delta)
        {
            if (GameSettings.healthDrain)
                DealPassiveDamage((float)delta);

            hpBar.Visible = SaveData.showHpBar;
            if (hpBar.Visible)
            {
                hpBar.Value = HpRatio;
                if (HpRatio < 0.5f) hpBar.Modulate = Colors.Red;
                else hpBar.Modulate = Colors.White;
            }

            sprite.Frame = GetSpriteFrame();

            foreach (var pair in Player.players)
            {
                pair.Value._Process(delta);
            }
        }
        void DealPassiveDamage(float delta)
        {
            float dps = 6;
            if (!NetworkManager.Singleplayer) dps *= 2f;
            if (Game.Wave > 40) dps *= 2f;
            if (Game.Wave > 25) dps *= 2f;
            Damage(delta * dps);
        }

        public bool dead = false;
        public void Death(bool local = true)
        {
            if (local && !NetworkManager.Singleplayer)
            {
                Client.instance.SendMessage(MessageType.Death);
            }

            PP.TryRecordPP();
            SaveData.gameCount++;
            SaveData.Save();

            Game.instance.GetTree().Paused = true;
            DeathScreen.instance.Show();
            (DeathScreen.instance.GetNode("ScoreLabel") as Label).Text = $"Level {Game.Wave} reached\n{PP.currentPP.ToString("0.000")} pp";
        }
    }
}
