using RogueDefense.Logic.Network;
using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.UI.MainMenu;

public partial class AbilityChooser : MenuButton
{
	public PopupMenu popup;
	public override void _Ready()
	{
		popup = GetPopup();
		for (int i = 0; i < AbilityManager.abilityTypes.Length; i++)
		{
			string name = AbilityManager.GetAbilityName(i);
			popup.AddItem(name, i);
		}
		popup.Connect("id_pressed", new Callable(this, "IdPressed"));

		ResetButtonText();
	}
	public static int chosen = -1;
	public void IdPressed(int id)
	{
		chosen = id;
		Client.instance.SendMessage(MessageType.SetAbility, new string[] { Client.myId.ToString(), id.ToString() });
		ResetButtonText();
	}

	public void ResetButtonText()
	{
		Text = AbilityManager.GetAbilityName(chosen);
	}
}