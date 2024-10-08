namespace RogueDefense.Logic.Enemy;

public class EnemySound
{
    readonly Enemy enemy;
    AudioStreamPlayer2D deathSound;
    public EnemySound(Enemy enemy)
    {
        this.enemy = enemy;
        InitializeDeathSound();
    }
    void InitializeDeathSound()
    {
        deathSound = new();
        enemy.AddSibling(deathSound);
        deathSound.Stream = (AudioStream)GD.Load("res://Assets/SFX/kill.wav");
        deathSound.ProcessMode = Node.ProcessModeEnum.Always;
        deathSound.Bus = "SFX";
        deathSound.Finished += deathSound.QueueFree;
    }
    public void PlayDeathSound()
    {
        deathSound.Position = enemy.Position;
        deathSound.PitchScale += GD.Randf() * 0.1f;
        deathSound.Play();
    }
}
