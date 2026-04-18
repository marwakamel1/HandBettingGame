using HandBettingGame.Constants;
using HandBettingGame.Models;
using HandBettingGame.Store;
using HandBettingGame.Utils;

namespace HandBettingGame.Services;

/// <summary>Deck build, dealing, reshuffles, betting, registry scaling, and game-over rules.</summary>
public sealed class GameService
{
    readonly GameStateService _state;

    public GameService(GameStateService state) => _state = state;

    public void StartNewGame()
    {
        _state.ResetForNewGame();

        var deck = DeckFactory.CreateTiles(GameConfig.CopiesPerTileType);
        deck.Shuffle();
        _state.Deck.DrawPile.AddRange(deck);
        _state.Deck.DiscardPile.Clear();
        _state.Deck.ReshuffleCount = 0;

        _state.GameStatus.Round = 1;
        _state.GameStatus.IsGameOver = false;
        _state.GameStatus.GameOverReason = null;
        _state.GameStatus.Phase = GameRoundPhase.AwaitingBet;

        if (!TryDealInitialHands(out var reason))
        {
            if (!_state.GameStatus.IsGameOver)
                EndGame(reason ?? "Unable to deal opening hands.");
            _state.NotifyStateChanged();
            return;
        }

        _state.NotifyStateChanged();
    }

    /// <summary>Bet that <see cref="GameStatusState.NextHand"/> total is strictly greater than <see cref="GameStatusState.CurrentHand"/>.</summary>
    public void BetHigher() => ResolveBet(higher: true);

    /// <summary>Bet that <see cref="GameStatusState.NextHand"/> total is strictly less than <see cref="GameStatusState.CurrentHand"/>.</summary>
    public void BetLower() => ResolveBet(higher: false);

    void ResolveBet(bool higher)
    {
        if (_state.GameStatus.IsGameOver || _state.GameStatus.Phase != GameRoundPhase.AwaitingBet)
            return;

        var current = _state.GameStatus.CurrentHand;
        var next = _state.GameStatus.NextHand;
        if (current is null || next is null)
            return;

        _state.GameStatus.Phase = GameRoundPhase.Resolving;

        var reg = _state.Registry.SpecialTileValues;
        var curTotal = current.ComputeTotal(reg);
        var nextTotal = next.ComputeTotal(reg);

        var won = higher ? nextTotal > curTotal : nextTotal < curTotal;

        if (won)
        {
            _state.Player.Score += GameConfig.PointsPerCorrectBet;
            _state.Player.Wins++;
            ApplyRegistryDelta(next, +1);
        }
        else
        {
            _state.Player.Score = Math.Max(0, _state.Player.Score - GameConfig.PointsPerIncorrectBet);
            _state.Player.Losses++;
            ApplyRegistryDelta(next, -1);
        }

        _state.GameStatus.HandHistory.Add(new HandRecord(_state.GameStatus.Round, next.Tiles, nextTotal));

        if (RegistryTriggersGameOver())
        {
            EndGame("A special tile reached 0 or 10.");
            _state.NotifyStateChanged();
            return;
        }

        if (_state.GameStatus.IsGameOver)
        {
            _state.NotifyStateChanged();
            return;
        }

        foreach (var tile in current.Tiles)
            _state.Deck.DiscardPile.Add(tile);

        _state.GameStatus.CurrentHand = next;

        if (!TryDealNextHand(out var dealReason))
        {
            if (!_state.GameStatus.IsGameOver)
                EndGame(dealReason ?? "No cards left to continue.");
            _state.NotifyStateChanged();
            return;
        }

        _state.GameStatus.Round++;
        _state.GameStatus.Phase = GameRoundPhase.AwaitingBet;
        _state.NotifyStateChanged();
    }

    bool TryDealInitialHands(out string? failReason)
    {
        failReason = null;
        if (!EnsureDrawable(GameConfig.HandSize * 2, ref failReason))
            return false;

        _state.GameStatus.CurrentHand = DealHand();
        _state.GameStatus.NextHand = DealHand();
        return true;
    }

    bool TryDealNextHand(out string? failReason)
    {
        failReason = null;
        if (!EnsureDrawable(GameConfig.HandSize, ref failReason))
            return false;

        _state.GameStatus.NextHand = DealHand();
        return true;
    }

    Hand DealHand()
    {
        var tiles = new List<Tile>(GameConfig.HandSize);
        for (var i = 0; i < GameConfig.HandSize; i++)
        {
            var pile = _state.Deck.DrawPile;
            tiles.Add(pile[^1]);
            pile.RemoveAt(pile.Count - 1);
        }

        return new Hand(tiles);
    }

    /// <summary>Ensures at least <paramref name="count"/> cards can be drawn, reshuffling discard when needed.</summary>
    bool EnsureDrawable(int count, ref string? failReason)
    {
               while (_state.Deck.DrawPile.Count < count)
        {
            if (_state.Deck.DiscardPile.Count == 0)
            {
                failReason = "Draw and discard piles are empty.";
                return false;
            }

            if (_state.Deck.ReshuffleCount >= GameConfig.MaxReshuffles)
            {
                failReason = "Maximum reshuffles reached.";
                EndGame(failReason);
                return false;
            }

            ReshuffleDiscardIntoDraw();
            if (_state.GameStatus.IsGameOver)
            {
                failReason = _state.GameStatus.GameOverReason;
                return false;
            }
        }

        return true;
    }

    void ReshuffleDiscardIntoDraw()
    {
        var discard = _state.Deck.DiscardPile;
        var draw = _state.Deck.DrawPile;
        var buffer = discard.ToList();
        discard.Clear();
        buffer.Shuffle();
        foreach (var t in buffer)
            draw.Add(t);

        _state.Deck.ReshuffleCount++;

        if (_state.Deck.ReshuffleCount >= GameConfig.MaxReshuffles)
            EndGame("Maximum reshuffles reached.");
    }

    void ApplyRegistryDelta(Hand hand, int delta)
    {
        foreach (var tile in hand.Tiles)
        {
            if (!tile.IsSpecial || string.IsNullOrEmpty(tile.RegistryKey))
                continue;

            var key = tile.RegistryKey;
            var cur = _state.Registry.SpecialTileValues.TryGetValue(key, out var v)
                ? v
                : GameConfig.InitialSpecialValue;

            _state.Registry.SpecialTileValues[key] = Math.Clamp(cur + delta, GameConfig.MinTileValue, GameConfig.MaxTileValue);
        }
    }

    bool RegistryTriggersGameOver()
    {
        foreach (var kv in _state.Registry.SpecialTileValues)
        {
            if (kv.Value == GameConfig.MinTileValue || kv.Value == GameConfig.MaxTileValue)
                return true;
        }

        return false;
    }

    void EndGame(string reason)
    {
        _state.GameStatus.IsGameOver = true;
        _state.GameStatus.GameOverReason = reason;
        _state.GameStatus.Phase = GameRoundPhase.GameOver;
    }
}
