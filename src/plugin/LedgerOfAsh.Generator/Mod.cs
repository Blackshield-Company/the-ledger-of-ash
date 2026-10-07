using Modkit.Mutagen;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using static Wave3Gen.Kit;
using static Wave3Gen.Vanilla;

namespace Wave3Gen;

/// <summary>The Ledger of Ash: everything specific to this mod. Kit/Program/Vanilla/ItemData are shared.</summary>
internal static class ModInfo
{
    public const string PluginName = "LedgerOfAsh.esp";
    public const string Description = "The Ledger of Ash: a Daedric relic hunt across three holds, three choices and enhanced artifacts.";

    const int APlayer = 0, AAulus = 1, APage = 2, ALoc1 = 3, AMark1 = 4, AThrall1 = 5, AShard1 = 6, ALoc2 = 7, AMark2 = 8, AThrall2 = 9, AShard2 = 10,
        ALoc3 = 11, AMark3 = 12, AThrall3 = 13, AShard3 = 14, ACollectorMarker = 15, ACollector = 16;
    const int IPlayer = 0, IMarker = 1, IAulus = 2;

    public static void Build(Kit k)
    {
        var qIntro = k.Id("LOA_Intro");
        var qMain = k.Id("LOA_Main");

        var gState = k.Global("LOA_State", 0);         // 0 new, 1 running, 2 bound, 3 forged, 4 destroyed
        var gMinLevel = k.Global("LOA_MinLevel", 10);
        var gShards = k.Global("LOA_ShardsFound", 0);
        var gShardsTotal = k.Global("LOA_ShardsTotal", 3);
        var gGold = k.Global("LOA_DestroyGold", 800);
        var gBOO = k.Global("LOA_SeriesBOO", 0);        // 0 = Bounties of Oblivion not installed, else BOO_ContractsDone + 1
        var gTTC = k.Global("LOA_SeriesTTC", 0);        // TTC_State + 1 when The Turncoat Contract is installed

        var keepers = k.Faction("LOA_KeeperFaction", "Ledger-Keepers");
        var collectors = k.Faction("LOA_CollectorFaction", "Ledger Collectors");
        var east = k.List("LOA_HoldsEast", new[] { RiftHoldLocation, EastmarchHoldLocation, WinterholdHoldLocation });
        var north = k.List("LOA_HoldsNorth", new[] { PaleHoldLocation, HjaalmarchHoldLocation, HaafingarHoldLocation });
        var west = k.List("LOA_HoldsWest", new[] { ReachHoldLocation, FalkreathHoldLocation, WhiterunHoldLocation });

        // --- Artifacts -------------------------------------------------------------------------------
        var amuletEnch0 = k.ArmorEnchant("LOA_EnchLedgerBound", "Ledger-Bound", (EnchResistMagicConstantSelf, 10), (EnchFortifyMagickaConstantSelf, 20));
        var amuletEnchA = k.ArmorEnchant("LOA_EnchAshenLedger", "Ashen Ledger",
            (EnchResistFireConstantSelf, 25), (EnchResistFrostConstantSelf, 25), (EnchResistShockConstantSelf, 25), (EnchFortifyMagickaConstantSelf, 40));
        var amulet0 = k.Jewelry("LOA_AmuletBound", T.Amulet0, false, MeshAmuletGoldRuby, GoldAmuletRubyAA, 700, amuletEnch0);
        var amuletA = k.Jewelry("LOA_AmuletAshen", T.AmuletA, false, MeshAmuletGoldGems, GoldAmuletGemsAA, 2600, amuletEnchA);
        var axeEnch = k.WeaponEnchant("LOA_EnchLedgerBreaker", "Three Debts", 45,
            (EnchFireDamageFFContact, 12, 0), (EnchFrostDamageFFContact, 12, 0), (EnchShockDamageFFContact, 12, 0));
        var axe = k.WeaponLike("Ebony War Axe", "LOA_LedgerBreaker", T.Axe, 16, 2400, axeEnch, 3000);
        var absolveFx = k.ValueEffect("LOA_AbsolutionEffect", T.Absolution, ActorValue.ResistMagic, T.AbsolutionDesc);
        var absolution = k.Ability("LOA_Absolution", T.Absolution, T.AbsolutionDesc, (absolveFx, 15), (EnchFortifyHealthConstantSelf, 25));

        var page = k.Note("LOA_TornPage", T.PageName, T.Page, second: true);
        var shard1 = k.Misc("LOA_ShardEmber", T.Shard1, MeshAmethyst, 0, 0.5f);
        var shard2 = k.Misc("LOA_ShardRime", T.Shard2, MeshAmethyst, 0, 0.5f);
        var shard3 = k.Misc("LOA_ShardStorm", T.Shard3, MeshAmethyst, 0, 0.5f);

        // --- Actors -----------------------------------------------------------------------------------
        var aulus = k.Speaker("LOA_Aulus", T.AulusName, T.AulusShort, EncGuardImperialM02MaleNordCommander, FineClothesOutfit02, keepers.FormKey, 10, 50,
            (K(0x013989), 1));
        var thrall1 = k.Enemy("LOA_ThrallEmber", T.Thrall1, T.ThrallShort, LCharDraugrBossNoDragonPriest, DraugrRace);
        var thrall2 = k.Enemy("LOA_ThrallRime", T.Thrall2, T.ThrallShort, LCharBanditBoss, NordRace);
        var thrall3 = k.Enemy("LOA_ThrallStorm", T.Thrall3, T.ThrallShort, LCharBanditBossNordM, NordRace);
        var collector = k.Enemy("LOA_Collector", T.CollectorName, T.CollectorShort, LCharBanditBoss, NordRace);
        // Collector: bandit looks and combat, own faction and AI so he waits until the forging choice.
        collector.Configuration.TemplateFlags &= ~(NpcConfiguration.TemplateFlag.Factions | NpcConfiguration.TemplateFlag.AIData | NpcConfiguration.TemplateFlag.Script);
        collector.Factions.Add(new RankPlacement { Faction = L<IFactionGetter>(collectors.FormKey), Rank = 0 });
        collector.AIData = new AIData { Aggression = Aggression.Unaggressive, Confidence = Confidence.Brave, Assistance = Assistance.HelpsAllies, EnergyLevel = 50 };
        collector.Configuration.Level = new PcLevelMult { LevelMult = 1.2f };

        var msgNoStart = k.Msg("LOA_MsgNoStart", T.NoStart);
        var msgStir = k.Msg("LOA_MsgStir", T.Stir);

        // --- Intro --------------------------------------------------------------------------------------
        var intro = k.Quest(qIntro, "LOA_Intro", T.QuestName, true, Quest.TypeEnum.None, 50);
        intro.Aliases.Add(Player(IPlayer));
        intro.Aliases.Add(Forced(IMarker, "Camp", RiftenStablesMapMarker));
        intro.Aliases.Add(Created(IAulus, "Aulus", aulus.FormKey, IMarker, false, Level.Medium, QuestAlias.Flag.StoresText));
        intro.NextAliasID = 3;
        intro.VirtualMachineAdapter = Vmad.QuestAdapter(Vmad.Script("LOA_IntroScript",
                Vmad.Obj("LOA_Main", qMain), Vmad.Obj("LOA_State", gState), Vmad.Obj("LOA_MsgNoStart", msgNoStart), Vmad.Alias("Aulus", qIntro, IAulus)))
            .WithAliasScripts(qIntro, IPlayer, Vmad.Script("LOA_SeriesScript", Vmad.Obj("LOA_SeriesBOO", gBOO), Vmad.Obj("LOA_SeriesTTC", gTTC)));

        // --- Main ----------------------------------------------------------------------------------------
        var main = k.Quest(qMain, "LOA_Main", T.QuestName, false, Quest.TypeEnum.Daedric, 60);
        main.TextDisplayGlobals.Add(L<IGlobalGetter>(gShards));
        main.TextDisplayGlobals.Add(L<IGlobalGetter>(gShardsTotal));
        Stage(main, 0, null, flags: QuestStage.Flag.StartUpStage);
        Stage(main, 10, T.Log10);
        Stage(main, 20, T.Log20);
        Stage(main, 30, T.Log30);
        Stage(main, 40, T.Log40);
        Stage(main, 50, T.Log50);
        Stage(main, 100, T.Log100, QuestLogEntry.Flag.CompleteQuest);
        Stage(main, 110, T.Log110, QuestLogEntry.Flag.CompleteQuest);
        Stage(main, 120, T.Log120, QuestLogEntry.Flag.CompleteQuest);
        Objective(main, 10, T.Obj10);
        Objective(main, 21, T.Obj21, AThrall1);
        Objective(main, 22, T.Obj22, AThrall2);
        Objective(main, 23, T.Obj23, AThrall3);
        Objective(main, 24, T.Obj24);
        Objective(main, 30, T.Obj30, AAulus);
        Objective(main, 40, T.Obj40, ACollector);
        Objective(main, 50, T.Obj50, AAulus);

        main.Aliases.Add(Player(APlayer));
        main.Aliases.Add(new QuestAlias { ID = AAulus, Type = QuestAlias.TypeEnum.Reference, Name = "Aulus", Flags = QuestAlias.Flag.Optional | QuestAlias.Flag.StoresText });
        main.Aliases.Add(Created(APage, "Page", page.FormKey, APlayer, true, Level.Easy, 0));
        main.Aliases.Add(Dungeon(ALoc1, "Loc1", east.FormKey, LocTypeDraugrCrypt));
        main.Aliases.Add(MarkerOf(AMark1, "Mark1", ALoc1));
        main.Aliases.Add(Created(AThrall1, "Thrall1", thrall1.FormKey, AMark1, false, Level.Hard, QuestAlias.Flag.StoresText));
        main.Aliases.Add(Created(AShard1, "Shard1", shard1.FormKey, AThrall1, true, Level.Easy, QuestAlias.Flag.QuestObject));
        main.Aliases.Add(Dungeon(ALoc2, "Loc2", north.FormKey, LocTypeDungeon));
        main.Aliases.Add(MarkerOf(AMark2, "Mark2", ALoc2));
        main.Aliases.Add(Created(AThrall2, "Thrall2", thrall2.FormKey, AMark2, false, Level.Hard, QuestAlias.Flag.StoresText));
        main.Aliases.Add(Created(AShard2, "Shard2", shard2.FormKey, AThrall2, true, Level.Easy, QuestAlias.Flag.QuestObject));
        main.Aliases.Add(Dungeon(ALoc3, "Loc3", west.FormKey, LocTypeDungeon));
        main.Aliases.Add(MarkerOf(AMark3, "Mark3", ALoc3));
        main.Aliases.Add(Created(AThrall3, "Thrall3", thrall3.FormKey, AMark3, false, Level.Hard, QuestAlias.Flag.StoresText));
        main.Aliases.Add(Created(AShard3, "Shard3", shard3.FormKey, AThrall3, true, Level.Easy, QuestAlias.Flag.QuestObject));
        main.Aliases.Add(Forced(ACollectorMarker, "CollectorMarker", RiftenStablesMapMarker));
        main.Aliases.Add(Created(ACollector, "Collector", collector.FormKey, ACollectorMarker, false, Level.Hard, QuestAlias.Flag.InitiallyDisabled | QuestAlias.Flag.StoresText));
        main.NextAliasID = (uint)main.Aliases.Count;
        main.VirtualMachineAdapter = Vmad.QuestAdapter(Vmad.Script("LOA_MainScript",
                Vmad.Alias("Aulus", qMain, AAulus), Vmad.AliasList("Shards", qMain, AShard1, AShard2, AShard3), Vmad.Alias("Collector", qMain, ACollector),
                Vmad.Obj("LOA_State", gState), Vmad.Obj("LOA_ShardsFound", gShards), Vmad.Obj("LOA_ShardsTotal", gShardsTotal), Vmad.Obj("LOA_DestroyGold", gGold),
                Vmad.Obj("LOA_AmuletBound", amulet0.FormKey), Vmad.Obj("LOA_AmuletAshen", amuletA.FormKey), Vmad.Obj("LOA_LedgerBreaker", axe.FormKey),
                Vmad.Obj("LOA_Absolution", absolution), Vmad.Obj("LOA_MsgStir", msgStir), Vmad.Obj("LOA_CollectorFaction", collectors.FormKey),
                Vmad.Obj("PlayerFaction", PlayerFaction), Vmad.Obj("Gold001", Gold001)))
            .WithAliasScripts(qMain, APage, Vmad.Script("LOA_PageAliasScript"))
            .WithAliasScripts(qMain, AShard1, Vmad.Script("LOA_ShardAliasScript", Vmad.Int("Index", 1)))
            .WithAliasScripts(qMain, AShard2, Vmad.Script("LOA_ShardAliasScript", Vmad.Int("Index", 2)))
            .WithAliasScripts(qMain, AShard3, Vmad.Script("LOA_ShardAliasScript", Vmad.Int("Index", 3)))
            .WithAliasScripts(qMain, ACollector, Vmad.Script("LOA_CollectorAliasScript"));

        // --- Dialogue --------------------------------------------------------------------------------------
        var sAulus = ("MaleNordCommander", T.AulusName);
        var isAulus = Cond.IsID(aulus.FormKey);
        Condition State(int v) => GlobalIs(gState, CompareOperator.EqualTo, v);
        Condition St(CompareOperator op, int s) => StageIs(qMain, op, s);

        var bIntro = k.Id("LOA_IntroBranch");
        var tStart = k.Topic("LOA_I_Start", qIntro, bIntro, T.TopicStart, sAulus);
        var tAccept = k.Topic("LOA_I_Accept", qIntro, bIntro, T.TopicAccept, sAulus);
        var tDecline = k.Topic("LOA_I_Decline", qIntro, bIntro, T.TopicDecline, sAulus);
        k.Branch(bIntro, "LOA_IntroBranch", qIntro, tStart);
        var offer = new[] { isAulus, State(0), PlayerLevelAtLeast(gMinLevel), Cond.QuestRunning(qMain, false) };
        k.Info(tStart, "LOA_I_StartBOO", T.StartBOO, offer.Append(GlobalIs(gBOO, CompareOperator.GreaterThanOrEqualTo, 2)).ToArray(), links: new[] { tAccept, tDecline });
        k.Info(tStart, "LOA_I_StartTTC", T.StartTTC, offer.Append(GlobalIs(gTTC, CompareOperator.EqualTo, 5)).ToArray(), links: new[] { tAccept, tDecline });
        k.Info(tStart, "LOA_I_StartOffer", T.Start, offer, links: new[] { tAccept, tDecline });
        k.Info(tStart, "LOA_I_StartDone", T.Done, new[] { isAulus, GlobalIs(gState, CompareOperator.GreaterThanOrEqualTo, 2) }, prompt: T.PromptDone);
        k.Info(tAccept, "LOA_I_AcceptInfo", T.Accept, new[] { isAulus }, goodbye: true, fragment: "LOA_TIF_Accept");
        k.Info(tDecline, "LOA_I_DeclineInfo", T.Decline, new[] { isAulus }, goodbye: true);

        var tStatus = k.TopTopic("LOA_A_Status", qMain, T.TopicStatus, sAulus);
        k.Info(tStatus, "LOA_A_Status10", T.Status10, new[] { isAulus, St(CompareOperator.EqualTo, 10) });
        k.Info(tStatus, "LOA_A_Status20", T.Status20, new[] { isAulus, St(CompareOperator.EqualTo, 20) });
        k.Info(tStatus, "LOA_A_Status40", T.Status40, new[] { isAulus, St(CompareOperator.EqualTo, 40) });

        // The choice
        var bChoice = k.Id("LOA_ChoiceBranch");
        var tShards = k.Topic("LOA_A_Shards", qMain, bChoice, T.TopicShards, sAulus);
        var tBind = k.Topic("LOA_A_Bind", qMain, bChoice, T.TopicBind, sAulus);
        var tForge = k.Topic("LOA_A_Forge", qMain, bChoice, T.TopicForge, sAulus);
        var tDestroy = k.Topic("LOA_A_Destroy", qMain, bChoice, T.TopicDestroy, sAulus);
        var tWait = k.Topic("LOA_A_Wait", qMain, bChoice, T.TopicWait, sAulus);
        k.Branch(bChoice, "LOA_ChoiceBranch", qMain, tShards);
        var atChoice = new[] { isAulus, St(CompareOperator.EqualTo, 30) };
        k.Info(tShards, "LOA_A_ShardsInfo", T.Shards, atChoice, links: new[] { tBind, tForge, tDestroy, tWait });
        k.Info(tBind, "LOA_A_BindInfo", T.Bind, atChoice, goodbye: true, fragment: "LOA_TIF_Bind");
        k.Info(tForge, "LOA_A_ForgeInfo", T.Forge, atChoice, goodbye: true, fragment: "LOA_TIF_Forge");
        k.Info(tDestroy, "LOA_A_DestroyInfo", T.Destroy, atChoice, goodbye: true, fragment: "LOA_TIF_Destroy");
        k.Info(tWait, "LOA_A_WaitInfo", T.Wait, atChoice, goodbye: true);

        var tCollected = k.TopTopic("LOA_A_Collected", qMain, T.TopicCollected, sAulus);
        k.Info(tCollected, "LOA_A_CollectedInfo", T.Collected, new[] { isAulus, St(CompareOperator.EqualTo, 50) }, goodbye: true, fragment: "LOA_TIF_Finish");
    }
}

