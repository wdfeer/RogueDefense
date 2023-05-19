using Godot;

namespace RogueDefense
{
    public class PlayerHpManager
    {
        readonly Player player;
        public PlayerHpManager(Player player)
        {
            this.player = player;
            hpBar = DefenseObjective.instance.GetNode("./HpBar") as ProgressBar;
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
            if (SaveData.killCount > 25)
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
        }
        public bool dead = false;
        public void Death(bool local = true)
        {
            if (local && !NetworkManager.Singleplayer)
            {
                Client.instance.SendMessage(MessageType.Death);
            }

            SaveData.gameCount++;
            SaveData.Save();

            Game.instance.GetTree().Paused = true;
            DeathScreen.instance.Show();
            (DeathScreen.instance.GetNode("ScoreLabel") as Label).Text = $"Level {Game.instance.generation} reached";
        }
    }
}
