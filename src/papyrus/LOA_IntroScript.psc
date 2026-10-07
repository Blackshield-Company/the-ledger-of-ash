Scriptname LOA_IntroScript extends Quest
{The Ledger of Ash - always-running quest. Holds Aulus near the Riften Stables and his opening topic.}

Quest Property LOA_Main Auto
GlobalVariable Property LOA_State Auto
{0 = not started, 1 = running, 2 = bound, 3 = forged, 4 = destroyed}
Message Property LOA_MsgNoStart Auto
ReferenceAlias Property Aulus Auto

Function Accept()
	If LOA_State.GetValueInt() != 0 || LOA_Main.IsRunning()
		Return
	EndIf
	If LOA_Main.Start()
		LOA_State.SetValue(1)
		(LOA_Main as LOA_MainScript).Begin(Aulus.GetReference())
		Debug.Trace("LOA_ questline started")
	Else
		Debug.Trace("LOA_ LOA_Main failed to start (needs three free dungeons)")
		LOA_MsgNoStart.Show()
	EndIf
EndFunction
