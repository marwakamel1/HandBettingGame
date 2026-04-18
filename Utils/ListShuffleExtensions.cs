namespace HandBettingGame.Utils;

public static class ListShuffleExtensions
{
    /// <summary>In-place Fisher–Yates shuffle.</summary>
    public static void Shuffle<T>(this IList<T> list, Random? rng = null)
    {
        rng ??= Random.Shared;
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
