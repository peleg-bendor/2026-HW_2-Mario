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
top of them. Stage 8.5 collects the projectile problems found while building other things, none of
which anything else depends on. Stage 9.5 holds optional work that is explicitly not required by
the exercise, and Stage 10 is the submission video. Each stage gets its own design discussion before code, the same
way every stage of HW1 and 1.5 did.

## Tile Roster

Three new tile types, not four: item 2's heart is the existing `Sprite_Strike`, already tile id
12, and gets renamed rather than added. The mapping's id is always the Tiled tileset's id plus
one, because Tiled's `firstgid` is 1, which also makes a `TilePrefabMap` id and a raw gid in a
Tiled export the same number.

The whole table rather than only the new rows, because nothing in either project verifies that the
two id spaces stay one apart, and the prefab and the Tiled image don't always share a name. Every
row below was read out of `TilePrefabMap.asset` by resolving its prefab GUID, not copied from
memory.

| `TilePrefabMap` id | Unity prefab | Tiled `.tsx` id | Tiled image | Stage |
|---|---|---|---|---|
| 1 | `Sprite_Gateway` | 0 | `Gateway.png` |  |
| 2 | `Sprite_Axe` | 1 | `Sprite_Axe.png` |  |
| 3 | `Sprite_Coin` | 2 | `Sprite_Coin.png` |  |
| 4 | `Sprite_Floor` | 3 | `Sprite_Earth.png` |  |
| 5 | `Sprite_Flower` | 4 | `Sprite_FireFlower.png` |  |
| 6 | `Sprite_Ghost` | 5 | `Sprite_Ghost.png` |  |
| 7 | `Sprite_Grave` | 6 | `Sprite_Grave.png` |  |
| 8 | `Sprite_Key` | 7 | `Sprite_Key.png` |  |
| 9 | `Sprite_Mario` | 8 | `Sprite_Mario.png` |  |
| 10 | `Sprite_Spikes` | 9 | `Sprite_Spikes.png` |  |
| 11 | `Sprite_Star` | 10 | `Sprite_Star.png` |  |
| 12 | `Sprite_Health` (renamed from Strike) | 11 | `Sprite_Health.png` | 2 |
| 13 | `Sprite_Vampire` | 12 | `Sprite_Vampire.png` |  |
| 14 | `Sprite_LightningBolt` | 13 | `Sprite_LightningBolt.png` | 1 |
| 15 | `Sprite_Rock` | 14 | `Sprite_Rock.png` | 3 |
| 16 | `Sprite_Cloud` | 15 | `Sprite_Cloud.png` | 4 |

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

### Stage 1 — Lightning bolt: temporary speed boost `[x]`

Exercise item 1. Mario collects a lightning bolt; his speed increases by 50% of his maximum for
5 seconds.

The fifth run of the pickup chain this codebase already uses four times (axe, fire flower, star,
strike): a trigger detector hands an `IPowerUp` to `PlayerPowerUp`, which applies it without
knowing which kind it got. No new abstraction — the pattern is being followed, not extended.

#### Step 1 — Design discussion `[x]`

#### Step 2 — Sprite and prefab `[x]`

`Sprite_LightningBolt.png` into `Assets/Sprites/` with Pixels Per Unit set to 48, then
`Sprite_LightningBolt.prefab` in `Assets/Prefabs/` with a trigger collider, mirroring
`Sprite_Star.prefab`.

#### Step 3 — `LightningBoltController` and `LightningBoltPowerUp` `[x]`

Detector plus effect, following `StarController`/`StarPowerUp` exactly, including the shared noun
across sprite, prefab, controller and power-up class.

#### Step 4 — `PlayerSpeedBoost` `[x]`

New receiver on Mario, coroutine-driven, mirroring `PlayerInvincible`. It captures Mario's base
speed once and restores to that value rather than dividing back out, so a second bolt collected
mid-boost can't compound the multiplier. A second pickup restarts the timer instead of stacking,
the same fix Stage 0.5 makes to invincibility.

Also subscribes to `SC_Death.OnHazardCollision` directly, the same event `PlayerDeath` and
`StrikesManager` already subscribe to independently, and cancels an active boost immediately on
any hazard hit rather than letting it run out on its own clock - Peleg's call, so a boost never
survives a respawn. Guarded by `PlayerInvincible.IsInvincible` the same way those two subscribers
already are, so a star still protects an active boost the same way it protects a strike.

#### Step 5 — Tile id and playtest `[x]`

`TilePrefabMap` row 14. Confirm the boost applies, expires on time, that a second bolt during the
boost restarts the clock without changing the speed, and that a hazard hit while boosted cancels
it immediately (speed back to base) unless a star is active, in which case the hit does nothing
and the boost keeps running.

**Confirmed working** by Peleg via `OutputLogsTemp.txt`: `activated`/`restarted`/`ended` fired
exactly once each across a boost and a mid-boost second bolt, with `restarted` only possible while
the first coroutine was still live; two independent hazard hits (one non-fatal, one the last
strike) each cancelled an active boost immediately, logged right after the respawn; and a star
picked up alongside a bolt protected both the strike count and the boost across five consecutive
hazard hits, with both effects then ending on their own separate natural schedules undisturbed.
The last-strike case cancelled the boost and reloaded the scene, coming back with no leftover
boost state.

### Stage 2 — Health points, built with MVC `[x]`

Exercise item 2, and the one item that dictates its own architecture. Mario has health points,
collectable up to a maximum of 3, lost on hazard contact; at zero he returns to the start and the
level restarts with every object reinitialized. The exercise requires MVC.

HW1's strike system already does all of this. The work is a rename plus a rebuild of the same
behavior in MVC shape, not new mechanics. The instructor confirmed directly that reusing the
existing system is fine.

#### Step 1 — Design discussion `[x]`

`IHealthModel` (`CurrentHealth`, `MaxHealth`, `Gain()`, `Lose()`, the latter two returning
whether they actually changed anything) and `IHealthView` (`ShowHealth(int)`), mirroring how
thin `StrikeCountManager`'s one job already is. `HealthModel` takes its max in its constructor
rather than carrying `[SerializeField]` itself, since it's plain C# - the tunable number moves up
to `HealthController`'s own `[SerializeField] private int maxHealth = 3`, the same field
`StrikesManager.startingStrikes` already is, just one layer out. Starting health equals max, the
same doubled meaning `startingStrikes` has today, so a heart pickup only does anything after
Mario's taken a hit.

