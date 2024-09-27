using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.UI.Lobby;

public partial class StartButton : Button
{
    public override void _Pressed()
    {
        if (NetworkManager.mode != NetMode.Server)
            return;

        if (Client.instance.others.Count == 0)
        {
            NetworkManager.NetStop();
            NetworkManager.mode = NetMode.Singleplayer;
            Network.Lobby.Instance.GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
        }
        else
            Server.instance.SendMessage(MessageType.StartGame, new string[0]);
    }
}