Scriptname LOA_TIF_Finish extends TopicInfo Hidden
{The Ledger of Ash - dialogue fragment: Collector dead.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as LOA_MainScript).FinishForged()
EndFunction