Hierarchy stays parallel to what's there now rather than being reshuffled: `HealthController`
takes over the standalone `StrikesManager` GameObject under `Scripts`, `HealthView` takes over
`StrikeCountManager`'s spot directly on `Txt_Strikes` (renamed `Txt_Health`), and
`HealthController` reaches it through a `[SerializeField] private HealthView healthView` field -
the same cross-object reference shape `StrikeCountManager`'s own `[SerializeField]
TextMeshProUGUI` field already uses, just one hop further out. The controller pushes
`healthView.ShowHealth(model.CurrentHealth)` once in `Start()` and again after any `Gain()`/
`Lose()` call that returns `true`; a no-op call redraws nothing, matching how the old event
isn't raised on the ignored branches either.

`OnGameOver` moves to `HealthController` as the same shape of static event
`StrikesManager.OnGameOver` is today, which means `GameEndManager.cs` needs a one-line edit
(`StrikesManager.OnGameOver` -> `HealthController.OnGameOver`) even though it isn't one of the
files being renamed.

`Tiles01/Tiles/Sprite_Strike.png` gets renamed to `Sprite_Health.png` alongside the Unity-side
rename, with the matching one-line fix to `MarioTiles.tsx`'s `<image source=.../>` - Peleg's call,
over leaving the Tiled-side asset saying "Strike" forever. Cheap since Tiled is already a
one-time bootstrap.

Everything else carries over from the strike system exactly as HW1 built it and the plan already
called: health loss fires on every hazard type, not literally only spikes; every hit teleports
Mario to start, not just the zero-health one; the Star suppresses loss the same way it suppresses
strike loss today.

#### Step 2 — Interfaces and model `[x]`

`IHealthModel`/`IHealthView` in `Assets/Scripts/Interfaces/`, and `HealthModel` as a plain C#
class owning the count, the maximum, and the rules for changing it. No Unity types in the model.
`Gain()`/`Lose()` return whether they actually changed anything, so `HealthController` can
reproduce the old ignored-branch logging without the model touching `Debug.Log` itself.

#### Step 3 — View and controller `[x]`

`HealthView` draws the number onto the `Txt_` label (now `Txt_Health`). `HealthController`
subscribes to `SC_Death.OnHazardCollision` and `HealthPowerUp.OnHealthGained`, updates the model,
pushes to the view, and raises game over. Bundled into this step rather than left for Step 4, once
it became clear both were needed for the controller to compile and function at all: the pickup
pair's rename (`StrikePickupController`/`StrikePowerUp` → `HealthPickupController`/
`HealthPowerUp`), and `GameEndManager` repointed from `StrikesManager.OnGameOver` to
`HealthController.OnGameOver`.

Hierarchy: `HealthController` took over the standalone `StrikesManager` GameObject under
`Scripts`. `HealthView` took over the separate `StrikeCountManager` GameObject (also under
`Scripts`, a sibling of `StrikesManager` — not a component riding on `Txt_Strikes` itself, an
assumption the first pass at these instructions got wrong and had to correct). `Txt_Strikes`
renamed to `Txt_Health`, carries no script, just the label.

Stray comments elsewhere naming the retired `StrikesManager` were also fixed as part of this step:
`Portal.cs`, `PlayerDeath.cs`, `PlayerSpeedBoost.cs`.

#### Step 4 — Retire the strike system and rename `[x]`

By the time Step 3 was done, `StrikesManager.cs`/`StrikeCountManager.cs` and the Unity-side
`Sprite_Strike` prefab/sprite renames had already happened alongside it. What was left: the
Tiled-side rename, since `MarioTiles.tsx` points at `Tiles01/Tiles/Sprite_Strike.png` by filename
rather than GUID. Renamed the file and fixed the one `<image source=.../>` line to match — Peleg's
call, over leaving the Tiled-side asset saying "Strike" forever.

#### Step 5 — Playtest `[x]`

**Confirmed working** by Peleg via `OutputLogsTemp.txt`: health lost from a garlic hit, from
spikes twice, and from a ghost touch that reached zero; health gained from the heart pickup;
zero health drove `Game over - message pending` straight out of `HealthController:OnHazardCollision`
into `GameEndManager:HandleGameOver`, confirming the event ownership actually moved; and the
reload afterward reinitialized state cleanly (`Starting with 1 axe(s)`, spawner back to `1/3`).
Star suppression confirmed too, on a second read of the same log: two ghost hits logged by
`SC_Death` while `Invincibility ended` hadn't fired yet produced no `Health lost` and no respawn —
the same silent-skip shape `PlayerDeath`'s own guard has always had. Cap-at-3 wasn't separately
exercised (health only ever climbed from 2 to 3 in the log, not attempted past it), but `Gain()`'s
own guard is the same tested code path as `Lose()`'s zero-floor guard.

A leftover from the first pass at Step 3's instructions surfaced during this check and got
cleaned up: an extra, orphaned `HealthView` briefly sat on `Txt_Health` itself (from an earlier,
incorrect version of the wiring instructions) alongside the correctly-wired one on the `HealthView`
GameObject. Removed; confirmed gone from the saved scene afterward.

### Stage 3 — Disappearing floor tile `[x]`

Exercise item 3: a floor tile that appears for 2 seconds and disappears for 2 seconds.

Cycling from scene load for every tile at once, not triggered by Mario touching it. The exercise
text describes a cycle with no trigger, it needs no per-tile detection of who stepped where, and
tiles started at load stay in sync with no coordinator. The touch-triggered version is the more
classic-feeling one and gets a sentence in the video as the road not taken.

#### Step 1 — Design discussion `[x]`

#### Step 2 — Sprite, prefab and behavior `[x]`

`Sprite_Rock` imported and prefabbed with `SC_Floor` and a solid collider, so it counts as real
ground for the jump check and the projectiles. The component toggles the `SpriteRenderer` and the
`Collider2D` rather than calling `SetActive(false)` on the GameObject: a disabled GameObject stops
running its own coroutine, so the tile would vanish and never come back. Durations are serialized
fields, not literals.

`Sprite_Rock.png` turned out to already be sitting in `Assets/Sprites/` with import settings
already matching the project convention (Single sprite mode, 48 pixels per unit) - nothing to do
there beyond building the prefab.

#### Step 3 — Tile id and playtest `[x]`

`TilePrefabMap` row 15. Confirm Mario falls when a tile he's standing on vanishes, that the tile
returns, and that jumping off a rock tile works the same as off a floor tile. Last, once those are
confirmed: deliberately try to land Mario in a tile's space right as it reappears under him, and
note what happens - an accepted edge case from the design discussion, not something the tile is
built to prevent.

**Confirmed working** by Peleg in the editor: Mario falls when a rock tile under him vanishes, the
tile returns solid and visible on schedule, and jumping off one feels identical to jumping off
`Sprite_Floor`. The edge case got a harder test than planned - Mario placed surrounded by rock
tiles on all four sides at once, rather than just one reappearing underfoot. He's held in place for
a moment while the surrounding tiles are solid, freed again the next time they vanish, with no
crash, no death, and nothing left stuck afterward. Confirms the design discussion's call: worth
watching once, not worth building around.

### Stage 4 — Moving floor tile `[x]`

Exercise item 4: a floor tile that moves 2 tiles right and 2 tiles left.

Read as a patrol between its home cell and two cells right, returning to where it started — which
is what "2 right and another 2 left" describes, and it keeps the tile's home cell the cell it
actually returns to, which matters because the level file stores one position per object. Each
tile is its own object with no grouping: the data format stores one tile per cell, and clouds
placed side by side move in lockstep for free as long as they share a speed and start together.

#### Step 1 — Design discussion `[x]`

#### Step 2 — Sprite, prefab and movement `[x]`

`Sprite_Cloud` imported and prefabbed with `SC_Floor`, a solid collider, and a Kinematic
`Rigidbody2D` — the project's first collider that moves every frame, and Unity's own guidance is
that a moving collider belongs on a Kinematic body driven by `MovePosition` rather than a bare
collider with its `Transform` overwritten directly. `travelDistance` (world units right of home,
defaults to 2) and `speed` (units/second) are serialized fields; the tile patrols home → home +
`travelDistance` → home, forever, with `Vector2.MoveTowards` picking the current leg's target the
same way `PlayerMovement`'s braking already uses `MoveTowards` for a linear approach to a value.

`Assets/Scripts/Tiles/` is now a folder, holding `SC_Floor`, `DisappearingFloor`, and this stage's
`MovingFloor` — mirroring `Enemy/`'s precedent of grouping a category's behavior scripts regardless
of file count, settled now that a second tile-behavior script actually exists to decide the
question with, as Stage 3's design discussion deferred it to.

#### Step 3 — Carrying Mario `[x]`

Built alongside Step 2 rather than after it, the same way Stage 2's Steps 3 and 4 collapsed
together: carrying riders is most of what `MovingFloor`'s body does, so there was no version of
the script that patrolled without it.

The part with real risk. Mario will not be carried for free: `PlayerMovement` brakes his X
velocity toward zero in world space every frame no key is held, which cancels the friction that
would otherwise drag him along with a kinematic platform. The tile therefore tracks whatever is
resting on it and adds its own per-frame movement to those bodies' positions — writing position
rather than velocity, so the braking doesn't fight it. It carries anything standing on it without
knowing what, the same instinct as `EnemyMovement` skipping the `Player` tag rather than naming
Mario.

Detection reuses `SC_Floor`'s own landed-on-top-vs-touched-from-the-side check, pulled out into a
public static `SC_Floor.IsAboveTile` now that a second real use case for it exists.
`MovingFloor.OnCollisionStay2D` calls it every physics step and, for anything above it, adds the
tile's own per-frame delta straight to `col.rigidbody.position`. That single code path carries a
landed axe (resting via `RigidbodyConstraints2D.FreezeAll`) and a patrolling enemy (driven by its
own velocity every `FixedUpdate`) the same way, with no special case for either: constraints and
collision response only gate what the physics solver can do to a body, not a direct script write
to its `.position`, and a velocity-driven move and a post-solve position offset don't fight each
other for the same reason Mario's braking doesn't.

#### Step 4 — Tile id and playtest `[x]`

`TilePrefabMap` row 16. Confirm Mario rides the tile in both directions, can jump off it and land
back on it, that several clouds in a row behave as one platform, and that an enemy or a landed axe
on top is carried too rather than being a special case — including whether the axe's frozen state
rides along smoothly or shows any jitter against the tile's `MovePosition`-driven motion. Once the
basics hold: deliberately stand Mario on the seam between two synced clouds and note whether being
above both at once doubles his carried speed for that stretch — an accepted edge case from the
design discussion, the same call Stage 3 made about being boxed in by rock tiles, not something the
tile is built to prevent.

**Confirmed working** by Peleg in the editor and via `OutputLogsTemp.txt`: Mario rides in both
directions, jumps off and lands back on, stands still on a moving tile without being left behind
(the braking case the whole design was built around), several clouds behave as one platform, a
patrolling enemy is carried, and the two-cloud seam produced nothing worth building around. A
landed axe resting on top rides along too, logged across a full patrol leg - `Axe landed` followed
by six `Cloud tile turned back toward home` lines with no `Axe's support vanished, falling again`
in between, then a clean `Axe picked back up`.

