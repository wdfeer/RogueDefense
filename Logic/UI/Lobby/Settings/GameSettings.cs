using Godot;
using System;

public static class GameSettings
{
    public static float totalDmgMult = 1f;
    public static void SyncSettings()
    {
        if (NetworkManager.mode != NetMode.Server) return;

        Client.instance.SendMessage(MessageType.UpdateSettings, new string[] { totalDmgMult.ToString("0.0") });
    }


    public static void UpdateFromSliders()
    {
        totalDmgMult = (float)Sliders.dmgMult.Slider.Value;
    }
    public static void UpdateSliders()
    {
        Sliders.dmgMult.SetValue(totalDmgMult);
    }
}
