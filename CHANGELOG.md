# Changelog

All notable changes to this project are documented here. Versions follow [Semantic Versioning](https://semver.org/).

## [0.1.0] - 2026-10-07

Early beta. First public release. It passes every automated check in the build (Papyrus compile, plugin and dialogue validation, round trip, voice file and SEQ checks, FOMOD schema, 15-plugin series compatibility check) but has not been playtested in-game yet.

### Added
- "That book you carry is burnt at the edges." Talk to Aulus near the Riften Stables (level 10+). Accept: you get the Torn Ledger Page and the Ledger-Bound Amulet (resist magic, fortify magicka).
- Read the page: the ash shows three places, one Nordic crypt in the east (Rift, Eastmarch, Winterhold) and one dungeon each in the north (Pale, Hjaalmarch, Haafingar) and the west (Reach, Falkreath, Whiterun).
- Three thralls, three shards (ember, rime, storm). Shards can be taken in any order, even before you read the page. The journal counts them.
- The choice, made with Aulus once you hold all three.
- Choice: Bind: The amulet becomes the Ashen Ledger Amulet (resist fire/frost/shock 25%, fortify magicka 40), swapped and re-equipped in place.
- Choice: Forge: Ledger-Breaker (ebony war axe, fire + frost + shock), but the Ledger sends its Collector after you at once. Kill it and tell Aulus. You keep the original amulet.
- Choice: Destroy: 800 gold and Ledger's Absolution, a permanent ability (resist magic 15%, health +25). You keep the original amulet.
- Soft integration with Bounties of Oblivion (runtime lookup, not a master).
- Soft integration with The Turncoat Contract (runtime lookup, not a master).
- Unvoiced dialogue with 24 silent voice files; `SEQ/LedgerOfAsh.seq` for the start-game-enabled dialogue quest.
- FOMOD installer with optional Papyrus sources.
