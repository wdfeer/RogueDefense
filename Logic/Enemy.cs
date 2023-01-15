
using Godot;
using RogueDefense.Logic;

public class Enemy : MovingKinematicBody2D
{
    public override void _Ready()
    {
        velocity = new Vector2(-1.1f, 0);
        var gen = Game.instance.generation;
        maxHp = 6f * Mathf.Pow(1f + gen * 0.5f, gen / 8f);
        Hp = maxHp;
    }

    public float maxHp;
    private float hp;
    public float Hp
    {
        get => hp; set
        {
            hp = value;
            (GetNode("./HpBar") as ProgressBar).Value = hp / maxHp;
        }
    }
    public void Damage(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            Game.instance.DeleteEnemy();
        }
    }
    public float dps = 10;
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (attacking)
            Game.instance.player.hpManager.Damage(dps * delta);
    }
    bool attacking = false;

    protected override void OnCollision(KinematicCollision2D collision)
    {
        if (collision.Collider == Game.instance.player)
        {
            attacking = true;
        }
    }
}
