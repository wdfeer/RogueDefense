namespace RogueDefense.Logic.Network;

public enum MessageType : sbyte
{
    FetchLobby,
    Register,
    Unregister,
    UpdateSettings,
    StartGame,
    EnemyKill,
    Upgrade,
    Death,
    AbilityActivated,
    PositionUpdated,
    TargetSelected
}