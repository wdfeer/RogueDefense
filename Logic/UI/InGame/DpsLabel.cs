using Godot;
using RogueDefense;
using RogueDefense.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DpsLabel : Label
{
    public static DpsLabel instance;
    public override void _Ready()
    {
        Visible = SaveData.showAvgDPS;
        instance = this;
    }

    public List<(float damage, float timeAgo)> hits = new List<(float damage, float timeAgo)>();
    float secondTimer = 0f;
    const float HIT_SAVE_DURATION = 1f;
    public override void _Process(double delta)
    {
        if (!Visible)
            return;

        secondTimer += (float)delta;
        if (secondTimer >= 1f)
        {
            UpdateShownDps();
            hits = hits.Select(x => (x.damage, x.timeAgo + 1)).Where(x => x.Item2 < HIT_SAVE_DURATION).ToList();
            secondTimer %= 1f;
        }
    }
    void UpdateShownDps()
    {
        float dps = hits.Aggregate(0f, (a, b) => a + b.damage) / HIT_SAVE_DURATION;
        Text = $"Avg DPS: {MathHelper.ToShortenedString(dps)}";
    }
}
