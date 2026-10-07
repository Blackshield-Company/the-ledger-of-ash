Scriptname LOA_ShardAliasScript extends ReferenceAlias
{The Ledger of Ash - on each debt-shard alias.}

Int Property Index Auto

Event OnContainerChanged(ObjectReference akNewContainer, ObjectReference akOldContainer)
	If akNewContainer == Game.GetPlayer()
		(GetOwningQuest() as LOA_MainScript).ShardTaken(Index)
	EndIf
EndEvent
