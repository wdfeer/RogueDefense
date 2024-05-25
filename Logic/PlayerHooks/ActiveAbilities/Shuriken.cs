using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
	public partial class Shuriken : Projectile
	{
		public override bool KillShieldOrbs => true;
		protected override void OnHit(Enemy enemy, float totalDmg)
		{
			enemy.AddBleed(totalDmg, 5f);
		}
		protected override bool UnhideableDamageNumbers => true;



		// private float spriteRotation = 0;
		private Texture2D texture;
		public override void Draw(CanvasItem drawer)
		{
			drawer.DrawTexture(texture, position);
		}
		public override void PhysicsProcess(float delta)
		{
			base.PhysicsProcess(delta);

			// spriteRotation += delta * Mathf.Pi * 2;
		}
	}
}