Re-confirmed afterwards on the plain rider write, once the constraint save/clear/restore added for
a wrong diagnosis had been deleted: an axe thrown onto a cloud top still lands and rides, with no
spurious `Axe's support vanished, falling again`. So those four lines were never doing any work.

One case is genuinely broken and deferred rather than fixed here: an axe frozen against a cloud's
*side* is left hanging in mid-air when the tile slides away. See Stage 8.5.

### Stage 5 — Tiled tileset and the level builder `[x]`

Exercise item 5: add the new tiles to the tile editor, export an image with all of them in it, and
develop the level-creating editor script to support them.

The builder half is already done by design and that's the point worth explaining rather than
hiding: `LevelWindow` reads `TilePrefabMap`, so a new tile type is a row in an asset, not an edit
to the tool. That's the Open/Closed Principle from Lesson 3 applied to the lesson's own hardcoded
`switch`, and it's a stronger answer than editing a switch statement — but only if it's said out
loud, since a reader expecting to watch the script change will otherwise see no change at all.

Confirmed by reading the files rather than repeated from the earlier draft of this plan:
`LevelWindow.cs` holds no tile id anywhere, `PlaceLayer` goes straight to
`tilePrefabMap.GetPrefab(tileId)` and collects anything unmapped into a warning, and
`TilePlacerWindow` builds its dropdown from `tilePrefabMap.Entries`. All 16 rows of
`TilePrefabMap.asset` already have their prefabs assigned. There is genuinely nothing to change in
either tool.

#### Step 1 — Fix the coin's image path in `MarioTiles.tsx` `[x]`

Tile id 2 points at `Tiles/Sprite_Coin 1.png`, which doesn't exist. The folder holds
`Sprite_Coin.png`, byte-identical to the coin art the game uses, so Tiled has been showing the coin
as a broken tile for as long as the tileset has existed. A one-line edit to the file with Tiled
closed, ahead of everything else: deleting and re-adding the tile through the UI would renumber it
and break every coin in the map, and a tileset saved while one entry is broken risks Tiled dropping
that entry.

#### Step 2 — Add the three tiles through Tiled `[x]`

`Tileset -> Add Tiles`, one at a time in the order lightning bolt, rock, cloud, so Tiled assigns
ids 13, 14 and 15 to line up with `TilePrefabMap`'s 14, 15 and 16. Adding all three in one
multi-select would let the file dialog's alphabetical order decide instead (Cloud, LightningBolt,
Rock), which wouldn't error - it would turn every cloud already in the level into a bolt at the next
export. Peleg's call over hand-editing the `.tsx`: the tiles genuinely get added in the tile editor,
which is what item 5 asks for and what the video shows. The 48px copies already sit in
`Tiles01/Tiles/`, all three byte-identical to their `Assets/Sprites/` twins.

#### Step 3 — Re-sync the map and fix its export target `[x]`

`Level01.tmx` is 41 cells behind the live level, not the four an earlier draft of this plan
recorded. That figure predated Stages 1, 3 and 4, each of which painted its new tile into the level
several times over on top of 1.5's Tile Placer test: 11 rocks, 6 clouds, 2 bolts, 2 hearts, a star,
a grave, five coins added and two removed, three floors added and seven removed. Grid size is
unchanged at 30x20.

Rather than repainting 41 cells by hand, the `<data>` block gets regenerated from
`Assets/Levels/Level01.txt`. `firstgid` is 1, so a map id and a gid are the same number and the
conversion is a reflow of the same 600 integers with no translation step. That hands Peleg the
current level to author on top of in Tiled instead of a stale one to reconstruct first.

The export target moves to `../2026-HW_2-Mario/Assets/Levels/Level01.txt`, `format="json"` kept.
Two things are wrong with it rather than one: it aims at the closed 1.5 project, and it ends in
`.json` while the live file has been `Level01.txt` since Stage 9 Step 1, so fixing only the folder
would drop a second, unread level file beside the real one. Repointed after the data is current,
since the moment it aims here, Export replaces the live level with whatever Tiled holds.

#### Step 4 — The all-tiles image, dropped `[x]`

Not produced, per Peleg. Item 5's "ולהוציא תמונה עם כולם בפנים" answers a problem that only exists
when the tileset is one packed spritesheet: there, the image with every tile in it is the tileset
file itself and comes for free. A Collection of Images has no such file, so satisfying the clause
would mean manufacturing a PNG that nothing in the project reads, purely to be looked at once.

Covered on camera instead, which is where it does some good: the Tiled tileset panel with all 16
tiles visible while the choice gets explained, so the shot is an image with all of them inside and
the reason there's no packed sheet at the same time. See Stage 10.

#### Step 5 — The export round trip `[x]`

The builder placing all three is already proven: Stages 1, 3 and 4 each played their tile in-game,
which means `LevelWindow` has built them out of the level file many times over. What has never been
tested is the direction from Tiled. Export once, check the resulting `Level01.txt` cell for cell
against what it held before, Build Level, then Save Level to return the file to `LevelWindow`'s own
compact shape. A Tiled export carries the whole Tiled JSON rather than the reduced form the tool
writes, and `JsonUtility` ignores every key `TiledMap` doesn't declare, so it parses either way.

Overlaps Stage 9 Step 2 on purpose, so video day repeats a path that already worked instead of
trying it for the first time on camera.

**Confirmed working** by Peleg, and by diffing the files afterwards rather than trusting the
counts. He ran a longer loop than the step asked for: Tiled export into `Level01.txt`, Build Level,
Save Level, Build Level again. All three logged 154 objects, no warnings of any kind, and all 600
cells of the resaved `Level01.txt` came back identical to `Level01.tmx`. The second half is the
part worth having: `Save Level` recovers a tile's id through
`PrefabUtility.GetCorrespondingObjectFromSource`, so an unchanged file proves the three new prefabs
are recoverable as ids and not merely placeable from them - 10 rocks, 6 clouds and 2 bolts among
the 154. Tiled's raw export can't be inspected after the fact, since Save Level overwrites it with
the compact form, but a bad export would have built a wrong scene and the resave couldn't have
matched.

