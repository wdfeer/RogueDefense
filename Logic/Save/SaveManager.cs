namespace RogueDefense.Logic.Save;

public static class SaveManager
{
    public static UserData user = new();
    public static ClientSettings client = new();
    
    public static void Load()
    {
        user = DeserializeStruct<UserData>(UserData.PATH);
        client = DeserializeStruct<ClientSettings>(ClientSettings.PATH);
    }
    
    public static void Save()
    {
        SerializeStruct(user, UserData.PATH);
        SerializeStruct(client, ClientSettings.PATH);
    }

    private static void SerializeStruct<T>(T obj, string path)
    {
        FileAccess fileAccess = FileAccess.Open(path, FileAccess.ModeFlags.Write);
        // TODO: store the 'obj' as serialized json in the file
    }
    
    private static T DeserializeStruct<T>(string path)
    {
        FileAccess fileAccess = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        // TODO: create a struct from serialized json in the file
    }
}