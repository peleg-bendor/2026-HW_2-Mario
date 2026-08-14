# HW-2 Plan — `2026-HW_2-Mario` (Exercise 2)

Shared working notes for Exercise 2, modeled on `2026-HW_1-Mario/HW_1_PLAN.md`. The assistant may
edit this file directly as we go (it's not game code, just shared notes).

The project starts from a copy of `2026-1.5-Mario`'s finished `Assets` — HW1's game plus the Star
power-up and the whole data-driven level pipeline (`LevelWindow`, `TilePlacerWindow`,
`TilePrefabMap`, `Assets/Levels/`). That project is closed; see its own `HW_1.5_PLAN.md` for how
its three stages went and why Stage 3 was dropped rather than finished.

## Status Legend

- `[ ]` not started
- `[~]` in progress
- `[x]` done AND confirmed working in-editor

## Git Workflow Reminder

- After any step that leaves the project in a working state, consider committing (small, working
  commits > one giant commit).
- After a whole stage is finished and confirmed working, push.

## Stage Order

Stages 1-9 match Exercise 2's own item numbers exactly, which also happens to be a workable build
order: items 1-4 create the four objects, 5-7 make the tooling handle them, 8 is independent of
all of it, and 9 needs everything to exist first. Stage 0 gets the project versioned before any
feature work; Stage 0.5 fixes things wrong in the inherited code before anything new is built on
top of them. Stage 9.5 holds optional work that is explicitly not required by the exercise, and
Stage 10 is the submission video. Each stage gets its own design discussion before code, the same
way every stage of HW1 and 1.5 did.

## Tile Roster

Three new tile types, not four: item 2's heart is the existing `Sprite_Strike`, already tile id
12, and gets renamed rather than added. The mapping's id is always the Tiled tileset's id plus
one, because Tiled's `firstgid` is 1.

| Object | Tiled `.tsx` id | `TilePrefabMap` id | Stage |
|---|---|---|---|
| Lightning bolt | 13 | 14 | 1 |
| Heart (renamed from Strike) | 11 | 12 | 2 |
| Rock (disappearing floor) | 14 | 15 | 3 |
| Cloud (moving floor) | 15 | 16 | 4 |

### Stage 0 — Repo and project setup `[x]`

Getting `2026-HW_2-Mario` from "a copied folder that runs" to "a versioned project with a clean
starting commit."

#### Step 1 — Project created from 1.5's `Assets` `[x]`

Done by Peleg before this plan existed: `Assets` copied wholesale from `2026-1.5-Mario` (as one
folder, so every `.meta` travels with its file and no GUID changes), Build Settings checked, the
`Tools → Level` and `Tools → Tile Placer` windows re-assigned their three references, both tools
tested end to end, and the game played through. Confirmed working.

#### Step 2 — Remove the template leftover `[x]`

`Assets/TutorialInfo/` came from the fresh URP template and survived the `Assets` copy — it's the
only difference between this project's `Assets` and 1.5's. Nothing references it. Delete, the
same call made about `Assets/_Recovery/` in 1.5.

#### Step 3 — Git `[x]`

`.gitignore` copied unchanged from 1.5. `README.md` written fresh, describing the project and
carrying the tooling-differences section (Step 4). Then `git init` and an initial commit, so
history starts from a working, cleaned-up state rather than the back-and-forth of getting there.

The remote is the one open question, because the repo itself is part of the hand-in: public is
the only setting that reliably works without knowing the grader's account, but the `Assets` folder
descends from the instructor's own Lesson 4 project, so a public repo republishes his material.
Private with the grader invited avoids that; public and switched to private after grading is the
middle road.

#### Step 4 — README: how this project's tooling differs from the lesson's `[x]`

The editor tooling deliberately departs from Lesson 6's `BuildLevel.cs`/`PrefabSpawnerWindow.cs`
in seven ways, each with a reason (see the Decisions Log). All of it is defensible and most of it
is a fix, but a reader expecting the lesson's shape will notice, so the README states the
differences and why. The video covers the same ground in about thirty seconds.

### Stage 0.5 — Fixes to inherited code `[x]`

Small things wrong in what came across from 1.5, worth fixing before new features are built on
top of them.

#### Step 1 — A second Star cuts invincibility short `[x]`

`PlayerInvincible.ActivateInvincibility` calls `StartCoroutine` unconditionally, so collecting a
second star while the first is active leaves two coroutines running. The first one still finishes
on its original schedule: it clears the tint and drops `IsInvincible` early, while the second
keeps running and re-clears later. So a second star doesn't extend the effect, it ends it sooner
and flickers the state on the way.

Fix by keeping the `Coroutine` handle and stopping it before starting a new one, which gives
restart-the-timer semantics. Stage 1's speed boost gets built the same way from the start.

Deliberately not done alongside it: no "already invincible, ignore this star" branch, since that
wastes the pickup rather than restarting it, and no guard for the object being disabled mid-effect
(which would strand `IsInvincible` at true), since Mario is never disabled and there is no second
case asking for it.

**Confirmed working** by Peleg, in the editor with a timer and via `OutputLogsTemp.txt`: two star
pickups produced one `activated`, one `restarted` and one `ended` rather than the old two-and-two,
and two ghost hits landing inside the restarted window cost no strikes at all - so the effect held
across the restart rather than merely logging as though it had.

### Stage 1 — Lightning bolt: temporary speed boost `[ ]`

Exercise item 1. Mario collects a lightning bolt; his speed increases by 50% of his maximum for
5 seconds.

The fifth run of the pickup chain this codebase already uses four times (axe, fire flower, star,
strike): a trigger detector hands an `IPowerUp` to `PlayerPowerUp`, which applies it without
knowing which kind it got. No new abstraction — the pattern is being followed, not extended.

#### Step 1 — Design discussion `[ ]`

#### Step 2 — Sprite and prefab `[ ]`

`Sprite_LightningBolt.png` into `Assets/Sprites/` with Pixels Per Unit set to 48, then
`Sprite_LightningBolt.prefab` in `Assets/Prefabs/` with a trigger collider, mirroring
`Sprite_Star.prefab`.

#### Step 3 — `LightningPickupController` and `LightningPowerUp` `[ ]`

Detector plus effect, following `StarController`/`StarPowerUp` exactly.

#### Step 4 — `PlayerSpeedBoost` `[ ]`

New receiver on Mario, coroutine-driven, mirroring `PlayerInvincible`. It captures Mario's base
speed once and restores to that value rather than dividing back out, so a second bolt collected
mid-boost can't compound the multiplier. A second pickup restarts the timer instead of stacking,
the same fix Stage 0.5 makes to invincibility.

#### Step 5 — Tile id and playtest `[ ]`

`TilePrefabMap` row 14. Confirm the boost applies, expires on time, and that a second bolt during
the boost restarts the clock without changing the speed.

### Stage 2 — Health points, built with MVC `[ ]`

Exercise item 2, and the one item that dictates its own architecture. Mario has health points,
collectable up to a maximum of 3, lost on hazard contact; at zero he returns to the start and the
level restarts with every object reinitialized. The exercise requires MVC.

HW1's strike system already does all of this. The work is a rename plus a rebuild of the same
behavior in MVC shape, not new mechanics. The instructor confirmed directly that reusing the
existing system is fine.

#### Step 1 — Design discussion `[ ]`

#### Step 2 — Interfaces and model `[ ]`

`IHealthModel`/`IHealthView` in `Assets/Scripts/Interfaces/`, and `HealthModel` as a plain C#
class owning the count, the maximum, and the rules for changing it. No Unity types in the model.

#### Step 3 — View and controller `[ ]`

`HealthView` draws the number onto the `Txt_` label. `HealthController` subscribes to
`SC_Death.OnHazardCollision` and the health pickup's event, updates the model, pushes to the view,
and raises game over. `GameEndManager` currently subscribes to `StrikesManager.OnGameOver`, so
that event has to survive the refactor on whichever piece ends up owning it.

#### Step 4 — Retire the strike system and rename `[ ]`

`StrikesManager` and `StrikeCountManager` go once the MVC trio is confirmed. "Strike" becomes
"health" across `StrikePowerUp`, `StrikePickupController`, the prefab, the GUI object and the
label text. Renaming `Sprite_Strike.prefab` is safe — `TilePrefabMap` holds GUID references — but
renaming `Tiles01/Tiles/Sprite_Strike.png` is not, since `MarioTiles.tsx` points at it by
filename. Rename the Unity side, and fix the `.tsx` in the same move or leave that file alone.

#### Step 5 — Playtest `[ ]`

Losing health to spikes and to both enemies, the cap at 3, game over at zero, and the scene
reload restoring collected coins, the key, and killed enemies (the exercise's "reinitialize all
objects" clause, which the reload already satisfies — confirm it deliberately rather than assume
it). Also confirm the Star still suppresses health loss.

### Stage 3 — Disappearing floor tile `[ ]`

Exercise item 3: a floor tile that appears for 2 seconds and disappears for 2 seconds.

Cycling from scene load for every tile at once, not triggered by Mario touching it. The exercise
text describes a cycle with no trigger, it needs no per-tile detection of who stepped where, and
tiles started at load stay in sync with no coordinator. The touch-triggered version is the more
classic-feeling one and gets a sentence in the video as the road not taken.

#### Step 1 — Design discussion `[ ]`

#### Step 2 — Sprite, prefab and behavior `[ ]`

`Sprite_Rock` imported and prefabbed with `SC_Floor` and a solid collider, so it counts as real
ground for the jump check and the projectiles. The component toggles the `SpriteRenderer` and the
`Collider2D` rather than calling `SetActive(false)` on the GameObject: a disabled GameObject stops
running its own coroutine, so the tile would vanish and never come back. Durations are serialized
fields, not literals.

#### Step 3 — Tile id and playtest `[ ]`

`TilePrefabMap` row 15. Confirm Mario falls when a tile he's standing on vanishes, that the tile
returns, and that jumping off a rock tile works the same as off a floor tile.

### Stage 4 — Moving floor tile `[ ]`

Exercise item 4: a floor tile that moves 2 tiles right and 2 tiles left.

Read as a patrol between its home cell and two cells right, returning to where it started — which
is what "2 right and another 2 left" describes, and it keeps the tile's home cell the cell it
actually returns to, which matters because the level file stores one position per object. Each
tile is its own object with no grouping: the data format stores one tile per cell, and clouds
placed side by side move in lockstep for free as long as they share a speed and start together.

#### Step 1 — Design discussion `[ ]`

#### Step 2 — Sprite, prefab and movement `[ ]`

`Sprite_Cloud` imported and prefabbed with `SC_Floor` and a solid collider. Travel distance and
step timing are serialized fields, defaulting to the 2 tiles the exercise asks for.

#### Step 3 — Carrying Mario `[ ]`

The part with real risk. Mario will not be carried for free: `PlayerMovement` brakes his X
velocity toward zero in world space every frame no key is held, which cancels the friction that
would otherwise drag him along with a kinematic platform. The tile therefore tracks whatever is
resting on it and adds its own per-frame movement to those bodies' positions — writing position
rather than velocity, so the braking doesn't fight it. It carries anything standing on it without
knowing what, the same instinct as `EnemyMovement` skipping the `Player` tag rather than naming
Mario.

#### Step 4 — Tile id and playtest `[ ]`

`TilePrefabMap` row 16. Confirm Mario rides the tile in both directions, can jump off it and land
back on it, that several clouds in a row behave as one platform, and that an enemy or a landed axe
on top is carried too rather than being a special case.

### Stage 5 — Tiled tileset and the level builder `[ ]`

Exercise item 5: add the new tiles to the tile editor, export an image with all of them in it, and
develop the level-creating editor script to support them.

The builder half is already done by design and that's the point worth explaining rather than
hiding: `LevelWindow` reads `TilePrefabMap`, so a new tile type is a row in an asset, not an edit
to the tool. That's the Open/Closed Principle from Lesson 3 applied to the lesson's own hardcoded
`switch`, and it's a stronger answer than editing a switch statement — but only if it's said out
loud, since a reader expecting to watch the script change will otherwise see no change at all.

#### Step 1 — Update `MarioTiles.tsx` `[ ]`

Three `<tile>` entries at ids 13-15 for the three new sprites, `tilecount` raised from 13 to 16.
The 48px copies already sit in `Tiles01/Tiles/`.

#### Step 2 — Re-sync the map and fix its export target `[ ]`

`Level01.tmx` and the level actually being played differ by exactly four cells: a coin added at
(18, 19), and a floor tile at (19, 12) plus spikes at (18, 11) and (20, 11) removed. That's the
Tile Placer test from the end of 1.5's Stage 2 and nothing else, so Tiled is four edits away from
current rather than a re-authoring job. Fix those cells, paint the new tile types in, and point
the export target at this project - it still writes to `../2026-1.5-Mario/Assets/Levels/Level01.json`,
the old project, which is harmless right up until someone presses Export.

#### Step 3 — The all-tiles image `[ ]`

A Collection of Images tileset has no packed sheet to export, so this is a single PNG laying all
tiles out in a grid, kept in `Tiles01/`, produced purely to satisfy the requirement's wording.
Skippable — it's packaging, not function, and the tile ids and level data are identical either
way — but it costs five minutes and removes the argument.

#### Step 4 — Confirm the builder places all three `[ ]`

No code change expected. Build a test level containing every new type and check each one lands in
the right cell.

### Stage 6 — Tile placer covers every object `[ ]`

Exercise item 6. Also already true by design: `TilePlacerWindow` builds its dropdown from
`TilePrefabMap`, so its list cannot disagree with the builder's. Verification rather than
construction — place one of each of the sixteen types, save, rebuild, confirm the round trip.

### Stage 7 — Deleting objects from the tile placer `[ ]`

Exercise item 7, and the only genuinely new tooling work. The window can currently only replace
what's in a cell as a side effect of placing something; there is no way to delete without placing.

An erase mode alongside the placing toggle, reusing the existing `ClearCell` helper, with the
Scene view preview changing color so the active mode is visible without looking back at the
window. Playtest is the save/build round trip after erasing.

### Stage 8 — Double jump and the in-air extension `[ ]`

Exercise item 8: an extension method reporting whether the player is in the air, used to allow a
second jump, with the double jump recharging on landing.

This is the item Stage 3 of the 1.5 project was going to build from the other direction, so its
design points carry over: port HW1's own box-cast probe rather than the lesson's degenerate
`Physics2D.OverlapArea` (both corners at the same point), and keep `SC_Floor.OnFloorCollision`
subscribed, since landing is what recharges the jump.

#### Step 1 — Design discussion `[ ]`

#### Step 2 — The extension `[ ]`

New `Assets/Scripts/Extensions/` folder. The extension goes on `Collider2D` rather than the
lesson's `Transform`, because our probe reads collider bounds while the lesson's reads only a
position — a `Transform` version would have to `GetComponent` on every call or hardcode a size.
Both `IsGrounded` and `IsInAir` are exposed, one line each: the exercise asks for the in-air one,
the first jump wants the grounded one.

#### Step 3 — `PlayerJump` `[ ]`

The `isJumping` bool becomes a jump counter with a serialized maximum, reset on landing.

#### Step 4 — Playtest `[ ]`

Two jumps and no third; walking off a ledge leaves one jump, not two (Peleg's call - the exercise
doesn't say, and spending the ground jump by stepping off an edge is the behavior that makes a
double jump feel like a recovery rather than a free second chance); recharge on landing; and no
regression in the corner-perch case the current probe was built for.

### Stage 9 — The full level `[ ]`

Exercise item 9: build a complete playable level with the tile editor and save the text file with
all the level information inside Unity.

#### Step 1 — Rename the level file `[ ]`

`Assets/Levels/Level01.json` becomes `Level01.txt`. Unity imports `.json` as a `TextAsset` exactly
like `.txt`, so this changes nothing functionally — it matches the instructor's own
`Level01-Mario00.txt` and removes any question about whether the deliverable is a text file.
Renamed from inside Unity's Project window so the `.meta` follows and the GUID survives.

#### Step 2 — Export from Tiled once, then author in Unity `[ ]`

Item 9 asks for the level to be built "using the tile editor", which most likely means Tiled, and
Stage 5 leaves Tiled holding a current map with the new tile types in it. So export from there
straight into `Assets/Levels/Level01.txt` and Build Level once, which covers the strict reading of
the requirement and gives the video one clean Tiled-to-Unity beat.

Everything after that is Unity-side: `Tools → Tile Placer` for the real placement work, putting
the new tile types where each can be demonstrated on camera, then `Save Level` and `Build Level`
to confirm the round trip.

#### Step 3 — Full playthrough `[ ]`

Every item in one session: bolt, health, rock, cloud, double jump, plus everything carried over
from HW1 and 1.5 — weapons, both enemies, the spawner, the star, key/gateway/portal, game over
and game won.

### Stage 9.5 — Optional extras `[ ]`

Not required by Exercise 2. Only if everything above is done, committed and pushed, and before the
video, since anything built here is something the video has to show.

- **Coin counter as MVC.** `SC_CoinsManager` is the only class in the project that both owns a
  count and draws it — strikes, axes and selected weapon all split those jobs already, so it's the
  odd one out regardless of MVC. Once Stage 2's trio exists, converting coins is largely a copy of
  a shape already built and tested.
- **Fade on the disappearing tile.** Also makes the tile's state readable a moment before it
  actually goes, which is a real gameplay improvement rather than only polish.
- **A log-to-file extension.** Lesson 7's `ErrorLoggerExtension` hooks
  `Application.logMessageReceived` and re-prints errors to the same Console that already showed
  them, which adds nothing (and has no unsubscribe, no double-subscribe guard, and responds to an
  error by logging an error). The version worth building writes the session's log to a file on its
  own, which would take a manual copy-out step out of the working loop here.
- **Not Lesson 8.** Nothing in Exercise 2 asks for reflection or a DLL. Building it now would be
  guessing at Exercise 3 the way 1.5 guessed at Exercise 2, which is the trade that closed that
  project. If Exercise 3 wants it, it belongs there or in an `HW_2.5` sandbox.

### Stage 10 — Video script `[ ]`

Exercise 2 asks for each item to be recorded working, the code shown and explained per item, and
the game played at the end. Modeled on `2026-HW_1-Mario/HW_1-Script.md`: spoken lines in block
quotes, stage directions outside them, each requirement called out in Hebrew so it's unmistakable
which item is being shown, one take per part.

Beyond the per-item walkthroughs, four things need saying explicitly on camera: why the health
system reuses HW1's strikes (asked and confirmed with the instructor), why the level builder
needed no code change to support new tiles, why Tiled is a bootstrap rather than a live source,
and that walking off a ledge leaves Mario one jump rather than two.

## Notes / Decisions Log

_(append entries here as we make design decisions.)_

- Exercise 2 arrived alongside Lesson 8, which ended `2026-1.5-Mario`. Two of that project's
  unstarted Stage 3 items are things Exercise 2 asks for directly (the ground-check extension as
  item 8's double jump, MVC as item 2's health system), so building them there first would have
  meant building them twice, in a project that isn't the one being submitted. Everything already
  finished there — the Star power-up and the whole level pipeline — travels here in the `Assets`
  copy.
- Item 2 keeps HW1's behavior of teleporting Mario back to the start on every hazard hit, rather
  than leaving him in place with a second of invincibility. Those are two solutions to the same
  problem, not standing on spikes while the whole health bar drains, and HW1 already solved it. The
  exercise text only specifies what happens when health reaches zero, which is unchanged.
- Item 3 cycles all tiles from scene load rather than triggering on Mario's touch. Closer to the
  text, much less code (no per-tile detection of who stepped where), and tiles started together
  stay in sync with no coordinator.
- Item 4 patrols from the home cell to two cells right and back, not two cells either side of it.
  Matches "2 right and another 2 left" literally, and keeps the home cell the cell the tile
  actually returns to, which the one-position-per-object level format needs.
- Item 4 carries riders by adding the tile's own movement to their positions rather than parenting
  them or relying on friction. Friction is ruled out by `PlayerMovement` braking Mario's X velocity
  toward zero in world space every frame, which cancels it; parenting works but makes the tile know
  about Mario, changes the Hierarchy at runtime, and is the usual source of jitter for a dynamic
  `Rigidbody2D`. Writing position keeps one responsibility on the tile and works for any rider.
- The Tiled tileset stays a Collection of Images rather than becoming a packed sheet. Both are
  routes to the same images with the same ids and the same level data; switching would mean
  re-authoring the tileset and renumbering every id, and 1.5's Stage 2 already lost a full day to
  one bulk change of that kind. The requirement's "image with all of them inside" is satisfied by a
  separate contact-sheet PNG instead.
- `Assets/Sprites/` and `Tiles01/Tiles/` stay as two separate copies of the same art. Tiled could
  point at the Unity files directly, but that means rewriting every `<image source=...>` line in
  `MarioTiles.tsx`, right next to the tile ids the whole mapping depends on. One file copy per new
  tile type buys a guarantee that Tiled can never reach into the Unity project.
- Walking off a ledge leaves Mario one jump rather than two, per Peleg. The exercise says only
  that the double jump recharges on landing, so this is interpretation: spending the ground jump
  by stepping off an edge makes the second jump a recovery rather than a free extra.
- Tiled appears twice in the work and twice on video, rather than being retired outright as 1.5's
  plan had it. Item 5 names the tile editor separately from the Level Create script, so the
  tileset genuinely has to be updated there, and item 9's "build the level using the tile editor"
  reads most naturally as Tiled too. Cheap to satisfy because `Level01.tmx` turned out to be four
  cells behind the live level rather than a rewrite behind it. The Tile Placer still does the real
  authoring work afterwards.
- Consider emailing the instructor about the editor-tooling differences with an eye to *future*
  hand-ins rather than this one — the seven departures from Lesson 6's `BuildLevel.cs` are all
  deliberate and all defensible, and it's worth knowing in advance whether he'd rather see his own
  shape in Exercise 3.
