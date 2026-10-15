# The Ledger of Ash

*A page torn from the Daedric Ledger. Three debts sealed in soul-glass. Three thralls who would rather keep them.*

A relic hunt for the Daedric Ledger theme of the series. **Aulus Ledger-Keeper** waits by the **Riften Stables** with a burnt copy of the Ledger: a page was torn out and fed to the fire, and the three debts it held are sealed in shards of soul-glass, carried off by thralls.

**Status: v0.1.2, early beta.** It passes every automated check in the build (Papyrus compile, plugin and dialogue validation, byte-exact round trip, voice file and SEQ checks, FOMOD schema, the series compatibility check) but has not been playtested in-game yet. Expect rough edges and please report what you find. See [docs/TESTING.md](docs/TESTING.md).

- Game: Skyrim Special Edition / Anniversary Edition 1.6.x (also fine on 1.5.97). Works on Windows and on Linux through Proton.
- Requirements: none beyond the base game. No SKSE, no SkyUI, no DLC, no Creation Club content required.
- Plugin: `LedgerOfAsh.esp`, ESL-flagged, Skyrim.esm is the only master. **No vanilla records edited** (zero overrides).
- Part of the synthalorian series and compatible with all of it: [Blackshield Company](https://github.com/synthalorian/blackshield-company), [Bounties of Oblivion](https://github.com/synthalorian/bounties-of-oblivion), [Contract Hunter](https://github.com/synthalorian/contract-hunter), [Steel Oath](https://github.com/synthalorian/steel-oath), [Ember Smithing](https://github.com/synthalorian/ember-smithing), [The Gray-Mane Debt](https://github.com/synthalorian/the-gray-mane-debt), [Blackshield Codex](https://github.com/synthalorian/blackshield-codex), [War Banners](https://github.com/synthalorian/war-banners), [Roadwarden](https://github.com/synthalorian/roadwarden), [Sellsword Roster](https://github.com/synthalorian/sellsword-roster), [The Turncoat Contract](https://github.com/synthalorian/the-turncoat-contract), [The Ledger of Ash](https://github.com/synthalorian/the-ledger-of-ash), [Oathbreaker's Price](https://github.com/synthalorian/oathbreakers-price), [Champion's Reforge](https://github.com/synthalorian/champions-reforge), [The Barrow-Iron Pact](https://github.com/synthalorian/barrow-iron-pact). Own prefix (`LOA_`), records, globals, scripts and files; checked by loading all 15 plugins together (see [tools/modkit](tools/modkit/README.md)).

## The quest

1. **"That book you carry is burnt at the edges."** Talk to Aulus near the Riften Stables (level 10+). Accept: you get the **Torn Ledger Page** and the **Ledger-Bound Amulet** (resist magic, fortify magicka).
2. **Read the page**: the ash shows three places, one Nordic crypt in the east (Rift, Eastmarch, Winterhold) and one dungeon each in the north (Pale, Hjaalmarch, Haafingar) and the west (Reach, Falkreath, Whiterun).
3. **Three thralls**, three shards (ember, rime, storm). Shards can be taken in any order, even before you read the page. The journal counts them.
4. **The choice**, made with Aulus once you hold all three.

| Choice | How | Reward |
|---|---|---|
| Bind | "Bind them to the amulet." | The amulet becomes the **Ashen Ledger Amulet** (resist fire/frost/shock 25%, fortify magicka 40), swapped and re-equipped in place |
| Forge | "Forge them into a weapon." | **Ledger-Breaker** (ebony war axe, fire + frost + shock), but the Ledger sends its **Collector** after you at once. Kill it and tell Aulus. You keep the original amulet |
| Destroy | "Break them. Let the debts die." | 800 gold and **Ledger's Absolution**, a permanent ability (resist magic 15%, health +25). You keep the original amulet |

## Items

- Ledger-Bound Amulet, upgradeable to the Ashen Ledger Amulet
- Ledger-Breaker (ebony war axe)
- Ledger's Absolution (ability)
- Three debt-shards, Torn Ledger Page

## Series integration (optional, no masters)

When other mods of the series are installed, this one notices. Nothing here is a master and nothing is required: a script on the player looks each plugin up at runtime with `Game.GetFormFromFile` (vanilla Papyrus since 2012, no SKSE) on every game load, and when the plugin is missing the call returns None and that feature is skipped.

| Mod | Reads | Effect |
|---|---|---|
| Bounties of Oblivion | `BOO_ContractsDone` | If you've completed at least one Ledger contract there, Aulus recognizes your mark in the Ledger (different opening). |
| The Turncoat Contract | `TTC_State` | If you took Vane's bribe, Aulus has heard you sold out a Blackshield sergeant, and likes you for it (different opening). |

## Unvoiced dialogue

All 24 new lines are unvoiced, shown as subtitles, and each one ships with a silent voice file sized to its length, so subtitles stay on screen (same approach as The Gray-Mane Debt). Turn subtitles on. Fuz Ro D'oh is not needed and doesn't conflict.

## Install

Grab `LedgerOfAsh-0.1.2.zip` from [Releases](https://github.com/synthalorian/the-ledger-of-ash/releases) (FOMOD installer).

- **Mod Organizer 2** or **Vortex**: install the zip and enable `LedgerOfAsh.esp`.
- **Manual (Linux/Proton)**: copy everything inside `00 Core/` (plugin, `Scripts/`, `SEQ/`, `Sound/`) into `.../steamapps/common/Skyrim Special Edition/Data/` and add `*LedgerOfAsh.esp` to `Plugins.txt` in the Proton prefix (`.../compatdata/489830/pfx/drive_c/users/steamuser/AppData/Local/Skyrim Special Edition/`).

Safe to add to an existing save. Finish the quest before uninstalling.

## Console tunables

```
set LOA_MinLevel to 10
set LOA_DestroyGold to 800
```

## What got scaled down

- The Collector is a hostile spawn at the Riften Stables map marker rather than a Daedric invasion with its own scene.
- The shard guardians are templated from vanilla draugr and bandit-boss lists, renamed; they have no custom visuals.
- Aulus is a new NPC at a vanilla map marker; no cell edits.

## Known issues and risks

- Not playtested in-game.
- Needs three free dungeons with map markers at quest start; if not, the start is refused with a message and nothing changes.
- If you fight the Collector inside Riften Stables, nearby guards may join in.

## Build from source

`./build.sh` on Linux builds the release zip: Papyrus is compiled with the open-source [papyrus-compiler](https://github.com/russo-2025/papyrus-compiler) against clean-room vanilla declaration stubs, the plugin is generated from C# with [Mutagen](https://github.com/Mutagen-Modding/Mutagen) (`src/plugin/LedgerOfAsh.Generator`: `Mod.cs` holds this mod, `Kit.cs`/`Program.cs`/`Vanilla.cs` are shared by the third wave), the SEQ file and silent voice files are written next to it, and everything is validated. The zip is reproducible: a fresh clone rebuilds it byte for byte. `src/plugin/formids.json` pins every FormID; never edit or reuse entries.

Current build: 62 new records (FormIDs 800-83D): Armor 2, Book 1, DialogBranch 4, DialogResponses 15, DialogTopic 10, Faction 2, FormList 3, GlobalFloat 7, MagicEffect 1, Message 2, MiscItem 3, Npc 5, ObjectEffect 3, Quest 2, Spell 1, Weapon 1; 11 Papyrus scripts; 24 silent voice files; 1 SEQ entry.

## Credits

- Built with [Mutagen](https://github.com/Mutagen-Modding/Mutagen) and [russo-2025/papyrus-compiler](https://github.com/russo-2025/papyrus-compiler).
- Skyrim and every referenced vanilla record and mesh belong to Bethesda Softworks. This mod references them from Skyrim.esm and ships no Bethesda assets.

## Support

Donations are optional. Nothing in the mod is gated on them.

- GitHub Sponsors: https://github.com/sponsors/synthalorian
- Ko-fi: https://ko-fi.com/synthalorian
