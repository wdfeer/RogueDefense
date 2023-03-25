using Godot;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RogueDefense
{
    public class DefenseObjective : Node2D
    {
        public static DefenseObjective instance;

        [Export]
        public PackedScene bulletScene;
        [Export]
        public PackedScene turretScene;

        public override void _Ready()
        {
            instance = this;

            hpBar = GetNode("./HpBar") as ProgressBar;
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
        private ProgressBar hpBar;

        public float damageMult = 1f;
        public void Damage(float dmg)
        {
            Hp -= dmg * damageMult;
            if (Hp <= 0)
            {
                Death();
            }
        }
        public override void _Process(float delta)
        {
            if (UserSaveData.killCount > 25)
            {
                float dps = 6;
                if (!NetworkManager.Singleplayer) dps *= 2f;
                if (Game.instance.generation > 40) dps *= 2f;
                if (Game.instance.generation > 25) dps *= 2f;
                Damage(delta * dps);
            }

            float hpOfMaxHp = hp / maxHp;
            hpBar.Value = hpOfMaxHp;
            if (hpOfMaxHp < 0.5f) hpBar.Modulate = Colors.Red;
            else hpBar.Modulate = Colors.White;

            foreach (var pair in Player.players)
            {
                pair.Value._Process(delta);
            }
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
