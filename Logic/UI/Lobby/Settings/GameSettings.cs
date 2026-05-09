using RogueDefense.Logic.Network;
using RogueDefense.Logic.Network.Messages;

namespace RogueDefense.Logic.UI.Lobby.Settings;

public static class GameSettings
{
    public static float totalDmgMult = 1f;
    public static float totalFireRateMult = 1f;
    public static bool healthDrain;
    public static void SendSettings()
    {
        if (NetworkManager.mode != NetMode.Server) return;

        Client.instance.SendMessage(MessageType.UpdateSettings, new UpdateSettingsMessage()
        {
            damage = totalDmgMult,
            fireRate = totalFireRateMult,
            healthDrain = healthDrain,
        });
    }

    public static void UpdateFromSliders()
    {
        totalDmgMult = (float)Sliders.dmgMult.Slider.Value;
        totalFireRateMult = (float)Sliders.fireRateMult.Slider.Value;
        healthDrain = Sliders.healthDrain.Slider.Value > 0;
        PP.UpdateLobbyPPMultDisplay();
    }
    public static void UpdateSliders()
    {
        Sliders.dmgMult.SetValue(totalDmgMult);
        Sliders.fireRateMult.SetValue(totalFireRateMult);
        Sliders.healthDrain.SetValue(healthDrain ? 1 : 0);
        PP.UpdateLobbyPPMultDisplay();
    }
}