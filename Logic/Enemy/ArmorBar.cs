using Godot;

namespace RogueDefense.Logic.Enemy;

public partial class ArmorBar : TextureProgressBar
{
    public void SetDisplay(float damageReduction)
    {
        if (damageReduction <= 0.01f)
            Hide();
        else
            Show();

        double range = MaxValue - MinValue;
        Value = damageReduction * range + MinValue;
    }
}