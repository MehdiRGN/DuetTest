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
  - `OrbitController.cs` — reads left/right screen input and rotates the ball pair.
  - `Ball.cs` — per-ball collision handling against obstacles.
  - `BallColor.cs` — the two-color enum (`A`/`B`) shared by balls and obstacles.
  - `Obstacle.cs` — a single obstacle piece; blocks one ball color or all colors.
  - `ObstacleMover.cs` — scrolls an obstacle down the screen and despawns it.
  - `ObstacleSpawner.cs` — spawns obstacle prefabs on a difficulty-scaled timer.
  - `GameManager.cs` — tracks score, ramps difficulty, and handles game over/restart.
- `Assets/Scenes/` — put the main gameplay scene here.
- `Assets/Prefabs/` — obstacle and ball prefabs go here once built in-editor.
- `Assets/Materials/` — sprite/material assets.

## Building out the scene

This scaffold ships scripts only (no scene/prefab binary assets, since those
need to be authored in the Unity Editor). To get a playable scene:

1. Create a scene in `Assets/Scenes/`.
2. Add an empty `GameManager` object with the `GameManager` script.
3. Add a "Pivot" object with the `OrbitController` script, and parent two
   ball GameObjects (each with a `Rigidbody2D` (kinematic) + `Collider2D`
   (`Is Trigger`) + `Ball` script, one colored `A`, one colored `B`) to it.
4. Build obstacle prefabs: geometry with `Collider2D` (`Is Trigger`) +
   `Obstacle` script (set `blocksAllColors` or `passableColor`) +
   `ObstacleMover` script, and save them to `Assets/Prefabs/`.
5. Add an `ObstacleSpawner` object, assign the prefabs and a spawn point
   above the camera's view.
6. Wire up UI (score text, game-over panel) reading from `GameManager`.
