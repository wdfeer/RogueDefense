namespace RogueDefense.Logic.Save;

public struct ClientSettings
{
    public ClientSettings() { }
    
    public bool ShowCombatText { get; set; } = true;
    public bool ShowHpBar { get; set; } = true;
    public bool ShowAvgDps { get; set; } = true;
    public bool ShowFps { get; set; } = false;
    public bool MusicOn { get; set; } = true;
    public bool SoundOn { get; set; } = true;

    public byte ToByte()
    {
        bool[] array = { ShowCombatText, ShowHpBar, ShowAvgDps, ShowFps, MusicOn, SoundOn, false, false };
        return MathHelper.BoolArrayToByte(array);
    }

    public static ClientSettings FromByte(byte data)
    {
        bool[] array = MathHelper.ByteToBoolArray(data);
        return new ClientSettings
        {
            ShowCombatText = array[0],
            ShowHpBar = array[1],
            ShowAvgDps = array[2],
            ShowFps = array[3],
            MusicOn = array[4],
            SoundOn = array[5]
        };
    }
    
    public const string PATH = "user://settings.json";
}