internal static class T
{
    public const string QuestName = "The Ledger of Ash";
    public const string AulusName = "Aulus Ledger-Keeper", AulusShort = "Aulus";
    public const string ThrallShort = "Ash-Thrall";
    public const string Thrall1 = "Ember-Bound Thrall", Thrall2 = "Rime-Bound Thrall", Thrall3 = "Storm-Bound Thrall";
    public const string CollectorName = "The Ledger's Collector", CollectorShort = "Collector";

    public const string TopicStart = "That book you carry is burnt at the edges.";
    public static readonly string[] Start =
    {
        "Burnt, and still warm. This is a copy of the Daedric Ledger, or what's left of it. Someone tore out a page and fed it to the fire.",
        "The page held three debts, sealed in three shards of soul-glass. Without them the Ledger can't close its accounts.",
        "The shards went to thralls who owe the Ledger their souls. I'll pay you to collect them. Interested?",
    };
    public static readonly string[] StartBOO =
    {
        "I've seen your mark in the Daedric Ledger. You've worked its contracts. Then you know a debt never stays buried.",
        "A page was torn out and burned. Three debts sealed in three shards of soul-glass, scattered with the thralls who owe them.",
        "Bring them back. You'll know better than most what to do with them.",
    };
    public static readonly string[] StartTTC =
    {
        "The Blackshield say you sold out their sergeant. A turncoat who takes coin from both sides. Good. I need exactly that.",
        "A page of the Daedric Ledger was torn out and burned. Three debts sealed in three shards of soul-glass, scattered with their thralls.",
        "Bring them to me and I'll pay. Don't ask who else is looking.",
    };
    public const string PromptDone = "Is the Ledger satisfied?";
    public static readonly string[] Done = { "The Ledger is never satisfied. But it's quiet, and that's as close as it comes." };
    public const string TopicAccept = "I'll find your shards.";
    public static readonly string[] Accept =
    {
        "Take the torn page; the ash still remembers where the debts went. Read it.",
        "And wear this. It's bound to the Ledger. When you hold the shards, it will listen.",
    };
    public const string TopicDecline = "I want nothing to do with Daedric debts.";
    public static readonly string[] Decline = { "Nobody does. That never stopped one from coming due." };

