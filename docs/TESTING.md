# In-game test checklist

The build verifies the plugin, scripts, dialogue structure, voice files and SEQ automatically; none of that proves the quest plays well. Turn on **dialogue subtitles** and Papyrus logging:

```ini
; Skyrim.ini (or SkyrimCustom.ini), [Papyrus]
bEnableLogging=1
bEnableTrace=1
bLoadDebugInformation=1
```

Every log line from this mod starts with `LOA_`. On each load you should see `LOA_ series check: ...` with the detected series mods. `help LOA_ 4` lists the records (ESL-flagged, FormIDs look like `FExxx8xx`).

## Opening

- [ ] Aulus stands at the Riften Stables map marker. "That book you carry is burnt at the edges." offered at level 10+.
- [ ] With Bounties of Oblivion and at least one completed contract, or The Turncoat Contract with the bribe ending, the opening changes.
- [ ] Accept: page + amulet, stage 10.

## Hunt

- [ ] Reading the page: stage 20, three objectives naming three different places plus the shard counter.
- [ ] Each thrall drops its shard; message on pickup; counter updates.
- [ ] Third shard: stage 30, objective to return to Aulus.

## Endings (save at stage 30)

- [ ] Bind: shards removed, amulet swapped (re-equipped if worn), quest completes.
- [ ] Forge: axe added, Collector appears and attacks; killing it: stage 50; Aulus: quest completes.
- [ ] Destroy: 800 gold, Ledger's Absolution in Active Effects.

## Series integration

- [ ] Without any other series mod: the plain opening is offered, the log shows all integration values 0, nothing errors.
- [ ] With each listed mod installed, its variant appears (see README, Series integration).

## What to report

Your game version, load order, the `LOA_` lines from Papyrus.0.log, and which step went wrong.
