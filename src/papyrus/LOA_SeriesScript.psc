Scriptname LOA_SeriesScript extends ReferenceAlias
{The Ledger of Ash - soft integration with the rest of the series, on the player alias of LOA_Intro.
Plugins are looked up at runtime (vanilla Papyrus, no SKSE); when one is missing the call returns None.}

GlobalVariable Property LOA_SeriesBOO Auto
{0 = Bounties of Oblivion not installed, otherwise BOO_ContractsDone + 1}
GlobalVariable Property LOA_SeriesTTC Auto
{0 = The Turncoat Contract not installed, otherwise TTC_State + 1 (5 = took Vane's bribe)}

Event OnInit()
	Refresh()
EndEvent

Event OnPlayerLoadGame()
	Refresh()
EndEvent

Function Refresh()
	GlobalVariable booDone = Game.GetFormFromFile(0x80B, "BountiesOfOblivion.esp") as GlobalVariable ; soft: BOO_ContractsDone
	If booDone
		LOA_SeriesBOO.SetValue(booDone.GetValue() + 1.0)
	Else
		LOA_SeriesBOO.SetValue(0.0)
	EndIf
	GlobalVariable ttcState = Game.GetFormFromFile(0x802, "TurncoatContract.esp") as GlobalVariable ; soft: TTC_State
	If ttcState
		LOA_SeriesTTC.SetValue(ttcState.GetValue() + 1.0)
	Else
		LOA_SeriesTTC.SetValue(0.0)
	EndIf
	Debug.Trace("LOA_ series check: BOO " + LOA_SeriesBOO.GetValueInt() + ", TTC " + LOA_SeriesTTC.GetValueInt())
EndFunction
