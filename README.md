# testpro

A top-down bullet hell shooter inspired by *Enter the Gungeon*. Built in Godot 4.6 with C#.

> **Status: Work in Progress.** Core systems are in place but the game is unfinished — expect missing content, placeholder art, broken edges, and rapidly changing code.

## Gameplay

Pixel-art bullet hell where you move room-to-room, blast enemies with a rotating arsenal, and dodge through incoming fire. The current loop covers:

- Player movement, aiming, and a dodge/warp dash with afterimage ghosts
- A weapon wheel for swapping between guns mid-fight
- Multiple guns (Pistol, Rifle, Shotgun, Nailgun, Zooka) with leveled stats
- Enemy AI driven by a state machine (idle / follow / surround) with their own guns
- A spawner-based room/round system on a tile-based map
- XP, hurtboxes, music handler, scene transitions, speedlines, and screen effects

## Controls

| Action      | Input              |
| ----------- | ------------------ |
| Move        | `WASD`             |
| Aim / Shoot | Mouse / Left click |
| Dodge       | Right click        |
| Interact    | `E`                |
| Weapon wheel| `Tab`              |
| (extra)     | `Space`            |

## Tech

- **Engine:** Godot 4.6 (Forward+ renderer)
- **Language:** C# (.NET)
- **Resolution:** 480×270 internal viewport, integer-scaled to 1440×810
- **Pixel-snap rendering** with nearest-neighbor filtering

## Project layout

```
Assets/        Art, audio, fonts, icons
Components/    Reusable scene/script components
Game/
  Entities/    Player, enemies, state machine, shared components
  Guns/        Player + enemy guns, bullets, damage effects
  GUI/         HUD, weapon wheel, transitions, mouse, speedlines
  Main/        Game entry, audio, music, map handler, enemy spawner
  Map/         Tilemap, rooms, spawners, walker generators
addons/        Third-party editor plugins (color-palette)
```

## Running

1. Install [Godot 4.6 .NET](https://godotengine.org/download) and the .NET 8 SDK.
2. Clone this repo and open the folder in Godot.
3. Let the editor build the C# solution (`testpro.sln`), then press **Play**.

## Known limitations

- Map/room transitions are functional but rough
- Lots of placeholder art and tuning numbers
- Content (guns, enemies, rooms) is sparse — this is an engine/prototype, not a finished game
