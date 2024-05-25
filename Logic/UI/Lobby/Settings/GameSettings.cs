using Godot;
using RogueDefense.Logic;

public static class GameSettings
{
    public static float totalDmgMult = 1f;
    public static float totalFireRateMult = 1f;
    public static bool healthDrain = false;
    public static void SendSettings()
    {
        if (NetworkManager.mode != NetMode.Server) return;

        Client.instance.SendMessage(MessageType.UpdateSettings, new string[]
        {
            totalDmgMult.ToString("0.0"),
            totalFireRateMult.ToString("0.0"),
            healthDrain ? "1" : "0"
        });
    }
    public static void ReceiveSettings(string[] args)
    {
        totalDmgMult = args[0].ToFloat();
        totalFireRateMult = args[1].ToFloat();
        healthDrain = args[2] == "1";
        UpdateSliders();
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