### Stage 6 — Tile placer covers every object `[x]`

Exercise item 6. Also already true by design: `TilePlacerWindow` builds its dropdown from
`TilePrefabMap`, so its list cannot disagree with the builder's. Verification rather than
construction — place one of each of the sixteen types, save, rebuild, confirm the round trip.

Stage 5's round trip already proved most of the underlying half. Save Level recovers a tile's id
through `PrefabUtility.GetCorrespondingObjectFromSource`, and the level came back unchanged, so 15
of the 16 mapped prefabs are known to survive scene-to-file in both directions. The exception is id
6, the ghost: nothing in `Level01.txt` places one, so that row has never been exercised end to end
in either direction. What's genuinely left for this stage is the window's own dropdown, and the
ghost.

**Confirmed working** by Peleg, and by reading the saved file rather than the object count. The
dropdown lists all 16, `1 - Sprite_Gateway` through `16 - Sprite_Cloud`. One of each was placed
along world y = 1, below the playfield in rows the level leaves empty, then saved: ids 1 to 16
came back in order at the cells they were clicked into, ghost included. Deleting the 16 and saving
again returned the file to its previous 154 objects with nothing stranded, and it still matches
`Level01.tmx` cell for cell, so nothing here left Tiled out of sync.

#### Also fixed here — `Parent` didn't survive an editor restart `[x]`

Found by Peleg closing and reopening Unity during this stage's testing, and the other half of a fix
Stage 3 already made once. Both windows' `Level File` and `Tile Prefabs` came back assigned while
`Parent` came back empty, because the first two are assets and serialize as a GUID, while a scene
`GameObject` has only a per-session instance id and a window's saved layout has nothing durable to
write. That's also why Stage 3's `[SerializeField]` fix looked complete: instance ids survive a
script recompile, so the field held through every domain reload and only broke on a full restart.

Fixed by serializing the object's *name* alongside the reference and looking it up again whenever
the reference is empty, in a shared `SceneObjectMemory` helper rather than the same four lines and
the same non-obvious explanation written into both windows - the two-real-use-cases threshold this
project used for `SC_Floor.IsAboveTile`. `GlobalObjectId` would survive renames and hierarchy moves
too, and was turned down: the only thing being pointed at is a root object called `World` that the
whole project's conventions already depend on, so it would be robustness against a case that can't
arise. The name defaults to `"World"`, so a window opened for the first time now resolves its own
parent with nothing assigned, which is better than where this started rather than merely back to
it. One accepted cost: clearing the field by hand no longer sticks, since the next repaint finds
`World` again.

### Stage 7 — Deleting objects from the tile placer `[x]`

Exercise item 7, and the only genuinely new tooling work in the exercise. The window could only
replace what's in a cell as a side effect of placing something; there was no way to delete without
placing.

`placingEnabled` became a three-state `Mode` (Off, Place, Erase) rather than gaining a second bool
beside it, so "placing off, erasing on" can't exist as a state something would have to interpret.
Erasing reuses `ClearCell`, which `Place` already called to stop two objects stacking in one cell,
with the one change that it now returns how many objects it destroyed so a click on an empty cell
stays silent instead of claiming it deleted something. Same undo collapse `Place` uses, so one
Ctrl+Z restores a whole cell rather than one press per object. The Scene view preview turns red
while erasing, which is where the mode needs to be visible since that's where the cursor is, and
the Tile dropdown greys out because erasing doesn't read it.

**Confirmed working** by Peleg via `OutputLogsTemp.txt`, cross-checked against the level file
rather than the object counts. Three erases at (8, 15), (9, 15) and (10, 15) logged one object
each, and exactly those three cells - all coins - came back zeroed in the saved file with nothing
else moved, 154 down to 151 across both Save and Build. No `Erased 0 object(s)` lines anywhere, so
clicks on empty cells stayed quiet. Undo confirmed on both paths: a placed heart in the log left no
trace in the saved file because it had been undone, which is the place path, and the erase path was
tested separately afterwards.

### Stage 8 — Double jump and the in-air extension `[ ]`

Exercise item 8: an extension method reporting whether the player is in the air, used to allow a
second jump, with the double jump recharging on landing.

This is the item Stage 3 of the 1.5 project was going to build from the other direction, so its
design points carry over: build the extension around the box probe this project already has rather
than the lesson's degenerate `Physics2D.OverlapArea` (both corners at the same point), and keep
`SC_Floor.OnFloorCollision` subscribed, since landing is what recharges the jump.

Worth knowing before the discussion starts, since the wording above once suggested otherwise:
the probe isn't somewhere else waiting to be ported. It's already sitting in
`PlayerJump.IsGrounded()`, an `OverlapBoxAll` across Mario's own footprint filtered to `SC_Floor`,
with its own comments explaining the width factor and the corner-perch case it was built for. Step
2 moves that body into an extension rather than writing a new one.

#### Step 1 — Design discussion `[x]`

Held before any code, same as every stage. Full outcome in the Decisions Log; the steps below are
what it settled on.

#### Step 2 — The extension `[ ]`

New `Assets/Scripts/Extensions/` folder, holding `GroundedExtension.cs` - the lesson's own file and
class name, so the parallel is immediate to anyone comparing the two.

The extension goes on `Collider2D` rather than the lesson's `Transform`, because our probe reads
collider bounds while the lesson's reads only a position: a `Transform` version would have to
`GetComponent` on every call or hardcode a size. `IsGrounded(float probeDepth)` carries the body
moved out of `PlayerJump`; `IsInAir(float probeDepth)` is its one-line negation. Both public.

