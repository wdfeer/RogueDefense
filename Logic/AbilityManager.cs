using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueDefense
{
    public class AbilityManager
	{
		Player player;
		CustomButton ability1Button;

        public AbilityManager(Player player)
		{
			this.player = player;
			ability1Button = player.GetNode("/root/Game/AbilityContainer/AbilityButton1") as CustomButton;
			ability1Button.onClick = () =>
			{
				if (!cooldowns.Any(x => x.type == 1))
				{
					cooldowns.Add(new Cooldown() { type = 1, duration = 60 });
					buffs.Add(new Buff() { process = () => { this.player.shootManager.shootInterval /= 2f; },
						duration = 5 });
				}
			};
		}

		public List<Buff> buffs = new List<Buff>();
		public List<Cooldown> cooldowns = new List<Cooldown>();
		public void Process(float delta)
		{
			foreach (Buff b in buffs)
			{
				b.process();
				b.duration -= delta;
			}
			buffs = buffs.Where(x => x.duration > 0).ToList();

			foreach (Cooldown c in cooldowns)
			{
				c.duration -= delta;
				switch (c.type)
				{
					case 1:
                        ability1Button.Disabled = c.duration > 0;
						(ability1Button.GetNode("./Cooldown") as ProgressBar).Value = c.duration / 60f;
                        break;
					default:
						break;
				}
			}
            cooldowns = cooldowns.Where(x => x.duration > 0).ToList();
        }
		public class Buff
		{
			public float duration;
			public Action process;
		}
		public class Cooldown
		{
			public byte type;
			public float duration;
		}
    }
}
