using System;
using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class ArmorStripAbility : ActiveAbility
{
    public ArmorStripAbility(Player player, Button button) : base(player, button)
    {
    }
    public override void Activate()
    {
        foreach (Enemy enemy in Enemy.enemies)
        {
            ArmorStrip(enemy);
        }
    }
    void ArmorStrip(Enemy enemy)
    {
        float strip = Math.Min(Strip * enemy.armor, enemy.armor);
        enemy.armor -= strip;
        if (strip > 0)
            enemy.Damage(strip * Damage, true, Colors.Gold);
    }
    public const float BASE_STRIP = 0.5f;



    public override float BaseCooldown => (NetworkManager.Singleplayer ? 25f : 33f) / Mathf.Sqrt(Duration);
    public float Strip => BASE_STRIP * Mathf.Sqrt(Strength);
    float Damage => 1f + (Strength - 1) / Mathf.Min(Mathf.Sqrt(Strength), 2);
    protected override string GetAbilityText()
        => @$"Remove {Math.Min(100, MathHelper.ToPercentAndRound(Strip))}% Enemy Armor
Deal {MathHelper.ToPercentAndRound(Damage)}% of it as damage 
Cooldown: {Cooldown:0.00} s";
}