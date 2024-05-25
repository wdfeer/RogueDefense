using Godot;

public partial class PlayerData : Panel
{
	public Label NameLabel => GetNode("./Name") as Label;
	public void SetPlayerName(string name)
	{
		NameLabel.Text = name;
		CallDeferred("SetNodeName", new[] { name });
	}
	private void SetNodeName(string name)
	{
		Name = name;
	}
	public Label AbilityLabel => GetNode("./AbilityLabel") as Label;
	public void SetAbilityText(string text)
	{
		AbilityLabel.Text = text;
	}
	public Label AbilityPointsLabel => GetNode<Label>("AugmentPoints");
	public void SetAugmentPoints(int amount)
	{
		AbilityPointsLabel.Text = amount + " AP";
	}
}
