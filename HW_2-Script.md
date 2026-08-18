# HW-2 Video Script — `2026-HW_2-Mario`

Recording script for the Exercise 2 submission video. 16:55, deliberately longer than HW1's 13:50 —
no exercise text or course material sets a limit.

## How to read this

- Lines in `>` blocks are said out loud. Everything else is a stage direction and is never spoken.
- One sentence per line inside a `>` block. Each line is one breath.
- `### **SHOW**` is a major beat — a new file, a new Play session. Plain `**SHOW**` is a small
  move inside that beat: scroll, flicker to a method, press a key.
- Each numbered requirement is called out in Hebrew (`דרישה 4:`) so it's unmistakable which item
  is being shown, then explained in English.
- Every part is its own take, edited together afterwards. No take depends on the one before it,
  though the level itself changes in Part 4 — see the recording-order note below.
- Items are covered 1, 2, 3, 4, 8, 5, 6, 7, 9 — grouped by whether they need Play mode or the
  Editor, not by number. Every one is announced, so nothing is ambiguous.

### About the timings

Every part is written to a word count, not to a feeling. HW1's script recorded 1722 spoken words in
13:50 — roughly 160 words a minute in the code parts and 90 in the Play parts, where most of the time
goes on walking and waiting. This one is 1888 words at the same pace, plus about a minute in Part 4
where the talking stops and the clicking happens.

Three parts run well under their word rate on purpose. Part 2B is four hazard hits and a respawn
each time, Part 3B is three axe throws and a walk across the level, and Part 6 is a full run with one
line of narration over it — all three are paced by playing, not by talking.

| Part | Content                              | Words | Time |
| ---- | ------------------------------------ | ----- | ---- |
| 1    | Intro and level tour, no Play        | 119   | 0:50 |
| 2A   | Code: items 1 and 2 (P1, P2)         | 307   | 1:55 |
| 2B   | Play: items 1 and 2                  | 117   | 1:55 |
| 3A   | Code: items 3, 4 and 8 (P1, P2, P3)  | 449   | 2:50 |
| 3B   | Play: items 3, 4 and 8, plus the axe | 196   | 2:20 |
| 4    | Items 5, 6 and 7 (P1 to P7)          | 584   | 4:45 |
| 5    | The unrequired work                  | 98    | 0:40 |
| 6    | The full playthrough                 | 16    | 1:30 |
| 7    | Sign-off                             | 2     | 0:10 |

**16:55.** There is no external limit — neither exercise text nor any course material sets one, and
HW1's 15:00 was a target Peleg set for himself. This runs longer on purpose: nine items need code
shown and explained for each, and three of them are editor tooling, which is all talk and no
gameplay.

If a rehearsal comes in long, these are the cheapest cuts in order, each with what it costs:

1. **Part 3B's two cloud-axe throws** (about 0:35 of playing, 60 words). The rock throw alone already
   makes the point about a landed axe being held up by whatever it froze against.
1. **Part 3A's probe explanation** (−45 words, about 0:17). The corner-perch case and the
   collider-versus-Transform line. The extension still gets explained without them.
1. **Part 4's off-grid save warning** (−45 words plus its dragging, about 0:50). A tool demo rather
   than an item, so nothing graded goes with it — but it's the best evidence in the video that the
   tools report what they did rather than silently fixing it.

What not to cut: Part 3A's ledge paragraph, Part 2A's line about reusing the strikes system, Part 4's
all-tiles-image reason, Part 4's Tiled-isn't-a-live-source line, and the crate. The first four have
to be said out loud or the work reads as something it isn't, and the crate is what turns "the tool
needed no change" from a claim into a demonstration.

## Before you record

- **Record in part order: 1, 2A, 2B, 3A, 3B, 4, 5, 6, 7.** Part 4 edits the level on camera, so the
  level in Part 6 is not the level in Part 1. That's the point rather than a continuity problem: the
  video plays the level, edits it in front of you, and plays the changed one.
