using HandBettingGame.Models;

namespace HandBettingGame.Utils;

/// <summary>Maps domain <see cref="Tile"/> models to asset names under <c>wwwroot/imgs/tiles/</c>.</summary>
public static class MahjongTileNaming
{
    public const string TypeDragon = "Dragon";
    public const string TypeWind = "Wind";
    public const string TypeNumber = "Number";

    public static string GetTileType(Tile tile) => tile.Family switch
    {
        TileFamily.Dragon => TypeDragon,
        TileFamily.Wind => TypeWind,
        TileFamily.Suit => TypeNumber,
        _ => TypeNumber
    };

    public static string GetTileName(Tile tile) => tile.Family switch
    {
        TileFamily.Dragon => tile.Dragon switch
        {
            DragonType.Red => "dragon_red",
            DragonType.Green => "dragon_green",
            DragonType.White => "dragon_white",
            _ => "dragon_red"
        },
        TileFamily.Wind => tile.Wind switch
        {
            WindType.North => "wind_north",
            WindType.South => "wind_south",
            WindType.East => "wind_east",
            WindType.West => "wind_west",
            _ => "wind_east"
        },
        TileFamily.Suit => $"{SuitFilePrefix(tile.Suit)}_{tile.Rank ?? 1}",
        _ => "dots_1"
    };

    static string SuitFilePrefix(SuitType? suit) => suit switch
    {
        SuitType.Circles => "dots",
        SuitType.Bamboo => "bamboo",
        SuitType.Characters => "characters",
        _ => "dots"
    };
}
