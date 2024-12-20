using System;
using System.Collections.Generic;
using System.Linq;
using RogueDefense.Logic.Enemy;
using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Player.Hooks;
using RogueDefense.Logic.Save;
using RogueDefense.Logic.UI.Lobby.Settings;

namespace RogueDefense.Logic.Network;

public partial class Client : Node
{
    public static string host;
    public static ushort port;
    public static Client instance = new();
    public static StreamPeerTcp client;
    public static int myId = -1;
    public void Start()
    {
        client = new();

        GD.Print($"Trying to connect to {host}:{port}");
        var err = client.ConnectToHost(host, port);
        if (err != Error.Ok)
        {
            GD.PrintErr("Unable to start client");
            SetProcess(false);
        }
    }
    public void Stop()
    {
        if (client != null)
            client.DisconnectFromHost();
        client = null;
    }
    public List<UserData> others = new();
    public UserData GetUserData(int id) => others.Find(x => x.id == id);
    private void RemoveUserData(int id) => others.Remove(GetUserData(id));
    private static void ChangeSceneToLobby()
    {
        GD.Print("This client connected! Loading lobby...");
        if (NetworkManager.mode == NetMode.Client)
            UI.JoinScene.JoinScene.TryChangeToLobbyScene();
    }
    private void ReceiveMessage(string message)
    {
        MessageType type = (MessageType)message[0];
        GD.Print($"Client received msg of type {type} with contents: {message}");
        ProcessMessage(type, message[1..].Split(' '));
    }
    private void ProcessMessage(MessageType type, string[] args)
    {
        switch (type)
        {
            case MessageType.FetchLobby:
                ChangeSceneToLobby();
                GD.Print($"Sending Register Message with augments {string.Join(",", Save.UserData.augmentAllotment)}");
                SendMessage(MessageType.Register, new string[] { args[0], Save.UserData.name, UI.MainMenu.AbilityChooser.chosen.ToString(), UserData.AugmentPointsAsString(Save.UserData.augmentAllotment) });
                myId = args[0].ToInt();
                for (int i = 1; i < args.Length; i++)
                {
                    var strs = args[i].Split(";");
                    RegisterUser(strs[0].ToInt(), strs[1], strs[2].ToInt(), strs[3].Split("/").Select(x => int.Parse(x)).ToArray());
                }
                break;
            case MessageType.Register:
                RegisterUser(args[0].ToInt(), args[1], args[2].ToInt(), UserData.AugmentPointsFromString(args[3]));
                break;
            case MessageType.SetAbility:
                if (Lobby.Instance == null) break;
                int id = args[0].ToInt();
                UserData data = GetUserData(id);
                data.ability = args[1].ToInt();
                UnregisterUser(id);
                RegisterUser(id, data.name, data.ability, data.augmentPoints);
                break;
            case MessageType.Unregister:
                id = args[0].ToInt();
                UnregisterUser(id);
                break;
            case MessageType.UpdateSettings:
                GameSettings.ReceiveSettings(args);
                break;
            case MessageType.StartGame:
                Lobby.Instance.GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
                break;
            case MessageType.EnemyKill:
                if (IsInstanceValid(Game.instance))
                {
                    int index = args[0].ToInt();
                    if (index >= Enemy.Enemy.enemies.Count || Enemy.Enemy.enemies[index] == null)
                        break;
                    Enemy.Enemy.enemies[index].Die(false);
                }
                else
                    GD.PrintErr("Received an EnemyKill message when the Game is not active");
                break;
            case MessageType.Upgrade:
                Upgrade up = new(UpgradeType.AllTypes[args[1].ToInt()], args[2].Replace(",", ".").ToFloat())
                {
                    risky = args[3] == "R"
                };
                UpgradeManager.AddUpgrade(up, args[0].ToInt());
                UI.InGame.UpgradeScreen.instance.upgradesMade++;
                if (UI.InGame.UpgradeScreen.instance.EveryoneUpgraded())
                {
                    UI.InGame.UpgradeScreen.instance.HideAndUnpause();
                }
                break;
            case MessageType.Death:
                DefenseObjective.instance.Death(false);
                break;
            case MessageType.Retry:
                Game.instance.GetTree().Paused = false;
                Game.instance.GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
                break;
            case MessageType.AbilityActivated:
                id = args[0].ToInt();
                string username = GetUserData(id).name;
                int abilityTypeIndex = args[1].ToInt();
                ActiveAbility ability = (ActiveAbility)PlayerManager.players[id].hooks.Find(x => x.GetType() == AbilityManager.abilityTypes[abilityTypeIndex]);
                ability.ActivateTryShare();
                UI.InGame.NotificationPopup.Notify($"{username} used {ability.GetName()}", 1.5f);
                break;
            case MessageType.PositionUpdated:
                Player.Core.Player player = PlayerManager.players[args[0].ToInt()];
                Player.Turret.Turret turret = player.turrets[args[1].ToInt()];
                float x = args[2].Replace(",", ".").ToFloat(), y = args[3].Replace(",", ".").ToFloat();
                turret.GlobalPosition = new Vector2(x, y);
                break;
            case MessageType.TargetSelected:
                player = PlayerManager.players[args[0].ToInt()];
                int enemyIndex = args[1].ToInt();
                player.SetTarget(enemyIndex, false);
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
    static void Broadcast(string data)
    {
        if (client == null)
            throw new NullReferenceException("variable StreamPeerTcp client is null! Please make sure it has initialized");
        client.PutUtf8String(data);
    }

    public void SendMessage(MessageType type, string[] args = null)
    {
        string msg = $"{(char)type}";
        if (args != null)
            msg += string.Join(" ", args);
        GD.Print($"Sending message to Server: {msg}");
        Broadcast(msg);
    }


    public void Poll() // important to always keep polling
    {
        client.Poll();
        if (client.GetStatus() != StreamPeerTcp.Status.Connected)
            return;

        int byteCount = client.GetAvailableBytes();
        if (byteCount > 0)
        {
            GD.Print($"Client: {byteCount} bytes are available");
            string data = client.GetUtf8String();
            ReceiveMessage(data);
        }
    }
}