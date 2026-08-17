# Conventions

The rules this project follows, in one place so they don't have to be re-typed at the start of
every session. Written for whoever is working on the code, not for a grader: each rule states what
to do, and the reasoning behind it lives in `HW_2_PLAN.md`'s Decisions Log.

The comment rules were agreed in Exercise 1 and revisited in Exercise 2 against how the code had
actually been written since. The logging rules were written down for the first time in Exercise 2,
from the habits 89 existing calls already followed. The naming, hierarchy and code-quality rules
had been carried by hand from session to session until they landed here.

This file travels to the next exercise along with `Assets`.

## Comments

1. Comment why, not what. The exception is a serialized field, which may carry a one-line gloss of
   what it means and what units it is in, since the Inspector shows its name and nothing else.
2. Every file has a header comment: below the `using` block, directly above the type or its
   attributes, `//` and not `///`. It says what the class is for and what it deliberately isn't.
3. A nested type gets its own header only when its existence is non-obvious.
4. Comment a field only when the value or its existence is non-obvious.
5. Comment a branch only when a reader would plausibly delete it.
6. Prefer naming the alternative that was rejected. "X rather than Y, because Z" is the shape most
   of this codebase's comments take, and it is what keeps a comment from restating the code.
7. Four lines maximum, on their own lines above what they describe. No trailing comments.
8. A comment makes one point. If it reaches for "also" or stacks a second unrelated claim, either
   that claim belongs at the line it actually explains or it isn't worth saying. A file header's
   "what it is for and what it deliberately isn't" counts as one point.
9. State what is true now. No changelog voice, nothing about what the code used to do.
10. Never repeat a value the code or the scene already holds. Explain what the number means instead:
    "the cutoff sits at 60 degrees off vertical" rather than the `0.5f` the code already carries.
11. No references to plan stages, lesson numbers, or `HW_2_PLAN.md`. That narrative belongs in the
    plan and in git.
12. A comment naming another class, event or method is a reference. Renaming that thing means
    updating the comment with it.
13. If in doubt, write it so a human reading the file cold understands it. That outranks the rules
    above where they conflict.

## Logging

1. Log at meaningful state transitions. Never per frame.
2. Name the subject first, then say what changed: `Health lost - 2 remaining`, not `Lost health`.
3. `" - "` introduces detail. `": "` introduces a name.
4. No terminal punctuation.
5. A rejected action reads `<action> ignored - <reason>`. Use "ignored" rather than "skipped".
6. Append `gameObject.name` only when several of that thing can exist at once. Mario is one object,
   so naming him repeats what the message already said.
7. Never put the logging class's own name in the message. The Console already shows the class and
   line. Naming a *different* type is fine and often necessary, as in `No HealthView assigned`.
8. One event, one line. The class that owns the decision logs it; the classes it passes through
   stay quiet.
9. When many instances make the same transition on the same frame, log it once for all of them,
   guarded on `Time.frameCount`. `DisappearingFloor` and `MovingFloor` both do this.
10. A missing Inspector reference is a warning, shaped `No X found, <what stops working>`.
11. A line that fires on a timer or on every contact goes to `GameLog.Verbose`, not `Info`. It
    stays available behind one dropdown without filling the Console.
12. Never put a side effect in a log argument. `Info` and `Verbose` are `[Conditional]`, so the
    call and everything inside it disappears from a release build.
13. `Assets/Scripts/Editor/` keeps plain `Debug` and reports results rather than events. Tool
    feedback is not game logging, and it never reaches a build.

## Naming and hierarchy

Sprites live at `Assets/Sprites/Sprite_X.png`, all 48px, with Pixels Per Unit set to the image's
own pixel width.

Prefabs for objects placed in the level take the `Sprite_` prefix. Prefabs for things spawned at
runtime don't.

The legacy `SC_` prefix stays on the scripts that already carry it. New scripts don't use it.

Scripts are grouped by category under `Assets/Scripts/`: `Player/` with `Pickable/`, `PowerUps/`,
`Projectiles/` and `Weapons/` beneath it, plus `Enemy/`, `Tiles/`, `Interfaces/`, `Extensions/` and
`Editor/`. Scene-level managers and GUI scripts sit at the root of `Scripts/`.

In the Hierarchy, `Scripts` holds logic-only manager objects, `World` holds Mario and every tile,
pickup and hazard as flat direct children, and `Canvas` sits at the root as a sibling holding the
`Txt_` GUI objects.

The level is data-driven. `Assets/Levels/Level01.txt` is the source and the scene is output:
`Tools > Level` builds the file into the scene and writes the scene back out, `Tools > Tile Placer`
stamps and erases single tiles, and building deletes and recreates every child of `World`. Saving
the level file deliberately does not save the scene, so a tile can be tried without committing it.

## Code quality

1. One class, one responsibility.
2. Use interfaces the way the course examples do, rather than growing a class an if/else branch at
   a time as new types arrive.
3. Null-check anything obtained from the Inspector, `GetComponent`, `Find`, or parsed from a file.
4. No magic numbers. A named `public` or `[SerializeField]` field when it is tunable, a `const`
   when it describes the code's own shape.
5. No abstraction before a second real use case. Two known future needs that would share no code
   are a naming convention, not a base class.
6. The prefab's value is what runs. A script's own default is not kept in sync with it, since
   retuning a number that has no effect is noise in a diff.
