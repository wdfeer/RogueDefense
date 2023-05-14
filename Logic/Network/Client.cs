using Godot;
using RogueDefense;
using System;
using System.Collections.Generic;
using System.Linq;

public class Client : Node
{
    public static string address;
    public static int port;
    public static string URL => $"ws://{address}:{port}";
    public static Client instance = new Client();
    public static WebSocketClient client;
    public static int myId = -1;
    public void Start()
    {
        client = new WebSocketClient();
        client.Connect("connection_closed", this, "Closed");
        client.Connect("connection_error", this, "Closed");
        client.Connect("connection_established", this, "Connected");
        client.Connect("data_received", this, "OnData");
        GD.Print($"Trying to connect to {URL}");
        var err = client.ConnectToUrl(URL);
        if (err != Error.Ok)
        {
            GD.PrintErr("Unable to start client");
            SetProcess(false);
        }
    }
    public List<UserData> others = new List<UserData>();
    public UserData GetUserData(int id) => others.Find(x => x.id == id);
    public void RemoveUserData(int id) => others.Remove(GetUserData(id));
    public void Connected(string protocol = "")
    {
        GD.Print("This client connected! Loading lobby...");
        if (NetworkManager.mode == NetMode.Client)
            JoinScene.TryChangeToLobbyScene();
    }
    public void Closed(bool wasCleanClose = false) { }
    public void OnData()
    {
        string data = client.GetPeer(1).GetPacket().GetStringFromUTF8();
        MessageType type = (MessageType)data[0];
        ProcessMessage(type, data.Substring(1).Split(' '));
    }
    public void ProcessMessage(MessageType type, string[] args)
    {
        switch (type)
        {
            case MessageType.FetchLobby:
                SendMessage(MessageType.Register, new string[] { args[0], UserSaveData.name, AbilityChooser.chosen.ToString(), UserData.UpgradePointsAsString(UserSaveData.upgradePointDistribution) });
                myId = args[0].ToInt();
                for (int i = 1; i < args.Length; i++)
                {
                    var strs = args[i].Split(";");
                    RegisterUser(strs[0].ToInt(), strs[1], strs[2].ToInt(), strs[3].Split("/").Select(x => int.Parse(x)).ToArray());
                }
                break;
            case MessageType.Register:
                RegisterUser(args[0].ToInt(), args[1], args[2].ToInt(), UserData.UpgradePointsFromString(args[3]));
                break;
            case MessageType.SetAbility:
                if (Lobby.Instance == null) break;
                int id = args[0].ToInt();
                UserData data = GetUserData(id);
                data.ability = args[1].ToInt();
                UnregisterUser(id);
                RegisterUser(id, data.name, data.ability, data.upgradePoints);
                break;
            case MessageType.Unregister:
                id = args[0].ToInt();
                UnregisterUser(id);
                break;
            case MessageType.UpdateSettings:
                GameSettings.ReceiveSettings(args);
                break;
            case MessageType.StartGame:
                Lobby.Instance.GetTree().ChangeScene("res://Scenes/Game.tscn");
                break;
            case MessageType.EnemyKill:
                if (IsInstanceValid(Game.instance))
                    Game.instance.DeleteEnemy(false);
                else
                    GD.PrintErr("Received an EnemyKill message when the Game is not active");
                break;
            case MessageType.Upgrade:
                Upgrade up = new Upgrade(UpgradeType.AllTypes[args[1].ToInt()], args[2].ToFloat());
                up.risky = args[3] == "R";
                UpgradeManager.AddUpgrade(up, args[0].ToInt());
                UpgradeScreen.instance.upgradesMade++;
                if (UpgradeScreen.instance.EveryoneUpgraded())
                {
                    UpgradeScreen.instance.HideAndUnpause();
                }
                break;
            case MessageType.Death:
                DefenseObjective.instance.Death(false);
                break;
            case MessageType.Retry:
                Game.instance.GetTree().Paused = false;
                Game.instance.GetTree().ChangeScene("res://Scenes/Game.tscn");
                break;
            case MessageType.AbilityActivated:
                id = args[0].ToInt();
                string username = GetUserData(id).name;
                int abilityTypeIndex = args[1].ToInt();
                ActiveAbility ability = (ActiveAbility)Player.players[id].hooks.Find(x => x.GetType() == AbilityManager.abilityTypes[abilityTypeIndex]);
                ability.ActivateTryShare();
                NotificationPopup.Notify($"{username} used {ability.GetName()}", 1.5f);
                break;
            default:
                break;
        }
    }
    void RegisterUser(int id, string name, int ability, int[] upgradePoints)
    {
        UserData d = new UserData(id, name, ability, upgradePoints);
        others.Add(d);
        if (Lobby.Instance != null)
        {
            Lobby.Instance.AddUser(d);
        }
    }
    void UnregisterUser(int id)
    {
        RemoveUserData(id);
        if (Lobby.Instance != null)
        {
            Lobby.Instance.RemoveUser(id);
        }
    }
    void Broadcast(string data) => client.GetPeer(1).PutPacket(System.Text.Encoding.UTF8.GetBytes(data));
    public void SendMessage(MessageType type, string[] args = null)
    {
        string msg = $"{(char)type}";
        if (args != null)
            msg += String.Join(" ", args);
        Broadcast(msg);
    }


    public void Poll() // important to always keep polling
    {
        client.Poll();
    }
}
public enum MessageType
{
    FetchLobby = '0',
    Register = '1',
    Unregister = '2',
    SetAbility = 'a',
    UpdateSettings = 'c',
    StartGame = 's',
    EnemyKill = 'k',
    Upgrade = 'u',
    Death = 'd',
    Retry = 'r',
    AbilityActivated = 'A'
}