    public const string TopicStatus = "About the shards...";
    public static readonly string[] Status10 = { "Read the page. The ash will show you where the thralls went to ground." };
    public static readonly string[] Status20 = { "Three shards, three thralls, three holds. The amulet grows heavier near them. Trust it." };
    public static readonly string[] Status40 = { "The Collector came for what you forged. Kill it before it collects you." };

    public const string TopicShards = "I have all three shards.";
    public static readonly string[] Shards =
    {
        "Ember, rime and storm. I can feel the debts from here.",
        "Now the choice is yours, since you bled for them. I can bind them into the amulet and close the Ledger's account.",
        "Or a smith could forge them into a weapon, and the debts would be yours to carry. Or we break them, and the debts die unpaid.",
    };
    public const string TopicBind = "Bind them to the amulet.";
    public static readonly string[] Bind = { "Wise. Hold still... There. The Ledger's account is closed, and the amulet remembers every debt it swallowed." };
    public const string TopicForge = "Forge them into a weapon.";
    public static readonly string[] Forge =
    {
        "Bold. Done, then. The axe holds all three debts now. But the Ledger always sends a Collector for unpaid accounts.",
    };
    public const string TopicDestroy = "Break them. Let the debts die.";
    public static readonly string[] Destroy = { "Unpaid, forever... The souls will thank you, even if the Ledger won't. Here. Coin, and what's left of the blessing." };
    public const string TopicWait = "Let me think on it.";
    public static readonly string[] Wait = { "Think quickly. Shards like those draw attention." };

