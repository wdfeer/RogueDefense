using Godot;
using RogueDefense;
using System;

public partial class JoinButton : Button
{
	LineEdit AddressEdit => GetNode("../IP Input/LineEdit") as LineEdit;
	public override void _Ready()
	{
		GetTree().CreateTimer(0.001f).Connect("timeout", new Callable(this, "LoadLastIp"));
	}
	public void LoadLastIp()
	{
		AddressEdit.Text = SaveData.lastIp;
	}
	ConnectingLabel connectingLabel => (ConnectingLabel)GetNode("../ConnectingLabel");
	public override void _Pressed()
	{
		NetworkManager.mode = NetMode.Client;
		string addr = AddressEdit.Text;
		Client.host = addr;
		SaveData.lastIp = addr;
		SaveData.Save();
		Client.port = ushort.Parse((GetNode("../Port Input/LineEdit") as LineEdit).Text);

		connectingLabel.Visible = true;

		NetworkManager.NetStart();
	}
}
