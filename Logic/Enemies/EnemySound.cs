using Godot;

namespace RogueDefense.Logic.Enemies;

public class EnemySound
{
    readonly Enemy enemy;
    AudioStreamPlayer2D deathSound;
    public EnemySound(Enemy enemy)
    {
        this.enemy = enemy;

        deathSound = new();
        enemy.AddSibling(deathSound);
        deathSound.Stream = (AudioStream)GD.Load("res://Assets/SFX/kill.wav");
        deathSound.ProcessMode = Node.ProcessModeEnum.Always;
        deathSound.Bus = "SFX";
    }
    public void PlayDeathSound()
    {
        deathSound.Position = enemy.Position;
        deathSound.PitchScale += GD.Randf() * 0.1f;
        deathSound.Play();
        deathSound.Finished += deathSound.QueueFree;
    }
}
