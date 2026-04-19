using System;
using System.Linq;
using RogueDefense.Logic.Enemy;
using RogueDefense.Logic.Network;
using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Save;
using RogueDefense.Logic.UI.InGame;
using RogueDefense.Logic.UI.Lobby.Settings;
using UserData = RogueDefense.Logic.Save.UserData;

namespace RogueDefense.Logic;

public partial class DefenseObjective : Node2D
{
    public static DefenseObjective instance;

    [Export] public PackedScene projectileManagerScene;
    [Export] public PackedScene turretScene;

    public override void _Ready()
    {
        instance = this;

        hpBar = GetNode<ProgressBar>("./HpBar");
        hpBar.Visible = UserData.clientSettings.ShowHpBar;

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

    public float evasionChance = 0f;
    public float damageMult = 1f;
    public float flatDamageReduction = 0f;

    public void Damage(float dmg)
    {
        if (GD.Randf() < evasionChance)
        {
            CombatTextDisplay.instance.AddCombatText(new CombatText()
            {
                direction = Vector2.Up * 1.5f,
                modulate = Colors.GreenYellow,
                position = GlobalPosition + Vector2.Up * 60 + 10 * Vector2.Right * (GD.Randf() - 0.5f),
                text = "MISS"
            });
            return;
        }

        dmg *= damageMult;

        // hits still deal 1 damage even if flatDamageReduction infinite
        if (dmg > 1f)
        {
            dmg = MathF.Max(1f, dmg - flatDamageReduction);
        }

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
        else
        {
            var corrosiveOnDamageTaken =
                PlayerManager.my.upgradeManager.SumEveryonesUpgradeValues(UpgradeType.CorrosiveOnDamageTaken);
            if (corrosiveOnDamageTaken > 0f)
            {
                Enemy.Enemy.enemies.ForEach(it =>
                {
                    for (int i = 0; i < corrosiveOnDamageTaken; i++)
                    {
                        it.corrosive.Add(5);
                    }
                });
            }
        }
    }

    public override void _Process(double delta)
    {
        hpBar.Visible = UserData.clientSettings.ShowHpBar;
        if (hpBar.Visible)
        {
            hpBar.Value = HpRatio;
            if (HpRatio < 0.5f) hpBar.Modulate = Colors.Red;
            else hpBar.Modulate = Colors.White;
        }

        sprite.Frame = GetSpriteFrame();

        foreach (var pair in PlayerManager.players)
        {
            pair.Value._Process(delta);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (var pair in PlayerManager.players)
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

        DeathScreen.instance.Activate();

        PP.TryRecordPP();
        UserData.gameCount++;
        SaveManager.Save();
    }
}