No `LayerMask` parameter, unlike the lesson. This project decides what counts as terrain by
`SC_Floor` rather than by layer, and that filter belongs inside the extension, which makes it a
project-specific helper rather than a general Unity one. The probe's width factor moves in as a
`const` (it describes the probe's own shape); `groundProbeDepth` stays serialized on `PlayerJump`
and is passed in, so it remains tunable per object.

#### Step 3 — `PlayerJump` `[ ]`

The `isJumping` bool becomes `jumpsUsed`, counting jumps spent since the last landing, against a
serialized `maxJumps` defaulting to 2. `SC_Floor.OnFloorCollision` resets it, still guarded so only
a real transition logs. `PlayerJump.IsGrounded()` is deleted outright rather than kept as a wrapper.

`Jump()` reads `bool inAir = bodyCollider.IsInAir(groundProbeDepth)` into a named local, charges
the ground jump when `inAir && jumpsUsed == 0` (the ledge rule), then rejects on
`jumpsUsed >= maxJumps`. Vertical velocity is zeroed before the impulse.

Two comments need rewriting rather than adjusting: the file header still describes the class as
owning "whether a jump is still in progress", and the two-condition comment inside `Jump()`
explains logic that no longer exists.

#### Step 4 — Playtest `[ ]`

Two jumps and no third; recharge on landing; walking off a ledge leaves exactly one jump; and no
regression in the corner-perch case the current probe was built for. Added from this stage's
discussion: that the double jump reaches the same height whether it's taken at the apex or deep
into a fall (the velocity zeroing), that a rock tile vanishing underfoot leaves one recovery jump
the same way a ledge does, and that riding a cloud reads as grounded and landing back on one
recharges normally.

`maxJumps` needs no Inspector work - a new field takes its code default - but should be eyeballed
on the prefab rather than assumed. Setting it to 3 for a triple jump and back is also the cheapest
demo of why the number is serialized at all.

### Stage 8.5 — Projectile problems found along the way `[ ]`

No exercise item asks for any of this. Three things turned up while building other stages, none of
them load-bearing; they are collected here rather than fixed on the spot because the required items
are worth more. One of the three is already done, since it turned out to cost nothing.

#### An axe frozen against a moving tile's side `[ ]`

A real bug found during Stage 4's playtest.

A landed `ProjectileAxe` rests by hard-freezing its `Rigidbody2D` (`RigidbodyConstraints2D.
FreezeAll`), so an axe that stops against a cloud tile's vertical face is held up by nothing but
that freeze. `MovingFloor` only carries what passes the above-the-tile check, correctly - dragging
anything merely touching a side would haul Mario sideways through walls too - so the tile slides
away and leaves the axe hanging in empty space. Stage 3's own fix for the disappearing tile can't
see this version: it watches `restingOn.enabled`, and a moving collider stays enabled the whole
time.

Two things keep this small. The axe despawns on its own `lifetime` (10s, fading from 7s), so a
stranded one clears itself within seconds. And an axe landing on *top* of a cloud already rides
along correctly, which is the case that actually comes up in play.

The fix worth making generalizes Stage 3's rule rather than adding a second one beside it: replace
the `restingOn.enabled` check in `ProjectileAxe.Update` with one that asks whether the axe is still
touching `restingOn` at all (`Collider2D.IsTouching`). "My support is gone" then covers a collider
that vanished and one that moved out from under it through the same branch, and the riding-on-top
case is untouched because contact holds the whole way. Two known costs: it edits code Stage 3
confirmed working, so the disappearing-tile case has to be re-tested alongside it, and a cloud
sliding *into* a frozen axe keeps contact, so the axe stays put while the tile passes through it
for a moment.

#### Fireball and garlic airtime `[x]`

Not part of Stage 8.5's bug and not deferred with it - raised by Peleg during Stage 5's discussion
and fixed on the spot, since it turned out to be two Inspector values and no code.

Both projectiles end `Attack` with `Destroy(gameObject, lifetime)`, so `lifetime` is seconds of
flight, but both also die early when `OnTriggerEnter2D` finds an `SC_Floor`. Only the garlic was
ever limited by its timer: `Garlic.prefab` already had `Gravity Scale` 0 so it flew level for the
full 3 seconds, while `Fireball.prefab`'s `Gravity Scale` of 0.5 dropped the fireball into the
ground within about half a second, two or three cells out, so its own 3-second `lifetime` was never
reached and raising it alone would have done nothing.

Fireball `Gravity Scale` 0.5 -> 0 and both `lifetime` values 3 -> 6. The fireball and the garlic now
have the same flight model, which is what they always read as on screen.

#### A latent bug in the axe throw, found alongside it `[ ]`

Nobody asked for this and nothing depends on it; recorded because it is real and nothing else has
it written down. `ProjectileAxe.Attack` calls `AddForce(new Vector2(direction * speedX, speedY))`
with no `ForceMode2D`, so it takes the default `Force` mode and one call lasts exactly one physics
step. `Axe.prefab`'s `speedX: 500` and `speedY: 30` therefore land as roughly 10 units/second
sideways and 0.6 up. Both `ProjectileFireball` and `ProjectileGarlic` pass `ForceMode2D.Impulse`
and carry the comment explaining why, so this codebase documents the lesson twice and applies it to
two projectiles out of three.

The throw feels fine as it is, because those two numbers were tuned against the behaviour the bug
produces. What the bug actually costs is that both of them silently depend on the project's Fixed
Timestep. Fixing it means passing `Impulse` and retuning: with mass 1, `speedX` 500 becomes 10 to
reproduce today's throw exactly.

### Stage 9 — The full level `[ ]`

Exercise item 9: build a complete playable level with the tile editor and save the text file with
all the level information inside Unity.

#### Step 1 — Rename the level file `[x]`

`Assets/Levels/Level01.json` becomes `Level01.txt`. Unity imports `.json` as a `TextAsset` exactly
like `.txt`, so this changes nothing functionally — it matches the instructor's own
`Level01-Mario00.txt` and removes any question about whether the deliverable is a text file.
Renamed from inside Unity's Project window so the `.meta` follows and the GUID survives.

Done early, during Stage 1's design discussion, since it cost nothing to do immediately rather
than wait for Stage 9. `LevelWindow` holds the file as a `TextAsset` object reference, not a
path string, so the rename needed no code change and the Inspector reference survived it.

#### Step 2 — Export from Tiled once, then author in Unity `[ ]`

Item 9 asks for the level to be built "using the tile editor", which most likely means Tiled, and
Stage 5 leaves Tiled holding a current map with the new tile types in it. So export from there
straight into `Assets/Levels/Level01.txt` and Build Level once, which covers the strict reading of
the requirement and gives the video one clean Tiled-to-Unity beat.

Stage 5 Step 5 runs this path once already, so what happens here is a repeat of something known to
work rather than the first attempt.

Everything after that is Unity-side: `Tools → Tile Placer` for the real placement work, putting
the new tile types where each can be demonstrated on camera, then `Save Level` and `Build Level`
to confirm the round trip.

Leaves one thing owed to Stage 10, because the Tile Placer work makes `Level01.tmx` stale the
moment it starts: re-sync the map from the finished `Level01.txt` before recording anything, or the
on-camera Export replaces the authored level with whatever Tiled still remembers.

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
and what walking off a ledge does to the jump count. That last one has to be worded as what it
is - stepping off an edge goes from no jump at all to exactly one - rather than as a restriction,
since the game never gave two there in the first place.

Item 5's beat is a live edit in Tiled followed by an export and a Build Level in Unity, per Peleg -
the requirement names the tile editor separately from the Level Create script, so showing the tool
being used answers it better than showing a finished file. It also carries the one part of item 5
that produces no file: with the tileset panel open and all 16 tiles on screen, say that each PNG
was added on its own rather than as one packed sheet, and that this is why there's no separate
all-tiles image to show. That shot is the image. That needs one thing set up beforehand:
`Level01.tmx` re-synced from the final `Level01.txt` the same way Stage 5 Step 3 does it, so the
on-camera edit is a small addition on top of the real level and pressing Export adds only that
change instead of replacing everything the Tile Placer authored.

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
  by stepping off an edge makes the second jump a recovery rather than a free extra. Worth stating
  carefully on video, because "one rather than two" describes the choice against the other
  interpretation, not against the game as it stands: today `Jump()` rejects outright when the
  probe finds no ground, and `isJumping` is false the whole way down a ledge, so stepping off an
  edge currently means no jump at all. The change is zero to one.
- Tiled appears twice in the work and twice on video, rather than being retired outright as 1.5's
  plan had it. Item 5 names the tile editor separately from the Level Create script, so the
  tileset genuinely has to be updated there, and item 9's "build the level using the tile editor"
  reads most naturally as Tiled too. Cheap to satisfy because `Level01.tmx` turned out to be four
  cells behind the live level rather than a rewrite behind it. The Tile Placer still does the real
  authoring work afterwards.
- Stage 1 design decisions, made across the stage's own discussion before any code was written:
    - There is no separate "maximum speed" field anywhere in the codebase - `PlayerMovement`
      writes `rigid.linearVelocity` straight from `speed` with no acceleration ramp, so Mario's
      horizontal speed is always either 0 or exactly `speed`. That makes `speed` itself the
      maximum the exercise refers to, and the boost is a plain `speed → speed * 1.5` for
      `boostDuration` seconds. Nothing to build beyond that; worth a sentence on video since a
      reader expecting a `maxSpeed` field won't find one.
    - `PlayerSpeedBoost` writes `PlayerMovement.speed` directly rather than either growing
      `PlayerMovement` with an `ApplyBoost`/`ClearBoost` pair or having `PlayerMovement` read a
      multiplier off it each frame. The former hands movement a timer it has no other reason to
      own; the latter points the dependency backward, making movement aware of power-ups.
    - Base speed is captured once in `PlayerSpeedBoost.Awake()` (mirroring `PlayerInvincible`'s
      base-color capture) and every activation computes from that captured value rather than the
      current one, so a second bolt collected mid-boost can't compound the multiplier and
      restoring never drifts from repeated division. The one accepted cost: `Awake`'s value
      becomes the permanent definition of "base" - nothing today ever changes Mario's walk speed
      permanently, so this isn't guarding against a real case yet.
    - No `IsBoosted` property. Nothing outside `PlayerSpeedBoost` needs to read boost state, unlike
      `PlayerInvincible.IsInvincible`, which `PlayerDeath` and `StrikesManager` both consult.
      Adding one now would be an abstraction with no second caller.
    - No visual cue on the boost. A tint would collide with the star's: both would write
      `SpriteRenderer.color` and restore to a base captured independently, so overlapping the two
      effects would end with whichever expires first stomping the other's tint back to the wrong
      color. Making that correct needs a tint arbiter, which is real new abstraction for a cosmetic
      the exercise never asked for. The boost is demonstrated on video by speed alone.
    - `deceleration` on `PlayerMovement` is left unscaled during a boost, so Mario slides further
      before stopping while boosted (stopping distance grows from 0.31 to 0.70 units at the
      exercise's 50%/5s numbers, per `PlayerMovement`'s own stopping-distance comment). Reads as
      momentum, which suits a lightning bolt; revisit only if it feels wrong in the playtest.
    - An active boost cancels immediately on any hazard hit - Peleg's call over the alternative of
      letting it run out on its own clock regardless of a respawn. New code with no direct
      precedent (`PlayerInvincible`'s own timer isn't touched by anything but a second star), but
      it reuses the exact subscribe/guard shape `PlayerDeath` and `StrikesManager` already use for
      the same event, including the same `PlayerInvincible.IsInvincible` guard, so a star still
      protects an active boost the way it protects a strike.
    - Naming follows the star quartet's single shared noun rather than the plan's original
      `LightningPickupController`/`LightningPowerUp` (which didn't match the already-existing
      `Sprite_LightningBolt` sprite and prefab): `LightningBoltController` and
      `LightningBoltPowerUp`, alongside `PlayerSpeedBoost` for the receiver - named for the effect,
      like `PlayerInvincible`, not the pickup, since the receiver has no business knowing a bolt
      caused it.
    - `Level01.json` renamed to `Level01.txt` during this stage's discussion rather than waiting
      for Stage 9 - see Stage 9 Step 1, done early because it cost nothing to do immediately.
- Stage 2 design decisions, made across the stage's own discussion and while implementing it:
    - `IHealthModel`'s `Gain()`/`Lose()` return whether they actually changed anything rather than
      the new count, so `HealthController` can tell an ignored change (already at max, already at
      zero) from a real one without re-reading the value, and so the model needs no `Debug.Log`
      of its own despite being the thing deciding whether anything happened.
    - The tunable health numbers live on `HealthController` as `[SerializeField] private int
      maxHealth`, not on `HealthModel` - the model is plain C# with no Unity types, so the
      Inspector-facing number necessarily sits one layer out, the same way `StrikesManager.
      startingStrikes` did before it. Starting health equals max, the same doubled meaning
      `startingStrikes` had.
    - The pickup rename (`StrikePickupController`/`StrikePowerUp` → `HealthPickupController`/
      `HealthPowerUp`) and the `GameEndManager` repoint (`StrikesManager.OnGameOver` →
      `HealthController.OnGameOver`) both got pulled into Step 3 rather than left for Step 4 as
      originally sketched, once it became clear `HealthController` couldn't compile or function
      without them - both were things Step 4 assumed already existed.
    - The first pass at the hierarchy-wiring instructions got it wrong: `StrikeCountManager` was
      assumed to be a component riding directly on `Txt_Strikes`, when it was actually its own
      standalone GameObject under `Scripts`, a sibling of `StrikesManager`, reaching `Txt_Strikes`'s
      `TextMeshProUGUI` through a plain cross-object field. Caught by re-reading the scene file
      rather than trusting the earlier read, after Peleg asked about a GameObject the first
      instructions never mentioned. Left a real loose end behind - an orphaned `HealthView` on
      `Txt_Strikes`/`Txt_Health` itself, wired to nothing - found and removed before Step 5's
      playtest closed the stage out.
    - `Tiles01/Tiles/Sprite_Strike.png` renamed to `Sprite_Health.png`, with the matching one-line
      fix to `MarioTiles.tsx`'s `<image source=.../>`, over leaving the Tiled-side asset saying
      "Strike" forever - Peleg's call, cheap since Tiled is already a one-time bootstrap.
- Stage 3 design decisions, made across the stage's own discussion before any code was written:
    - The new component is `DisappearingFloor`, not `DisappearingRock`/`DisappearingTile` - the
      project already has `Sprite_Floor.prefab` carrying `SC_Floor` for plain terrain, so "Floor" is
      the established noun for this category, and naming after the sprite would tie the class to one
      specific look rather than the behavior. Chosen with Stage 4's `MovingFloor` in mind too: both
      are adjective-first compounds naming the behavior described in the exercise's own wording (a
      floor that disappears, a floor that moves), so the family reads consistently even though the
      two tiles' sprites and implementations share nothing. No base class or interface between them,
      though - `DisappearingFloor` is a timer toggling a `SpriteRenderer`/`Collider2D`, `MovingFloor`
      will be a per-frame position write with rider tracking, and the only thing they actually share
      (being a floor tile, participating in the jump check) already lives in `SC_Floor`, which both
      carry as their own separate component. Two known future use cases don't justify an abstraction
      when neither would share any code through it - only a naming convention.
    - Sits on the `Sprite_Rock` prefab as a second component alongside `SC_Floor`, not a replacement
      for it - `SC_Floor` keeps owning "is this a floor tile" and the landing signal,
      `DisappearingFloor` only owns the visibility cycle.
    - `visibleDuration` and `hiddenDuration` are two separate serialized fields, both defaulting to
      2, rather than one shared number - independently tunable even though the exercise asks for the
      same value on both sides.
    - The cycle starts visible, not hidden, matching the exercise's own word order (appears, then
      disappears) - the level opens with every rock tile solid.
    - No coordinator object. Every tile runs its own identical coroutine
      (`while (true) { Show(); wait; Hide(); wait; }`) started from its own `Start()`. They stay in
      sync because the Level builder places every tile before Play begins, so every instance's
      `Start()` runs the same frame, and identical `WaitForSeconds` calls from the same frame resolve
      together - including after a mid-game reload, which rebuilds the scene and restarts every
      instance from the same frame again.
    - Logging follows the codebase's per-transition convention, with one addition: a static
      `Time.frameCount` guard (one for the show transition, one for hide) means whichever tile's
      coroutine reaches a transition first in a given frame logs it, and every other tile hitting the
      same transition that frame stays quiet - one log line per collective appear/disappear
      regardless of how many rock tiles are in the level, without a shared timer object or any change
      to the per-tile toggle logic itself.
    - `Assets/Scripts/Tiles/` as a home for `DisappearingFloor` and Stage 4's `MovingFloor` is
      deferred to Stage 4's own discussion, once a second tile-behavior script actually exists to
      decide the question with.
    - One known edge case, deliberately not designed around: a tile reappearing at the exact moment
      Mario's collider overlaps the space it resolidifies in could shove him out abruptly that frame.
      Left as a playtest check at the end of Step 3 rather than prevention code, since building
      around it would mean the tile needing to know about Mario specifically. Tested harder than
      planned - Mario boxed in by rock tiles on all four sides at once - and held in place for a
      moment, then freed clean on the next vanish, with no crash, no death, and nothing left stuck.
    - `LevelWindow` and `TilePlacerWindow` had their `ObjectField`-backed fields as plain `private`
      rather than `[SerializeField]`, so Unity's own window-layout persistence never picked them up,
      and a full editor close and reopen reset the level file, tile prefab map, and parent
      references every time. Found and fixed during this stage's testing, unrelated to the stage
      itself: marking the fields `[SerializeField]` lets Unity persist them the same way it already
      persists a window's position and size.
    - Throwing an axe against a rock tile and letting the tile disappear exposed a real bug in
      `ProjectileAxe`: a landed axe rests by hard-freezing its `Rigidbody2D`
      (`RigidbodyConstraints2D.FreezeAll`), not by gravity and a normal contact force, so the tile
      vanishing from under or beside it left the axe locked in mid-air with nothing supporting it.
      Fixed by having the axe track the specific collider it froze against and check every frame
      whether that collider is still enabled - if not, it drops back to the prefab's own resting
      constraint (`FreezeRotation`) and resets to the same not-yet-landed state a freshly thrown
      axe starts in, so the same collision branch handles what happens next: killing an enemy it
      falls onto, or landing again elsewhere. This only reacts to a collider being disabled, not
      to one that stays enabled but moves - which is what Stage 4's moving floor tile does, so a
      landed axe resting on or against a cloud is a case that stage's own design needs to consider
      separately, not something this fix already covers.
