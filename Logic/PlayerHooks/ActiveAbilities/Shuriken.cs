using Godot;

namespace RogueDefense
{
    public partial class Shuriken : Bullet
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

        public override void _Process(double delta)
        {
            base._Process(delta);

            this.Rotate((float)delta * Mathf.Pi * 2);
        }

        protected override bool UnhideableDamageNumbers => true;
    }
}