- Only three of those orderings are forced. **1, 2B and 3B before 4**, because they show the pre-edit
  level; **6 after 4**, because it shows the edited one; and **5 after any Play session**, because it
  shows `GameLog.txt` and `LogFileWriter` truncates that file at the start of every session. 2A, 3A
  and 7 have no constraint at all.
- The takes stay independent as performances — each has its own opening line and nothing to carry
  over. What isn't independent is the project state, which only moves one way: don't reshoot Part 1,
  2B or 3B after Part 4 without rebuilding the pre-edit level first.
- **Part 4's crate needs prep.** `Crate.png` at 48px in both `Tiles01/Tiles/` and `Assets/Sprites/`,
  and a `Sprite_Crate.prefab` beside the other tile prefabs. The `TilePrefabMap` row is the only part
  added on camera — everything else has to exist before you press record.
- Three windows: Unity, VS Code, and Tiled for Part 4 only.
- Unity Console: **Clear on Play** on, **Collapse** off, **Error Pause** off.
- The Console is only on screen in Part 4, where the editor tools report what they did and one beat
  depends on reading a warning. The Play parts never point at it, so every claim there has to be
  visible in the game or on the GUI labels.
- `Tools → Logs`: leave every category at **Info**. Part 4's tool lines are plain `Debug` and aren't
  filtered by it at all, so nothing there can be hidden by a wrong setting.
- Nothing has to be toggled off and back on mid-video. No component gets disabled anywhere.
- Open every file a part needs as VS Code tabs beforehand, in order:
    - **2A** — `LightningBoltController.cs`, `LightningBoltPowerUp.cs`, `PlayerSpeedBoost.cs`,
      `IHealthModel.cs`, `HealthModel.cs`, `HealthView.cs`, `HealthController.cs`
    - **3A** — `DisappearingFloor.cs`, `MovingFloor.cs`, `SC_Floor.cs`, `GroundedExtension.cs`,
      `PlayerJump.cs`
    - **4** — `LevelWindow.cs`, `TilePlacerWindow.cs`, `Level01.txt`
    - **5** — `README.md`
- Part 4 also needs Tiled open on `Level01.tmx`, with the `MarioTiles` tileset panel visible.
  `Level01.tmx`, `Level01.txt` and the saved scene all hold the same 259 objects as things stand, so
  the on-camera edit is a small addition on top of the real level.
- After recording, `Level01.tmx` will be behind by whatever the Tile Placer did in Part 4, because
  those edits happen in Unity and never travel back. It costs nothing now — `Tiles01/` is in no git
  repo, so the `.tmx` is neither versioned nor submitted, and `Level01.txt` is the graded file and
  holds everything. It only matters the next time you export from Tiled, which would overwrite the
  placer's changes. Re-sync then, not before.
- Line numbers in the SHOW lines are current as of writing. Re-check them if you edit any script.
- The level is 30x20, so a frame holding all of it makes the sprites small. Frame the whole level
  only for Part 1's tour and zoom in for everything else.
- Do a 20-second test recording and check the Console text is legible at your export resolution.

## The level

```text
    012345678901234567890123456789
 19 ==============================
 18 =............................=
 17 =.......c.c......c...c...c...=
 16 =..c....c.c..*..c.c.c.c.c.cG.=
 15 =.c.c.~...~.....===.......====
 14 =......c...c.........o...=====
 13 =..o..^^ccc^^...........======
 12 =....=========...............=
 11 ==................ccc........=
 10 ===....c...........H..L......=
  9 ====.oc.c.V.c...cooooo......c=
  8 =....c...~=..c.c..^^^.~....cc=
  7 =.K.c.........a...===.......c=
  6 =.~~~.........=...........=..=
  5 =....c...............VH......=
  4 =...ccc...M.ccc.*...===....===
  3 =a.ccccc.L......o.........====
  2 ==......=====F.....ccc..R=====
  1 ===^^^^^======.........=======
  0 ==============================
```

