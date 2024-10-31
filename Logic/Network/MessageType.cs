namespace RogueDefense.Logic.Network;

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
    AbilityActivated = 'A',
    PositionUpdated = 'p',
    TargetSelected = 't'
}