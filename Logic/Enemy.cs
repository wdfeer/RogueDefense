
using Godot;
using RogueDefense.Logic;

public class Enemy : MovingKinematicBody2D
{
    public override void _Ready()
    {
        velocity = new Vector2(-1.15f, 0);
        var gen = Game.instance.generation;
        maxHp = 6.9f * Mathf.Pow(1f + gen * 0.4f, gen / 10f);
        Hp = maxHp;
        damage = 10f * Mathf.Sqrt(1f + gen);
    }

    public float maxHp;
    private float hp;
    public float Hp
    {
        get => hp; set
        {
            hp = value;
            (GetNode("./HpBar") as ProgressBar).Value = hp / maxHp;
            (GetNode("./HpBar/HpText") as Label).Text = $"{hp.ToString("0.0")} / {maxHp.ToString("0.0")}";
        }
    }
    [Export]
    public PackedScene combatText;
    public void Damage(float damage, Color textColor)
    {
        Hp -= damage;
        Label dmgText = combatText.Instance() as CombatText;
        GetNode("/root/Game").AddChild(dmgText);
        dmgText.Modulate = textColor;
        dmgText.Text = damage.ToString("0.0");
        dmgText.SetGlobalPosition(GlobalPosition + new Vector2(-80 + GD.Randf() * 80, -120));
        if (Hp <= 0)
        {
            Game.instance.DeleteEnemy();
        }
    }
    public float damage = 10f;
    public float attackInterval = 1f;
    float attackTimer = 0f;
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (attacking)
        {
            attackTimer += delta;
            if (attackTimer > attackInterval)
            {
                attackTimer = 0f;
                Game.instance.myPlayer.hpManager.Damage(damage);
            }
        }
    }
    bool attacking = false;

    protected override void OnCollision(KinematicCollision2D collision)
    {
        if (collision.Collider == Game.instance.myPlayer)
        {
            attacking = true;
        }
    }
}