`=` earth, `o` rock (disappearing), `~` cloud (moving), `L` lightning bolt, `H` heart,
`M` Mario's start, `c` coin, `a` axe pickup, `F` fire flower, `*` star, `^` spikes, `K` key,
`G` gateway with the portal inside it, `R` grave and spawner, `V` vampire. The header row is each
column's tens digit dropped; every object sits at its own cell's coordinates.

Landmarks used below:

- **Start block** — the earth at x 8 to 13, y 2. Mario spawns at (10, 4) and lands on top of it.
- **First bolt** — (9, 3), a cell down-left of where Mario lands. The second is at (22, 10).
- **Spike pit** — the spikes at (3, 1) to (7, 1), immediately left of and below the start block.
- **Overhead vampire** — (10, 9), directly above the start block. The second is at (21, 5).
- **Rock bridge** — the five rock tiles at (17, 9) to (21, 9), with a heart at (19, 10) above it.
  The other heart is at (22, 5), beside the second vampire.
- **Cloud trio** — (2, 6), (3, 6), (4, 6). Three moving tiles side by side.
- **Pass-through cloud** — (9, 8), which patrols straight through the earth tile at (10, 8).
- **Grave end** — the grave at (24, 2), bottom right. Ghosts spawn and patrol from there.
- **Key ledge** — the key at (2, 7). **Gateway** — (27, 16), top right.
- **Stars** — (16, 4) and (13, 16). **Fire flower** — (13, 2), the right edge of the start block.

---

## Part 1 — Intro and level tour `0:50`

**SHOW** — Unity, Scene view, **Play not pressed**. Frame the whole level, then pan slowly as you
name each area.

**SAY:**

> שלום, זה פלג בן דור, זאת ההגשה שלי למטלת שיעורי בית 2
>
> This is Exercise 2, built on top of my Exercise 1 project.
> I'll take the nine requirements in turn, showing the code for each and then showing it run.
> There's a full playthrough at the end.
>
> This is the level.
> It's closed with earth tiles on all four sides, so there's no falling out of it.
> It isn't stored in the scene — it's a text file, and an editor tool builds it.
> That's requirements five, six and nine.
>
> Different from HW 1: the lightning bolt, the heart (this time with MVC and refactoring), the rock tile that appears and disappears, and the
> cloud tile that moves.

---

## Part 2A — Code: items 1 and 2 `1:55`

## Part 2A P1 — Code: item 1

### **SHOW** — `LightningBoltController.cs`.

**SAY:**

> דרישה 1:
>
> The lightning bolt, giving temporary speed boost.

> The bolt is like every other pickup in this project: detect Mario, hand over a power-up, switch itself off.

**SHOW** — `LightningBoltPowerUp.cs`.

> The power-up finds the component on Mario that runs the effect.

### **SHOW** — `PlayerSpeedBoost.cs`.

> There's no maximum-speed field anywhere in this project.
> `PlayerMovement` writes the velocity straight from `speed`, so Mario is either stopped or going
> exactly that fast — which makes `speed` the maximum the requirement is talking about.
> The boost adds half of it for five seconds.

**SHOW** — Flicker to `ActivateBoost` (line 38).

> A second bolt restarts the timer rather than leaving two of them running.

**SHOW** — Flicker to `RestoreSpeed` (line 83).

> And it always restores to the speed captured once in `Awake`, rather than dividing the boost back
> out, so nothing here can drift across a restart.

**SHOW** — Flicker to `OnHazardCollision` (line 66).

> And a hazard hit cancels the boost, so one never survives a respawn.

## Part 2A P2 — Code: item 2

### **SHOW** — `IHealthModel.cs`.

> דרישה 2:
>
> Health points, built with MVC.

> Exercise 1 already had a strikes system that did all of this.
> This is that system rebuilt as a model, a view and a controller.
>
> `Gain` and `Lose` return whether anything actually changed, so the controller can tell "already at
> three" from a real gain.

