Scriptname LOA_TIF_Destroy extends TopicInfo Hidden
{The Ledger of Ash - dialogue fragment: destroy the shards.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as LOA_MainScript).Destroy()
EndFunction
