Scriptname LOA_ThrallAliasScript extends ReferenceAlias
{The Ledger of Ash - on each ash thrall. 0.1.1: re-checks that it carries its shard.}

Int Property Index Auto

Event OnLoad()
	(GetOwningQuest() as LOA_MainScript).EnsureShards()
EndEvent

Event OnDeath(Actor akKiller)
	(GetOwningQuest() as LOA_MainScript).EnsureShards()
EndEvent
