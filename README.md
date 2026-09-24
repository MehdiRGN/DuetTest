# DuetTest

A Duet-inspired mobile reflex game: two balls orbit a shared pivot, and the
player rotates them clockwise/counter-clockwise (tap left/right half of the
screen) to dodge falling obstacles. The twist on the original Duet formula:
obstacles are color-coded, and each ball can only pass through obstacle
sections matching its own color.

## Opening the project

1. Install Unity Hub and Unity `2022.3.21f1` (or update
   `ProjectSettings/ProjectVersion.txt` to match whatever LTS version you have).
2. Open Unity Hub → Add → select this repository's root folder.
3. Open the project. Unity will regenerate the `Library/` cache on first open
   (this is intentionally gitignored).

## Project layout

- `Assets/Scripts/` — gameplay code:
  - `GameBootstrapper.cs` — builds the entire playable scene in code (camera,
    player, obstacle templates, spawner, HUD) at startup. See below.
  - `OrbitController.cs` — reads left/right screen input and rotates the ball pair.
  - `Ball.cs` — per-ball visuals + collision handling against obstacles.
  - `BallColor.cs` — the two-color enum (`A`/`B`) shared by balls and obstacles.
  - `Obstacle.cs` — a single obstacle piece; blocks one ball color or all colors.
  - `ObstacleMover.cs` — scrolls an obstacle down the screen and despawns it.
  - `ObstacleSpawner.cs` — spawns obstacle "prefab" templates on a difficulty-scaled timer.
  - `GameManager.cs` — tracks score, ramps difficulty, and handles game over/restart.
  - `HudController.cs` — drives the score label and game-over panel, and restarts on tap.
  - `RuntimeSpriteFactory.cs` — generates placeholder circle/rectangle sprites
    at runtime, so the project has no dependency on imported art.
- `Assets/Scenes/` — put the main gameplay scene here (see below).
- `Assets/Prefabs/` — swap in real prefabs here once you replace the
  code-built obstacle templates with hand-authored ones.
- `Assets/Materials/` — sprite/material assets, once you move past the
  runtime-generated placeholder visuals.

## Getting a playable scene (fastest path)

This scaffold intentionally ships no hand-authored `.unity`/`.prefab` binary
assets — those are easy to corrupt by hand outside the Unity Editor. Instead,
`GameBootstrapper` builds the whole scene procedurally on `Awake`:

1. Create an empty scene in `Assets/Scenes/` (e.g. `Main.unity`) and add it to
   **File > Build Settings**.
2. Add one empty GameObject to it and attach the `GameBootstrapper` script.
3. Press Play. It creates the camera, the two-ball player pivot, both obstacle
   types (a classic gap bar and the color-matched gate), the spawner, and a
   basic score/game-over HUD — fully wired, no manual dragging of references.

From there, iterate by either tuning `GameBootstrapper`'s exposed fields
(orbit radius, obstacle width/gap, ball radius) or by replacing pieces of it
with real hand-built prefabs/scenes once the design is locked and you're
working inside the Editor.

## Gameplay notes

- **Gap bar** obstacle: a classic Duet-style wall with a gap in the middle —
  rotate the ball pair so both balls fit through the gap.
- **Color gate** obstacle: a wall split into a left half (only passable by
  `BallColor.A`) and a right half (only passable by `BallColor.B`) — rotate so
  each ball stays over its matching half as the gate scrolls past.