**SHOW** — `HealthModel.cs`.

> Plain C#, not a MonoBehaviour.
> Nothing Unity-specific reaches into it, so it can be built without a scene at all.

**SHOW** — `HealthView.cs`.

> The view draws a number onto a label and knows nothing else.

### **SHOW** — `HealthController.cs`.

> The controller is the only one of the three that touches Unity's events.
> It subscribes to hazard hits and to the heart, updates the model, and pushes the result to the
> view.
> The maximum sits here rather than on the model, because the model has no Inspector. Three, and
> health starts full.

**SHOW** — Flicker to `OnHazardCollision` (line 50).

> Any hazard costs health, not only spikes.
> At zero it says the game is over and stops there — it doesn't restart anything itself.

**SHOW** — Flicker to `OnHealthGained` (line 69).

> And a heart collected at three is refused. That's the cap.

---

## Part 2B — Play: items 1 and 2 `1:55`

One take. Health has to reach zero, so the arithmetic matters: **3, spikes → 2, ghost → 1, heart →
2, garlic → 1, ghost → 0.** Four hazard hits and one pickup. Every hit puts Mario back on the start
block, so each leg begins from spawn.

The star is collected on the way. **It suppresses health loss for five seconds and tints Mario
gold** — let the tint fade before taking the garlic hit, or the hit costs nothing and the count
never reaches zero.

### **SHOW** — Press Play. `Txt_Health` reads `Health: 3`. Collect the **first bolt** at (9, 3).

> דרישה 1:
>
> Three health to start.

**SHOW** — Run a stretch of the start block boosted, and let the boost expire while you're still moving.

> Picking up the bolt makes Mario faster for five seconds.

### **SHOW** — Walk left off the start block into the **spike pit**. Health drops to 2, Mario is back at the start.

> דרישה 2:
>
> Spikes, down to two health.

**SHOW** — Cross to the **grave end** and let a ghost touch Mario. Health drops to 1.

> A ghost, down to one.

**SHOW** — Collect a **star**, then reach a **heart**.

> A star on the way, so nothing can hurt him for a few seconds.
>
> And a heart. So now we're back up to two.

**SHOW** — Wait for the gold tint to fade, then take a **garlic** hit. Health drops to 1.

> Garlic, and we're down to our last health.

**SHOW** — Take one more hit. `GAME OVER` in red, then the scene reloads.

> And at zero the level restarts, with every object in it rebuilt from the level file.

### **SHOW** — After the reload, `Health: 3`. Collect the **first bolt** at (9, 3) — it's back.

> Back to three health, and the bolt I already took is back too.
> The level was reinitialized rather than resumed, part of the requirements.

**SHOW** — Reach a **heart** at full health. `Txt_Health` stays at 3 — that label is the evidence, since the Console isn't on screen.

> A heart at three is refused rather than taking him to four, since our cap is 3.

---

## Part 3A — Code: items 3, 4 and 8 `2:50`

## Part 3A P1 — Code: item 3

### **SHOW** — `DisappearingFloor.cs`.

**SAY:**

> דרישה 3:
>
> A floor tile that appears for two seconds and disappears for two seconds.

> The cycle runs from level load rather than from Mario touching the tile — the requirement
> describes a cycle with no trigger in it.

**SHOW** — Flicker to `CycleVisibility` (line 38).

> Every tile runs its own copy of this loop, and the builder places them all before Play starts — so
> they begin on the same frame and stay in step with no coordinator.

**SHOW** — Flicker to `Hide` (line 65).

> It switches off the sprite renderer and the collider, not the GameObject.
> A disabled GameObject stops running its own coroutine, so the tile would vanish and never come
> back.

## Part 3A P2 — Code: item 4

### **SHOW** — `MovingFloor.cs`.

> דרישה 4:
>
> A floor tile that moves two tiles right and two back.

