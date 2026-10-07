Scriptname LOA_TIF_Accept extends TopicInfo Hidden
{The Ledger of Ash - dialogue fragment: accept.}

Function Fragment_0(ObjectReference akSpeakerRef)
	(GetOwningQuest() as LOA_IntroScript).Accept()
EndFunction
