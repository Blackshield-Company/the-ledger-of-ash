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
ReferenceAlias[] Property Thralls Auto
ReferenceAlias[] Property BossSpots Auto
{Optional: each shard dungeon's boss spot.}
Form Property LOA_Shard1 Auto
Form Property LOA_Shard2 Auto
Form Property LOA_Shard3 Auto

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
	Int i = 0
	While i < 3
		PlaceAtBoss(Thralls[i], BossSpots[i])
		i += 1
	EndWhile
	EnsureShards()
	SetObjectiveDisplayed(10)
EndFunction

Form Function ShardBase(Int aiIndex)
	If aiIndex == 1
		Return LOA_Shard1
	ElseIf aiIndex == 2
		Return LOA_Shard2
	EndIf
	Return LOA_Shard3
EndFunction

; 0.1.1: at quest start, and when a thrall loads or dies.
Function EnsureShards()
	If GetStage() < 10 || GetStage() >= 30
		Return
	EndIf
	If !Found
		Found = new Bool[4]
	EndIf
	Actor player = Game.GetPlayer()
	Int i = 1
	While i <= 3
		If !Found[i]
			If player.GetItemCount(ShardBase(i)) > 0
				ShardTaken(i)
			Else
				EnsureOn(Thralls[i - 1], Shards[i - 1], ShardBase(i))
			EndIf
		EndIf
		i += 1
	EndWhile
EndFunction

; 0.1.1: Aulus's "the thrall had no shard" dialogue: hand over the shard of every dead (or missing) thrall.
Function RecoverShards()
	If GetStage() < 10 || GetStage() >= 30
		Return
	EndIf
	EnsureShards()
	Int given = 0
	Int i = 1
	While i <= 3
		If !Found[i] && Lost(Thralls[i - 1], ShardBase(i))
			GiveToPlayer(Shards[i - 1], ShardBase(i))
			ShardTaken(i)
			given += 1
		EndIf
		i += 1
	EndWhile
	If given == 0
		Debug.Notification("No shard is missing. The thralls still carry them.")
	EndIf
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

; ---------------------------------------------------------------------------
; 0.1.1: alias-created quest items could be missing from the NPC that should carry them
; (same bug as Champion's Reforge 0.1.0 / The Gray-Mane Debt 0.1.0). These re-add them by script.
; ---------------------------------------------------------------------------

; Make sure akHolder carries akBase (unless the player already has it).
Function EnsureOn(ReferenceAlias akHolder, ReferenceAlias akItem, Form akBase)
	Actor h = akHolder.GetReference() as Actor
	If !h || !akBase || Game.GetPlayer().GetItemCount(akBase) > 0 || h.GetItemCount(akBase) > 0
		Return
	EndIf
	ObjectReference r = akItem.GetReference()
	If !r
		r = h.PlaceAtMe(akBase, 1, True)
		akItem.ForceRefTo(r)
	EndIf
	h.AddItem(r, 1, True)
	Debug.Trace("LOA_ EnsureOn: re-added " + akBase + " to " + h)
EndFunction

; Hand akBase straight to the player (unstick path). The item alias's own pickup event then advances the quest.
Function GiveToPlayer(ReferenceAlias akItem, Form akBase)
	Actor player = Game.GetPlayer()
	If !akBase || player.GetItemCount(akBase) > 0
		Return
	EndIf
	ObjectReference r = akItem.GetReference()
	If !r
		r = player.PlaceAtMe(akBase, 1, True)
		akItem.ForceRefTo(r)
	EndIf
	player.AddItem(r, 1)
	Debug.Trace("LOA_ GiveToPlayer: " + akBase)
EndFunction

; True when the carrier is dead or gone and no longer has the item.
Bool Function Lost(ReferenceAlias akHolder, Form akBase)
	Actor h = akHolder.GetReference() as Actor
	Return !h || (h.IsDead() && h.GetItemCount(akBase) == 0)
EndFunction

; Move a spawned NPC from the outdoor map marker to the location's boss spot, if it has one.
Function PlaceAtBoss(ReferenceAlias akActor, ReferenceAlias akSpot)
	Actor a = akActor.GetReference() as Actor
	ObjectReference spot = akSpot.GetReference()
	If a && spot && !a.IsDead()
		a.MoveTo(spot)
		Debug.Trace("LOA_ moved " + a + " to the boss spot")
	ElseIf a
		Debug.Trace("LOA_ no boss spot for " + a + "; stays at the map marker")
	EndIf
EndFunction
