using System.Text.Json;
using HandBettingGame.Constants;
using HandBettingGame.Models;
using Microsoft.JSInterop;

namespace HandBettingGame.Services;

/// <summary>Persists top scores to <c>localStorage</c> via JS interop.</summary>
public sealed class LeaderboardService
{
    static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    readonly IJSRuntime _js;

    public LeaderboardService(IJSRuntime js) => _js = js;

    public async Task<IReadOnlyList<LeaderboardEntry>> GetTopAsync(int take = GameConfig.LeaderboardSize)
    {
        var json = await _js.InvokeAsync<string>("hbgLeaderboard.get", GameConfig.LeaderboardStorageKey);
        var list = Deserialize(json);
        return list
            .OrderByDescending(e => e.Score)
            .ThenBy(e => e.SavedAt)
            .Take(take)
            .ToList();
    }

    public async Task AddAsync(LeaderboardEntry entry)
    {
        var json = await _js.InvokeAsync<string>("hbgLeaderboard.get", GameConfig.LeaderboardStorageKey);
        var list = Deserialize(json);
        list.Add(entry);
        list = list
            .OrderByDescending(e => e.Score)
            .ThenBy(e => e.SavedAt)
            .Take(GameConfig.LeaderboardSize)
            .ToList();

        var outJson = JsonSerializer.Serialize(list, JsonOptions);
        await _js.InvokeVoidAsync("hbgLeaderboard.set", GameConfig.LeaderboardStorageKey, outJson);
    }

    static List<LeaderboardEntry> Deserialize(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new List<LeaderboardEntry>();

        try
        {
            return JsonSerializer.Deserialize<List<LeaderboardEntry>>(json, JsonOptions) ?? new List<LeaderboardEntry>();
        }
        catch
        {
            return new List<LeaderboardEntry>();
        }
    }
}
