Scriptname LOA_PageAliasScript extends ReferenceAlias
{The Ledger of Ash - on the torn page.}

Event OnRead()
	Utility.Wait(0.1)
	(GetOwningQuest() as LOA_MainScript).PageRead()
EndEvent
