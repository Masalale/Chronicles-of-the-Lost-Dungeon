# Chronicles of the Lost Dungeon: The Flame Keepers

A top-down 2D dungeon game in Unity: as the last Flame Keeper, fight through five dark Silos and bring back the shards of their lost light.

## Features

- **Five Silos**: beat every guardian to unseal the chest, take its shard, and the next Silo unlocks.
- **Your torch is your health**: every hit or wrong answer shrinks the light around you and dims the whole Silo, and what's left carries into the next one.
- **Two fire spells**: a fireball that flies at the nearest enemy, and another that fires in every direction.
- **Three kinds of enemies**: Chasers find their way around walls to you, Shooters fire bolts, and Guards follow you slowly with a shield.
- **Riddle Keeper**: answer a true/false question pulled from the internet through an API to open the exit. Get it right on the first try and you win back a torch; a wrong answer costs one torch, and after that a right answer only opens the exit.
- **Saved progress**: cleared Silos and volume settings stay between plays, and the victory screen ranks your Silos by torch left.

## Quick Start

1. Install [Git LFS](https://git-lfs.com) (the art, sounds and font are stored with it), then clone the repository:
   ```bash
   git lfs install
   git clone https://github.com/Masalale/Chronicles-of-the-Lost-Dungeon.git
   ```
2. Open the folder in Unity 6.
3. Open `Assets/Scenes/MainMenu.unity`, press **Play**, and start a new game. Move with `WASD` or the arrow keys, cast with `Q` and `E`, interact with `F` for chest and the Riddle Keeper, and pause with `Esc`. A gamepad works too, and phones get an on-screen stick and buttons.

## Structure

- `Assets/Scripts/Core/GameManager.cs` : starting Silos, pausing, winning and losing
- `Assets/Scripts/Core/SaveManager.cs`, `SaveData.cs` : saves progress to a JSON file
- `Assets/Scripts/Core/RoomLoader.cs` : loads each Silo's room and music when it starts
- `Assets/Scripts/Core/AudioManager.cs` : music, sound effects and volume
- `Assets/Scripts/Player/` : movement, spells, torch light and pressing F on things
- `Assets/Scripts/Combat/` : health, the fireball, the Flame Nova and reused projectiles
- `Assets/Scripts/Enemies/` : how enemies wake up, plus the Chaser, Guard and Shooter
- `Assets/Scripts/Algorithms/GridPathfinder.cs` : shortest way around the walls
- `Assets/Scripts/World/RoomController.cs`, `Chest.cs`, `Shard.cs`, `ExitDoor.cs` : what happens inside a Silo
- `Assets/Scripts/World/RiddleKeeper.cs`, `TriviaGate.cs` : the riddle and the online questions
- `Assets/Scripts/UI/` : main menu, HUD, pause, settings and end screens
- `Assets/Prefabs/Rooms/` : the five Silo rooms
- `Assets/Tests/` : edit mode tests
- `Assets/Scenes/MainMenu.unity`, `Game.unity` : the menu and the dungeon

## Architecture

- `GameManager`, `SaveManager` and `AudioManager` are created once in the menu and survive scene changes, so every scene reads the same game state, save and volume.
- Each Silo is one room prefab. Its `RoomController` counts the enemies, unseals the chest, wakes the Riddle Keeper and dims the light as the torch burns down.
- Systems talk through C# events instead of calling each other: enemies announce when they die, `Health` announces every change, and the HUD, audio and save listen. Adding a new listener doesn't touch the sender.
- Abilities share one interface (`IAbility`) and enemy types share another (`IEnemyBehaviour`), so a new spell or enemy is a new component, not a change to `PlayerCombat` or `Enemy`.
- An enemy is always in one state (idle, active or dead), and the state decides what it does each frame.

## Algorithms

- **Nearest enemy** (`NearestFinder.cs`, O(n))
  - Purpose: picks the enemy the fireball flies at.
  - Approach: one pass over every living enemy, keeping the one with the smallest squared distance.
  - Why: there are only a handful of enemies per silo, so checking each one once is fast, simple and always exact.
- **Breadth-first search** (`GridPathfinder.cs`, O(cells))
  - Purpose: finds an enemy's next step around the walls to the player.
  - Approach: the room becomes a grid of cells with the walls marked blocked. The search spreads out from the enemy one ring of cells at a time using a queue, remembers where each cell was reached from, and walks back from the player to find the first step. Enemies only use it when a wall blocks the straight line.
  - Why: every step costs the same on a tile grid, so the first path it finds is already the shortest. It is simpler than A* and the rooms are small.
- **Insertion sort** (`InsertionSorter.cs`, O(n²))
  - Purpose: ranks the five Silos on the victory screen, brightest torch first.
  - Approach: takes each Silo result in turn and slides it left past every result with less torch left.
  - Why: with only five items it is as fast as anything else, it is short enough to read at a glance, and it keeps ties in Silo order.

## Content Management

- **Approach**: Unity Addressables (`com.unity.addressables`), with local content only.
- **Assets managed**: the five Silo rooms (`Rooms` group: `Room1` to `Room5`, each with its enemies, chest and Riddle Keeper, plus a bolt pool in Silos with Shooters) and the two dungeon music tracks (`Music` group). Each Silo's `LevelData` asset points to its room and music with an `AssetReference`, so adding or swapping a Silo means editing that asset, not the code.
- **Loading**: when a Silo starts, `RoomLoader.cs` calls `InstantiateAsync` for the room and `LoadAssetAsync` for the music. A loading panel covers the screen until the room arrives.
- **Failures**: if the room can't load, the loading panel is replaced by an error message with a button back to the menu, so the game never hangs.
- **Releasing**: when the Silo ends (moving on, retrying after a game over, or going back to the menu), `RoomLoader` calls `ReleaseInstance` on the room and `Release` on the music, so only the current Silo is in memory.
- **Build and deploy**:
  1. Open **File > Build Profiles**, pick **Windows**, **Android** or **Web**, and press **Switch Platform**.
  2. Press **Build**. **Build Addressables on Player Build** is on, so the `Rooms` and `Music` content is built for that platform and packed inside the player. Nothing is hosted separately.

## Credits

- Sprites and tiles: [DungeonTileset II](https://0x72.itch.io/dungeontileset-ii) by 0x72 (CC0)
- Icons: [Noto Emoji](https://github.com/googlefonts/noto-emoji) by Google (Apache 2.0)
- Glow sprite: [Particle Pack](https://kenney.nl/assets/particle-pack) by Kenney (CC0)
- Sound effects: [RPG Audio](https://kenney.nl/assets/rpg-audio), [Interface Sounds](https://kenney.nl/assets/interface-sounds), [Impact Sounds](https://kenney.nl/assets/impact-sounds) and [Music Jingles](https://kenney.nl/assets/music-jingles) by Kenney (CC0)
- Menu music: [Calm and Simple Title Music](https://opengameart.org/content/calm-and-simple-title-music) by Kosmo The Cat (CC0)
- Dungeon music: [Dark Cavern Ambient](https://opengameart.org/content/dark-cavern-ambient) by Paul Wortmann and [dungeon-ambience](https://opengameart.org/content/dungeon-ambience) by yd (CC0)
- Font: [Thaleah Fat](https://tinyworlds.itch.io/free-pixel-font-thaleah) by Rick Hoppmann (CC BY 4.0)
- Trivia questions: [Open Trivia Database](https://opentdb.com) (CC BY-SA 4.0)
