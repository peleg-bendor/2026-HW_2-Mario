# 2026-HW_2-Mario

Homework Exercise 2 for the "Methods in Game Development" Unity course — a 2D Mario-style
platformer, continuing the game built for Exercise 1.

## Starting point

The `Assets` folder started as a direct copy of `2026-1.5-Mario`, a sandbox project that ran
between the two exercises and absorbed the course material from Lessons 5 and 6. That means this
project begins with everything Exercise 1 was graded on, plus two things built since: a Star
power-up that grants temporary invincibility, and a data-driven level pipeline — the level lives
in a text file, and custom Editor tools build it into the scene and write it back out.

Exercise 1 itself started from the instructor's Lesson 4 project. See `2026-HW_1-Mario/README.md`
and `2026-1.5-Mario/HW_1.5_PLAN.md` for those two earlier steps.

## What this exercise adds

Per the requirements: a lightning-bolt pickup granting a temporary speed boost, a health-points
system built with MVC, a floor tile that appears and disappears on a cycle, a floor tile that
moves back and forth, support for all of those in both Editor tools, an object-deletion mode in
the tile placer, a double jump gated by an extension method that reports whether the player is
airborne, and a full level authored with the tooling.

Full requirements are in `Course/Exercises/Exercise 02.md` (outside this repo, in the shared
course folder). Stage-by-stage progress, decisions, and what's left is tracked in
[`HW_2_PLAN.md`](HW_2_PLAN.md). The comment, logging, naming and code-quality rules the code
follows are in [`CONVENTIONS.md`](CONVENTIONS.md).

## The Editor tooling, and how it differs from the lesson's

Lesson 6 covered a level builder (`BuildLevel.cs`) and a click-to-place spawner
(`PrefabSpawnerWindow.cs`). This project's equivalents, `LevelWindow` and `TilePlacerWindow`,
follow the same idea but depart from that code in several deliberate ways. Every departure was a
decision with a reason, recorded in `2026-1.5-Mario/HW_1.5_PLAN.md`'s Decisions Log:

- Tile ids map to prefabs through a serialized `TilePrefabMap` asset rather than a hardcoded
  `switch`, so adding a tile type is a row in an asset instead of an edit to the tool.
- Level data is parsed with Unity's own `JsonUtility` against small `[Serializable]` classes
  rather than the third-party `MiniJSON` parser. A Tiled export is a fixed, known shape, which is
  what `JsonUtility` is for.
- Tiles are created with `PrefabUtility.InstantiatePrefab` rather than plain `Instantiate`, so a
  later edit to a tile prefab reaches tiles already built into the level — and so a tile's id can
  be recovered from the scene later, which is what makes saving possible at all.
- There is no `Assets/Resources/` folder and no `Resources.Load`. The mapping holds direct prefab
  references, which are GUID-based, so renaming a prefab can't silently break the level.
- Building clears the parent's previous children first, rather than stacking a second copy of the
  level on top of the first.
- The scene can be written back out to the level file, a direction the lesson's tool doesn't have.
  Without it the pipeline only runs one way and every scene edit is erased by the next build.
- The placer places on left click (the lesson's right click is the Scene view's own camera
  control), parents what it places under the level parent instead of dropping it at scene root,
  and reads the same mapping asset as the builder so their tile lists can't disagree.

The Tiled tileset is also a Collection of Images rather than a single packed sheet, so each tile
is its own file and adding one doesn't mean regenerating a sheet.

## The level

`Assets/Levels/Level01.txt` holds the whole level as a grid of tile ids;
`Assets/Levels/TilePrefabMap.asset` says which prefab each id means. `Tools → Level` builds the
file into the scene and saves the scene back out, and `Tools → Tile Placer` stamps single tiles.
Building deletes and recreates every child of `World`, so the data file is the source and the
scene is output.

Tiled authored the first version of the level and is used for tileset work; day-to-day editing
happens in Unity.

## Running it

Open the project in Unity, then open `Assets/Scenes/Scene_Physics.unity` — that's the real
playable scene.

## Submission

Graded from a video showing every implemented requirement, both in code and running live —
per the instructor, anything not shown in the video counts as not done.
