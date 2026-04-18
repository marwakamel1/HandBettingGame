using System.Text.Json;
using HandBettingGame.Configuration;
using HandBettingGame.Models;
using Microsoft.Extensions.Options;
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
    readonly GameRulesOptions _rules;

    public LeaderboardService(IJSRuntime js, IOptions<GameRulesOptions> rules)
    {
        _js = js;
        _rules = rules.Value;
    }

    public async Task<IReadOnlyList<LeaderboardEntry>> GetTopAsync(int? take = null)
    {
        var cap = take ?? _rules.LeaderboardSize;
        var json = await _js.InvokeAsync<string>("hbgLeaderboard.get", _rules.LeaderboardStorageKey);
        var list = Deserialize(json);
        return list
            .OrderByDescending(e => e.Score)
            .ThenBy(e => e.SavedAt)
            .Take(cap)
            .ToList();
    }

    public async Task AddAsync(LeaderboardEntry entry)
    {
        var json = await _js.InvokeAsync<string>("hbgLeaderboard.get", _rules.LeaderboardStorageKey);
        var list = Deserialize(json);
        list.Add(entry);
        list = list
            .OrderByDescending(e => e.Score)
            .ThenBy(e => e.SavedAt)
            .Take(_rules.LeaderboardSize)
            .ToList();

        var outJson = JsonSerializer.Serialize(list, JsonOptions);
        await _js.InvokeVoidAsync("hbgLeaderboard.set", _rules.LeaderboardStorageKey, outJson);
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
