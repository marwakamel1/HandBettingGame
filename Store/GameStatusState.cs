using HandBettingGame.Models;

namespace HandBettingGame.Store;

public enum GameRoundPhase
{
    Idle,
    AwaitingBet,
    Resolving,
    GameOver
}

/// <summary>Round flow, hands used for betting comparison, and history strip.</summary>
public sealed class GameStatusState
{
    public bool IsGameOver { get; set; }

    public string? GameOverReason { get; set; }

    public GameRoundPhase Phase { get; set; }

    public Hand? CurrentHand { get; set; }

    public Hand? NextHand { get; set; }

    /// <summary>1-based round counter; incremented after each resolved bet.</summary>
    public int Round { get; set; }

    public List<HandRecord> HandHistory { get; } = new();
}
