using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class DefenseObjective : Node2D
{
    public static DefenseObjective instance;

    [Export]
    public PackedScene projectileManagerScene;
    [Export]
    public PackedScene turretScene;

    public override void _Ready()
    {
        instance = this;

        hpBar = GetNode<ProgressBar>("./HpBar");
        hpBar.Visible = SaveData.ShowHpBar;

        sprite = GetNode<Sprite2D>("./Sprite2D");

        Hp = maxHp;

        if (GameSettings.healthDrain)
            SetHealthDrainTimer();
    }


    private float hp;
    public float Hp
    {
        get => hp;
        set => hp = value;
    }
    public float HpRatio => Hp / maxHp;
    public const float BASE_MAX_HP = 100;
    public float maxHp = BASE_MAX_HP;
    private ProgressBar hpBar;

    private Sprite2D sprite;
    private int GetSpriteFrame()
    {
        int result = 4 - (int)(HpRatio / 0.2f);
        return result >= 0 ? result : 0;
    }

    public float damageMult = 1f;
    public float evasionChance = 0f;
    public void Damage(float dmg)
    {
        if (GD.Randf() < evasionChance)
            return;

        dmg *= damageMult;

        CombatTextDisplay.instance.AddCombatText(new CombatText()
        {
            direction = Vector2.Up * 1.5f,
            modulate = Colors.Red,
            position = GlobalPosition + Vector2.Up * 60,
            text = dmg.ToString("0.0")
        });

        Hp -= dmg;
        if (Hp <= 0)
        {
            Death();
        }
    }
    public override void _Process(double delta)
    {
        hpBar.Visible = SaveData.ShowHpBar;
        if (hpBar.Visible)
        {
            hpBar.Value = HpRatio;
            if (HpRatio < 0.5f) hpBar.Modulate = Colors.Red;
            else hpBar.Modulate = Colors.White;
        }

        sprite.Frame = GetSpriteFrame();

        foreach (var pair in Player.players)
        {
            pair.Value._Process(delta);
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        foreach (var pair in Player.players)
        {
            pair.Value._PhysicsProcess(delta);
        }
    }

    void SetHealthDrainTimer()
    {
        ToSignal(GetTree().CreateTimer(1, false), "timeout").OnCompleted(() =>
        {
            DrainHealth();
            SetHealthDrainTimer();
        });
    }
    void DrainHealth()
    {
        float dps = 6;
        if (!NetworkManager.Singleplayer) dps *= 1.5f;
        if (Game.Wave > 40) dps *= 2f;
        if (Game.Wave > 25) dps *= 2f;
        Damage(dps);
    }

    public bool dead = false;
    public void Death(bool local = true)
    {
        if (local && !NetworkManager.Singleplayer)
        {
            Client.instance.SendMessage(MessageType.Death);
        }

        PP.TryRecordPP();
        SaveData.gameCount++;
        SaveData.Save();

        Game.instance.GetTree().Paused = true;
        DeathScreen.instance.Show();
        (DeathScreen.instance.GetNode("ScoreLabel") as Label).Text = $"{Game.GetStage() - 1} Stages cleared\n{PP.currentPP.ToString("0.000")} pp";
    }
}
