using HandBettingGame.Models;

namespace HandBettingGame.Utils;

/// <summary>Builds a full mahjong-style tile pool: dragons, winds, and 1–9 in three suits.</summary>
public static class DeckFactory
{
    public static List<Tile> CreateTiles(int copiesPerType)
    {
        var list = new List<Tile>(copiesPerType * 34);
        for (var c = 0; c < copiesPerType; c++)
        {
            foreach (DragonType d in Enum.GetValues<DragonType>())
                list.Add(CreateDragon(d));

            foreach (WindType w in Enum.GetValues<WindType>())
                list.Add(CreateWind(w));

            foreach (var suit in new[] { SuitType.Characters, SuitType.Bamboo, SuitType.Circles })
            {
                for (var rank = 1; rank <= 9; rank++)
                    list.Add(CreateSuit(suit, rank));
            }
        }

        return list;
    }

    static Tile CreateDragon(DragonType d) => new()
    {
        InstanceId = Guid.NewGuid().ToString("N"),
        Family = TileFamily.Dragon,
        Dragon = d,
        IsSpecial = true,
        RegistryKey = DefaultSpecialKeys.Key(d)
    };

    static Tile CreateWind(WindType w) => new()
    {
        InstanceId = Guid.NewGuid().ToString("N"),
        Family = TileFamily.Wind,
        Wind = w,
        IsSpecial = true,
        RegistryKey = DefaultSpecialKeys.Key(w)
    };

    static Tile CreateSuit(SuitType suit, int rank) => new()
    {
        InstanceId = Guid.NewGuid().ToString("N"),
        Family = TileFamily.Suit,
        Suit = suit,
        Rank = rank,
        IsSpecial = false
    };
}
