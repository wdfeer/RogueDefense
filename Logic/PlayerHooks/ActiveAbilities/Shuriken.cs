using Godot;

namespace RogueDefense
{
    public class Shuriken : Bullet
    {
        protected override void OnHit(float totalDmg)
        {
            Game.instance.enemy.AddBleed(totalDmg, 5f * Player.localInstance.abilityManager.durationMult);
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            this.Rotate(delta * Mathf.Pi * 2);
        }
    }
}