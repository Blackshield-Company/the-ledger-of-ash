Scriptname LOA_TIF_Recover extends TopicInfo Hidden
{The Ledger of Ash - dialogue fragment (0.1.1): hand over the shards of dead thralls that never had them.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as LOA_MainScript).RecoverShards()
EndFunction
