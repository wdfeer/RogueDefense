using Godot;
using RogueDefense;

public class UpgradeScreen : Panel
{
    [Export]
    public PackedScene upgradeButtonScene;
    public void Activate()
    {
        Show();
        buttons = new CustomButton[]
        {
            upgradeButtonScene.Instance() as CustomButton,
            upgradeButtonScene.Instance() as CustomButton,
            upgradeButtonScene.Instance() as CustomButton
        };
        bool basedUpgrades = Game.instance.generation % 5 == 0;
        upgrades = new Upgrade[]
        {
            Upgrade.RandomUpgrade(basedUpgrades),
            Upgrade.RandomUpgrade(basedUpgrades),
            Upgrade.RandomUpgrade(basedUpgrades),
        };
        for (int i = 0; i < buttons.Length; i++)
        {
            CustomButton butt = buttons[i];

            AddChild(butt);
            int index = i;
            butt.onClick = () => OnButtonClicked(index);
            butt.Text = upgrades[i].ToString();
        }
        buttons[0].RectPosition += new Vector2(-240, -40);
        buttons[1].RectPosition += new Vector2(0, -40);
        buttons[2].RectPosition += new Vector2(240, -40);
    }
    CustomButton[] buttons;
    Upgrade[] upgrades;
    void OnButtonClicked(int index)
    {
        GD.Print($"Button {index} clicked");
        Game.instance.myPlayer.upgradeManager.AddUpgrade(upgrades[index]);
        foreach (var butt in buttons)
        {
            butt.QueueFree();
        }
        Hide();
        GetTree().Paused = false;
    }
}