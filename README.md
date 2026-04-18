# Hand Betting Game

A **Blazor WebAssembly** betting game with Mahjong-style tiles. You compare the **current hand’s total** against a **hidden next hand** and bet **higher** or **lower**. **Honor tiles** (dragons and winds) use a shared **registry** of dynamic values that shift with wins and losses—stay away from the **0** and **10** boundaries.

## Tech stack

- **.NET** (Blazor WebAssembly)
- **Tailwind CSS** (CDN) + `wwwroot/css/app.css` for animations
- **Dependency injection**: scoped services per user session in WASM

## Run locally

```bash
cd HandBettingGame
dotnet run
```

Open the URL shown in the console (default is often `http://localhost:5250`).

---


| Setting | Role |
|----------|------|
| `InitialSpecialValue` | Starting registry value for each honor key |
| `MinTileValue` / `MaxTileValue` | Clamp range; hitting min or max ends the game |
| `MaxReshuffles` | Game over when reshuffle count reaches this threshold |
| `HandSize` | Tiles per hand |
| `CopiesPerTileType` | Deck size multiplier |
| `PointsPerCorrectBet` / `PointsPerIncorrectBet` | Score delta on win / loss |
| `LeaderboardSize` / `LeaderboardStorageKey` | Leaderboard cap and storage key |

Edit **`wwwroot/appsettings.json`** to tune balance without changing C# (then rebuild/redeploy so the WASM bundle picks up the file).

## Human vs. AI-assisted work

**Handwritten / maintainer-owned:** Game rules and flow (betting, registry scaling, deck and hand logic), visual direction and table UX, and final decisions on what gets merged and released.

**AI-assisted:** Parts of the codebase and docs were drafted or refactored with coding assistants . 

