using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.UI.MainMenu;

public partial class MPButton : GoToSceneButton
{
    [Export]
    public bool host;

    public override void _Pressed()
    {
        NetworkManager.mode = host ? NetMode.Server : NetMode.Client;
        base._Pressed();
    }
}