# Nexus Mods page: The Ledger of Ash

Early beta: v0.1.1 passed every automated build check but hasn't been playtested in-game. Exact upload-form values are in [NEXUS_FIELDS.md](NEXUS_FIELDS.md).

## Page settings

- **Game:** Skyrim Special Edition
- **Mod name:** The Ledger of Ash
- **Version:** 0.1.1 (early beta)
- **Category:** Quests and Adventures
- **Suggested tags:** Quests, Lore-Friendly, Daedric, Dialogue, Choices and Consequences, Jewelry, Weapons, ESL, No SKSE, Scripted
- **Requirements:** none.
- **Permissions (suggested):** source on GitHub under MIT; allow modification with credit, no re-uploads of the archive.
- **File to upload:** `LedgerOfAsh-0.1.1.zip` (FOMOD), main file, named "The Ledger of Ash".

## Description (paste into the BBCode editor)

Same text as `nexus/loa-description.txt` in the workspace:

```bbcode
[center][color=#e06c3c][b]EARLY RELEASE (v0.1.1)[/b]: new mod, not yet widely playtested. Please report bugs in the Bugs tab with your Papyrus log lines (grep LOA_). Source: [url=https://github.com/synthalorian/the-ledger-of-ash]GitHub[/url][/color][/center]

[center][size=6][b]THE LEDGER OF ASH[/b][/size]
[size=3][i]A page torn from the Daedric Ledger. Three debts sealed in soul-glass. Three thralls who would rather keep them.[/i][/size][/center]

[line]

[size=4][b]Summary[/b][/size]
[b]Status: Early beta (v0.1.1).[/b] Every automated build check passes, but it hasn't had wide in-game playtesting yet. It's playable now. Bug reports help shape 0.1.x.
A relic hunt for the Daedric Ledger theme of the series. [b]Aulus Ledger-Keeper[/b] waits by the [b]Riften Stables[/b] with a burnt copy of the Ledger: a page was torn out and fed to the fire, and the three debts it held are sealed in shards of soul-glass, carried off by thralls.

[size=4][b]The quest[/b][/size]
[list]
[*][b]"That book you carry is burnt at the edges."[/b] Talk to Aulus near the Riften Stables (level 10+). Accept: you get the [b]Torn Ledger Page[/b] and the [b]Ledger-Bound Amulet[/b] (resist magic, fortify magicka).
[*][b]Read the page[/b]: the ash shows three places, one Nordic crypt in the east (Rift, Eastmarch, Winterhold) and one dungeon each in the north (Pale, Hjaalmarch, Haafingar) and the west (Reach, Falkreath, Whiterun).
[*][b]Three thralls[/b], three shards (ember, rime, storm). Shards can be taken in any order, even before you read the page. The journal counts them.
[*][b]The choice[/b], made with Aulus once you hold all three.
[/list]

[size=4][b]Choices and rewards[/b][/size]
[list]
[*][b]Bind[/b] ("Bind them to the amulet."): The amulet becomes the [b]Ashen Ledger Amulet[/b] (resist fire/frost/shock 25%, fortify magicka 40), swapped and re-equipped in place
[*][b]Forge[/b] ("Forge them into a weapon."): [b]Ledger-Breaker[/b] (ebony war axe, fire + frost + shock), but the Ledger sends its [b]Collector[/b] after you at once. Kill it and tell Aulus. You keep the original amulet
[*][b]Destroy[/b] ("Break them. Let the debts die."): 800 gold and [b]Ledger's Absolution[/b], a permanent ability (resist magic 15%, health +25). You keep the original amulet
[/list]

[size=4][b]Enhanced items[/b][/size]
[list]
[*]Ledger-Bound Amulet, upgradeable to the Ashen Ledger Amulet
[*]Ledger-Breaker (ebony war axe)
[*]Ledger's Absolution (ability)
[*]Three debt-shards, Torn Ledger Page
[/list]

[size=4][b]Part of the series[/b][/size]
Notices the other synthalorian mods when they're installed and never requires them. Each one is looked up at runtime with vanilla Papyrus (no SKSE, no masters):
[list]
[*][b]Bounties of Oblivion[/b]: If you've completed at least one Ledger contract there, Aulus recognizes your mark in the Ledger (different opening).
[*][b]The Turncoat Contract[/b]: If you took Vane's bribe, Aulus has heard you sold out a Blackshield sergeant, and likes you for it (different opening).
[/list]

[size=4][b]Unvoiced dialogue that you can actually read[/b][/size]
The new lines are unvoiced and shown as subtitles. Every line ships with a silent voice file sized to its length, so subtitles stay on screen. [b]No Fuz Ro D'oh needed[/b]. Turn on dialogue subtitles.

[size=4][b]Requirements[/b][/size]
[list]
[*]Skyrim Special Edition or Anniversary Edition (1.5.97 or 1.6.x). Works on Steam under Proton on Linux.
[*]Nothing else. No SKSE, no SkyUI, no DLC, no Creation Club content.
[/list]

[size=4][b]Installation[/b][/size]
Install with [b]Mod Organizer 2[/b] or [b]Vortex[/b] (FOMOD installer) and enable [b]LedgerOfAsh.esp[/b]. It's ESL-flagged. Safe to add mid-playthrough.

[size=4][b]Compatibility[/b][/size]
[list]
[*][b]No edits to vanilla records[/b], no cells, no navmesh. Skyrim.esm is the only master.
[*]New NPCs appear at vanilla map markers; mods that rebuild those spots may leave them standing somewhere odd.
[*]Compatible with all fifteen mods of the series (same author), checked by loading every plugin together: unique FormIDs, EditorIDs, scripts, globals and files, zero overrides.
[/list]

[size=4][b]What got scaled down / known issues[/b][/size]
[list]
[*]The Collector is a hostile spawn at the Riften Stables map marker rather than a Daedric invasion with its own scene.
[*]The shard guardians are templated from vanilla draugr and bandit-boss lists, renamed; they have no custom visuals.
[*]Aulus is a new NPC at a vanilla map marker; no cell edits.
[*]Not playtested in-game.
[*]Needs three free dungeons with map markers at quest start; if not, the start is refused with a message and nothing changes.
[*]If you fight the Collector inside Riften Stables, nearby guards may join in.
[*]Settings are console globals: [code]set LOA_MinLevel to 10 / set LOA_DestroyGold to 800[/code]
[/list]

[size=4][b]Uninstalling[/b][/size]
As with any scripted mod, finish the quest before removing it.

[size=4][b]Source[/b][/size]
Open source (MIT): [url=https://github.com/synthalorian/the-ledger-of-ash]github.com/synthalorian/the-ledger-of-ash[/url]

[size=4][b]Credits[/b][/size]
[list]
[*][url=https://github.com/Mutagen-Modding/Mutagen]Mutagen[/url] by Noggog and contributors.
[*][url=https://github.com/russo-2025/papyrus-compiler]papyrus-compiler[/url] by russo-2025.
[*][url=https://github.com/GandaG/fomod-schema]FOMOD schema[/url] by GandaG.
[*]Bethesda Game Studios for Skyrim.
[/list]

```
