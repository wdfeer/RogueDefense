using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.Player.Hooks;

public abstract class PlayerHooks
{
	public PlayerHooks(Core.Player player)
	{
		this.player = player;
	}
	public Core.Player player;
	public virtual void PreUpdate(float delta) { }
	public virtual void PostUpgradeUpdate(float delta) { }
	public virtual void PostUpdate(float delta) { }
	public virtual void ModifyHitEnemyWithProj(Enemy.Enemy enemy, Projectile.Projectile p, ref float damagePreCrit, ref int critLevel, ref float critMult) { }
	public virtual void OnHitWithProj(Enemy.Enemy enemy, Projectile.Projectile p, float postCritDmg) { }
	public virtual void OnAnyHit(float afterEffectsDmg) { }
	public virtual void PreShoot(ShootManager shooter) { }
	public virtual void OnWaveEnd() { }
	public virtual void OnKill(Enemy.Enemy enemy) { }

	public static T GetLocalHooks<T>() where T : PlayerHooks
	{
		return GetHooks<T>(PlayerManager.my);
	}
	public static T GetHooks<T>(Core.Player player) where T : PlayerHooks
	{
		return (T)player.hooks.Find(x => x is T);
	}
}
