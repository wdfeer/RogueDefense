using System.Linq;

public static class NetworkManager
{
    public static int PlayerCount => 1 + (Singleplayer ? 0 : Client.instance.others.Count);
    public static bool Singleplayer => mode == NetMode.Singleplayer;
    public static NetMode mode = NetMode.Singleplayer;
    public static bool active = false;
    public static void NetStart()
    {
        if (mode == NetMode.Server)
        {
            Server.instance.Start();
            Client.host = "localhost";
            Client.port = Server.PORT;
            Client.instance.Start();
        }
        else if (mode == NetMode.Client)
        {
            Client.instance.Start();
        }
        active = true;
    }
    public static void NetStop()
    {
        active = false;
        if (mode == NetMode.Server)
        {
            Server.instance.Stop();
            Server.instance = new Server();
        }
        Client.instance.Stop();
        Client.instance = new Client();
    }
    public static void Poll()
    {
        if (mode == NetMode.Server) Server.instance.Poll();
        Client.instance.Poll();
    }
}
public partial class UserData
{
    public int id;
    public string name;
    public int ability;
    public int[] augmentPoints;
    public static string AugmentPointsAsString(int[] augments) => string.Join("/", augments.Select(x => x.ToString()));
    public static int[] AugmentPointsFromString(string str) => str.Split("/").Select(x => int.Parse(x)).ToArray();
    public UserData(int id, string name, int ability, int[] augments)
    {
        this.id = id;
        this.name = name;
        this.ability = ability;
        this.augmentPoints = augments;
    }
}
public enum NetMode
{
    Singleplayer,
    Server,
    Client
}
