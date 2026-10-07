Scriptname LOA_MainScript extends Quest
{The Ledger of Ash - questline controller.
Stages: 10 read the page, 20 hunt the three shards, 30 choose, 40 Collector hunt (forged), 50 Collector dead,
100/110/120 completed (bound / forged / destroyed).}

ReferenceAlias Property Aulus Auto
ReferenceAlias[] Property Shards Auto
ReferenceAlias Property Collector Auto
GlobalVariable Property LOA_State Auto
GlobalVariable Property LOA_ShardsFound Auto
GlobalVariable Property LOA_ShardsTotal Auto
GlobalVariable Property LOA_DestroyGold Auto
Armor Property LOA_AmuletBound Auto
Armor Property LOA_AmuletAshen Auto
Weapon Property LOA_LedgerBreaker Auto
Spell Property LOA_Absolution Auto
Message Property LOA_MsgStir Auto
Faction Property LOA_CollectorFaction Auto
Faction Property PlayerFaction Auto
MiscObject Property Gold001 Auto

Bool[] Found
Bool PendingStop = False

Function Begin(ObjectReference akAulus)
	If akAulus
		Aulus.ForceRefTo(akAulus)
	EndIf
	Found = new Bool[4]
	LOA_ShardsFound.SetValue(0)
	UpdateCurrentInstanceGlobal(LOA_ShardsFound)
	Game.GetPlayer().AddItem(LOA_AmuletBound, 1)
	SetStage(10)
	SetObjectiveDisplayed(10)
EndFunction

Function PageRead()
	If GetStage() != 10
		Return
	EndIf
	SetObjectiveCompleted(10)
	SetStage(20)
	Int i = 1
	While i <= 3
		If !Found[i]
			SetObjectiveDisplayed(20 + i)
		EndIf
		i += 1
	EndWhile
	SetObjectiveDisplayed(24)
	CheckAll()
EndFunction

; Shard alias i moved into the player's inventory (may happen before the page is read).
Function ShardTaken(Int aiIndex)
	If GetStage() < 10 || GetStage() >= 30 || Found[aiIndex]
		Return
	EndIf
	Found[aiIndex] = True
	LOA_ShardsFound.Mod(1)
	UpdateCurrentInstanceGlobal(LOA_ShardsFound)
	LOA_MsgStir.Show()
	If GetStage() >= 20
		SetObjectiveCompleted(20 + aiIndex)
		SetObjectiveDisplayed(24, True, True)
		CheckAll()
	EndIf
EndFunction

Function CheckAll()
	If LOA_ShardsFound.GetValueInt() >= LOA_ShardsTotal.GetValueInt() && GetStage() == 20
		SetObjectiveCompleted(21)
		SetObjectiveCompleted(22)
		SetObjectiveCompleted(23)
		SetObjectiveCompleted(24)
		SetStage(30)
		SetObjectiveDisplayed(30)
	EndIf
EndFunction

Function TakeShards()
	Actor player = Game.GetPlayer()
	Int i = 0
	While i < Shards.Length
		ObjectReference s = Shards[i].GetReference()
		If s && player.GetItemCount(s.GetBaseObject()) > 0
			player.RemoveItem(s.GetBaseObject(), 1, True)
		EndIf
		i += 1
	EndWhile
EndFunction

Function Bind()
	If GetStage() != 30
		Return
	EndIf
	TakeShards()
	Actor player = Game.GetPlayer()
	Bool wasWorn = player.IsEquipped(LOA_AmuletBound)
	If player.GetItemCount(LOA_AmuletBound) > 0
		player.RemoveItem(LOA_AmuletBound, 1, True)
	EndIf
	player.AddItem(LOA_AmuletAshen, 1)
	If wasWorn
		player.EquipItem(LOA_AmuletAshen, False, True)
	EndIf
	SetObjectiveCompleted(30)
	LOA_State.SetValue(2)
	SetStage(100)
	Finish()
EndFunction

Function Forge()
	If GetStage() != 30
		Return
	EndIf
	TakeShards()
	Game.GetPlayer().AddItem(LOA_LedgerBreaker, 1)
	SetObjectiveCompleted(30)
	SetStage(40)
	SetObjectiveDisplayed(40)
	Actor c = Collector.GetActorReference()
	If c
		c.Enable()
		LOA_CollectorFaction.SetEnemy(PlayerFaction)
		c.SetActorValue("Aggression", 1.0)
		c.StartCombat(Game.GetPlayer())
	EndIf
EndFunction

Function CollectorDied()
	If GetStage() != 40
		Return
	EndIf
	SetObjectiveCompleted(40)
	SetStage(50)
	SetObjectiveDisplayed(50)
EndFunction

Function FinishForged()
	If GetStage() != 50
		Return
	EndIf
	SetObjectiveCompleted(50)
	LOA_State.SetValue(3)
	SetStage(110)
	Finish()
EndFunction

Function Destroy()
	If GetStage() != 30
		Return
	EndIf
	TakeShards()
	Actor player = Game.GetPlayer()
	player.AddItem(Gold001, LOA_DestroyGold.GetValueInt())
	player.AddSpell(LOA_Absolution)
	SetObjectiveCompleted(30)
	LOA_State.SetValue(4)
	SetStage(120)
	Finish()
EndFunction

Function Finish()
	PendingStop = True
	RegisterForSingleUpdate(4.0)
EndFunction

Event OnUpdate()
	If PendingStop
		PendingStop = False
		Debug.Trace("LOA_ questline finished at stage " + GetStage() + ", state " + LOA_State.GetValueInt())
		Stop()
	EndIf
EndEvent
