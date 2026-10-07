Scriptname LOA_TIF_Forge extends TopicInfo Hidden
{The Ledger of Ash - dialogue fragment: forge the shards.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as LOA_MainScript).Forge()
EndFunction