- Stage 4 design decisions, made across the stage's own discussion before any code was written:
    - `Assets/Scripts/Tiles/` is created now and holds `SC_Floor`, `DisappearingFloor`, and
      `MovingFloor` together, not just the two behavior scripts. `Enemy/` is the precedent - four
      files grouped by category regardless of count - and leaving `SC_Floor` behind at `Scripts/`
      root would split one category across two locations for no reason. A plain file move inside
      the editor, so the prefab's script reference survives on its GUID with nothing to reassign.
    - The Cloud prefab gets a Kinematic `Rigidbody2D`, moved with `MovePosition`, rather than a bare
      collider with its `Transform` overwritten directly. Every other floor tile has no
      `Rigidbody2D` at all, which is fine for something that never moves, but this would be the only
      moving collider in the project without one - against Unity's own guidance, which is to drive
      a moving collider from a Kinematic body rather than force the physics engine to keep pulling a
      "static" collider out of and back into the broadphase every frame. Costs Peleg one extra
      Inspector step (Body Type to Kinematic) beyond what the other tile prefabs needed.
    - Movement is continuous, not the discrete stepped motion the plan's original "step timing"
      phrasing could be read as - `Vector2.MoveTowards` toward whichever end of the patrol is
      currently the target, flipping the target on arrival, driven by two serialized fields:
      `travelDistance` (world units right of home, defaults to 2) and `speed` (units/second,
      matching `EnemyMovement`'s own field name and role). The patrol loops forever from scene load
      (home → +2 → home → +2 → ...), the same as `DisappearingFloor`'s cycle never stopping.
    - No coroutine or `Start()`-timing trick is needed to keep several clouds in lockstep, unlike
      `DisappearingFloor`'s reliance on every instance's `Start()` landing on the same frame.
      `MoveTowards`-driven motion from `FixedUpdate` is a pure function of elapsed fixed ticks since
      each instance began, with no `WaitForSeconds` to drift - same `speed` and `travelDistance`
      keeps same-speed tiles bit-for-bit synchronized for a different reason than `DisappearingFloor`
      gets its sync from, not the same mechanism.
    - Rider carrying reuses `SC_Floor.OnCollisionEnter2D`'s own landed-on-top-vs-touched-from-the-
      side check rather than a new trigger zone or per-frame `GetContacts` polling, since the
      geometry test - is the other object's centre above the tile's by roughly its own collider's
      half-height - is identical either way. Pulled out into a public static
      `SC_Floor.IsAboveTile(Collision2D, Transform)`, called from both `SC_Floor` and the new
      `MovingFloor.OnCollisionStay2D`, rather than duplicated - the second real use case the
      project's own rule about not generalizing early was waiting for. `OnCollisionStay2D` (fired
      every physics step two colliders remain in contact) reuses the tile's existing solid collider,
      so nothing new needs Inspector setup.
    - For a contact that passes the check, `MovingFloor` adds its own per-frame delta straight to
      `col.rigidbody.position`, not velocity, and not through `MovePosition` - that call moves the
      tile's own body, this one moves whatever is resting on it. This one code path was checked
      against both a landed axe and a patrolling enemy without needing either as a special case:
      `RigidbodyConstraints2D.FreezeAll` (how a landed `ProjectileAxe` rests, see the bullet above)
      only blocks what the physics solver's own forces and collision response can do to a body, not
      a plain script write to `.position`; and `EnemyMovement`'s own velocity writes and
      `MovingFloor`'s post-solve position offset move the same rigidbody through two different
      channels that don't contend, the same property Mario's braking not fighting the carry already
      relied on. Whether this holds up smoothly against the axe's frozen state specifically - no
      jitter, no lag - is a playtest check at the end of Step 4 rather than something taken on faith.
    - One known edge case, deliberately not designed around, the same call Stage 3 made about being
      boxed in by rock tiles: Mario standing exactly on the seam between two synced Cloud tiles can
      get flagged as "above" both at once, so both add their delta the same physics step and his
      carried speed doubles for as long as he straddles it. Left as a playtest note at the end of
      Step 4 rather than bookkeeping to prevent it.
    - Confirmed by reading `LevelWindow.Save()` rather than assumed: it reads each tile's
      `transform.localPosition` at the moment the button is clicked, and MonoBehaviour lifecycle
      methods only run in Play mode, so a Cloud tile never actually moves during the Edit-mode
      authoring workflow Save Level is used in. Its home cell is always what gets saved - not a risk
      that needed designing around.
    - The design discussion's claim that a landed axe would be carried "with zero axe-specific code"
      turned out right, but only after two wrong diagnoses of a symptom that was never the on-top
      case at all. First theory: `RigidbodyConstraints2D.FreezeAll` was fighting the rider write, so
      `MovingFloor` saved, cleared and restored the rider's constraints around it. Second theory:
      the axe's `Rigidbody2D` falls asleep once landed (nothing writes its velocity, unlike Mario
      and the enemy, whose `FixedUpdate`s assign `linearVelocity` every step) and Unity stops
      sending `OnCollisionStay2D` to a sleeping body. Both were reasoned out rather than tested, and
      both fell over when Peleg logged an axe genuinely landing on a cloud's top: it rode along
      fine. The sleep argument misses that the carry write itself wakes the body, so a rider being
      carried can't stay asleep. The constraint workaround was deleted afterwards rather than kept
      as harmless insurance, since a comment explaining a problem that doesn't exist is worse than
      no code at all.
    - The real symptom throughout was an axe frozen against a cloud's *side*, not one resting on
      top - the Stage 3 bug in a form Stage 3's own fix can't detect. Deferred to Stage 8.5 rather
      than fixed here, per Peleg: nothing else in the exercise depends on it, the axe's own 10s
      lifetime clears a stranded one on its own, and the on-top case that actually comes up in play
      already works.
