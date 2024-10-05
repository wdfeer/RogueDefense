using System.Text.Json;

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
        using (fileAccess)
        {
            fileAccess.StoreString(JsonSerializer.Serialize(obj));
        }
    }
    
    private static T DeserializeStruct<T>(string path)
    {
        FileAccess fileAccess = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        using (fileAccess)
        {
            return JsonSerializer.Deserialize<T>(fileAccess.GetAsText());
        }
    }
}