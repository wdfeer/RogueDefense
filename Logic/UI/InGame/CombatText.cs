
namespace RogueDefense.Logic.UI.InGame;

public class CombatText
{
    public CombatText()
    {
        queuedForDeletion = false;
    }

    public Vector2 position;
    public Vector2 direction;
    public Color modulate;
    public string text;
    public int fontSize = 15;

    private static Font Font => ThemeDB.FallbackFont;
    private const float SPEED = 50;

    public void Draw(CanvasItem canvas)
    {
        canvas.DrawString(Font, position, text, HorizontalAlignment.Center, fontSize: fontSize, modulate: modulate);
    }

    public const float LIFESPAN = 0.75f;
    public void Process(float delta)
    {
        position += direction * SPEED * delta;
        modulate.A -= delta;
        if (modulate.A < 0)
            queuedForDeletion = true;
    }
    public bool queuedForDeletion;
}