- Stage 5 design decisions, made across the stage's own discussion before anything was touched:
    - The three tiles go in through Tiled's own `Add Tiles` rather than a hand-edited `.tsx` -
      Peleg's call, since item 5 names the tile editor separately from the Level Create script and
      the video is stronger for showing the tool actually being used. The cost is that Tiled picks
      the ids, so they have to be added one at a time in the order bolt, rock, cloud; a multi-select
      would take the file dialog's alphabetical order and hand id 13 to the cloud, which fails
      silently rather than loudly because every existing cloud in the level would simply come back
      as a bolt.
    - `Level01.tmx` is re-synced by regenerating its `<data>` block from `Level01.txt` rather than
      by repainting in Tiled, because the gap turned out to be 41 cells rather than the four the
      plan had recorded before Stages 1, 3 and 4 filled the level with test tiles. The conversion is
      free (a map id and a gid are the same number here), and it leaves Peleg editing the current
      level in Tiled instead of reconstructing it first. He still authors freely on top - the point
      of the regeneration is only to skip the reconstruction.
    - No all-tiles image gets produced at all - Peleg's call, after the step was first scoped as a
      scratch map of the four tiles and then as a one-click export of the level itself. The clause
      asking for it assumes the lesson's packed spritesheet, where that image is the tileset file
      and costs nothing; against a Collection of Images it means manufacturing a PNG nothing reads.
      The tiles were added one PNG at a time because that's the structure this project chose, and
      saying so on camera over a shot of the full tileset panel answers the requirement in the place
      it actually gets graded.
    - Nothing is added to `LevelWindow` to make the tooling half of item 5 look like work. There is
      no code to change, and inventing some would be building for an examiner rather than a need,
      against this project's own rule about not generalizing early. The demo that makes the
      data-driven design visible costs nothing instead: pull a row out of `TilePrefabMap`, press
      Build, and let the existing `Tile ids skipped, nothing mapped to them` warning name the id.
    - `MarioTiles.tsx`'s coin entry pointed at `Tiles/Sprite_Coin 1.png`, a filename that doesn't
      exist in `Tiles01/Tiles/`, so the coin has been a broken tile in Tiled since the tileset was
      built. Found while checking this stage's assumptions rather than reported by anything. Fixed
      first and by hand, because a collection tileset offers no relink for a single tile, deleting
      and re-adding would renumber it and break every coin in the map, and saving a tileset with a
      broken entry in it is a good way to lose the entry.
