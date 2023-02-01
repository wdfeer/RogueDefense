using Godot;
using RogueDefense;
using System;

public class JoinButton : GoToSceneButton
{
    LineEdit AddressEdit => (GetNode("../IP Input/LineEdit") as LineEdit);
    public override void _Ready()
    {
        GetTree().CreateTimer(0.001f).Connect("timeout", this, "LoadLastIp");
    }
    public void LoadLastIp()
    {
        AddressEdit.Text = RogueDefense.UserData.lastIp;
    }
    public override void _Pressed()
    {
        NetworkManager.mode = NetMode.Client;
        string addr = AddressEdit.Text;
        Client.address = addr;
        RogueDefense.UserData.lastIp = addr;
        RogueDefense.UserData.Save();
        Client.port = int.Parse((GetNode("../Port Input/LineEdit") as LineEdit).Text);
        base._Pressed();
    }
}
