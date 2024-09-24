using System;
using Godot;
using RogueDefense.Logic.Network;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense.Logic.PlayerHooks;

public abstract class ActiveAbility : PlayerHooks
{
    Button button;
    public ActiveAbility(Player player, Button button) : base(player)
    {
        if (button == null)
            return;
        this.button = button;
        this.button.Pressed += OnButtonClick;
    }
    void OnButtonClick()
    {
        if (!CanBeActivated()) return;
        cooldownTimer = 0;
        ActivateTryShare();
        if (!NetworkManager.Singleplayer) NetSendActivation();
    }
    public virtual bool CanBeActivated() => true;
    public void ActivateTryShare()
    {
        if (Shared)
            ActivateForceShare();
        else
            Activate();
    }
    public abstract void Activate();
    public virtual bool Shared => true;
    public void ActivateForceShare()
    {
        foreach (var item in Player.players)
        {
            item.Value.hooks.ForEach(x =>
            {
                if (x is ActiveAbility ability && ability.GetType() == this.GetType())
                {
                    ability.Activate();
                }
            });
        }
    }
    public void NetSendActivation()
    {
        Network.Client.instance.SendMessage(MessageType.AbilityActivated, new string[] { Network.Client.myId.ToString(), GetAbilityIndex().ToString() });
    }

    public virtual float BaseCooldown => 25f;
    public float Cooldown => BaseCooldown * player.abilityManager.cooldownMult;
    public void ResetCooldown() => cooldownTimer = float.PositiveInfinity;
    protected float cooldownTimer = float.PositiveInfinity;
    float TimeRemaining => Cooldown - cooldownTimer;
    public bool Cooling => cooldownTimer < Cooldown;
    public override void PreUpdate(float delta)
    {
        if (button == null || !player.Local)
            return;

        button.Disabled = Cooling;
        Label cooldownLabel = button.GetNode<Label>("CooldownLabel");
        if (Cooling)
        {
            cooldownTimer += (float)delta;
            cooldownLabel.Text = TimeRemaining.ToString("0.0");
            cooldownLabel.Modulate = (TimeRemaining / Cooldown) > 0.5f ? Colors.DarkRed : Colors.Yellow;
        }
        else
        {
            cooldownLabel.Text = "";
            cooldownLabel.Modulate = Colors.White;
        }
    }

    public float Strength => player.abilityManager.strengthMult;
    public float Duration => player.abilityManager.durationMult;

    public void ResetText()
    {
        var label = button.GetNode("./Label") as Label;
        label.Text = GetAbilityText();
        if (ConstantValues) label.Text += "\n\n(These values are constant)";
    }
    protected abstract string GetAbilityText();
    public virtual bool ConstantValues => false;

    public string GetName()
        => AbilityManager.GetAbilityName(GetAbilityIndex());
    public int GetAbilityIndex()
    {
        return Array.IndexOf(AbilityManager.abilityTypes, GetType());
    }
}