- Stage 6 design decisions:
    - Nothing was added to `TilePlacerWindow` either. Its dropdown is built from
      `TilePrefabMap.Entries`, so item 6's "including the objects that already existed" was answered
      by the same asset that answered item 5, and the stage was testing rather than building. Two
      items in a row where the deliverable is an explanation is worth knowing before the video is
      scripted: items 5 and 6 are one design decision, and item 7 is where the tooling work is.
    - `Parent` is remembered by name rather than by `GlobalObjectId`. The latter is the general
      answer and survives renames and hierarchy moves; the thing being pointed at here is a root
      object called `World` that the level pipeline, the README and the hierarchy conventions all
      already assume, so the general answer would be guarding a case that can't happen. Serializing
      the name also let the default do something the old code never did, which is find the parent
      with nothing assigned at all.
    - The `SceneObjectMemory` helper exists because both windows needed the same four lines *and*
      the same paragraph of explanation for why a scene reference dies but an asset reference
      doesn't. Duplicating code is cheap at that size; duplicating the explanation is what makes it
      worth a file.
- Stage 7 design decisions, made across the stage's own discussion before any code was written:
    - One `Mode` enum replacing the `placingEnabled` bool, not a second bool beside it. Two bools
      allow "placing off, erasing on", a state with no meaning that something would nonetheless have
      to decide what to do with. `EnemyRangedAttack`'s own private nested `enum Direction` is the
      precedent for the shape.
    - `Mode` stays un-serialized, keeping the reason `placingEnabled` already had: an active mode
      stops the Scene view selecting on click, so it should be off after every restart. The argument
      is stronger for erasing, since reopening Unity one click away from deleting a tile is worse
      than one click away from placing one.
    - Right-click to erase was rejected, though it needs no UI at all. The README already records
      that this window places on left click precisely because right click is the Scene view's own
      camera control, and alt-click is Unity's orbit and already excluded. Shift-click would work,
      but a destructive action on an undiscoverable modifier is worse than a visible mode, and a
      visible mode is what the video can show.
    - No interface and no strategy object for the two modes, which is worth saying out loud because
      the OCP material invites one. The click branch is a two-way `if`, not the growing chain that
      argument is aimed at, and an `IPlacerMode` for two cases in an editor window is the exact
      abstraction-for-a-hypothetical-need this project keeps declining. Items 5 and 6 are where this
      project's OCP answer already lives.
    - The hover preview deliberately doesn't indicate whether the cell under the cursor holds
      anything, though it would be cheap and erasing is destructive. The exercise doesn't ask, and
      the log says what happened immediately afterwards.
- Stage 8 design decisions, made across the stage's own discussion before any code was written:
    - The plan's claim about the probe was checked against the file rather than repeated:
      `PlayerJump.IsGrounded()` really is the `OverlapBoxAll` box, 0.9 of the collider's width by
      `groundProbeDepth` tall, sitting under `bounds.min.y` and filtered by `GetComponent<SC_Floor>`.
      The body moves into the extension unchanged, with `bodyCollider` becoming the `this` parameter
      and `groundProbeDepth` becoming an argument.
    - The counter is "jumps spent since the last landing", not "a ground jump plus a separate
      air-jump allowance". Both shapes give two jumps; only the first survives the reason
      `isJumping` existed. `AddForce` lands on the next `FixedUpdate`, so the probe still finds the
      tile under Mario's feet for a frame or two after take-off - under the split shape a fast
      double-tap re-enters the ground branch and he climbs indefinitely. With one total counter the
      counter is the gate rather than the probe, so a stale grounded reading costs nothing.
    - `inAir` is pulled out as a named local rather than called inline, and carries the comment
      saying it is what makes the second jump conditional. The alternative was an explicit
      `else if (inAir && jumpsUsed < maxJumps)` branch, which reads more literally against the
      requirement's own wording but eats a genuinely fast double-tap during those same stale-probe
      frames. Naming the local buys the same visibility on camera without the input cost.
    - `IsGrounded` stays public despite nothing calling it, as a deliberate exception to this
      project's rule about not exposing what has no second use case. It is the probe itself, and
      the method the exercise actually asks for is its one-line negation; hiding the primitive
      would mean giving it a private name chosen only to keep it out of the way. Said out loud
      here rather than dressed up as the first jump needing it, which it does not - the ground
      jump is the absence of a rejection.
    - Vertical velocity is zeroed before the impulse. `AddForce` in `Impulse` mode adds to whatever
      velocity is already there, and Mario's prefab (mass 1, gravity scale 1.5, linear damping 5)
      falls at a terminal 2.94 units/second against a `jumpSpeed` of 10, so an air jump at the apex
      launches at 10 and one taken mid-fall at about 7 - roughly half the height off the same
      button. One line, applied unconditionally since a grounded Mario's vertical velocity is
      already zero, so the first jump is unaffected and there is no second branch.
    - No belt-and-braces recharge beside the landing event. `SC_Floor` only raises
      `OnFloorCollision` when `IsAboveTile` passes, so a landing that reads as a side touch leaves
      the counter unreset - true of `isJumping` today too, so not a regression. The obvious safety
      net ("if the probe says grounded, zero the counter") reintroduces the take-off staleness
      above and would refund the ground jump mid-ascent.
    - The extension is deliberately not a general Unity helper. It filters on `SC_Floor` rather
      than taking the lesson's `LayerMask`, because that is how this project has always decided
      what terrain is, and moving the filter out to the caller would mean every caller repeating
      it. Stated in the file header so nobody reaches for it in another project.
    - Both new tile types were checked against the design rather than left to the playtest to
      discover: a rock tile disables its collider when it vanishes, so the probe stops finding it
      and Mario gets the ledge rule's one recovery jump for free, which is the best behavior that
      tile could have; a cloud keeps its collider enabled throughout, so riding one reads as
      grounded and landing back on one fires `OnCollisionEnter2D` normally because contact
      genuinely ended.
- Consider emailing the instructor about the editor-tooling differences with an eye to *future*
  hand-ins rather than this one — the seven departures from Lesson 6's `BuildLevel.cs` are all
  deliberate and all defensible, and it's worth knowing in advance whether he'd rather see his own
  shape in Exercise 3.
