using Godot;
using RogueDefense;

public class UpgradeScreen : Panel
{
	[Export]
	public PackedScene upgradeButtonScene;
	public void Activate()
	{
		Show();
        buttons = new UpgradeButton[]
        {
            upgradeButtonScene.Instance() as UpgradeButton,
            upgradeButtonScene.Instance() as UpgradeButton,
            upgradeButtonScene.Instance() as UpgradeButton
        };
        upgrades = new Upgrade[]
        {
            Upgrade.RandomUpgrade(),
            Upgrade.RandomUpgrade(),
            Upgrade.RandomUpgrade(),
        };
        for (int i = 0; i < buttons.Length; i++)
        {
            UpgradeButton butt = buttons[i];

            AddChild(butt);
            int index = i;
            butt.onClick = () => OnButtonClicked(index);
            butt.Text = upgrades[i].ToString();
        }
        buttons[1].RectPosition += new Vector2(-200, 0);
        buttons[2].RectPosition += new Vector2(200, 0);
    }
    UpgradeButton[] buttons;
    Upgrade[] upgrades;
    void OnButtonClicked(int index)
    {
        GD.Print($"Button {index} clicked");
        Game.instance.player.upgradeManager.AddUpgrade(upgrades[index]);
        foreach (var butt in buttons)
        {
            butt.QueueFree();
        }
        Hide();
        GetTree().Paused = false;
    }
}