    public const string TopicCollected = "Your Collector is dead.";
    public static readonly string[] Collected = { "Then the debts are yours and the axe is yours. Carry them both carefully, ledger-hand." };

    public const string Log10 = "Aulus Ledger-Keeper asked me to recover three shards of soul-glass torn from a copy of the Daedric Ledger. He gave me a torn page and a Ledger-Bound amulet.";
    public const string Log20 = "The ash on the torn page shows where the three thralls went: <Alias=Loc1>, <Alias=Loc2> and <Alias=Loc3>.";
    public const string Log30 = "I have all three shards. Aulus says I must choose what becomes of them.";
    public const string Log40 = "I had the shards forged into an axe. The Ledger has sent its Collector after the unpaid debts.";
    public const string Log50 = "The Collector is dead. I should tell Aulus.";
    public const string Log100 = "Aulus bound the three shards into my amulet. The Ledger's account is closed.";
    public const string Log110 = "The shards are an axe now, the Ledger-Breaker, and the Collector that came for them is dead.";
    public const string Log120 = "We broke the shards. The debts died unpaid, and I carry what is left of their blessing.";

    public const string Obj10 = "Read the torn ledger page";
    public const string Obj21 = "Take the ember shard from the thrall at <Alias=Loc1>";
    public const string Obj22 = "Take the rime shard from the thrall at <Alias=Loc2>";
    public const string Obj23 = "Take the storm shard from the thrall at <Alias=Loc3>";
    public const string Obj24 = "Shards recovered: <Global=LOA_ShardsFound>/<Global=LOA_ShardsTotal>";
    public const string Obj30 = "Bring the shards to Aulus near the Riften Stables";
    public const string Obj40 = "Kill the Ledger's Collector";
    public const string Obj50 = "Tell Aulus the Collector is dead";

    public const string PageName = "Torn Ledger Page";
    public const string Page = "...the account of three, sealed in glass, ember and rime and storm...\n\n(The rest is ash. As you read, the ash shifts into three marks on a rough map of Skyrim: <Alias=Loc1>, <Alias=Loc2> and <Alias=Loc3>.)";
    public const string Shard1 = "Ember Debt-Shard", Shard2 = "Rime Debt-Shard", Shard3 = "Storm Debt-Shard";
    public const string Amulet0 = "Ledger-Bound Amulet";
    public const string AmuletA = "Ashen Ledger Amulet";
    public const string Axe = "Ledger-Breaker";
    public const string Absolution = "Ledger's Absolution";
    public const string AbsolutionDesc = "Three debts died unpaid. Resist Magic and Health are increased.";
    public const string NoStart = "Aulus shakes his head. \"The ash isn't settled. Come back later.\"\n\n(The questline could not start: three free dungeons are needed, one in each part of Skyrim. Nothing has changed; try again later.)";
    public const string Stir = "The Ledger-Bound Amulet grows warm. A debt-shard has been found.";
}
