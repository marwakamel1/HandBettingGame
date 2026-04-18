using HandBettingGame.Models;

namespace HandBettingGame.Utils;

/// <summary>Stable registry keys for tiles that participate in dynamic scaling.</summary>
public static class DefaultSpecialKeys
{
    public static readonly string[] All =
    [
        Key(DragonType.Red),
        Key(DragonType.Green),
        Key(DragonType.White),
        Key(WindType.North),
        Key(WindType.South),
        Key(WindType.East),
        Key(WindType.West)
    ];

    public static string Key(DragonType d) => $"dragon:{d.ToString().ToLowerInvariant()}";

    public static string Key(WindType w) => $"wind:{w.ToString().ToLowerInvariant()}";
}