> A patrol out two cells and home again, so the home cell stays the one it returns to — which the
> level file needs, since it stores one position per object.

**SHOW** — Flicker to `FixedUpdate` (line 37).

> It moves through a Kinematic `Rigidbody2D` rather than by writing the Transform.
> It's the only collider in the project that moves, and driving one from a kinematic body is Unity's
> own guidance.

**SHOW** — Flicker to `OnCollisionStay2D` (line 76).

> Mario is not carried for free.
> `PlayerMovement` brakes his velocity toward zero every frame no key is held, which cancels the
> friction that would otherwise drag him along.
> So the tile adds its own movement straight to the position of whatever rests on it — position
> rather than velocity, so the braking has nothing to fight.

**SHOW** — `SC_Floor.cs`, scrolled to `IsAboveTile` (line 32).

> It finds its riders with the same check the landing event uses, so there's just one copy of that
> geometry.
> And it never learns what it's carrying. Mario, a patrolling enemy and a thrown axe all go through
> that one line, with no special case for any of them.

## Part 3A P3 — Code: item 8

### **SHOW** — `GroundedExtension.cs`.

> דרישה 8:
>
> The double jump, and the extension method behind it.

> `IsGrounded` is an extension on `Collider2D`, and `IsInAir` is its one-line negation — the method
> the requirement asks for.
> It's on the collider rather than the Transform because the probe reads collider bounds, not just a
> position.
>
> And it's a box across Mario's whole footprint rather than a point under his centre, because on a
> platform's last tile his centre hangs past the edge while he's still perched on the corner.
>
> It also takes no `LayerMask`: ground here means anything carrying the floor component.

### **SHOW** — `PlayerJump.cs`, scrolled to `Jump` (line 61).

> Jumps spent since the last landing, against a serialized maximum of two.
>
> `inAir` is the extension being called, and this branch is the one to read carefully.
> Being in the air with nothing spent means Mario walked off a ledge rather than jumped, so the
> ground jump is charged here instead of given away.
>
> Which leaves him exactly one jump off a ledge.

**SHOW** — Flicker to `OnFloorCollision` (line 50).

> And landing resets the counter. That's the double-jump recharge.

---

## Part 3B — Play: items 3, 4 and 8 `2:20`

### **SHOW** — Press Play. Throw axe at the **near rock** at y 3, x 16.

> דרישה 3:
>
> The rocks, disappearing tiles. Two seconds solid, two seconds gone.

> A thrown axe freezes solid where it lands, so this one is being held up by the rock and nothing
> else.
> When the rock goes, the axe has nothing under it and drops.

**SHOW** — The axe is stuck while the rock's there, then falls down when the rock disappears.

**SHOW** — Jump on that rock. Let them vanish under Mario. He falls.

> They're real floor while they're there.

**SHOW** — Meanwhile, head towards the clouds (going through the **rock bridge** at y 9, x 17 to 21, showing more of the mechanic in play)

### **SHOW** — Get to the **cloud trio** at y 6, x 2 to 4.

> דרישה 4:
>
> The clouds, moving tiles. Two cells right, then back.

**SHOW** — Throw an axe to land on top of them.

> An axe landing on a cloud rides it, through the same line that carries Mario.
> The tile never checks what it's moving.

**SHOW** — Step on and stand completely still.

> Standing completely still is the hard case, and it's what the carrying code was written for.
> Nothing parents Mario to the tile, and nothing relies on friction.
> Three in a row behave as one platform.

**SHOW** — Walk a bit while on them, get off.

**SHOW** — Then head over to the cloud at y 15, x 6, and throw the axe so it hits the tile's side. It freezes in place, and drops once the cloud has moved out from beside it.

> Against the side it's different, because the tile only carries what's resting on top of it.
> So the cloud slides away and the axe stops touching what it froze against, which is the same check
> the rock case just used.

**SHOW** — Fall to spikes, so we're back at the starting point.

