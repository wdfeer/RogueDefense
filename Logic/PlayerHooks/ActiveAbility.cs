using System;
using Godot;

namespace RogueDefense
{
    public abstract class ActiveAbility : PlayerHooks
    {
        CustomButton button;
        public ActiveAbility(Player player, CustomButton button) : base(player)
        {
            if (button == null)
                return;
            this.button = button;
            this.button.onClick = OnButtonClick;
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
            Client.instance.SendMessage(MessageType.AbilityActivated, new string[] { Client.myId.ToString(), GetAbilityIndex().ToString() });
        }

        public virtual float BaseCooldown => 25f;
        public float Cooldown => BaseCooldown * player.abilityManager.cooldownMult;
        public void ResetCooldown() => cooldownTimer = float.PositiveInfinity;
        protected float cooldownTimer = float.PositiveInfinity;
        public bool Cooling => cooldownTimer < Cooldown;
        public override void PreUpdate(float delta)
        {
            if (button == null || !player.Local)
                return;

            button.Disabled = Cooling;
            (button.GetNode("./Cooldown") as ProgressBar).Value = (Cooldown - cooldownTimer) / Cooldown;
            if (Cooling)
            {
                cooldownTimer += delta;
            }
        }

        public float Strength => player.abilityManager.strengthMult;
        public float Duration => player.abilityManager.durationMult;

        public void ResetText()
        {
            var label = (button.GetNode("./Label") as Label);
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
}