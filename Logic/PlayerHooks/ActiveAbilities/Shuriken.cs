using Godot;

namespace RogueDefense
{
    public class Shuriken : Bullet
    {
        public override void _Ready()
        {
            base._Ready();
            killShieldOrbs = true;
        }
        protected override void OnHit(float totalDmg)
        {
            Game.instance.enemy.AddBleed(totalDmg, 5f);
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            this.Rotate(delta * Mathf.Pi * 2);
        }

        protected override bool UnhideableDamageNumbers => true;
    }
}