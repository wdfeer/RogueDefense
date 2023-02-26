using Godot;
using System;

public class SettingsSlider : VBoxContainer
{
    public Label Label => GetNode("Label") as Label;
    public Slider Slider => GetNode("Slider") as Slider;
    public string defaultText;
    public double defaultValue;
    public override void _Ready()
    {
        defaultText = Label.Text;
        defaultValue = Slider.Value;
        UpdateLabel((float)defaultValue);
        Slider.Connect("value_changed", this, "UpdateLabel");
    }
    public void UpdateLabel(float value)
    {
        Label.Text = defaultText + value.ToString("0.0");
    }

    public void SetValue(float value)
    {
        Slider.Value = value;
        UpdateLabel(value);
    }
    public void Reset()
    {
        SetValue((float)defaultValue);
    }
}
