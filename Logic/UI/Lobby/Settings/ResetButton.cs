using Godot;
using System;

public class ResetButton : Button
{
    public override void _Pressed()
    {
        VBoxContainer sliderContainer = (VBoxContainer)GetNode("../Sliders");
        int sliderCount = sliderContainer.GetChildCount();
        for (int i = 0; i < sliderCount; i++)
        {
            SettingsSlider slider = sliderContainer.GetChild<SettingsSlider>(i);
            slider.Reset();
        }

        GameSettings.UpdateFromSliders();
        GameSettings.SendSettings();
    }
}
