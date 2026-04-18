using HandBettingGame.Configuration;
using HandBettingGame.Store;
using HandBettingGame.Utils;
using Microsoft.Extensions.Options;

namespace HandBettingGame.Services;

/// <summary>Scoped application state: deck, player, game flow, and special-tile registry. Raises <see cref="OnChange"/> for Blazor re-renders.</summary>
public sealed class GameStateService
{
    readonly GameRulesOptions _rules;

    public GameStateService(IOptions<GameRulesOptions> rules) => _rules = rules.Value;

    public DeckState Deck { get; } = new();

    public PlayerState Player { get; } = new();

    public GameStatusState GameStatus { get; } = new();

    public RegistryState Registry { get; } = new();

    /// <summary>Subscribe from components (<c>OnChange += StateHasChanged</c>) for reactive updates.</summary>
    public event Action? OnChange;

    /// <summary>Clears round state and counters. Reseeds special registry entries at <see cref="GameRulesOptions.InitialSpecialValue"/>.</summary>
    public void ResetForNewGame()
    {
        Deck.DrawPile.Clear();
        Deck.DiscardPile.Clear();
        Deck.ReshuffleCount = 0;

        Player.Score = 0;
        Player.Wins = 0;
        Player.Losses = 0;

        GameStatus.IsGameOver = false;
        GameStatus.GameOverReason = null;
        GameStatus.Phase = GameRoundPhase.Idle;
        GameStatus.CurrentHand = null;
        GameStatus.NextHand = null;
        GameStatus.Round = 0;
        GameStatus.HandHistory.Clear();

        Registry.SpecialTileValues.Clear();
        SeedDefaultRegistry();

        NotifyStateChanged();
    }

    public void NotifyStateChanged() => OnChange?.Invoke();

    void SeedDefaultRegistry()
    {
        foreach (var key in DefaultSpecialKeys.All)
            Registry.SpecialTileValues[key] = _rules.InitialSpecialValue;
    }
}
