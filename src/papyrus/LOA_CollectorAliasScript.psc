Scriptname LOA_CollectorAliasScript extends ReferenceAlias
{The Ledger of Ash - on the Collector.}

Event OnDeath(Actor akKiller)
	(GetOwningQuest() as LOA_MainScript).CollectorDied()
EndEvent