### **SHOW** — Press space twice, then a third time.

> דרישה 8:
>
> Two jumps, and a third does nothing until Mario lands. Landing recharges it.

**SHOW** — Go over to the ledge at (20, 4), walk off its left edge without jumping, then press space once and again.

> And walking off a ledge leaves him one jump rather than two.

**SHOW** — Stand on a rock tile, let it vanish, and jump once on the way down.

> Same when a disappearing tile goes out from under him.

---

## Part 4 — Items 5, 6 and 7 `4:45`

Edit mode throughout, and the only part where the Console is on screen.

## Part 4 P1: Tiled

### **SHOW** — Tiled, `Level01.tmx` open, `MarioTiles` tileset panel showing all 16 tiles.

**SAY:**

> דרישה 5:
>
> Adding the new tiles to the tile editor, and supporting them in the level-creating script.

> The tileset, with all sixteen tiles in it.
> The three new ones are the bolt, the rock and the cloud. The heart was already a tile, renamed from strike.
>
> The requirement also asks for one image with all the tiles inside it.
> I kept them as separate PNGs instead, one file per tile, because that's how every other sprite in
> this project is already stored and it's what I'd rather work with.

**SHOW** — Make a few changes to the map:

1. add a speed boost close by to another speed boost
2. another rock
3. Replace a rock that's somewhere else with 2 clouds
4. And also add another heart

> A few changes first, with tiles that are already in the set.

**SHOW** — `Tileset → Add Tiles`, pick `Crate.png`. It lands as Tiled id 16. Then place one in the level.

> And now a tile type that didn't exist until just now — a crate.
> It does nothing in the game and it comes back out again before the end. It's here to show what
> adding a tile type actually costs.

**SHOW** — Export, then Ctrl+S to save the map itself.

> Export writes the level straight into the Unity project, and I save the map too so the two don't
> drift apart.
>
> Tiled isn't a live source, though. It authored this level, but the text file is what the game
> reads.

## Part 4 P2: Unity

### **SHOW** — Unity, `Tools → Level`. Press **Build Level**.

> Build reads that file and rebuilds the level from it.

**SHOW** — Console: `Tile ids skipped, nothing mapped to them: 17`, then
`Level built from Level01.txt - <n> objects under World`. The warning prints first. The crate is
missing from the Scene view.

> Everything I moved in Tiled is here. The crate isn't, and the tool says why — it doesn't know what
> id seventeen is supposed to be.

## Part 4 P3: VS Code

### **SHOW** — `LevelWindow.cs`, scrolled to `PlaceLayer` (line 109).

> The requirement says to develop the level script so it supports the new tiles.
> This file has no tile ids in it at all, so there was nothing in it to develop.

**SHOW** — Flicker to line 121, `tilePrefabMap.GetPrefab(tileId)`.

> It asks an asset which prefab an id means, and places whatever comes back.

## Part 4 P4: Unity

**SHOW** — `TilePrefabMap.asset` selected, all 16 rows in the Inspector.

> That asset is the mapping, and it's the only place an id becomes an object.
> The lesson's version of this tool has a switch statement over tile ids, so a new tile there means
> editing the tool. Here it's a row — closed to modification, open to extension.
>
> Which is exactly what the crate needs.

**SHOW** — Add row 17 pointing at `Sprite_Crate.prefab`, then press **Build Level** again.

> Seventeen rows, no code touched, and there it is.

### **SHOW** — `Tools → Tile Placer`. Open the **Tile** dropdown so the whole list is visible.

> דרישה 6:
>
> Every object available in the second editor tool.

> Same asset as the builder, so the two lists can't disagree.
> All seventeen — the crate plus the sixteen that were already there.

**SHOW** — Pick the coin, set **Mode** to **Place**, click two empty cells. Then switch to the crate and place three of those.

> Place mode. Click a cell, and that's a tile in the level.

### **SHOW** — Set **Mode** to **Erase**. The Scene view preview turns red and the Tile dropdown greys out.

