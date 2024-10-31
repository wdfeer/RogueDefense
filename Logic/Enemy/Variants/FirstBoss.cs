using System.Linq;
using RogueDefense.Logic.Enemy.Statuses;

namespace RogueDefense.Logic.Enemy.Variants;

public partial class FirstBoss : Enemy
{
	public override float GetBaseSpeed()
		=> (Hp / maxHp) < 0.4f ? 2f : 1f;

	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp *= 1.5f;
		maxHp += 50f;
	}
	protected override void ModifyImmunities(ref Status[] statuses)
	{
		Viral viral = (Viral)statuses.FirstOrDefault(x => x is Viral, null);
		if (viral != null)
		{
			viral.immune = false;
		}
	}
}