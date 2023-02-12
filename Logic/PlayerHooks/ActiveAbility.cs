using System;
using Godot;

namespace RogueDefense
{
    public abstract class ActiveAbility : PlayerHooks
    {
        CustomButton button;
        public ActiveAbility(CustomButton button = null)
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
            Activate();
            if (!NetworkManager.Singleplayer) NetSendActivation();
        }
        public virtual bool CanBeActivated() => true;
        public abstract void Activate();
        public void NetSendActivation()
        {
            Client.instance.SendMessage(MessageType.AbilityActivated, new string[] { Client.myId.ToString(), GetAbilityIndex().ToString() });
        }

        public virtual float BaseCooldown => 25f;
        public float Cooldown => BaseCooldown * Player.abilityManager.cooldownMult;
        public void ResetCooldown() => cooldownTimer = float.PositiveInfinity;
        protected float cooldownTimer = float.PositiveInfinity;
        public bool Cooling => cooldownTimer < Cooldown;
        public override void PreUpdate(float delta)
        {
            if (button == null)
                return;

            button.Disabled = Cooling;
            (button.GetNode("./Cooldown") as ProgressBar).Value = (Cooldown - cooldownTimer) / Cooldown;
            if (Cooling)
            {
                cooldownTimer += delta;
            }
        }

        public float Strength => Player.abilityManager.strengthMult;
        public float Duration => Player.abilityManager.durationMult;

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