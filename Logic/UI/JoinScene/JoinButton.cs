using Godot;
using RogueDefense;
using System;

public class JoinButton : Button
{
    LineEdit AddressEdit => (GetNode("../IP Input/LineEdit") as LineEdit);
    public override void _Ready()
    {
        GetTree().CreateTimer(0.001f).Connect("timeout", this, "LoadLastIp");
    }
    public void LoadLastIp()
    {
        AddressEdit.Text = RogueDefense.UserSaveData.lastIp;
    }
    ConnectingLabel connectingLabel => (ConnectingLabel)GetNode("../ConnectingLabel");
    public override void _Pressed()
    {
        NetworkManager.mode = NetMode.Client;
        string addr = AddressEdit.Text;
        Client.address = addr;
        RogueDefense.UserSaveData.lastIp = addr;
        RogueDefense.UserSaveData.Save();
        Client.port = int.Parse((GetNode("../Port Input/LineEdit") as LineEdit).Text);

        connectingLabel.Visible = true;

        NetworkManager.NetStart();
    }
}
