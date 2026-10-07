Scriptname LOA_TIF_Bind extends TopicInfo Hidden
{The Ledger of Ash - dialogue fragment: bind the shards.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as LOA_MainScript).Bind()
EndFunction