> דרישה 7:
>
> Erase mode, we can erase tiles.

**SHOW** — Click two coins away. Console: `Erased 1 object(s) at (...)`. Then Ctrl+Z once.

> It says how many it removed, and one undo brings back a whole cell.

**SHOW** — Erase all four crates, then remove row 17 from `TilePrefabMap`.

> And the crates come back out, since they were only ever an example.

**Erase the crates before you touch `TilePrefabMap`.** Removing the row while crates are still in the
scene makes the next Save Level warn that it's leaving them out of the file, which is correct
behaviour but not what this beat is about.

## Part 4 P5: VS Code

### **SHOW** — `TilePlacerWindow.cs`, scrolled to the `Mode` enum (line 11), then `ClearCell` (line 200).

> The on-off boolean became a three-state mode rather than a second boolean beside it. Two booleans
> would allow "placing off, erasing on", which means nothing.
>
> And erasing reuses the method placing already called to clear a cell.

## Part 4 P6: Unity

### **SHOW** — Drag one cloud tile off the grid, to about x 10, y 14 and a half. Press **Save Level**.

> A level file stores one tile per cell, so nothing can sit between two — and if something does,
> saving gives off a warning.

**SHOW** — Console: `Moved to the nearest cell: Sprite_Cloud (10.0, 14.5, 0.0)`.

> There it is, by name and by position, and it went into the file at the nearest cell.

**SHOW** — Drag the cloud back onto its own cell and press **Save Level** again.

> I'll put it back and save again.

Don't reach for Build to undo it — Build reads the file, and the file already holds the rounded
cell, so it would put the cloud right back there.

## Part 4 P7: VS Code

### **SHOW** — Open `Level01.txt` in VS Code and scroll through it once.

> דרישה 9:
>
> The level file, saved from inside Unity, with the whole level in it.

> Thirty by twenty, one integer per cell, zero meaning nothing there.
> This is what Tiled exported, plus everything I just did with the two tools — Save Level reads the
> scene and writes all of it back here.
>
> One thing worth being clear about: Save Level writes this file, not the Unity scene.
> Those are two separate saves on purpose, so a tile can be dropped in and looked at without
> committing it to the level.

---

## Part 5 — Things the exercise didn't ask for `0:40`

Keep this moving. None of it is a graded item; it's on screen throughout the video anyway.

### **SHOW** — `README.md`, scrolled to the Editor tooling section.

**SAY:**

> Two things that aren't in the requirements.
>
> Both editor tools depart from the lesson's versions in seven places — the mapping asset instead of
> a switch, writing the scene back out to the file, and five more.
> They're all in the README with the reason for each. They were choices, not corrections.

### **SHOW** — `Tools → Logs`, then `GameLog.txt` in VS Code.

> And the logging goes through one static class with a category per line, rather than `Debug.Log`
> scattered around.
> `Debug.Log` isn't stripped from a built game, so the informational levels here are compiled out of
> a release build entirely.
> This window sets a level per category, live.

---

## Part 6 — The full playthrough `1:30`

One complete run to the portal, narrated live. Everything has been explained by now, so this is
naming what's on screen rather than re-explaining it. Adapt to what actually happens.

This is the level **after** Part 4's edits, which is the point of running it last — the two bolts are
side by side now, and there's an extra rock, two extra clouds and a third heart.

Budget note: 1:30 is your own measured time for a full run collecting everything and killing both
vampires. The 0:35 dash to the key and gateway is the fallback if a take keeps going wrong.

### **SHOW** — Press Play. A full run: both bolts back to back, coins, both new tile types, the double jump, a heart, both weapons, both enemies, a ghost from the grave, the star, the key, the gateway.

**SAY:**

> That's all nine requirements. Here's a full run, on the level as I just edited it.

---

## Part 7 — Sign-off `0:10`

**SHOW** — Anything. The Scene view is fine.

> תודה רבה!