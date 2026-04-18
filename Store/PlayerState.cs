namespace HandBettingGame.Store;

/// <summary>Score and win/loss counters.</summary>
public sealed class PlayerState
{
    public int Score { get; set; }

    public int Wins { get; set; }

    public int Losses { get; set; }
}
