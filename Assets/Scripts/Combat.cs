using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Globalization;

public class Combat : MonoBehaviour
{
    //testing purposes
    public BadGuy testBadGuy;
    public int testNumber;

    public Ego ego;
    public BadGuy[] allBadGuys;
    public GameObject combatScreen;
    public GameObject arrow, egoDoneArrow, slot1DoneArrow, slot2aDoneArrow, slot2bDoneArrow, slot3bDoneArrow, slot3cDoneArrow, slotFlank1DoneArrow, slotFlank2DoneArrow;
    public GameObject[] egoArrowPositions, slot1ArrowPositions, slot2aArrowPositions, slot2bArrowPositions, slot3bArrowPositions, slot3cArrowPositions, slotFlank1ArrowPositions, slotFlank2ArrowPositions;
    public GameObject borderEgo, border1, border2a, border2b, border3b, border3c, borderFlank1, borderFlank2;
    public TMP_Text combatWeaponDisplay, combatArmorDisplay, combatShieldDisplay;
    public GameObject slotEgo, slot1, slot2a, slot2b, slot3b, slot3c, slotFlank1, slotFlank2;
    public TMP_Text effects1, effects2a, effects2b, effects3b, effects3c, effectsFlank1, effectsFlank2;
    public TMP_Text name1, name2a, name2b, name3b, name3c, nameFlank1, nameFlank2;
    public TMP_Text special1, special2a, special2b, special3b, special3c, specialFlank1, specialFlank2;
    public TMP_Text attack1, attack2a, attack2b, attack3b, attack3c, attackFlank1, attackFlank2;
    public TMP_Text defend1, defend2a, defend2b, defend3b, defend3c, defendFlank1, defendFlank2;
    public TMP_Text inventory1, inventory2a, inventory2b, inventory3b, inventory3c, inventoryFlank1, inventoryFlank2;
    public TMP_Text curHPEgo, maxHPEgo, curHP1, maxHP1, curHP2a, maxHP2a, curHP2b, maxHP2b, curHP3b, maxHP3b, curHP3c, maxHP3c, curHPFlank1, maxHPFlank1, curHPFlank2, maxHPFlank2;
    public TMP_Text[] turnOrderNames;
    public TMP_Text[] turnOrderActions;
    public TMP_Text[] egoCombatOptions;
    public GameObject battleLog, battleLogGreyScreen, turnOrderBlackScreen, enemySlotGreyScreen, fightOverFade, fightOverFadedScreen, fightOverWhiteScreen, fadeFromWhite1, battleStartFadeFromBlack, battleStartFadeToBlack, battleStartBox;
    public TMP_Text battleText, effectsText, invText, battleStartText;
    public GameObject invDisplay, invDisplayBorder, invOptions, invOptionsBorder, continueArrow, battleStartContinueArrow;
    public TMP_Text combatInvDamage, combatInvCritMultiplier, combatInvToHitMod, combatInvArmorClass, combatInvCritResist, combatInvDmgReduction;
    public Scrollbar scrollBar;
    public List<TMP_Text> invActions = new List<TMP_Text>();
    public AudioSource cursorMove, cursorSelect, cursorCancel, egoTurn;
    public AudioSource badGuyCursorMove, badGuyCursorSelect, badGuyTurn, badGuyDie;
    public AudioSource winBattle, winCoda, loseBattle, hit, criticalHit, gunHit, gunCrit, miss, strangeOccurence, derpSound, joinedTheBattle;
    public AudioSource goodEffect, badEffect, goodInstant, badInstant, blockEffect, battleStartDrum;
    public AudioSource sicklyMusic, creeperMusic, mongerMusic, strategistMusic, bruteMusic;
    public Effect[] allEffects;
    public TMP_Text potion0, potion1, potion2;
    [HideInInspector] public float scrollRectValue;

    BadGuy[] activeBadGuys = { null, null, null, null, null };
    Character[] turnOrder = { null, null, null, null, null, null };
    List<int> usedInitValues = new List<int>();
    List<BadGuy> deadThisRound = new List<BadGuy>();
    List<Item> lootBox = new List<Item>();
    int lootPurse = 0;
    int currentArrowPosition, endingCharacter;
    bool actionSelected, actionComplete, messageComplete, activateBattleLogComplete, inventoryComplete, deadCheckComplete, multipleCorpses, potionComplete, effectComplete, battleIsWon;
    bool unstrap = false;
    int totalBaddiesToKill = 0;
    int deadCount = 0;
    Effect priorityEffect;
    Color darkGrey = new Color(0.09411765f, 0.09411765f, 0.09411765f);
    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
    AudioSource currentTheme, returnTheme;
    IEnumerator egoHP, badGuy0HP, badGuy1HP, badGuy2HP, badGuy3HP, badGuy4HP;

    GameController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
        currentArrowPosition = 0;
        for (int i = 0; i < turnOrderNames.Length; i++)
        {
            turnOrderNames[i].text = "";
            turnOrderActions[i].text = "";
        }
        effectsText.text = "";
        deadCheckComplete = false;

        //testing purposes
        //InitiateCombat(testBadGuy, testNumber);
        //ego.defeatedBadGuys.Add(allBadGuys[1]);
        //ego.defeatedBadGuys.Add(allBadGuys[2]);
        //ego.defeatedBadGuys.Add(allBadGuys[3]);
    }

    // Stat elements in character arrays
    //
    // CurrentHP = allStats[0]
    // TargetHP = allStats[1]
    // MaxHP = allStats[2]
    // ArmorClass = allStats[3]
    // CritResist = allStats[4]
    // DamageReduction = allStats[5]
    // DamageDie = allStats[6]
    // Damage = allStats[7]
    // CritMultiplier = allStats[8]
    // ToHitMod = allStats[9]
    // InitMod = allStats[10]

    //Effect elements in element array
    //
    //Bolstered = allEffects[0]
    //Death = allEffects[1]
    //Exposed = allEffects[2]
    //Flanked = allEffects[3]
    //Guarded = allEffects[4]
    //Shaken = allEffects[5]
    //Subdued = allEffects[6]

    //BadGuy elements in badguy array
    //
    //A Small, Sickly Child = allBadGuys[0]
    //The Creeper in the Dark = allBadGuys[1]
    //An Aggressive Peace Monger = allBadGuys[2]
    //A Bold Yet Discretionary Strategist = allBadGuys[3]
    //A Big Burly Brute = allBadGuys[4]

    public IEnumerator BeginBattle(BadGuy badGuy, int numberOfBadGuys)
    {
        if (numberOfBadGuys == 1) { battleStartText.text = badGuy.battleCry; }
        else { battleStartText.text = badGuy.multiBattleCry; }
        battleStartText.maxVisibleCharacters = 0;
        returnTheme = controller.roomNavigation.currentMusic;

        StartCoroutine(controller.roomNavigation.FadeAudioOut(returnTheme, .25f));
        battleStartFadeToBlack.SetActive(true);
        yield return new WaitForSeconds(.98f);
        battleStartBox.SetActive(true);
        yield return new WaitForSeconds(.01f);
        int totalVisibleCharacters = battleStartText.textInfo.characterCount;
        int counter = 0;
        yield return new WaitForSeconds(.01f);
        battleStartDrum.Play();
        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            battleStartText.maxVisibleCharacters = visibleCount;
            if (Input.GetKey(KeyCode.Return))
            {
                battleStartText.maxVisibleCharacters = totalVisibleCharacters;
                visibleCount = totalVisibleCharacters;
                counter = totalVisibleCharacters;
            }

            if (visibleCount >= totalVisibleCharacters) { break; }
            counter += 1;

            yield return new WaitForSeconds(.0125f);
        }
        yield return new WaitForSeconds(.25f);
        battleStartContinueArrow.SetActive(true);
        combatScreen.SetActive(true);
        yield return new WaitUntil(controller.EnterPressed);
        InitiateCombat(badGuy, numberOfBadGuys);
        battleStartText.text = "";
        battleStartBox.SetActive(false);
        battleStartContinueArrow.SetActive(false);
        battleStartFadeFromBlack.SetActive(true);
        battleStartFadeToBlack.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        battleStartFadeFromBlack.SetActive(false);
    }
    public void InitiateCombat(BadGuy badGuy, int numberOfBadGuys)
    {
        battleIsWon = false;
        totalBaddiesToKill = numberOfBadGuys;
        deadCount = 0;
        FindBadGuyTheme();
        StartCoroutine(controller.roomNavigation.FadeAudioIn(currentTheme, .5f));
        curHPEgo.text = ego.allStats[0].value.ToString();
        maxHPEgo.text = ego.allStats[2].value.ToString();
        //reset previous battle gameobjects
        turnOrderBlackScreen.SetActive(false);
        fightOverFadedScreen.SetActive(false);
        WhiteWash();
        //clear loot list
        lootBox.Clear();
        lootPurse = 0;
        multipleCorpses = true;
        if (numberOfBadGuys == 1) { multipleCorpses = false; }

        //populate badguy array
        for (int i = 0; i < numberOfBadGuys; i++)
        {
            activeBadGuys[i] = Instantiate(badGuy);
            for (int j = 0; j < activeBadGuys[i].allStats.Length; j++)
            {
                activeBadGuys[i].allStats[j] = Instantiate(activeBadGuys[i].allStats[j]);
            }
        }
        for (int j = numberOfBadGuys; j < 4; j++) { activeBadGuys[j] = null; }
        //bs for bs coroutine stuff
        egoHP = HPRoll(ego);
        if (activeBadGuys[0] != null) { badGuy0HP = HPRoll(activeBadGuys[0]); }
        if (activeBadGuys[1] != null) { badGuy1HP = HPRoll(activeBadGuys[1]); }
        if (activeBadGuys[2] != null) { badGuy2HP = HPRoll(activeBadGuys[2]); }
        if (activeBadGuys[3] != null) { badGuy3HP = HPRoll(activeBadGuys[3]); }
        if (activeBadGuys[4] != null) { badGuy4HP = HPRoll(activeBadGuys[4]); }

        //determine proper badguy UI layout
        if (numberOfBadGuys != 2)
        {
            slot1.SetActive(true);
            activeBadGuys[0].combatSlot = slot1;
            activeBadGuys[0].combatBorder = border1;
            curHP1.text = activeBadGuys[0].allStats[0].value.ToString();
            maxHP1.text = activeBadGuys[0].allStats[2].value.ToString();
            name1.text = activeBadGuys[0].nome;
            special1.text = activeBadGuys[0].specialAbility;
        }
        else
        {
            slot2a.SetActive(true);
            activeBadGuys[0].combatSlot = slot2a;
            activeBadGuys[0].combatBorder = border2a;
            curHP2a.text = activeBadGuys[0].allStats[0].value.ToString();
            maxHP2a.text = activeBadGuys[0].allStats[2].value.ToString();
            name2a.text = activeBadGuys[0].nome;
            special2a.text = activeBadGuys[0].specialAbility;
            slot2b.SetActive(true);
            activeBadGuys[1].combatSlot = slot2b;
            activeBadGuys[1].combatBorder = border2b;
            curHP2b.text = activeBadGuys[1].allStats[0].value.ToString();
            maxHP2b.text = activeBadGuys[1].allStats[2].value.ToString();
            name2b.text = activeBadGuys[1].nome;
            special2b.text = activeBadGuys[1].specialAbility;
        }
        if (numberOfBadGuys > 2)
        {
            slot3b.SetActive(true);
            activeBadGuys[1].combatSlot = slot3b;
            activeBadGuys[1].combatBorder = border3b;
            curHP3b.text = activeBadGuys[1].allStats[0].value.ToString();
            maxHP3b.text = activeBadGuys[1].allStats[2].value.ToString();
            name3b.text = activeBadGuys[1].nome;
            special3b.text = activeBadGuys[1].specialAbility;
            slot3c.SetActive(true);
            activeBadGuys[2].combatSlot = slot3c;
            activeBadGuys[2].combatBorder = border3c;
            curHP3c.text = activeBadGuys[2].allStats[0].value.ToString();
            maxHP3c.text = activeBadGuys[2].allStats[2].value.ToString();
            name3c.text = activeBadGuys[2].nome;
            special3c.text = activeBadGuys[2].specialAbility;
        }
        if (numberOfBadGuys > 3)
        {
            slotFlank1.SetActive(true);
            activeBadGuys[3].combatSlot = slotFlank1;
            activeBadGuys[3].combatBorder = borderFlank1;
            curHPFlank1.text = activeBadGuys[3].allStats[0].value.ToString();
            maxHPFlank1.text = activeBadGuys[3].allStats[2].value.ToString();
            nameFlank1.text = activeBadGuys[3].nome;
            specialFlank1.text = activeBadGuys[3].specialAbility;
            Effect flank = Instantiate(allEffects[3]);
            AddEffect(ego, 1, flank);
        }
        if (numberOfBadGuys > 4)
        {
            slotFlank2.SetActive(true);
            activeBadGuys[4].combatSlot = slotFlank2;
            activeBadGuys[4].combatBorder = borderFlank2;
            curHPFlank2.text = activeBadGuys[4].allStats[0].value.ToString();
            maxHPFlank2.text = activeBadGuys[4].allStats[2].value.ToString();
            nameFlank2.text = activeBadGuys[4].nome;
            specialFlank2.text = activeBadGuys[4].specialAbility;
            Effect flank = Instantiate(allEffects[3]);
            AddEffect(ego, 1, flank);
        }



        CalculateTurnOrder();
        DisplayTurnOrder();
        StartCoroutine(TurnDistributor());


        void FindBadGuyTheme()
        {
            if (badGuy.nome == "The Creeper in the Dark") { currentTheme = creeperMusic; }
            else if (badGuy.nome == "A Small, Sickly Child") { currentTheme = sicklyMusic; }
            else if (badGuy.nome == "An Aggressive Peace Monger") { currentTheme = mongerMusic; }
            else if (badGuy.nome == "A Bold Yet Discretionary Strategist") { currentTheme = strategistMusic; }
            else if (badGuy.nome == "A Big Burly Brute") { currentTheme = bruteMusic; }
        }
    }
    void CalculateTurnOrder()
    {
        usedInitValues.Clear();
        //ego roll
        ego.currentInit = Random.Range(0, 21) + (int)ego.allStats[10].value;
        usedInitValues.Add(ego.currentInit);

        //badguy rolls
        for (int i = 0; i < activeBadGuys.Length; i++) { if (activeBadGuys[i] != null) { activeBadGuys[i].currentInit = GenerateUniqueRoll(activeBadGuys[i]); } }
        
        int GenerateUniqueRoll(BadGuy badGuy)
        {
            int roll = Random.Range(1, 21) + (int)badGuy.allStats[10].value;
            while (usedInitValues.Contains(roll)) { roll--; }
            usedInitValues.Add(roll);
            return roll;
        }
    }
    void DisplayTurnOrder()
    {
        List<int> transientInitValues = new List<int>();

        //create transient init list
        for (int i = 0; i < usedInitValues.Count; i++) { transientInitValues.Add(usedInitValues[i]); }

        //erase turn order
        for (int i = 0; i < turnOrder.Length; i++) { turnOrder[i] = null; }
        for (int i = 0; i < turnOrderNames.Length; i++) { turnOrderNames[i].text = ""; }
        for (int i = 0; i < activeBadGuys.Length; i++) { if (activeBadGuys[i] != null) { activeBadGuys[i].displayAction = ""; } }
        ego.displayAction = "";

        //Alternate Defend/Maneuver if applicable
        bool flanked = false;
        for (int i = 0; i < ego.activeEffects.Count; i++)
        {
            if (ego.activeEffects[i].title == "Flanked") { flanked = true; }
        }
        if (flanked) { egoCombatOptions[1].text = "Maneuver"; }
        else { egoCombatOptions[1].text = "Defend"; }

        //populate order array & display names
        for (int i = 0; i < turnOrder.Length; i++)
        {
            int nextHighest = 0;
            if (transientInitValues.Count > 1) { nextHighest = transientInitValues.Max(); }
            else if (transientInitValues.Count == 1) { nextHighest = transientInitValues[0]; }
            else { break; }
            
            if (ego.currentInit == nextHighest)
            {
                turnOrder[i] = ego;
                turnOrderNames[i].text = "You";
                ego.currentTurnOrder = i;
                for (int k = 0; k < transientInitValues.Count; k++) { if (nextHighest == transientInitValues[k]) { transientInitValues.RemoveAt(transientInitValues.IndexOf(nextHighest)); } }
            }
            else
            {
                for (int j = 0; j < activeBadGuys.Length; j++)
                {
                    if (activeBadGuys[j] != null)
                    {
                        if (activeBadGuys[j].currentInit == nextHighest)
                        {
                            turnOrder[i] = activeBadGuys[j];
                            turnOrderNames[i].text = activeBadGuys[j].nome;
                            activeBadGuys[j].currentTurnOrder = i;
                            for (int k = 0; k < transientInitValues.Count; k++) { if (nextHighest == transientInitValues[k]) { transientInitValues.RemoveAt(transientInitValues.IndexOf(nextHighest)); } }
                        }
                    }
                }
            }
        }
    }
    IEnumerator TurnDistributor()
    {
        for (int i = 0; i < turnOrder.Length; i++)
        {
            StartCoroutine(CheckTheDead());
            yield return new WaitUntil(DeadCheckComplete);
            yield return new WaitForSeconds(.01f);
            deadCheckComplete = false;
            actionSelected = false;
            if (deadThisRound.Contains(turnOrder[i])) { actionSelected = true; }
            else if (activeBadGuys[0] != null && turnOrder[i] == activeBadGuys[0])
            {
                activeBadGuys[0].chosenAction = BadGuyLogic(activeBadGuys[0]);
                StartCoroutine(BadGuyActionSelect(activeBadGuys[0]));
            }
            else if (activeBadGuys[1] != null && turnOrder[i] == activeBadGuys[1])
            {
                activeBadGuys[1].chosenAction = BadGuyLogic(activeBadGuys[1]);
                StartCoroutine(BadGuyActionSelect(activeBadGuys[1]));
            }
            else if (activeBadGuys[2] != null && turnOrder[i] == activeBadGuys[2])
            {
                activeBadGuys[2].chosenAction = BadGuyLogic(activeBadGuys[2]);
                StartCoroutine(BadGuyActionSelect(activeBadGuys[2]));
            }
            else if (activeBadGuys[3] != null && turnOrder[i] == activeBadGuys[3])
            {
                activeBadGuys[3].chosenAction = BadGuyLogic(activeBadGuys[3]);
                StartCoroutine(BadGuyActionSelect(activeBadGuys[3]));
            }
            else if (activeBadGuys[4] != null && turnOrder[i] == activeBadGuys[4])
            {
                activeBadGuys[4].chosenAction = BadGuyLogic(activeBadGuys[4]);
                StartCoroutine(BadGuyActionSelect(activeBadGuys[4]));
            }
            else if (turnOrder[i] == ego) { StartCoroutine(EgoActionSelect()); }
            else { actionSelected = true; }

            yield return new WaitUntil(ActionSelected);
            //Delay
            if (ego.chosenAction == "Delay")
            {
                ego.chosenAction = "";
                turnOrderBlackScreen.SetActive(true);
                yield return new WaitForSeconds(.5f);
                RedistributeTurnOrder(i);
                i--;
                yield return new WaitForSeconds(.5f);
                turnOrderBlackScreen.SetActive(false);
            }
        }
        yield return new WaitForSeconds(.2f);
        StartCoroutine(ExecuteActions());

        //If Delay is used
        void RedistributeTurnOrder(int currentTurn)
        {
            turnOrder[currentTurn] = turnOrder[currentTurn + 1];
            turnOrder[currentTurn].currentTurnOrder--;
            turnOrderNames[currentTurn].text = turnOrder[currentTurn].nome;

            turnOrder[currentTurn + 1] = ego;
            ego.currentTurnOrder++;
            turnOrderNames[currentTurn + 1].text = "You";
        }
    }
    IEnumerator ExecuteActions()
    {
        if (!battleIsWon)
        {
            for (int i = 0; i < turnOrder.Length; i++)
            {
                //lower spell effect duration at turn order i
                for (int j = 0; j < ego.activeEffects.Count; j++)
                {
                    if (ego.activeEffects[j].turnOrderTick == i)
                    {
                        ego.activeEffects[j].duration--;
                        if (ego.activeEffects[j].compounding && ego.activeEffects[j].priorityEffect == null)
                        {
                            if (ego.activeEffects[j].beneficial) { goodInstant.Play(); }
                            else { badInstant.Play(); }
                            if (ego.activeEffects[j].allStatsNumber != 1) { ego.allStats[ego.activeEffects[j].allStatsNumber].effectValue += ego.activeEffects[j].potency; }
                            else
                            {
                                ego.allStats[ego.activeEffects[j].allStatsNumber].value += ego.activeEffects[j].potency;
                                ActivateHPRoll(ego);
                            }
                            //if second effect, otherwise it is set to -1
                            if (ego.activeEffects[j].allStatsNumber2 != -1) { ego.allStats[ego.activeEffects[j].allStatsNumber2].effectValue += ego.activeEffects[j].potency2; }
                            activateBattleLogComplete = false;
                            StartCoroutine(ActivateBattleLog($"{ego.nome} {ego.activeEffects[j].compoundMessage}", null, 0, 1f));
                            yield return new WaitUntil(ActivateBattleLogComplete);
                            activateBattleLogComplete = false;
                        }
                    }
                    //Must be last! Because it removes it
                    if (ego.activeEffects[j].duration == 0) { RemoveEffect(ego, ego.activeEffects[j]); }
                }
                for (int j = 0; j < activeBadGuys.Length; j++)
                {
                    if (activeBadGuys[j] != null)
                    {
                        for (int k = 0; k < activeBadGuys[j].activeEffects.Count; k++)
                        {
                            if (activeBadGuys[j].activeEffects[k].turnOrderTick == i)
                            {
                                activeBadGuys[j].activeEffects[k].duration--;
                                if (activeBadGuys[j].activeEffects[k].compounding && activeBadGuys[j].activeEffects[k].priorityEffect == null)
                                {
                                    if (activeBadGuys[j].activeEffects[k].beneficial) { goodInstant.Play(); }
                                    else { badInstant.Play(); }
                                    if (activeBadGuys[j].activeEffects[k].allStatsNumber != 1) { activeBadGuys[j].allStats[activeBadGuys[j].activeEffects[k].allStatsNumber].effectValue += activeBadGuys[j].activeEffects[k].potency; }
                                    else
                                    {
                                        activeBadGuys[j].allStats[activeBadGuys[j].activeEffects[k].allStatsNumber].value += activeBadGuys[j].activeEffects[k].potency;
                                        ActivateHPRoll(activeBadGuys[j]);
                                    }
                                    //if second effect, otherwise it is set to -1
                                    if (activeBadGuys[j].activeEffects[k].allStatsNumber2 != -1) { activeBadGuys[j].allStats[activeBadGuys[j].activeEffects[k].allStatsNumber2].effectValue += activeBadGuys[j].activeEffects[k].potency2; }
                                    activateBattleLogComplete = false;
                                    StartCoroutine(ActivateBattleLog($"{activeBadGuys[j].nome} {activeBadGuys[j].activeEffects[k].compoundMessage2}", null, 0, 1f));
                                    yield return new WaitUntil(ActivateBattleLogComplete);
                                    activateBattleLogComplete = false;
                                }
                            }
                            //Must be last! Because it removes it
                            if (activeBadGuys[j].activeEffects[k].duration == 0) { RemoveEffect(activeBadGuys[j], activeBadGuys[j].activeEffects[k]); }
                        }
                    }
                }
                //commence turn
                StartCoroutine(CheckTheDead());
                yield return new WaitUntil(DeadCheckComplete);
                yield return new WaitForSeconds(.01f);
                deadCheckComplete = false;
                if (deadThisRound.Contains(turnOrder[i])) { continue; }
                else if (turnOrder[i] != null)
                {
                    //remove guarded on own turn
                    for (int j = 0; j < turnOrder[i].activeEffects.Count; j++)
                    {
                        if (turnOrder[i].activeEffects[j].title == "Guarded") { RemoveEffect(turnOrder[i], turnOrder[i].activeEffects[j]); }
                    }
                    //short break (in-game time)
                    yield return new WaitForSeconds(.5f);
                    if (turnOrder[i] == ego)
                    {
                        if (deadThisRound.Contains(ego.chosenTarget))
                        {
                            List<BadGuy> randomTarget = new List<BadGuy>();
                            for (int j = 0; j < activeBadGuys.Length; j++)
                            {
                                if (activeBadGuys[j] != null && !deadThisRound.Contains(activeBadGuys[j])) { randomTarget.Add(activeBadGuys[j]); }
                            }
                            if (randomTarget.Count > 0) { ego.chosenTarget = randomTarget[Random.Range(0, randomTarget.Count)]; }
                            else { battleIsWon = true; }
                        }
                        if (!battleIsWon)
                        {
                            egoTurn.Play();
                            StartCoroutine(SelectFlicker(borderEgo));
                            //Execute Attack
                            if (ego.chosenAction == "Attack") { StartCoroutine(ExecuteEgoAttack((BadGuy)ego.chosenTarget)); }
                            //Execute Defend
                            else if (ego.chosenAction == "Defend")
                            {
                                Effect guarded = Instantiate(allEffects[4]);
                                AddEffect(ego, i, guarded);
                                battleLog.SetActive(true);
                                battleText.text = "You take a defensive stance.";
                                yield return new WaitForSeconds(.01f);
                                messageComplete = false;
                                StartCoroutine(BattleMessage(0));
                                yield return new WaitUntil(MessageComplete);
                                yield return new WaitForSeconds(1.25f);
                                battleLogGreyScreen.SetActive(true);
                                yield return new WaitForSeconds(.5f);
                                battleLog.SetActive(false);
                                yield return new WaitForSeconds(.5f);
                                battleLogGreyScreen.SetActive(false);
                                actionComplete = true;
                            }
                            //Execute Maneuver
                            else if (ego.chosenAction == "Maneuver")
                            {
                                //check how much space there is to maneuver (0, 1, or 2 slots up top)
                                int availableSlots = 3;
                                for (int j = 0; j < activeBadGuys.Length; j++)
                                {
                                    if (activeBadGuys[j] != null) { availableSlots--; }
                                }
                                if (slotFlank1.activeInHierarchy) { availableSlots++; }
                                if (slotFlank2.activeInHierarchy) { availableSlots++; }

                                //check how many flanks there are
                                int totalFlanks = 0;
                                if (slotFlank1.activeInHierarchy) { totalFlanks++; }
                                if (slotFlank2.activeInHierarchy) { totalFlanks++; }

                                //consequences
                                string text1 = "You attempt to maneuver,";
                                string text2 = "";
                                AudioSource sound = blockEffect;

                                battleLog.SetActive(true);
                                battleText.text = text1;
                                yield return new WaitForSeconds(.01f);
                                messageComplete = false;
                                StartCoroutine(BattleMessage(0));
                                yield return new WaitUntil(MessageComplete);
                                yield return new WaitForSeconds(.5f);
                                //outcome based on availableSlots
                                if (availableSlots == 0) { text2 = "but there's just too many of them!"; }
                                else if (availableSlots == 1)
                                {
                                    RemoveFlank();
                                    sound = goodEffect;
                                    if (totalFlanks == 1) { text2 = "and deftly step out of your compromised position."; }
                                    else { text2 = "and manage to slip one of them."; }
                                }
                                else
                                {
                                    RemoveFlank();
                                    sound = goodEffect;
                                    text2 = "and deftly step out of your compromised position.";
                                    if (totalFlanks == 2) { RemoveFlank(); }
                                }
                                //second half of text
                                sound.Play();
                                battleText.text += " " + text2;
                                yield return new WaitForSeconds(.01f);
                                messageComplete = false;
                                StartCoroutine(BattleMessage(endingCharacter));
                                yield return new WaitUntil(MessageComplete);
                                yield return new WaitForSeconds(1.25f);
                                battleLogGreyScreen.SetActive(true);
                                yield return new WaitForSeconds(.5f);
                                battleLog.SetActive(false);
                                yield return new WaitForSeconds(.5f);
                                battleLogGreyScreen.SetActive(false);
                                actionComplete = true;
                            }
                            //Execute Equip
                            else if (ego.chosenAction == "Equip")
                            {
                                if (ego.chosenItem is Weapon)
                                {
                                    controller.GetEquipped((Weapon)ego.chosenItem);
                                    if (unstrap)
                                    {
                                        unstrap = false;
                                        controller.GetUnStrapped();
                                    }
                                    battleLog.SetActive(true);
                                    battleText.text = $"You equip the {myTI.ToTitleCase(ego.chosenItem.nome)}.";
                                    yield return new WaitForSeconds(.01f);
                                    messageComplete = false;
                                    StartCoroutine(BattleMessage(0));
                                    yield return new WaitUntil(MessageComplete);
                                    yield return new WaitForSeconds(1.25f);
                                    battleLogGreyScreen.SetActive(true);
                                    yield return new WaitForSeconds(.5f);
                                    battleLog.SetActive(false);
                                    yield return new WaitForSeconds(.5f);
                                    battleLogGreyScreen.SetActive(false);
                                }
                                else if (ego.chosenItem is Armor)
                                {
                                    controller.GetDressed((Armor)ego.chosenItem);
                                    battleLog.SetActive(true);
                                    battleText.text = $"You equip the {myTI.ToTitleCase(ego.chosenItem.nome)}.";
                                    yield return new WaitForSeconds(.01f);
                                    messageComplete = false;
                                    StartCoroutine(BattleMessage(0));
                                    yield return new WaitUntil(MessageComplete);
                                    yield return new WaitForSeconds(1.25f);
                                    battleLogGreyScreen.SetActive(true);
                                    yield return new WaitForSeconds(.5f);
                                    battleLog.SetActive(false);
                                    yield return new WaitForSeconds(.5f);
                                    battleLogGreyScreen.SetActive(false);
                                }
                                else if (ego.chosenItem is Shield)
                                {
                                    controller.GetStrapped((Shield)ego.chosenItem);
                                    battleLog.SetActive(true);
                                    battleText.text = $"You equip the {myTI.ToTitleCase(ego.chosenItem.nome)}.";
                                    yield return new WaitForSeconds(.01f);
                                    messageComplete = false;
                                    StartCoroutine(BattleMessage(0));
                                    yield return new WaitUntil(MessageComplete);
                                    yield return new WaitForSeconds(1.25f);
                                    battleLogGreyScreen.SetActive(true);
                                    yield return new WaitForSeconds(.5f);
                                    battleLog.SetActive(false);
                                    yield return new WaitForSeconds(.5f);
                                    battleLogGreyScreen.SetActive(false);
                                }
                                //equip shield with weapon (or fail to equip non shield)
                                if (ego.chosenItem2 != null)
                                {
                                    yield return new WaitForSeconds(.25f);
                                    battleLog.SetActive(true);
                                    if (ego.chosenItem2 is Shield)
                                    {
                                        controller.GetStrapped((Shield)ego.chosenItem2);
                                        battleText.text = $"You also equip the {myTI.ToTitleCase(ego.chosenItem2.nome)}.";
                                    }
                                    else
                                    {
                                        if (ego.chosenItem2 is Weapon) { battleText.text = "...and nice try."; }
                                        else if (ego.chosenItem2 is Potion)
                                        {
                                            controller.interactableItems.inventory.Remove(ego.chosenItem2);
                                            battleText.text = $"The {myTI.ToTitleCase(ego.chosenItem2.nome)} drops and breaks after you place it on your forearm!";
                                        }
                                        else { battleText.text = $"Getting the {myTI.ToTitleCase(ego.chosenItem2.nome)} onto your arm is awkward so you put it away."; }
                                    }
                                    yield return new WaitForSeconds(.01f);
                                    messageComplete = false;
                                    StartCoroutine(BattleMessage(0));
                                    yield return new WaitUntil(MessageComplete);
                                    yield return new WaitForSeconds(1.25f);
                                    battleLogGreyScreen.SetActive(true);
                                    yield return new WaitForSeconds(.5f);
                                    battleLog.SetActive(false);
                                    yield return new WaitForSeconds(.5f);
                                    battleLogGreyScreen.SetActive(false);
                                    ego.chosenItem2 = null;
                                }
                                actionComplete = true;
                            }
                            //Execute Use
                            else if (ego.chosenAction == "Use")
                            {
                                if (ego.chosenItem is Weapon || ego.chosenItem is Armor || ego.chosenItem is Shield)
                                {
                                    if (ego.chosenItem.useEffect != null)
                                    {
                                        battleLog.SetActive(true);
                                        battleText.text = $"You use the {myTI.ToTitleCase(ego.chosenItem.nome)}!";
                                        yield return new WaitForSeconds(.01f);
                                        messageComplete = false;
                                        StartCoroutine(BattleMessage(0));
                                        yield return new WaitUntil(MessageComplete);
                                        yield return new WaitForSeconds(.5f);
                                        battleText.text = ego.chosenItem.useMessage;
                                        yield return new WaitForSeconds(.01f);
                                        messageComplete = false;
                                        StartCoroutine(BattleMessage(0));
                                        yield return new WaitUntil(MessageComplete);
                                        if (ego.chosenItem.useMessage2 != null)
                                        {
                                            yield return new WaitForSeconds(.5f);
                                            if (ego.chosenTarget == ego) { battleText.text += $"You are {ego.chosenItem.useMessage2}"; }
                                            else { battleText.text += $"{ego.chosenTarget} is {ego.chosenItem.useMessage2}"; }
                                            yield return new WaitForSeconds(.01f);
                                            messageComplete = false;
                                            StartCoroutine(BattleMessage(endingCharacter));
                                            yield return new WaitUntil(MessageComplete);
                                        }
                                        ego.chosenTarget.activeEffects.Add(ego.chosenItem.useEffect);
                                        yield return new WaitForSeconds(1.25f);
                                        battleLogGreyScreen.SetActive(true);
                                        yield return new WaitForSeconds(.5f);
                                        battleLog.SetActive(false);
                                        yield return new WaitForSeconds(.5f);
                                        battleLogGreyScreen.SetActive(false);
                                    }
                                    else
                                    {
                                        battleLog.SetActive(true);
                                        battleText.text = $"You use the {myTI.ToTitleCase(ego.chosenItem.nome)}!";
                                        yield return new WaitForSeconds(.01f);
                                        messageComplete = false;
                                        StartCoroutine(BattleMessage(0));
                                        yield return new WaitUntil(MessageComplete);
                                        yield return new WaitForSeconds(.5f);
                                        battleText.text = ego.chosenItem.useMessage;
                                        yield return new WaitForSeconds(.01f);
                                        messageComplete = false;
                                        StartCoroutine(BattleMessage(0));
                                        yield return new WaitUntil(MessageComplete);
                                        if (ego.chosenItem.useMessage2 != null)
                                        {
                                            yield return new WaitForSeconds(.5f);
                                            battleText.text += ego.chosenItem.useMessage2;
                                            yield return new WaitForSeconds(.01f);
                                            messageComplete = false;
                                            StartCoroutine(BattleMessage(endingCharacter));
                                            yield return new WaitUntil(MessageComplete);
                                        }
                                        yield return new WaitForSeconds(1.25f);
                                        battleLogGreyScreen.SetActive(true);
                                        yield return new WaitForSeconds(.5f);
                                        battleLog.SetActive(false);
                                        yield return new WaitForSeconds(.5f);
                                        battleLogGreyScreen.SetActive(false);
                                    }
                                }
                                else if (ego.chosenItem is Undroppable)
                                {
                                    battleLog.SetActive(true);
                                    battleText.text = $"You use the {myTI.ToTitleCase(ego.chosenItem.nome)}!";
                                    yield return new WaitForSeconds(.01f);
                                    messageComplete = false;
                                    StartCoroutine(BattleMessage(0));
                                    yield return new WaitUntil(MessageComplete);
                                    battleText.text = "By safely putting it away.";
                                    yield return new WaitForSeconds(.01f);
                                    messageComplete = false;
                                    StartCoroutine(BattleMessage(0));
                                    yield return new WaitUntil(MessageComplete);
                                    yield return new WaitForSeconds(1.25f);
                                    battleLogGreyScreen.SetActive(true);
                                    yield return new WaitForSeconds(.5f);
                                    battleLog.SetActive(false);
                                    yield return new WaitForSeconds(.5f);
                                    battleLogGreyScreen.SetActive(false);
                                }
                                else if (ego.chosenItem is Potion)
                                {
                                    potionComplete = false;
                                    StartCoroutine(UsePotion(ego, (Potion)ego.chosenItem, ego.chosenTarget));
                                    yield return new WaitUntil(PotionComplete);
                                    potionComplete = false;
                                }
                                actionComplete = true;
                            }
                        }                        
                    }
                    else //if (turnOrder[i] != jesse)
                    {
                        BadGuy currentTurn = (BadGuy)turnOrder[i];
                        //disallow sound for Lurk ability
                        if (currentTurn.chosenAbility != null)
                        {
                            if (currentTurn.chosenAbility.title == "Lurk") { }
                            else { badGuyTurn.Play(); }
                        }
                        else { badGuyTurn.Play(); }
                        StartCoroutine(SelectFlicker(currentTurn.combatBorder));
                        if (turnOrder[i].displayAction == "Attack") { StartCoroutine(ExecuteBadGuyAttack((BadGuy)turnOrder[i])); }
                        else if (turnOrder[i].displayAction == "Inventory")
                        {
                            potionComplete = false;
                            StartCoroutine(UsePotion(currentTurn, currentTurn.potionBelt[0], currentTurn.chosenTarget));
                            yield return new WaitUntil(PotionComplete);
                            potionComplete = false;
                            actionComplete = true;
                        }
                        else
                        {
                            //if (turnOrder[i].displayAction == "Defend") { AddEffect(turnOrder[i], i, turnOrder[i].chosenAbility.effect); }
                            StartCoroutine(SpecialAbility(currentTurn));
                        }
                    }
                    yield return new WaitUntil(ActionComplete);
                    actionComplete = false;
                }
            }
            StartCoroutine(CheckTheDead());
            yield return new WaitUntil(DeadCheckComplete);
            yield return new WaitForSeconds(.01f);
            deadCheckComplete = false;
        }
        
        StartCoroutine(EndTurn());


        void RemoveFlank()
        {
            //check if 2a/2b need to be moved
            if (slot2a.activeInHierarchy || slot2b.activeInHierarchy)
            {
                GameObject newSlot = FindAvailableCombatSlot();
                BadGuy badGuyAB = null;
                if (slot2a.activeInHierarchy)
                {
                    for (int i = 0; i < activeBadGuys.Length; i++)
                    {
                        if (activeBadGuys[i] != null)
                        {
                            if (activeBadGuys[i].combatSlot == slot2a) { badGuyAB = activeBadGuys[i]; }
                        }
                    }
                }
                else if (slot2b.activeInHierarchy)
                {
                    for (int i = 0; i < activeBadGuys.Length; i++)
                    {
                        if (activeBadGuys[i] != null)
                        {
                            if (activeBadGuys[i].combatSlot == slot2b) { badGuyAB = activeBadGuys[i]; }
                        }
                    }
                }
                ShiftCombatSlot(badGuyAB, newSlot);
            }
            //find and remove the effect
            Effect removingFlank = null;
            for (int j = 0; j < ego.activeEffects.Count; j++)
            {
                if (ego.activeEffects[j].title == "Flanked")
                {
                    removingFlank = ego.activeEffects[j];
                    break;
                }
            }
            RemoveEffect(ego, removingFlank);

            //combatslot shift
            BadGuy movingBadGuy = null;
            for (int j = 0; j < activeBadGuys.Length; j++)
            {
                if (activeBadGuys[j] != null)
                {
                    if (activeBadGuys[j].combatSlot == slotFlank1)
                    {
                        movingBadGuy = activeBadGuys[j];
                        break;
                    }
                    else if (activeBadGuys[j].combatSlot == slotFlank2)
                    {
                        movingBadGuy = activeBadGuys[j];
                        break;
                    }
                }                
            }
            GameObject badGuyEffects = movingBadGuy.combatSlot.transform.Find("Effects").gameObject;
            TMP_Text badGuyEffectsText = badGuyEffects.transform.GetComponent<TMP_Text>();
            movingBadGuy.combatSlot.SetActive(false);
            if (!slot1.activeInHierarchy)
            {
                movingBadGuy.combatSlot = slot1;
                GameObject newBadGuyEffects = slot1.transform.Find("Effects").gameObject;
                TMP_Text newBadGuyEffectsText = newBadGuyEffects.transform.GetComponent<TMP_Text>();
                newBadGuyEffectsText.text = badGuyEffectsText.text;
                movingBadGuy.combatBorder = border1;
                curHP1.text = movingBadGuy.allStats[0].value.ToString();
                maxHP1.text = movingBadGuy.allStats[2].value.ToString();
                name1.text = movingBadGuy.nome;
                special1.text = movingBadGuy.specialAbility;
            }
            else if (!slot3b.activeInHierarchy)
            {
                movingBadGuy.combatSlot = slot3b;
                GameObject newBadGuyEffects = slot3b.transform.Find("Effects").gameObject;
                TMP_Text newBadGuyEffectsText = newBadGuyEffects.transform.GetComponent<TMP_Text>();
                newBadGuyEffectsText.text = badGuyEffectsText.text;
                movingBadGuy.combatBorder = border3b;
                curHP3b.text = movingBadGuy.allStats[0].value.ToString();
                maxHP3b.text = movingBadGuy.allStats[2].value.ToString();
                name3b.text = movingBadGuy.nome;
                special3b.text = movingBadGuy.specialAbility;
            }
            else if (!slot3c.activeInHierarchy)
            {
                movingBadGuy.combatSlot = slot3c;
                GameObject newBadGuyEffects = slot3c.transform.Find("Effects").gameObject;
                TMP_Text newBadGuyEffectsText = newBadGuyEffects.transform.GetComponent<TMP_Text>();
                newBadGuyEffectsText.text = badGuyEffectsText.text;
                movingBadGuy.combatBorder = border3c;
                curHP3c.text = movingBadGuy.allStats[0].value.ToString();
                maxHP3c.text = movingBadGuy.allStats[2].value.ToString();
                name3c.text = movingBadGuy.nome;
                special3c.text = movingBadGuy.specialAbility;
            }
            badGuyEffectsText.text = "";
            movingBadGuy.combatSlot.SetActive(true);
        }
    }
    IEnumerator ExecuteBadGuyAttack(BadGuy badGuy)
    {
        int badGuyToHit = (int)(badGuy.allStats[9].value + badGuy.allStats[9].effectValue);
        int badGuyDamageDie = (int)(badGuy.allStats[6].value + badGuy.allStats[6].effectValue);
        int badGuyDamage = (int)(badGuy.allStats[7].value + badGuy.allStats[7].effectValue);
        float badGuyCritMultiplier = badGuy.allStats[8].value + badGuy.allStats[8].effectValue;

        int egoArmorClass = (int)(ego.allStats[3].value + ego.allStats[3].effectValue);
        int egoDamageReduction = (int)(ego.allStats[5].value + ego.allStats[5].effectValue);
        float egoCritResist = ego.allStats[4].value + ego.allStats[4].effectValue;

        int d20 = Random.Range(1, 21);
        int attackRoll = d20 + badGuyToHit;
        int rolledDamage = 0;

        if (attackRoll >= egoArmorClass && (d20 < 20))
        {
            rolledDamage = Random.Range(1, badGuyDamageDie) + badGuyDamage - egoDamageReduction;
            if (rolledDamage < 0) { rolledDamage = 0; }
        }
        else if (d20 == 20)
        {
            rolledDamage = (int)(Mathf.Round(((badGuyCritMultiplier * (Random.Range(1, badGuyDamageDie) + badGuyDamage)) * egoCritResist) - egoDamageReduction));
            if (rolledDamage < 0) { rolledDamage = 0; }
        }

        battleLog.SetActive(true);
        string message = "";
        for (int i = 0; i < badGuy.normalAIRay.Length; i++)
        {
            if (badGuy.normalAIRay[i].title == "Attack") { message = badGuy.normalAIRay[i].messages[0];}
        }
        battleText.text = message;
        yield return new WaitForSeconds(.01f);
        messageComplete = false;
        StartCoroutine(BattleMessage(0));
        yield return new WaitUntil(MessageComplete);
        yield return new WaitForSeconds(.5f);

        if (attackRoll >= egoArmorClass && (d20 < 20))
        {
            hit.Play();
            if (message[message.Length -1] == '!') { battleText.text += " A"; }
            else if (message[message.Length - 1] == ',') { battleText.text += " a"; }
            battleText.text += $"nd hits for {rolledDamage} damage!";

            ego.allStats[1].value = ego.allStats[1].value - rolledDamage;
            ActivateHPRoll(ego);
        }
        else if (d20 == 20)
        {
            criticalHit.Play();
            if (message[message.Length - 1] == '!') { battleText.text += " A"; }
            else if (message[message.Length - 1] == ',') { battleText.text += " a"; }
            battleText.text += $"nd critically hits for {rolledDamage} damage!";

            ego.allStats[1].value = ego.allStats[1].value - rolledDamage;
            ActivateHPRoll(ego);
        }
        else if (attackRoll == (egoArmorClass -1))
        {
            miss.Play();
            if (message[message.Length - 1] == '!') { battleText.text += " Just missed!"; }
            else if (message[message.Length - 1] == ',') { battleText.text += " and just missed!"; }
        }
        else
        {
            miss.Play();
            if (message[message.Length - 1] == '!') { battleText.text += " B"; }
            else if (message[message.Length - 1] == ',') { battleText.text += " b"; }
            battleText.text += "ut misses!";
        }
        yield return new WaitForSeconds(.01f);
        messageComplete = false;
        StartCoroutine(BattleMessage(endingCharacter));
        yield return new WaitUntil(MessageComplete);
        yield return new WaitForSeconds(.75f);
        battleLogGreyScreen.SetActive(true);
        yield return new WaitForSeconds(.5f);
        battleLog.SetActive(false);
        yield return new WaitForSeconds(.5f);
        battleLogGreyScreen.SetActive(false);
        actionComplete = true;
    }
    IEnumerator UsePotion(Character user, Potion potion, Character target)
    {
        string verb = "drink";
        string s = "";
        if (!potion.beneficial) { verb = "throw"; }
        if (user is BadGuy) { s = "s"; }
        //remove potion from belt
        for (int i = 0; i < user.potionBelt.Count; i++)
        {
            if (potion.nome == user.potionBelt[i].nome)
            {
                user.potionBelt.Remove(user.potionBelt[i]);
                break;
            }
        }
        //make sure target isn't null (certain cases like badguy using beneficial has no target assigned)
        if (target == null) { target = user; }

        //determine proper wording
        battleLog.SetActive(true);
        //odd case: user gives other beneficial
        if (potion.beneficial && user != target) { battleText.text = $"{user.nome} take{s} out a {myTI.ToTitleCase(potion.nome)} and give{s} it to {target.nome}. "; }
        //odd case: user uses detrimental on self
        else if (!potion.beneficial && user == target)
        {
            if (user is Ego) { battleText.text = $"You take out a {myTI.ToTitleCase(potion.nome)} and break it over your head. "; }
            else { battleText.text = $"{user.nome} takes out a {myTI.ToTitleCase(potion.nome)} and breaks it over his head. "; }
        }
        //regular case
        else { battleText.text = $"{user.nome} take{s} out a {myTI.ToTitleCase(potion.nome)} and {verb}{s} it. "; }
        
        yield return new WaitForSeconds(.01f);
        messageComplete = false;
        StartCoroutine(BattleMessage(0));
        yield return new WaitUntil(MessageComplete);
        yield return new WaitForSeconds(.5f);
        //potion effect
        effectComplete = false;
        AddEffect(target, user.currentTurnOrder, potion.useEffect);
        yield return new WaitUntil(EffectComplete);
        effectComplete = false;
        //battle text pt2
        //case: overheal
        if (potion.useEffect.allStatsNumber == 1 && priorityEffect == null)
        {
            if (user.allStats[1].value >= (user.allStats[2].value + user.allStats[2].effectValue)) { user.allStats[1].value = user.allStats[2].value + user.allStats[2].effectValue; }
            if (user.allStats[1].value == (user.allStats[2].value + user.allStats[2].effectValue))
            {
                if (target is Ego) { battleText.text += "Your HP are maxed out!"; }
                else { battleText.text += $"{user.nome}'s HP are maxed out!"; }
            }
            //same as regular case
            else
            {
                if (target is Ego) { battleText.text += $"{target.nome} {potion.useMessage}"; }
                else { battleText.text += $"{target.nome} {potion.useMessage2}"; }
            }
        }
        //case: effect delayed
        else if (priorityEffect != null)
        {
            priorityEffect = null;
            battleText.text += $"\n\nThe influence of the {myTI.ToTitleCase(potion.nome)} is suppressed by a more powerful effect.";
        }
        //case: regular
        else
        {
            if (user is Ego) { battleText.text += $"{user.nome} {potion.useMessage}"; }
            else { battleText.text += $"{user.nome} {potion.useMessage2}"; }
        }
        yield return new WaitForSeconds(.01f);
        messageComplete = false;
        StartCoroutine(BattleMessage(endingCharacter));
        yield return new WaitUntil(MessageComplete);
        yield return new WaitForSeconds(1.25f);
        battleLogGreyScreen.SetActive(true);
        yield return new WaitForSeconds(.5f);
        battleLog.SetActive(false);
        yield return new WaitForSeconds(.5f);
        battleLogGreyScreen.SetActive(false);
        potionComplete = true;
    }
    IEnumerator SpecialAbility(BadGuy badGuy)
    {
        string messageOne = badGuy.chosenAbility.messages[0];
        string messageTwo = null;
        Character target = null;

        //assign target
        if (badGuy.chosenAbility.beneficial) { target = badGuy; }
        else { target = ego; }
        //most abilities will have a second message
        if (badGuy.chosenAbility.messages.Count > 1) { messageTwo = badGuy.chosenAbility.messages[1]; }

        //commence ability
        //special cases
        if (badGuy.chosenAbility.title == "Fall Down")
        {
            battleLog.SetActive(true);
            battleText.text = badGuy.normalAIRay[5].messages[0];
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(0));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(.5f);
            //next
            derpSound.Play();
            battleText.text += "\n\n" + badGuy.normalAIRay[5].messages[1];
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(battleText.textInfo.characterCount));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(1f);
            //next
            derpSound.Play();
            battleText.text += " " + badGuy.normalAIRay[5].messages[2];
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(battleText.textInfo.characterCount));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(1.5f);
            //next
            strangeOccurence.Play();
            battleText.text += " " + badGuy.normalAIRay[5].messages[3];
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(battleText.textInfo.characterCount));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(.5f);
            battleLogGreyScreen.SetActive(true);
            yield return new WaitForSeconds(.5f);
            battleLog.SetActive(false);
            yield return new WaitForSeconds(.5f);
            battleLogGreyScreen.SetActive(false);
        }
        else if (badGuy.chosenAbility.title == "Flank")
        {
            battleLog.SetActive(true);
            battleText.text = messageOne;
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(0));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(.5f);
            //second half with close
            AddEffect(target, badGuy.currentTurnOrder, badGuy.chosenAbility.effect);
            badEffect.Play();
            //combatslot shift
            GameObject badGuyEffects = badGuy.combatSlot.transform.Find("Effects").gameObject;
            TMP_Text badGuyEffectsText = badGuyEffects.transform.GetComponent<TMP_Text>();
            badGuy.combatSlot.SetActive(false);
            if (!slotFlank1.activeInHierarchy)
            {
                badGuy.combatSlot = slotFlank1;
                GameObject newBadGuyEffects = slotFlank1.transform.Find("Effects").gameObject;
                TMP_Text newBadGuyEffectsText = newBadGuyEffects.transform.GetComponent<TMP_Text>();
                newBadGuyEffectsText.text = badGuyEffectsText.text;
                badGuy.combatBorder = borderFlank1;
                curHPFlank1.text = badGuy.allStats[0].value.ToString();
                maxHPFlank1.text = badGuy.allStats[2].value.ToString();
                nameFlank1.text = badGuy.nome;
                specialFlank1.text = badGuy.specialAbility;
            }            
            else
            {
                badGuy.combatSlot = slotFlank2;
                GameObject newBadGuyEffects = slotFlank2.transform.Find("Effects").gameObject;
                TMP_Text newBadGuyEffectsText = newBadGuyEffects.transform.GetComponent<TMP_Text>();
                newBadGuyEffectsText.text = badGuyEffectsText.text;
                badGuy.combatBorder = borderFlank2;
                curHPFlank2.text = badGuy.allStats[0].value.ToString();
                maxHPFlank2.text = badGuy.allStats[2].value.ToString();
                nameFlank2.text = badGuy.nome;
                specialFlank2.text = badGuy.specialAbility;
            }
            badGuyEffectsText.text = "";
            badGuy.combatSlot.SetActive(true);
            //
            battleText.text += " " + messageTwo;
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(endingCharacter));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(1.25f);
            battleLogGreyScreen.SetActive(true);
            yield return new WaitForSeconds(.5f);
            battleLog.SetActive(false);
            yield return new WaitForSeconds(.5f);
            battleLogGreyScreen.SetActive(false);
        }
        else if (badGuy.chosenAbility.title == "Call")
        {
            battleLog.SetActive(true);
            battleText.text = $"{badGuy.nome} calls for help!";
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(0));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(.5f);
            //second half with close
            //roll help call
            int helpRoll = Random.Range(1, 101);
            int badGuyCount = 0;
            float maxRoll = 50f;
            for (int i = 0; i < activeBadGuys.Length; i++)
            {
                if (activeBadGuys[i] != null) { badGuyCount++; }
            }
            if (badGuyCount == 5) { maxRoll = 0; }
            else { maxRoll = maxRoll / badGuyCount; }
            //roll effect
            if (helpRoll <= maxRoll)
            {
                BadGuy newBadGuy = null;
                multipleCorpses = true;
                totalBaddiesToKill++;
                //create new badguy
                int callRoll = 0;
                if (ego.defeatedBadGuys.Count > 0)
                {
                    callRoll = Random.Range(0, ego.defeatedBadGuys.Count);
                    newBadGuy = Instantiate(ego.defeatedBadGuys[callRoll]);
                    int rareRoll = Random.Range(0, 128);
                    if (rareRoll == 128) { newBadGuy = Instantiate(allBadGuys[0]); }
                }
                else { newBadGuy = Instantiate(allBadGuys[0]); }
                for (int i = 0; i < newBadGuy.allStats.Length; i++)
                {
                    newBadGuy.allStats[i] = Instantiate(newBadGuy.allStats[i]);
                }
                //assign him a slot
                for (int i = 0; i < activeBadGuys.Length; i++)
                {
                    if (activeBadGuys[i] == null)
                    {
                        activeBadGuys[i] = newBadGuy;
                        if (i == 0) { badGuy0HP = HPRoll(activeBadGuys[0]); }
                        else if (i == 1) { badGuy1HP = HPRoll(activeBadGuys[1]); }
                        else if (i == 2) { badGuy2HP = HPRoll(activeBadGuys[2]); }
                        else if (i == 3) { badGuy3HP = HPRoll(activeBadGuys[3]); }
                        else if (i == 4) { badGuy4HP = HPRoll(activeBadGuys[4]); }
                        break;
                    }
                }
                //
                //work around slots 2a/2b
                bool skipAssignment = false;
                //if just 1 badguy in 2a/2b, assign new badguy to the other one
                if ((slot2a.activeInHierarchy || slot2b.activeInHierarchy) && badGuyCount == 1)
                {
                    skipAssignment = true;
                    if (slot2a.activeInHierarchy)
                    {
                        newBadGuy.combatSlot = slot2b;
                        newBadGuy.combatBorder = border2b;
                        curHP2b.text = newBadGuy.allStats[0].value.ToString();
                        maxHP2b.text = newBadGuy.allStats[2].value.ToString();
                        name2b.text = newBadGuy.nome;
                        special2b.text = newBadGuy.specialAbility;
                    }
                    else
                    {
                        newBadGuy.combatSlot = slot2a;
                        newBadGuy.combatBorder = border2a;
                        curHP2a.text = newBadGuy.allStats[0].value.ToString();
                        maxHP2a.text = newBadGuy.allStats[2].value.ToString();
                        name2a.text = newBadGuy.nome;
                        special2a.text = newBadGuy.specialAbility;
                    }                    
                }
                //if more than 1 badguy left, shift 2a/2b into 1-2-3 formation, and let the rest of the code run its course
                else if (slot2a.activeInHierarchy || slot2b.activeInHierarchy)
                {
                    BadGuy badGuySlot2a = null;
                    BadGuy badGuySlot2b = null;
                    for (int i = 0; i < activeBadGuys.Length; i++)
                    {
                        if (activeBadGuys[i].combatBorder == slot2a) { badGuySlot2a = activeBadGuys[i]; }
                        if (activeBadGuys[i].combatBorder == slot2b) { badGuySlot2b = activeBadGuys[i]; }
                    }
                    if (badGuySlot2a != null)
                    {
                        if (!slot1.activeInHierarchy)
                        {
                            badGuySlot2a.combatSlot = slot1;
                            effects1.text = effects2a.text;
                            badGuySlot2a.combatBorder = border1;
                            curHP1.text = badGuySlot2a.allStats[0].value.ToString();
                            maxHP1.text = badGuySlot2a.allStats[2].value.ToString();
                            name1.text = badGuySlot2a.nome;
                            special1.text = badGuySlot2a.specialAbility;
                        }
                        else if (!slot3b.activeInHierarchy)
                        {
                            badGuySlot2a.combatSlot = slot3b;
                            effects3b.text = effects2a.text;
                            badGuySlot2a.combatBorder = border3b;
                            curHP3b.text = badGuySlot2a.allStats[0].value.ToString();
                            maxHP3b.text = badGuySlot2a.allStats[2].value.ToString();
                            name3b.text = badGuySlot2a.nome;
                            special3b.text = badGuySlot2a.specialAbility;
                        }
                        else if (!slot3c.activeInHierarchy)
                        {
                            badGuySlot2a.combatSlot = slot3c;
                            effects3c.text = effects2a.text;
                            badGuySlot2a.combatBorder = border3c;
                            curHP3c.text = badGuySlot2a.allStats[0].value.ToString();
                            maxHP3c.text = badGuySlot2a.allStats[2].value.ToString();
                            name3c.text = badGuySlot2a.nome;
                            special3c.text = badGuySlot2a.specialAbility;
                        }
                        else if (!slotFlank1.activeInHierarchy)
                        {
                            badGuySlot2a.combatSlot = slotFlank1;
                            effectsFlank1.text = effects2a.text;
                            badGuySlot2a.combatBorder = borderFlank1;
                            curHPFlank1.text = badGuySlot2a.allStats[0].value.ToString();
                            maxHPFlank1.text = badGuySlot2a.allStats[2].value.ToString();
                            nameFlank1.text = badGuySlot2a.nome;
                            specialFlank1.text = badGuySlot2a.specialAbility;
                        }
                        else if (!slotFlank2.activeInHierarchy)
                        {
                            badGuySlot2a.combatSlot = slotFlank2;
                            effectsFlank2.text = effects2a.text;
                            badGuySlot2a.combatBorder = borderFlank2;
                            curHPFlank2.text = badGuySlot2a.allStats[0].value.ToString();
                            maxHPFlank2.text = badGuySlot2a.allStats[2].value.ToString();
                            nameFlank2.text = badGuySlot2a.nome;
                            specialFlank2.text = badGuySlot2a.specialAbility;
                        }
                        badGuySlot2a.combatSlot.SetActive(false);
                        effects2a.text = "";
                    }
                    if (badGuySlot2b != null)
                    {
                        if (!slot1.activeInHierarchy)
                        {
                            badGuySlot2b.combatSlot = slot1;
                            effects1.text = effects2b.text;
                            badGuySlot2b.combatBorder = border1;
                            curHP1.text = badGuySlot2b.allStats[0].value.ToString();
                            maxHP1.text = badGuySlot2b.allStats[2].value.ToString();
                            name1.text = badGuySlot2b.nome;
                            special1.text = badGuySlot2b.specialAbility;
                        }
                        else if (!slot3b.activeInHierarchy)
                        {
                            badGuySlot2b.combatSlot = slot3b;
                            effects3b.text = effects2b.text;
                            badGuySlot2b.combatBorder = border3b;
                            curHP3b.text = badGuySlot2b.allStats[0].value.ToString();
                            maxHP3b.text = badGuySlot2b.allStats[2].value.ToString();
                            name3b.text = badGuySlot2b.nome;
                            special3b.text = badGuySlot2b.specialAbility;
                        }
                        else if (!slot3c.activeInHierarchy)
                        {
                            badGuySlot2b.combatSlot = slot3c;
                            effects3c.text = effects2b.text;
                            badGuySlot2b.combatBorder = border3c;
                            curHP3c.text = badGuySlot2b.allStats[0].value.ToString();
                            maxHP3c.text = badGuySlot2b.allStats[2].value.ToString();
                            name3c.text = badGuySlot2b.nome;
                            special3c.text = badGuySlot2b.specialAbility;
                        }
                        else if (!slotFlank1.activeInHierarchy)
                        {
                            badGuySlot2b.combatSlot = slotFlank1;
                            effectsFlank1.text = effects2b.text;
                            badGuySlot2b.combatBorder = borderFlank1;
                            curHPFlank1.text = badGuySlot2b.allStats[0].value.ToString();
                            maxHPFlank1.text = badGuySlot2b.allStats[2].value.ToString();
                            nameFlank1.text = badGuySlot2b.nome;
                            specialFlank1.text = badGuySlot2b.specialAbility;
                        }
                        else if (!slotFlank2.activeInHierarchy)
                        {
                            badGuySlot2b.combatSlot = slotFlank2;
                            effects1.text = effects2b.text;
                            badGuySlot2b.combatBorder = borderFlank2;
                            curHPFlank2.text = badGuySlot2b.allStats[0].value.ToString();
                            maxHPFlank2.text = badGuySlot2b.allStats[2].value.ToString();
                            nameFlank2.text = badGuySlot2b.nome;
                            specialFlank2.text = badGuySlot2b.specialAbility;
                        }
                        badGuySlot2b.combatSlot.SetActive(false);
                        effects2b.text = "";
                    }
                }
                //
                //assign first open slot to new badguy
                if (skipAssignment) { skipAssignment = false; }
                else if (!slot1.activeInHierarchy)
                {
                    newBadGuy.combatSlot = slot1;
                    newBadGuy.combatBorder = border1;
                    curHP1.text = newBadGuy.allStats[0].value.ToString();
                    maxHP1.text = newBadGuy.allStats[2].value.ToString();
                    name1.text = newBadGuy.nome;
                    special1.text = newBadGuy.specialAbility;
                }
                else if (!slot3b.activeInHierarchy)
                {
                    newBadGuy.combatSlot = slot3b;
                    newBadGuy.combatBorder = border3b;
                    curHP3b.text = newBadGuy.allStats[0].value.ToString();
                    maxHP3b.text = newBadGuy.allStats[2].value.ToString();
                    name3b.text = newBadGuy.nome;
                    special3b.text = newBadGuy.specialAbility;
                }
                else if (!slot3c.activeInHierarchy)
                {
                    newBadGuy.combatSlot = slot3c;
                    newBadGuy.combatBorder = border3c;
                    curHP3c.text = newBadGuy.allStats[0].value.ToString();
                    maxHP3c.text = newBadGuy.allStats[2].value.ToString();
                    name3c.text = newBadGuy.nome;
                    special3c.text = newBadGuy.specialAbility;
                }
                else if (!slotFlank1.activeInHierarchy)
                {
                    newBadGuy.combatSlot = slotFlank1;
                    newBadGuy.combatBorder = borderFlank1;
                    curHPFlank1.text = newBadGuy.allStats[0].value.ToString();
                    maxHPFlank1.text = newBadGuy.allStats[2].value.ToString();
                    nameFlank1.text = newBadGuy.nome;
                    specialFlank1.text = newBadGuy.specialAbility;
                    Effect flank = Instantiate(allEffects[3]);
                    AddEffect(ego, 1, flank);
                }
                else if (!slotFlank2.activeInHierarchy)
                {
                    newBadGuy.combatSlot = slotFlank2;
                    newBadGuy.combatBorder = borderFlank2;
                    curHPFlank2.text = newBadGuy.allStats[0].value.ToString();
                    maxHPFlank2.text = newBadGuy.allStats[2].value.ToString();
                    nameFlank2.text = newBadGuy.nome;
                    specialFlank2.text = newBadGuy.specialAbility;
                    Effect flank = Instantiate(allEffects[3]);
                    AddEffect(ego, 1, flank);
                }
                newBadGuy.combatSlot.SetActive(true);
                joinedTheBattle.Play();
                battleText.text += $" {newBadGuy.nome} joined the battle.";
            }
            else
            {
                derpSound.Play();
                battleText.text += " But nobody came.";
            }
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(endingCharacter));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(1.25f);
            battleLogGreyScreen.SetActive(true);
            yield return new WaitForSeconds(.5f);
            battleLog.SetActive(false);
            yield return new WaitForSeconds(.5f);
            battleLogGreyScreen.SetActive(false);
        }
        //default case
        else
        {
            activateBattleLogComplete = false;
            StartCoroutine(ActivateBattleLog(messageOne, messageTwo, .5f, 1.25f, null, badGuy.chosenAbility.effect, target, badGuy));
            yield return new WaitUntil(ActivateBattleLogComplete);
            activateBattleLogComplete = false;
        }
        
        actionComplete = true;
    }
    void AddEffect(Character target, int turnOrderOfCaster, Effect effect)
    {
        //
        //DO NOT MODIFY NEW EFFECT UNTIL AFTER INSTANTIATED AND ADDED TO ACTIVEEFFECTS!!
        //
        string color = "white";
        if (effect.color == Color.green) { color = "green"; }
        else if (effect.color == Color.red) { color = "red"; }
        //check for priority override
        List<Effect> priorityEffectsCurrent = new List<Effect>();
        priorityEffect = null;
        if (effect.priorityLine != "None")
        {
            for (int i = 0; i < target.activeEffects.Count; i++)
            {
                if (effect.priorityLine == target.activeEffects[i].priorityLine)
                {
                    if (Mathf.Abs(target.activeEffects[i].potency) > Mathf.Abs(effect.potency)) { priorityEffectsCurrent.Add(target.activeEffects[i]); }
                    else
                    {
                        //check to see if new, more powerful effect will outlast the weaker one
                        if (target.activeEffects[i].duration <= effect.duration) { RemoveEffect(target, target.activeEffects[i]); }
                        else
                        {
                            DeactivateDelayedEffect(target, target.activeEffects[i]);
                            target.activeEffects[i].priorityEffect = effect;
                        }
                        
                    }
                }
            }
            if (priorityEffectsCurrent.Count == 1) { priorityEffect = priorityEffectsCurrent[0]; }
            else if (priorityEffectsCurrent.Count > 1)
            {
                priorityEffect = priorityEffectsCurrent[0];
                for (int i = 0; i < priorityEffectsCurrent.Count - 1; i++)
                {
                    if (priorityEffectsCurrent[i + 1].duration > priorityEffectsCurrent[i].duration) { priorityEffect = priorityEffectsCurrent[i + 1]; }
                }
            }            
        }
        //play correct sound
        if (effect.title == "Guarded" || effect.title == "Lurk") { }//no sound
        else if (effect.duration == -1)
        {
            if (effect.beneficial) { goodInstant.Play(); }
            else { badInstant.Play(); }
        }
        else if (priorityEffect != null) { blockEffect.Play(); }
        else
        {
            if (effect.beneficial) { goodEffect.Play(); }
            else { badEffect.Play(); }
        }
        //if duration is not instantaneous
        if (effect.duration != -1)
        {
            //write on screen
            if (target == ego) { effectsText.text += $"<color={color}>{effect.title}</color>\n"; }
            else
            {
                BadGuy badTarget = (BadGuy)target;
                GameObject badTargetEffects = badTarget.combatSlot.transform.Find("Effects").gameObject;
                TMP_Text badTargetEffectsText = badTargetEffects.transform.GetComponent<TMP_Text>();
                badTargetEffectsText.text += $"<color={color}>{effect.abbreviation}</color> ";
            }
            //add effect
            effect.turnOrderTick = turnOrderOfCaster;
            //New effect instantiated and can be modified hereafter in activeEffects
            target.activeEffects.Add(Instantiate(effect));
            //create delayed duration if overridden
            if (priorityEffect != null) { target.activeEffects[target.activeEffects.Count - 1].priorityEffect = priorityEffect; }
        }  
        //modify stats now only if no priority override
        if (priorityEffect == null)
        {
            //modify stats (instantaneous HP affects value (not effectValue))
            if (effect.allStatsNumber != 1) { target.allStats[effect.allStatsNumber].effectValue += effect.potency; }
            else
            {
                target.allStats[effect.allStatsNumber].value += effect.potency;
                ActivateHPRoll(target);
            }
            //if second effect, otherwise it is set to -1
            if (effect.allStatsNumber2 != -1) { target.allStats[effect.allStatsNumber2].effectValue += effect.potency2; }
        }
        effectComplete = true;
    }
    Effect FindPriorityEffect(Effect effect, Character target)
    {
        List<Effect> priorityEffectsCurrent = new List<Effect>();
        priorityEffect = null;
        if (effect.priorityLine != "None")
        {
            for (int i = 0; i < target.activeEffects.Count; i++)
            {
                if (effect.priorityLine == target.activeEffects[i].priorityLine)
                {
                    if (Mathf.Abs(target.activeEffects[i].potency) > Mathf.Abs(effect.potency)) { priorityEffectsCurrent.Add(target.activeEffects[i]); }                    
                }
            }
            if (priorityEffectsCurrent.Count == 1) { priorityEffect = priorityEffectsCurrent[0]; }
            else if (priorityEffectsCurrent.Count > 1)
            {
                priorityEffect = priorityEffectsCurrent[0];
                for (int i = 0; i < priorityEffectsCurrent.Count - 1; i++)
                {
                    if (priorityEffectsCurrent[i + 1].duration > priorityEffectsCurrent[i].duration) { priorityEffect = priorityEffectsCurrent[i + 1]; }
                }
            }
        }
        return priorityEffect;
    }
    void ActivateDelayedEffect(Character target, Effect effect)
    {
        //modify stats (elixirs get compounded with no initial effect here)
        if (effect.allStatsNumber != 1) { target.allStats[effect.allStatsNumber].effectValue += effect.potency; }
        //if second effect, otherwise it is set to -1
        if (effect.allStatsNumber2 != -1) { target.allStats[effect.allStatsNumber2].effectValue += effect.potency2; }
    }
    void DeactivateDelayedEffect(Character target, Effect effect)
    {
        //modify stats (no need to worry about instantaneous effects like activating)
        target.allStats[effect.allStatsNumber].effectValue -= effect.potency;
        if (effect.allStatsNumber2 != -1) { target.allStats[effect.allStatsNumber2].effectValue -= effect.potency2; }
    }
    void RemoveEffect(Character target, Effect effect)
    {
        string color = "white";
        if (effect.color == Color.green) { color = "green"; }
        else if (effect.color == Color.red) { color = "red"; }

        //remove text on screen
        if (target == ego)
        {
            string replaceText = effectsText.text.Remove(effectsText.text.IndexOf($"<color={color}>{effect.title}</color>\n"), $"<color={color}>{effect.title}</color>\n".Length);
            effectsText.text = replaceText;
        }
        else
        {
            BadGuy badTarget = (BadGuy)target;
            GameObject badTargetEffects = badTarget.combatSlot.transform.Find("Effects").gameObject;
            TMP_Text badTargetEffectsText = badTargetEffects.transform.GetComponent<TMP_Text>();
            Debug.Log(badTargetEffectsText.text);
            string replaceText = badTargetEffectsText.text.Remove(badTargetEffectsText.text.IndexOf($"<color={color}>{effect.abbreviation}</color> "), $"<color={color}>{effect.abbreviation}</color> ".Length);
            badTargetEffectsText.text = replaceText;
        }

        //modify stats
        target.allStats[effect.allStatsNumber].effectValue -= effect.potency;
        if (effect.allStatsNumber2 != -1) { target.allStats[effect.allStatsNumber2].effectValue -= effect.potency2; }


        //remove effect
        target.activeEffects.Remove(effect);

        //check if effect was blocking another, and assign new if necessary
        for (int i = 0; i < target.activeEffects.Count; i++)
        {
            if (target.activeEffects[i].priorityEffect == effect)
            {
                target.activeEffects[i].priorityEffect = FindPriorityEffect(target.activeEffects[i], target);
                if (target.activeEffects[i].priorityEffect == null) { ActivateDelayedEffect(target, target.activeEffects[i]); }
            }
        }
    }
    GameObject FindAvailableCombatSlot()
    {
        //will return earliest available slot 1, 3b, 3c, Flank1, Flank2 (no 2a/2b)
        if (!slot1.activeInHierarchy) { return slot1; }
        else if (!slot3b.activeInHierarchy) { return slot3b; }
        else if (!slot3c.activeInHierarchy) { return slot3c; }
        else if (!slotFlank1.activeInHierarchy) { return slotFlank1; }
        else if (!slotFlank2.activeInHierarchy) { return slotFlank2; }
        return null;
    }
    void ShiftCombatSlot(BadGuy movingBadGuy, GameObject newSlot)
    {
        //associate border and stats with new slot
        GameObject newBorder = null;
        TMP_Text newCurHP = null;
        TMP_Text newMaxHP = null;
        TMP_Text newName = null;
        TMP_Text newSpecial = null;
        if (newSlot == slot1)
        {
            newBorder = border1;
            newCurHP = curHP1;
            newMaxHP = maxHP1;
            newName = name1;
            newSpecial = special1;
        }
        else if (newSlot == slot2a)
        {
            newBorder = border2a;
            newCurHP = curHP2a;
            newMaxHP = maxHP2a;
            newName = name2a;
            newSpecial = special2a;
        }
        else if (newSlot == slot2b)
        {
            newBorder = border2b;
            newCurHP = curHP2b;
            newMaxHP = maxHP2b;
            newName = name2b;
            newSpecial = special2b;
        }
        else if (newSlot == slot3b)
        {
            newBorder = border3b;
            newCurHP = curHP3b;
            newMaxHP = maxHP3b;
            newName = name3b;
            newSpecial = special3b;
        }
        else if (newSlot == slot3c)
        {
            newBorder = border3c;
            newCurHP = curHP3c;
            newMaxHP = maxHP3c;
            newName = name3c;
            newSpecial = special3c;
        }
        else if (newSlot == slotFlank1)
        {
            newBorder = borderFlank1;
            newCurHP = curHPFlank1;
            newMaxHP = maxHPFlank1;
            newName = nameFlank1;
            newSpecial = specialFlank1;
        }
        else if (newSlot == slotFlank2)
        {
            newBorder = borderFlank2;
            newCurHP = curHPFlank2;
            newMaxHP = maxHPFlank2;
            newName = nameFlank2;
            newSpecial = specialFlank2;
        }

        //copy effects text
        GameObject badGuyEffects = movingBadGuy.combatSlot.transform.Find("Effects").gameObject;
        TMP_Text badGuyEffectsText = badGuyEffects.transform.GetComponent<TMP_Text>();
        //shut off old slot
        movingBadGuy.combatSlot.SetActive(false);
        //assign new slot
        movingBadGuy.combatSlot = newSlot;
        //transcribe effects text and erase old
        GameObject movedBadGuyEffects = movingBadGuy.combatSlot.transform.Find("Effects").gameObject;
        TMP_Text movedBadGuyEffectsText = movedBadGuyEffects.transform.GetComponent<TMP_Text>();
        movedBadGuyEffectsText.text = badGuyEffectsText.text;
        badGuyEffectsText.text = "";
        //assign new border
        movingBadGuy.combatBorder = newBorder;
        newCurHP.text = movingBadGuy.allStats[0].value.ToString();
        newMaxHP.text = movingBadGuy.allStats[2].value.ToString();
        newName.text = movingBadGuy.nome;
        newSpecial.text = movingBadGuy.specialAbility;
        //turn on new slot
        movingBadGuy.combatSlot.SetActive(true);
    }
    IEnumerator ExecuteEgoAttack(BadGuy badGuy)
    {
        int egoToHit = (int)(ego.allStats[9].value + ego.allStats[9].effectValue);
        int egoDamageDie = (int)(ego.allStats[6].value + ego.allStats[6].effectValue);
        int egoDamage = (int)(ego.allStats[7].value + ego.allStats[7].effectValue);
        float egoCritMultiplier = ego.allStats[8].value + ego.allStats[8].effectValue;

        int badGuyArmorClass = (int)(badGuy.allStats[3].value + badGuy.allStats[3].effectValue);
        int badGuyDamageReduction = (int)(badGuy.allStats[5].value + badGuy.allStats[5].effectValue);
        float badGuyCritResist = badGuy.allStats[4].value + badGuy.allStats[4].effectValue;

        int d20 = Random.Range(1, 21);
        int attackRoll = d20 + egoToHit;
        int rolledDamage = 0;

        if (attackRoll >= badGuyArmorClass && (d20 < 20))
        {
            rolledDamage = Random.Range(1, egoDamageDie) + egoDamage - badGuyDamageReduction;
            if (rolledDamage < 0) { rolledDamage = 0; }
        }
        else if (d20 == 20)
        {
            rolledDamage = (int)(Mathf.Round(((egoCritMultiplier * (Random.Range(1, egoDamageDie) + egoDamage)) * badGuyCritResist) - badGuyDamageReduction));
            if (rolledDamage < 0) { rolledDamage = 0; }
        }

        battleLog.SetActive(true);
        string verb = "";
        if (ego.equippedWeapon == null) { verb = "swing"; }
        else if (ego.equippedWeapon.type == "Slashing" || ego.equippedWeapon.type == "Blunt") { verb = "swing"; }
        else if (ego.equippedWeapon.type == "Piercing") { verb = "stab"; }
        else if (ego.equippedWeapon.type == "Polearm") { verb = "jab"; }
        else if (ego.equippedWeapon.type == "Ranged") { verb = "shoot"; }

        bool gunHeld = false;
        if (ego.equippedWeapon != null) { if (ego.equippedWeapon.nome == "gun") { gunHeld = true; } }

        battleText.text = $"You {verb} at {badGuy.nome},";
        yield return new WaitForSeconds(.01f);
        messageComplete = false;
        StartCoroutine(BattleMessage(0));
        yield return new WaitUntil(MessageComplete);
        yield return new WaitForSeconds(.5f);

        if (attackRoll >= badGuyArmorClass && (d20 < 20))
        {
            if (gunHeld) { gunHit.Play(); }
            else { hit.Play(); }
            battleText.text += $" and hit for {rolledDamage} damage!";
            
            badGuy.allStats[1].value = badGuy.allStats[1].value - rolledDamage;
            ActivateHPRoll(badGuy);
        }
        else if (d20 == 20)
        {
            if (gunHeld) { gunCrit.Play(); }
            else { criticalHit.Play(); }
            battleText.text += $" and critically hit for {rolledDamage} damage!";

            badGuy.allStats[1].value = badGuy.allStats[1].value - rolledDamage;
            ActivateHPRoll(badGuy);
        }
        else if (attackRoll == (badGuyArmorClass - 1))
        {
            miss.Play();
            battleText.text += " and just miss!";
        }
        else
        {
            miss.Play();
            battleText.text += " but miss!";
        }
        yield return new WaitForSeconds(.01f);
        messageComplete = false;
        StartCoroutine(BattleMessage(endingCharacter));
        yield return new WaitUntil(MessageComplete);
        yield return new WaitForSeconds(.75f);
        battleLogGreyScreen.SetActive(true);
        yield return new WaitForSeconds(.5f);
        battleLog.SetActive(false);
        yield return new WaitForSeconds(.5f);
        battleLogGreyScreen.SetActive(false);
        actionComplete = true;
    }
    IEnumerator EgoActionSelect()
    {
        int attackSelectionMemory = -1;
        IEnumerator selection;
        currentArrowPosition = 0;
        egoDoneArrow.SetActive(false);
        egoCombatOptions[2].color = Color.white;
        arrow.SetActive(true);
        while (!battleIsWon)
        {
            if (currentArrowPosition < 0) { currentArrowPosition = 5; }
            if (currentArrowPosition > 5) { currentArrowPosition = 0; }
            arrow.transform.position = egoArrowPositions[currentArrowPosition].transform.position;
            yield return new WaitUntil(controller.UpDownEnterPressed);
            if (battleIsWon) { break; }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                cursorMove.Play();
                currentArrowPosition--;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                cursorMove.Play();
                currentArrowPosition++;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                cursorSelect.Play();
                egoDoneArrow.transform.position = arrow.transform.position;
                egoDoneArrow.SetActive(true);
                arrow.SetActive(false);
                egoCombatOptions[currentArrowPosition].color = Color.grey;

                //Attack
                if (currentArrowPosition == 0)
                {
                    List<GameObject> activeBadGuyBorders = new List<GameObject>();
                    int borderSelected = 0;

                    //construct target cycle
                    for (int i = 0; i < activeBadGuys.Length; i++)
                    {
                        if (activeBadGuys[i] != null)
                        {
                            if (!deadThisRound.Contains(activeBadGuys[i])) { activeBadGuyBorders.Add(activeBadGuys[i].combatBorder); }                            
                        }
                    }
                    //
                    while (true)
                    {
                        if (attackSelectionMemory != -1)
                        {
                            borderSelected = attackSelectionMemory;
                            attackSelectionMemory = -1;
                        }
                        if (borderSelected < 0) { borderSelected = activeBadGuyBorders.Count -1; }
                        if (borderSelected >= activeBadGuyBorders.Count) { borderSelected = 0; }
                        yield return new WaitForSeconds(.01f);
                        selection = SelectAnimation(activeBadGuyBorders[borderSelected]);
                        StartCoroutine(selection);
                        //take ego out of name highlight possibility
                        BadGuy currentSelection = activeBadGuys[0];
                        for (int i = 0; i < activeBadGuys.Length; i++)
                        {
                            if (activeBadGuys[i] != null)
                            {
                                if (activeBadGuys[i].combatBorder == activeBadGuyBorders[borderSelected]) { currentSelection = activeBadGuys[i]; }
                            }
                        }
                        int nameToHighlight = currentSelection.currentTurnOrder;
                        //
                        turnOrderNames[nameToHighlight].text = BoldText(turnOrderNames[nameToHighlight].text);
                        yield return new WaitUntil(controller.LeftRightEnterEscPressed);
                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            cursorMove.Play();
                            StopCoroutine(selection);
                            activeBadGuyBorders[borderSelected].SetActive(true);
                            turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                            borderSelected--;
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            cursorMove.Play();
                            StopCoroutine(selection);
                            activeBadGuyBorders[borderSelected].SetActive(true);
                            turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                            borderSelected++;
                        }
                        else if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            cursorCancel.Play();
                            egoDoneArrow.SetActive(false);
                            arrow.SetActive(true);
                            egoCombatOptions[currentArrowPosition].color = Color.white;
                            StopCoroutine(selection);
                            activeBadGuyBorders[borderSelected].SetActive(true);
                            turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                            attackSelectionMemory = borderSelected;
                            break;
                        }
                        else if (Input.GetKeyDown(KeyCode.Return))
                        {
                            cursorSelect.Play();
                            StopCoroutine(selection);
                            activeBadGuyBorders[borderSelected].SetActive(true);
                            turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                            ego.displayAction = "Attack";
                            ego.chosenAction = "Attack";
                            //assign chosen target to ego.chosentarget
                            for (int i = 0; i < activeBadGuys.Length; i++)
                            {
                                if (activeBadGuys[i] != null)
                                {
                                    if (activeBadGuys[i].combatBorder == activeBadGuyBorders[borderSelected]) { ego.chosenTarget = activeBadGuys[i]; }
                                }                                
                            }
                            actionSelected = true;
                            break;
                        }
                    }
                }
                //Defend
                else if (currentArrowPosition == 1)
                {
                    if (egoCombatOptions[1].text == "Maneuver")
                    {
                        ego.displayAction = "Maneuver";
                        ego.chosenAction = "Maneuver";
                    }
                    else
                    {
                        ego.displayAction = "Defend";
                        ego.chosenAction = "Defend";
                    }
                    actionSelected = true;
                }
                //Delay
                else if (currentArrowPosition == 2)
                {
                    int currentTurn = 0;
                    //find current turn
                    for (int i = 0; i < turnOrder.Length; i++) { if (turnOrder[i] is Ego) { currentTurn = i; } }

                    //make sure ego is not last (both in the order and the end of turn array)
                    if (currentTurn +1 < 7)
                    {
                        if (turnOrder[currentTurn + 1] != null)
                        {
                            ego.chosenAction = "Delay";
                            actionSelected = true;
                        }
                        else
                        {
                            currentArrowPosition = 0;
                            egoDoneArrow.SetActive(false);
                            arrow.SetActive(true);
                            egoCombatOptions[2].color = Color.white;
                        }
                    }
                    else
                    {
                        currentArrowPosition = 0;
                        egoDoneArrow.SetActive(false);
                        arrow.SetActive(true);
                        egoCombatOptions[2].color = Color.white;
                    }
                }
                //Flee
                else if (currentArrowPosition == 3)
                {
                    //ego.displayAction = "Flee";

                    //testing status, inventory, and badguy deaths
                    //AddEffect(ego, 1, Instantiate(allEffects[0]));
                    //AddEffect(ego, 1, Instantiate(allEffects[2]));
                    //AddEffect(ego, 1, Instantiate(allEffects[4]));
                    //controller.interactableItems.inventory.Add(controller.registerObjects.allWeapons[10]);
                    //controller.interactableItems.inventory.Add(controller.registerObjects.allArmor[2]);
                    //controller.interactableItems.inventory.Add(controller.registerObjects.allShields[3]);
                    //controller.interactableItems.inventory.Add(controller.registerObjects.allPotions[4]);
                    //controller.interactableItems.inventory.Add(controller.registerObjects.allUndroppables[0]);
                    //ego.potionBelt.Add(controller.registerObjects.allPotions[3]);
                    //ego.potionBelt.Add(controller.registerObjects.allPotions[4]);
                    //ego.potionBelt.Add(controller.registerObjects.allPotions[2]);
                    //for (int i = 0; i < activeBadGuys.Length; i++)
                    //{
                    //    if (activeBadGuys[i] != null)
                    //    {
                    //        activeBadGuys[i].allStats[1].value = 1;
                    //        ActivateHPRoll(activeBadGuys[i]);
                    //    }
                    //}

                    controller.OpenPopUpWindow("", "", "There is no escape!");
                    yield return new WaitForSeconds(1.5f);
                    controller.ClosePopUpWindow();
                    currentArrowPosition = 0;
                    egoDoneArrow.SetActive(false);
                    arrow.SetActive(true);
                    egoCombatOptions[3].color = Color.white;
                }
                //Status
                else if (currentArrowPosition == 4)
                {
                    if (ego.activeEffects.Count > 0)
                    {
                        string normalEffectsText = effectsText.text;
                        string manipulatedEffectsText = effectsText.text;
                        Effect selectedEffect = ego.activeEffects[0];
                        int selectedElement = 0;
                        while (true)
                        {
                            int doublesCounter = 0;
                            effectsText.text = normalEffectsText;
                            manipulatedEffectsText = normalEffectsText;
                            if (selectedElement < 0) { selectedElement = ego.activeEffects.Count -1; }
                            if (selectedElement > ego.activeEffects.Count -1) { selectedElement = 0; }

                            int effectLength = ego.activeEffects[selectedElement].title.Length;
                            //check if repeated effect below current selection in activeEffects
                            for (int i = 0; i < selectedElement; i++)
                            {
                                if (ego.activeEffects[selectedElement].title == ego.activeEffects[i].title) { doublesCounter++; }
                            }
                            //remove excess occurrences in text to get correct occurrence
                            for (int i = 0; i < doublesCounter; i++)
                            {
                                manipulatedEffectsText = effectsText.text.Remove(effectsText.text.IndexOf(ego.activeEffects[selectedElement].title), effectLength);
                            }
                            //rewrite text with selection (accounting for removed doubles)
                            int effectIndex = 0;
                            if (doublesCounter > 0) { effectIndex = manipulatedEffectsText.IndexOf(ego.activeEffects[selectedElement].title) + (effectLength * doublesCounter); }
                            else { effectIndex = effectsText.text.IndexOf(ego.activeEffects[selectedElement].title); }
                            
                            string newText = "";

                            for (int i = 0; i < effectIndex; i++) { newText += effectsText.text[i]; }

                            newText += "<b><size=14>[";

                            for (int i = effectIndex; i < effectIndex + effectLength; i++) { newText += effectsText.text[i]; }
                            
                            newText += "]</size></b>";

                            for (int i = effectIndex + effectLength; i < effectsText.text.Length; i++) { newText += effectsText.text[i]; }
                            
                            effectsText.text = newText;

                            
                            yield return new WaitUntil(controller.UpDownEnterEscPressed);
                            if (Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                cursorMove.Play();
                                selectedElement--;
                            }
                            else if (Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                cursorMove.Play();
                                selectedElement++;
                            }
                            else if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                cursorCancel.Play();
                                effectsText.text = normalEffectsText;
                                egoDoneArrow.SetActive(false);
                                arrow.SetActive(true);
                                egoCombatOptions[4].color = Color.white;
                                break;
                            }
                            else if (Input.GetKeyDown(KeyCode.Return))
                            {
                                cursorSelect.Play();
                                selectedEffect = ego.activeEffects[selectedElement];
                                string duration = "";
                                string suppression = "";
                                string s = "s";
                                if (selectedEffect.duration == 1) { s = ""; }
                                if (selectedEffect.duration > 0) { duration = $"<size=10>Remaining: {selectedEffect.duration} round{s}</size>"; }
                                if (selectedEffect.priorityEffect != null) { suppression = "<i><size=10>Suppressed</size></i>"; }
                                controller.OpenPopUpWindow(selectedEffect.title, "", selectedEffect.description, "", suppression, "", duration, "Press ESC to return");
                                //copying font from achievements for simplicity
                                controller.achievements.originalFont = controller.popUpMessage.font;
                                controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                yield return new WaitUntil(controller.EscPressed);
                                cursorCancel.Play();
                                controller.ClosePopUpWindow();
                            }
                        }
                    }
                    else
                    {
                        currentArrowPosition = 0;
                        egoDoneArrow.SetActive(false);
                        arrow.SetActive(true);
                        egoCombatOptions[4].color = Color.white;
                    }
                }
                //Inventory
                else if (currentArrowPosition == 5)
                {
                    StartCoroutine(DisplayBattleInventory());
                    yield return new WaitUntil(InventoryComplete);
                    invDisplay.SetActive(false);
                    invDisplayBorder.SetActive(false);
                    inventoryComplete = false;
                }
                if (actionSelected) { break; }
            }
            
        }
    }
    IEnumerator BadGuyActionSelect(BadGuy badGuy)
    {
        if (badGuy.combatSlot == slot1) { arrow.transform.position = slot1ArrowPositions[0].transform.position; }
        else if (badGuy.combatSlot == slot2a) { arrow.transform.position = slot2aArrowPositions[0].transform.position; }
        else if (badGuy.combatSlot == slot2b) { arrow.transform.position = slot2bArrowPositions[0].transform.position; }
        else if (badGuy.combatSlot == slot3b) { arrow.transform.position = slot3bArrowPositions[0].transform.position; }
        else if (badGuy.combatSlot == slot3c) { arrow.transform.position = slot3cArrowPositions[0].transform.position; }
        else if (badGuy.combatSlot == slotFlank1) { arrow.transform.position = slotFlank1ArrowPositions[0].transform.position; }
        else if (badGuy.combatSlot == slotFlank2) { arrow.transform.position = slotFlank2ArrowPositions[0].transform.position; }       
        arrow.SetActive(true);
        int howManyMoves = 0;
        int currentArrowPosition = 0;
        bool stutter = false;
        if (badGuy.chosenAction == "Attack") { howManyMoves = 0; }
        else if (badGuy.chosenAction == "Defend") { howManyMoves = 1; }
        //else if (badGuy.chosenAction == "Special") { howManyMoves = 2; }
        else if (badGuy.chosenAction == "Inventory") { howManyMoves = 3; }
        else { howManyMoves = 2; }
        yield return new WaitForSeconds(1f);
        
        while (howManyMoves != 0)
        {
            howManyMoves--;
            currentArrowPosition++;
            if (currentArrowPosition > 1 && Random.Range(1, 21) == 1)
            {
                howManyMoves += 2;
                currentArrowPosition -= 2;
                yield return new WaitForSeconds(.5f);
                stutter = true;
            }//stutter
            badGuyCursorMove.Play();
            if (badGuy.combatSlot == slot1) { arrow.transform.position = slot1ArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slot2a) { arrow.transform.position = slot2aArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slot2b) { arrow.transform.position = slot2bArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slot3b) { arrow.transform.position = slot3bArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slot3c) { arrow.transform.position = slot3cArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slotFlank1) { arrow.transform.position = slotFlank1ArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slotFlank2) { arrow.transform.position = slotFlank2ArrowPositions[currentArrowPosition].transform.position; }
            yield return new WaitForSeconds(.5f);
            if (stutter)
            {
                yield return new WaitForSeconds(.5f);
                stutter = false;
            }
        }
        badGuyCursorSelect.Play();
        if (badGuy.combatSlot == slot1)
        {
            slot1DoneArrow.transform.position = arrow.transform.position;
            slot1DoneArrow.SetActive(true);
            if (badGuy.chosenAction == "Attack") { attack1.color = Color.grey; }
            else if (badGuy.chosenAction == "Defend") { defend1.color = Color.grey; }
            //else if (badGuy.chosenAction == "Special") { special1.color = Color.grey; }
            else if (badGuy.chosenAction == "Inventory") { inventory1.color = Color.grey; }
            else { special1.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slot2a)
        {
            slot2aDoneArrow.transform.position = arrow.transform.position;
            slot2aDoneArrow.SetActive(true);
            if (badGuy.chosenAction == "Attack") { attack2a.color = Color.grey; }
            else if (badGuy.chosenAction == "Defend") { defend2a.color = Color.grey; }
            //else if (badGuy.chosenAction == "Special") { special2a.color = Color.grey; }
            else if (badGuy.chosenAction == "Inventory") { inventory2a.color = Color.grey; }
            else { special2a.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slot2b)
        {
            slot2bDoneArrow.transform.position = arrow.transform.position;
            slot2bDoneArrow.SetActive(true);
            if (badGuy.chosenAction == "Attack") { attack2b.color = Color.grey; }
            else if (badGuy.chosenAction == "Defend") { defend2b.color = Color.grey; }
            //else if (badGuy.chosenAction == "Special") { special2b.color = Color.grey; }
            else if (badGuy.chosenAction == "Inventory") { inventory2b.color = Color.grey; }
            else { special2b.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slot3b)
        {
            slot3bDoneArrow.transform.position = arrow.transform.position;
            slot3bDoneArrow.SetActive(true);
            if (badGuy.chosenAction == "Attack") { attack3b.color = Color.grey; }
            else if (badGuy.chosenAction == "Defend") { defend3b.color = Color.grey; }
            //else if (badGuy.chosenAction == "Special") { special3b.color = Color.grey; }
            else if (badGuy.chosenAction == "Inventory") { inventory3b.color = Color.grey; }
            else { special3b.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slot3c)
        {
            slot3cDoneArrow.transform.position = arrow.transform.position;
            slot3cDoneArrow.SetActive(true);
            if (badGuy.chosenAction == "Attack") { attack3c.color = Color.grey; }
            else if (badGuy.chosenAction == "Defend") { defend3c.color = Color.grey; }
            //else if (badGuy.chosenAction == "Special") { special3c.color = Color.grey; }
            else if (badGuy.chosenAction == "Inventory") { inventory3c.color = Color.grey; }
            else { special3c.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slotFlank1)
        {
            slotFlank1DoneArrow.transform.position = arrow.transform.position;
            slotFlank1DoneArrow.SetActive(true);
            if (badGuy.chosenAction == "Attack") { attackFlank1.color = Color.grey; }
            else if (badGuy.chosenAction == "Defend") { defendFlank1.color = Color.grey; }
            //else if (badGuy.chosenAction == "Special") { specialFlank1.color = Color.grey; }
            else if (badGuy.chosenAction == "Inventory") { inventoryFlank1.color = Color.grey; }
            else { specialFlank1.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slotFlank2)
        {
            slotFlank2DoneArrow.transform.position = arrow.transform.position;
            slotFlank2DoneArrow.SetActive(true);
            if (badGuy.chosenAction == "Attack") { attackFlank2.color = Color.grey; }
            else if (badGuy.chosenAction == "Defend") { defendFlank2.color = Color.grey; }
            //else if (badGuy.chosenAction == "Special") { specialFlank2.color = Color.grey; }
            else if (badGuy.chosenAction == "Inventory") { inventoryFlank2.color = Color.grey; }
            else { specialFlank2.color = Color.grey; }
        }
        arrow.SetActive(false);
        if (badGuy.chosenAction == "Attack" || badGuy.chosenAction == "Defend" || badGuy.chosenAction == "Inventory") { badGuy.displayAction = badGuy.chosenAction; }
        else { badGuy.displayAction = badGuy.specialAbility; }      
        yield return new WaitForSeconds(.75f);
        actionSelected = true;
    }
    string BadGuyLogic(BadGuy badGuy)
    {
        int currentHP = (int)badGuy.allStats[1].value;
        int combinedMaxHP = (int)badGuy.allStats[2].value + (int)badGuy.allStats[2].effectValue;

        if (currentHP >= (.25 * combinedMaxHP)) { return RegularAction(); }
        else
        {
            if (badGuy.potionBelt.Count > 0) { return "Inventory"; }
            else
            {   //strategist call for help
                if (badGuy.nome == "A Bold Yet Discretionary Strategist")
                {
                    badGuy.chosenAbility = badGuy.normalAIRay[6];
                    return "Call";
                }
                else { return RegularAction(); }
            }
        }

        string RegularAction()
        {
            int d100 = Random.Range(1, 101);
            Debug.Log(d100);
            badGuy.chosenAbility = null;

            //all AIRays must be arranged in ascending order
            for (int i = 0; i < badGuy.normalAIRay.Length; i++)
            {
                if (d100 <= badGuy.normalAIRay[i].likelihood)
                {
                    //do not flank if
                    if (badGuy.normalAIRay[i].title == "Flank")
                    {
                        //if already flanking
                        if (badGuy.combatSlot == slotFlank1 || badGuy.combatSlot == slotFlank2) { return "Attack"; }
                        //if another is already going to flank this round (so 2 don't flank into the same position)
                        for (int j = 0; j < activeBadGuys.Length; j++) { if ((activeBadGuys[j] != null) && (activeBadGuys[j].chosenAction == "Flank")) { return "Attack"; } }
                        //if flanking positions are full
                        if (slotFlank1.activeInHierarchy && slotFlank2.activeInHierarchy) { return "Attack"; }
                    }
                    //do not defend if
                    if (badGuy.normalAIRay[i].title == "Defend")
                    {
                        //only badguy left
                        int liveCounter = 0;
                        for (int j = 0; j < activeBadGuys.Length; j++) { if (activeBadGuys[j] != null) { liveCounter++; } }
                        //HP above half (or only badguy left)
                        if ((currentHP > (.5 * combinedMaxHP)) || (liveCounter == 1)) { return "Attack"; }
                    }
                    //don't repeat priority effects
                    if (badGuy.normalAIRay[i].effect != null)
                    {
                        if (badGuy.normalAIRay[i].effect.priorityLine != "None")
                        {
                            if (badGuy.normalAIRay[i].beneficial)
                            {
                                for (int j = 0; j < badGuy.activeEffects.Count; j++)
                                {
                                    if (badGuy.normalAIRay[i].effect.title == badGuy.activeEffects[j].title) { return "Attack"; }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < ego.activeEffects.Count; j++)
                                {
                                    if (badGuy.normalAIRay[i].effect.title == ego.activeEffects[j].title) { return "Attack"; }
                                }
                            }
                        }
                    }

                    //assign special ability if appropriate and return action
                    if (badGuy.normalAIRay[i] is BadGuyCombatActions) { badGuy.chosenAbility = badGuy.normalAIRay[i]; }
                    return badGuy.normalAIRay[i].title;
                }
            }
            //shouldn't ever be reached but here as a catch
            return "Attack";
        }
    }

    IEnumerator DisplayBattleInventory()
    {
        //copied from interactableitems with necessary changes and coding improvements
        string toPassIn;
        List<Item> alreadyListed = new List<Item>();
        IEnumerator blinker;
        IEnumerator selection;
        int itemSelectionMemory = -1;
        int selfSelectionMemory = -1;
        bool failedEquip = false;

        invDisplay.SetActive(true);
        invDisplayBorder.SetActive(true);
        InvStats(null);
        DisplayPotionBelt();
        alreadyListed.Clear();
        toPassIn = "";
        if (controller.interactableItems.inventory.Count == 0) { toPassIn += "Your inventory is empty! How sad.\n"; }
        else
        {
            for (int i = 0; i < controller.interactableItems.inventory.Count; i++)
            {
                if (alreadyListed.Contains(controller.interactableItems.inventory[i])) { continue; }
                else
                {
                    int counter = 0;
                    for (int j = i; j < controller.interactableItems.inventory.Count; j++)
                    {
                        if (controller.interactableItems.inventory[i] == controller.interactableItems.inventory[j]) { counter++; }
                    }
                    alreadyListed.Add(controller.interactableItems.inventory[i]);
                    string total = counter.ToString();
                    toPassIn += total + " " + myTI.ToTitleCase(controller.interactableItems.inventory[i].nome) + "\n";
                }
            }
        }
        invText.text = toPassIn;
        void DisplayPotionBelt()
        {
            potion0.text = "";
            potion1.text = "";
            potion2.text = "";
            if (ego.potionBelt.Count > 0) { potion0.text = myTI.ToTitleCase(ego.potionBelt[0].nome); }
            if (ego.potionBelt.Count > 1) { potion1.text = myTI.ToTitleCase(ego.potionBelt[1].nome); }
            if (ego.potionBelt.Count > 2) { potion2.text = myTI.ToTitleCase(ego.potionBelt[2].nome); }
        }


        if (controller.interactableItems.inventory.Count > 0)
        {
            string normalInvText = invText.text;
            Item selectedItem = alreadyListed[0];
            int selectedElement = 0;
            int memoryElement = 0;
            bool itemUsed = false;
            bool dropUsed = false;
            bool doubleBreak = false;
            while (true)
            {
                invText.text = normalInvText;
                if (selectedElement < 0) { selectedElement = alreadyListed.Count - 1; }
                if (selectedElement > alreadyListed.Count - 1) { selectedElement = 0; }
                int itemLength = alreadyListed[selectedElement].nome.Length;
                int invIndex = 0;
                invIndex = invText.text.IndexOf(myTI.ToTitleCase(alreadyListed[selectedElement].nome));

                string newText = "";

                for (int i = 0; i < invIndex; i++) { newText += invText.text[i]; }

                newText += "<color=yellow>";

                for (int i = invIndex; i < invIndex + itemLength; i++) { newText += invText.text[i]; }

                newText += "</color>";

                for (int i = invIndex + itemLength; i < invText.text.Length; i++) { newText += invText.text[i]; }

                invText.text = newText;

                InvStats(alreadyListed[selectedElement]);

                yield return new WaitUntil(controller.RightUpDownEnterEscPressed);
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    cursorMove.Play();
                    selectedElement--;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    cursorMove.Play();
                    selectedElement++;
                }
                //Potion Belt
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    cursorMove.Play();
                    if (ego.potionBelt.Count > 0)
                    {
                        invText.text = normalInvText;
                        memoryElement = selectedElement;
                        int potionElement = 0;
                        InvStats(ego.potionBelt[potionElement]);
                        string plainPotion0 = potion0.text;
                        string plainPotion1 = potion1.text;
                        string plainPotion2 = potion2.text;

                        while (true)
                        {
                            if (potionElement < 0) { potionElement = ego.potionBelt.Count - 1; }
                            if (potionElement > ego.potionBelt.Count - 1) { potionElement = 0; }
                            potion0.text = plainPotion0;
                            potion1.text = plainPotion1;
                            potion2.text = plainPotion2;

                            if (potionElement == 0) { potion0.text = $"<color=yellow>{potion0.text}</color>"; }
                            else if (potionElement == 1) { potion1.text = $"<color=yellow>{potion1.text}</color>"; }
                            else if (potionElement == 2) { potion2.text = $"<color=yellow>{potion2.text}</color>"; }

                            yield return new WaitUntil(controller.LeftUpDownEnterEscPressed);
                            if (Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                cursorMove.Play();
                                potionElement--;
                            }
                            else if (Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                cursorMove.Play();
                                potionElement++;
                            }
                            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                cursorMove.Play();
                                potion0.text = plainPotion0;
                                potion1.text = plainPotion1;
                                potion2.text = plainPotion2;
                                selectedElement = memoryElement;
                                break;
                            }
                            else if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                cursorCancel.Play();
                                potion0.text = plainPotion0;
                                potion1.text = plainPotion1;
                                potion2.text = plainPotion2;
                                invDisplay.SetActive(false);
                                invDisplayBorder.SetActive(false);
                                egoDoneArrow.SetActive(false);
                                arrow.SetActive(true);
                                egoCombatOptions[5].color = Color.white;
                                inventoryComplete = true;
                                doubleBreak = true;
                                break;
                            }
                            else if (Input.GetKeyDown(KeyCode.Return))
                            {
                                invOptions.SetActive(true);
                                invOptionsBorder.SetActive(true);
                                cursorSelect.Play();
                                selectedItem = ego.potionBelt[potionElement];
                                int option = 1;
                                invActions[0].color = darkGrey;
                                bool useUsed = false;
                                while (true)
                                {
                                    invOptions.SetActive(true);
                                    invOptionsBorder.SetActive(true);
                                    if (option < 1) { option = 2; }
                                    if (option > 2) { option = 1; }
                                    yield return new WaitForSeconds(.01f);
                                    blinker = TextBlinker(invActions[option]);
                                    StartCoroutine(blinker);
                                    yield return new WaitUntil(controller.UpDownEnterEscPressed);
                                    if (Input.GetKeyDown(KeyCode.UpArrow))
                                    {
                                        cursorMove.Play();
                                        StopCoroutine(blinker);
                                        invActions[option].color = Color.white;
                                        option--;
                                    }
                                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                                    {
                                        cursorMove.Play();
                                        StopCoroutine(blinker);
                                        invActions[option].color = Color.white;
                                        option++;
                                    }
                                    else if (Input.GetKeyDown(KeyCode.Escape))
                                    {
                                        cursorCancel.Play();
                                        StopCoroutine(blinker);
                                        invActions[option].color = Color.white;
                                        invOptions.SetActive(false);
                                        invOptionsBorder.SetActive(false);
                                        break;
                                    }
                                    else if (Input.GetKeyDown(KeyCode.Return))
                                    {
                                        cursorSelect.Play();
                                        StopCoroutine(blinker);
                                        invActions[option].color = Color.white;
                                        invOptions.SetActive(false);
                                        invOptionsBorder.SetActive(false);
                                        //Use
                                        if (option == 1)
                                        {
                                            invDisplay.SetActive(false);
                                            invDisplayBorder.SetActive(false);
                                            invOptions.SetActive(false);
                                            invOptionsBorder.SetActive(false);
                                            List<GameObject> activeBorders = new List<GameObject>();
                                            int borderSelected = 0;

                                            //construct target cycle
                                            for (int i = 0; i < activeBadGuys.Length; i++)
                                            {
                                                if (activeBadGuys[i] != null) { activeBorders.Add(activeBadGuys[i].combatBorder); }
                                            }
                                            activeBorders.Add(borderEgo);
                                            if (selectedItem.beneficial)
                                            {
                                                borderSelected = activeBorders.IndexOf(borderEgo);
                                                selfSelectionMemory = 0;
                                            }

                                            while (true)
                                            {
                                                if (itemSelectionMemory != -1)
                                                {
                                                    borderSelected = itemSelectionMemory;
                                                    itemSelectionMemory = -1;
                                                }
                                                if (borderSelected < 0) { borderSelected = activeBorders.Count - 1; }
                                                if (borderSelected >= activeBorders.Count) { borderSelected = 0; }
                                                yield return new WaitForSeconds(.01f);
                                                selection = SelectAnimation(activeBorders[borderSelected]);
                                                //match target with turn order name
                                                Character currentSelection = ego;
                                                for (int i = 0; i < activeBadGuys.Length; i++)
                                                {
                                                    if (activeBadGuys[i] != null)
                                                    {
                                                        if (activeBadGuys[i].combatBorder == activeBorders[borderSelected]) { currentSelection = activeBadGuys[i]; }
                                                    }
                                                }
                                                int nameToHighlight = currentSelection.currentTurnOrder;
                                                //
                                                StartCoroutine(selection);
                                                turnOrderNames[nameToHighlight].text = BoldText(turnOrderNames[nameToHighlight].text);
                                                yield return new WaitUntil(controller.LeftRightUpDownEnterEscPressed);
                                                if (Input.GetKeyDown(KeyCode.RightArrow))
                                                {
                                                    cursorMove.Play();
                                                    StopCoroutine(selection);
                                                    activeBorders[borderSelected].SetActive(true);
                                                    turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                                    borderSelected--;
                                                }
                                                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                                                {
                                                    cursorMove.Play();
                                                    StopCoroutine(selection);
                                                    activeBorders[borderSelected].SetActive(true);
                                                    turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                                    borderSelected++;
                                                }
                                                else if (Input.GetKeyDown(KeyCode.DownArrow))
                                                {
                                                    cursorMove.Play();
                                                    if (activeBorders[borderSelected] != borderEgo)
                                                    {
                                                        StopCoroutine(selection);
                                                        activeBorders[borderSelected].SetActive(true);
                                                        turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                                        selfSelectionMemory = borderSelected;
                                                        borderSelected = activeBorders.IndexOf(borderEgo);
                                                    }
                                                    else
                                                    {
                                                        StopCoroutine(selection);
                                                        activeBorders[borderSelected].SetActive(true);
                                                        turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                                    }
                                                }
                                                else if (Input.GetKeyDown(KeyCode.UpArrow))
                                                {
                                                    cursorMove.Play();
                                                    if (activeBorders[borderSelected] == borderEgo)
                                                    {
                                                        StopCoroutine(selection);
                                                        activeBorders[borderSelected].SetActive(true);
                                                        turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                                        borderSelected = selfSelectionMemory;
                                                    }
                                                    else
                                                    {
                                                        StopCoroutine(selection);
                                                        activeBorders[borderSelected].SetActive(true);
                                                        turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                                        continue;
                                                    }
                                                }
                                                else if (Input.GetKeyDown(KeyCode.Escape))
                                                {
                                                    cursorCancel.Play();
                                                    invDisplay.SetActive(true);
                                                    invDisplayBorder.SetActive(true);
                                                    invOptions.SetActive(true);
                                                    invOptionsBorder.SetActive(true);
                                                    StopCoroutine(selection);
                                                    activeBorders[borderSelected].SetActive(true);
                                                    turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                                    itemSelectionMemory = borderSelected;
                                                    break;
                                                }
                                                else if (Input.GetKeyDown(KeyCode.Return))
                                                {
                                                    doubleBreak = true;
                                                    cursorSelect.Play();
                                                    StopCoroutine(selection);
                                                    activeBorders[borderSelected].SetActive(true);
                                                    turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                                    ego.displayAction = "Use";
                                                    ego.chosenAction = "Use";
                                                    ego.chosenItem = selectedItem;
                                                    //assign chosen target to ego.chosentarget
                                                    if (borderEgo == activeBorders[borderSelected]) { ego.chosenTarget = ego; }
                                                    else
                                                    {
                                                        for (int i = 0; i < activeBadGuys.Length; i++)
                                                        {
                                                            if (activeBadGuys[i] != null)
                                                            {
                                                                if (activeBadGuys[i].combatBorder == activeBorders[borderSelected]) { ego.chosenTarget = activeBadGuys[i]; }
                                                            }
                                                        }
                                                    }
                                                    useUsed = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //Drop
                                        else if (option == 2)
                                        {
                                            bool yesSelected = false;
                                            while (true)
                                            {
                                                if (yesSelected) { controller.OpenPopUpWindow($"Drop {selectedItem.nome}?", "", "This action cannot be undone.", "", "<b>[Yes]</b><color=white>. I'm not afraid.</color>", "", "<color=white>No! Take me back!</color>", ""); }
                                                else { controller.OpenPopUpWindow($"Drop {selectedItem.nome}?", "", "This action cannot be undone.", "", "<color=white>Yes. I'm not afraid.</color>", "", "<b>[No]</b><color=white>! Take me back!</color>", ""); }
                                                yield return new WaitUntil(controller.LeftRightEnterPressed);
                                                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                                                {
                                                    cursorMove.Play();
                                                    yesSelected = !yesSelected;
                                                }
                                                else if (Input.GetKeyDown(KeyCode.Return))
                                                {
                                                    cursorSelect.Play();
                                                    if (yesSelected)
                                                    {
                                                        if (selectedItem is Undroppable)
                                                        {
                                                            controller.OpenPopUpWindow($"Oops!", "", "You spend the remainder of the battle on all fours searching for your lost quest item. There was one time when you thought you found it, but it was actually just your arm as it got hacked off by your foe. You bleed out, lose consciousness, and eventually die.", "", "", "", "", "Press ESC to exit");
                                                            controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                            yield return new WaitUntil(controller.EscPressed);
                                                            controller.OpenPopUpWindow($"Nah you fine.", "", $"After a flash of an outer body experience, you decide just to put {myTI.ToTitleCase(selectedItem.nome)} away.", "", "", "", "", "Press ESC to return");
                                                            controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                            yield return new WaitForSeconds(.25f);
                                                        }
                                                        else
                                                        {
                                                            controller.OpenPopUpWindow($"", "", $"You drop the {myTI.ToTitleCase(selectedItem.nome)}.", "", "", "", "", "Press ESC to return");
                                                            controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                            if (selectedItem is Potion) { ego.potionBelt.Remove((Potion)selectedItem); }
                                                            else { controller.interactableItems.inventory.Remove(selectedItem); }
                                                        }
                                                        controller.popUpMessage.font = controller.achievements.originalFont;
                                                        dropUsed = true;
                                                        yield return new WaitUntil(controller.EscPressed);
                                                    }
                                                    controller.ClosePopUpWindow();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    //boolean gatekeepers
                                    if (useUsed)
                                    {
                                        useUsed = false;
                                        itemUsed = true;
                                        break;
                                    }
                                    if (dropUsed)
                                    {
                                        itemUsed = true;
                                        break;
                                    }
                                }
                            }
                            if (doubleBreak)
                            {
                                doubleBreak = false;
                                break;
                            }
                        }
                    }                   
                }
                //End Potion Belt
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    cursorCancel.Play();
                    invDisplay.SetActive(false);
                    invDisplayBorder.SetActive(false);
                    egoDoneArrow.SetActive(false);
                    arrow.SetActive(true);
                    egoCombatOptions[5].color = Color.white;
                    inventoryComplete = true;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    cursorSelect.Play();
                    selectedItem = alreadyListed[selectedElement];
                    int option = 0;
                    invActions[0].color = Color.white;
                    if (selectedItem is Undroppable || selectedItem is Potion)
                    {
                        option = 1;
                        invActions[0].color = darkGrey;
                    }
                    bool useUsed = false;
                    while (true)
                    {
                        invOptions.SetActive(true);
                        invOptionsBorder.SetActive(true);
                        if (selectedItem is Undroppable || selectedItem is Potion)
                        {
                            if (option < 1) { option = 2; }
                            if (option > 2) { option = 1; }
                        }
                        else
                        {
                            if (option < 0) { option = 2; }
                            if (option > 2) { option = 0; }
                        }
                        yield return new WaitForSeconds(.01f);
                        blinker = TextBlinker(invActions[option]);
                        StartCoroutine(blinker);
                        yield return new WaitUntil(controller.UpDownEnterEscPressed);
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            cursorMove.Play();
                            StopCoroutine(blinker);
                            invActions[option].color = Color.white;
                            option--;
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            cursorMove.Play();
                            StopCoroutine(blinker);
                            invActions[option].color = Color.white;
                            option++;
                        }
                        else if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            cursorCancel.Play();
                            StopCoroutine(blinker);
                            invActions[option].color = Color.white;
                            invOptions.SetActive(false);
                            invOptionsBorder.SetActive(false);
                            break;
                        }
                        else if (Input.GetKeyDown(KeyCode.Return))
                        {
                            cursorSelect.Play();
                            StopCoroutine(blinker);
                            invActions[option].color = Color.white;
                            invOptions.SetActive(false);
                            invOptionsBorder.SetActive(false);
                            //Equip
                            if (option == 0)
                            {
                                //exceptions for shield/two-handed weapons
                                if (ego.equippedWeapon != null)
                                {
                                    //two handed weapon blocking shield
                                    if (selectedItem is Shield && ego.equippedWeapon.twoHanded)
                                    {
                                        controller.OpenPopUpWindow("", "", $"You can't equip the {myTI.ToTitleCase(selectedItem.nome)} while wielding a two-handed weapon.", "", "", "", "", "Press ESC to return");
                                        controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                        yield return new WaitUntil(controller.EscPressed);
                                        controller.ClosePopUpWindow();
                                        break;
                                    }
                                    //allow sword and shield to replace a two handed weapon
                                    else if (selectedItem is Weapon && ego.equippedWeapon.twoHanded)
                                    {
                                        Weapon equippingWeapon = (Weapon)selectedItem;
                                        bool shieldPossible = false;
                                        for (int i = 0; i < controller.interactableItems.inventory.Count; i++)
                                        {
                                            if (controller.interactableItems.inventory[i] is Shield)
                                            {
                                                shieldPossible = true;
                                                break;
                                            }
                                        }

                                        if (!equippingWeapon.twoHanded && shieldPossible)
                                        {
                                            shieldPossible = false;
                                            bool yesSelected = true;
                                            while (true)
                                            {
                                                if (yesSelected) { controller.OpenPopUpWindow("", "", "Equip a shield as well, hero?", "", "<b>[Yes]</b><color=white>! Good thinking.</color>", "", "<color=white>Nah. It'll be fine.</color>", ""); }
                                                else { controller.OpenPopUpWindow("", "", "Equip a shield as well, hero?", "", "<color=white>Yes! Good thinking.</color>", "", "<b>[Nah]</b><color=white>. It'll be fine.</color>", ""); }
                                                controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                yield return new WaitUntil(controller.LeftRightEnterPressed);
                                                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                                                {
                                                    cursorMove.Play();
                                                    yesSelected = !yesSelected;
                                                }
                                                else if (Input.GetKeyDown(KeyCode.Return))
                                                {
                                                    cursorSelect.Play();
                                                    controller.popUpMessage.font = controller.achievements.originalFont;
                                                    controller.ClosePopUpWindow();
                                                    //allow shield also to be equipped
                                                    if (yesSelected)
                                                    {
                                                        invText.text = normalInvText;
                                                        string normalShieldInvText = invText.text;
                                                        Item selectedShield = alreadyListed[0];
                                                        int selectedShieldElement = 0;
                                                        while (true)
                                                        {
                                                            invText.text = normalShieldInvText;
                                                            if (selectedShieldElement < 0) { selectedShieldElement = alreadyListed.Count - 1; }
                                                            if (selectedShieldElement > alreadyListed.Count - 1) { selectedShieldElement = 0; }
                                                            int shieldLength = alreadyListed[selectedShieldElement].nome.Length;
                                                            int invShieldIndex = 0;
                                                            invShieldIndex = invText.text.IndexOf(myTI.ToTitleCase(alreadyListed[selectedShieldElement].nome));

                                                            string newShieldText = "";

                                                            for (int i = 0; i < invShieldIndex; i++) { newShieldText += invText.text[i]; }

                                                            newShieldText += "<b><size=40>[";

                                                            for (int i = invShieldIndex; i < invShieldIndex + shieldLength; i++) { newShieldText += invText.text[i]; }

                                                            newShieldText += "]</size></b>";

                                                            for (int i = invShieldIndex + shieldLength; i < invText.text.Length; i++) { newShieldText += invText.text[i]; }

                                                            invText.text = newShieldText;

                                                            InvStats(alreadyListed[selectedShieldElement]);

                                                            yield return new WaitUntil(controller.UpDownEnterEscPressed);
                                                            if (Input.GetKeyDown(KeyCode.UpArrow))
                                                            {
                                                                cursorMove.Play();
                                                                selectedShieldElement--;
                                                            }
                                                            else if (Input.GetKeyDown(KeyCode.DownArrow))
                                                            {
                                                                cursorMove.Play();
                                                                selectedShieldElement++;
                                                            }
                                                            else if (Input.GetKeyDown(KeyCode.Escape))
                                                            {
                                                                cursorSelect.Play();
                                                                selectedShield = null;
                                                                break;
                                                            }
                                                            else if (Input.GetKeyDown(KeyCode.Return))
                                                            {
                                                                if (alreadyListed[selectedShieldElement] is Shield)
                                                                {
                                                                    cursorSelect.Play();
                                                                    selectedShield = alreadyListed[selectedShieldElement];
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    controller.OpenPopUpWindow("Last I checked...", "", $"{myTI.ToTitleCase(alreadyListed[selectedShieldElement].nome)} is not a shield.", "", "", "", "", "Press ESC to return");
                                                                    controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                                    yield return new WaitUntil(controller.EscPressed);
                                                                    controller.popUpMessage.font = controller.achievements.originalFont;
                                                                    controller.ClosePopUpWindow();
                                                                }
                                                            }
                                                        }
                                                        if (selectedShield != null) { ego.chosenItem2 = selectedShield; }
                                                    }                                                    
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }        
                                if (ego.equippedShield != null && selectedItem is Weapon)
                                {
                                    //two handed weapon unequiping both slots
                                    Weapon equippingWeapon = (Weapon)selectedItem;
                                    if (equippingWeapon.twoHanded)
                                    {
                                        bool yesSelected = false;
                                        while (true)
                                        {
                                            failedEquip = false;
                                            if (yesSelected) { controller.OpenPopUpWindow("", "", "This weapon is two-handed, so your shield will also be unequipped.", "", "<b>[Yeah]</b><color=white>, duh.</color>", "", "<color=white>No! As if!</color>", ""); }
                                            else { controller.OpenPopUpWindow("", "", "This weapon is two-handed, so your shield will also be unequipped.", "", "<color=white>Yeah, duh.</color>", "", "<b>[No]</b><color=white>! As if!</color>", ""); }
                                            yield return new WaitUntil(controller.LeftRightEnterPressed);
                                            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                                            {
                                                cursorMove.Play();
                                                yesSelected = !yesSelected;
                                            }
                                            else if (Input.GetKeyDown(KeyCode.Return))
                                            {
                                                cursorSelect.Play();
                                                controller.ClosePopUpWindow();
                                                if (yesSelected) { unstrap = true; }
                                                else { failedEquip = true; }
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (!failedEquip)
                                {
                                    ego.displayAction = "Equip";
                                    ego.chosenAction = "Equip";
                                    ego.chosenItem = selectedItem;
                                    itemUsed = true;
                                }
                                break;
                            }
                            //Use
                            else if (option == 1)
                            {
                                invDisplay.SetActive(false);
                                invDisplayBorder.SetActive(false);
                                invOptions.SetActive(false);
                                invOptionsBorder.SetActive(false);
                                List<GameObject> activeBorders = new List<GameObject>();
                                int borderSelected = 0;

                                //construct target cycle
                                for (int i = 0; i < activeBadGuys.Length; i++)
                                {
                                    if (activeBadGuys[i] != null) { activeBorders.Add(activeBadGuys[i].combatBorder); }
                                }
                                activeBorders.Add(borderEgo);
                                if (selectedItem.beneficial)
                                {
                                    borderSelected = activeBorders.IndexOf(borderEgo);
                                    selfSelectionMemory = 0;
                                }

                                while (true)
                                {
                                    if (itemSelectionMemory != -1)
                                    {
                                        borderSelected = itemSelectionMemory;
                                        itemSelectionMemory = -1;
                                    }
                                    if (borderSelected < 0) { borderSelected = activeBorders.Count - 1; }
                                    if (borderSelected >= activeBorders.Count) { borderSelected = 0; }
                                    yield return new WaitForSeconds(.01f);
                                    selection = SelectAnimation(activeBorders[borderSelected]);
                                    //match target with turn order name
                                    Character currentSelection = ego;
                                    for (int i = 0; i < activeBadGuys.Length; i++)
                                    {
                                        if (activeBadGuys[i] != null)
                                        {
                                            if (activeBadGuys[i].combatBorder == activeBorders[borderSelected]) { currentSelection = activeBadGuys[i]; }
                                        }
                                    }
                                    int nameToHighlight = currentSelection.currentTurnOrder;
                                    //
                                    StartCoroutine(selection);
                                    turnOrderNames[nameToHighlight].text = BoldText(turnOrderNames[nameToHighlight].text);
                                    yield return new WaitUntil(controller.LeftRightUpDownEnterEscPressed);
                                    if (Input.GetKeyDown(KeyCode.RightArrow))
                                    {
                                        cursorMove.Play();
                                        StopCoroutine(selection);
                                        activeBorders[borderSelected].SetActive(true);
                                        turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                        borderSelected--;
                                    }
                                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                                    {
                                        cursorMove.Play();
                                        StopCoroutine(selection);
                                        activeBorders[borderSelected].SetActive(true);
                                        turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                        borderSelected++;
                                    }
                                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                                    {
                                        cursorMove.Play();
                                        if (activeBorders[borderSelected] != borderEgo)
                                        {
                                            StopCoroutine(selection);
                                            activeBorders[borderSelected].SetActive(true);
                                            turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                            selfSelectionMemory = borderSelected;
                                            borderSelected = activeBorders.IndexOf(borderEgo);
                                        }
                                        else
                                        {
                                            StopCoroutine(selection);
                                            activeBorders[borderSelected].SetActive(true);
                                            turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                        }
                                    }
                                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                                    {
                                        cursorMove.Play();
                                        if (activeBorders[borderSelected] == borderEgo)
                                        {
                                            StopCoroutine(selection);
                                            activeBorders[borderSelected].SetActive(true);
                                            turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                            borderSelected = selfSelectionMemory;
                                        }
                                        else
                                        {
                                            StopCoroutine(selection);
                                            activeBorders[borderSelected].SetActive(true);
                                            turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                            continue;
                                        }
                                    }
                                    else if (Input.GetKeyDown(KeyCode.Escape))
                                    {
                                        cursorCancel.Play();
                                        invDisplay.SetActive(true);
                                        invDisplayBorder.SetActive(true);
                                        invOptions.SetActive(true);
                                        invOptionsBorder.SetActive(true);
                                        StopCoroutine(selection);
                                        activeBorders[borderSelected].SetActive(true);
                                        turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                        itemSelectionMemory = borderSelected;
                                        break;
                                    }
                                    else if (Input.GetKeyDown(KeyCode.Return))
                                    {
                                        cursorSelect.Play();
                                        StopCoroutine(selection);
                                        activeBorders[borderSelected].SetActive(true);
                                        turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                        ego.displayAction = "Use";
                                        ego.chosenAction = "Use";
                                        ego.chosenItem = selectedItem;
                                        //assign chosen target to ego.chosentarget
                                        if (borderEgo == activeBorders[borderSelected]) { ego.chosenTarget = ego; }
                                        else
                                        {
                                            for (int i = 0; i < activeBadGuys.Length; i++)
                                            {
                                                if (activeBadGuys[i] != null)
                                                {
                                                    if (activeBadGuys[i].combatBorder == activeBorders[borderSelected]) { ego.chosenTarget = activeBadGuys[i]; }
                                                }
                                            }
                                        }
                                        useUsed = true;
                                        break;
                                    }
                                }
                            }
                            //Drop
                            else if (option == 2)
                            {
                                bool yesSelected = false;
                                while (true)
                                {
                                    if (yesSelected) { controller.OpenPopUpWindow($"Drop {selectedItem.nome}?", "", "This action cannot be undone.", "", "<b>[Yes]</b><color=white>. I'm not afraid.</color>", "", "<color=white>No! Take me back!</color>", ""); }
                                    else { controller.OpenPopUpWindow($"Drop {selectedItem.nome}?", "", "This action cannot be undone.", "", "<color=white>Yes. I'm not afraid.</color>", "", "<b>[No]</b><color=white>! Take me back!</color>", ""); }
                                    yield return new WaitUntil(controller.LeftRightEnterPressed);
                                    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                                    {
                                        cursorMove.Play();
                                        yesSelected = !yesSelected;
                                    }
                                    else if (Input.GetKeyDown(KeyCode.Return))
                                    {
                                        cursorSelect.Play();
                                        if (yesSelected)
                                        {
                                            if (selectedItem is Undroppable)
                                            {
                                                controller.OpenPopUpWindow($"Oops!", "", "You spend the remainder of the battle on all fours searching for your lost quest item. There was one time when you thought you found it, but it was actually just your arm as it got hacked off by your foe. You bleed out, lose consciousness, and eventually die.", "", "", "", "", "Press ESC to exit");
                                                controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                yield return new WaitUntil(controller.EscPressed);
                                                controller.OpenPopUpWindow($"Nah you fine.", "", $"After a flash of an outer body experience, you decide just to put {myTI.ToTitleCase(selectedItem.nome)} away.", "", "", "", "", "Press ESC to return");
                                                controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                yield return new WaitForSeconds(.25f);
                                            }
                                            else
                                            {
                                                controller.OpenPopUpWindow($"", "", $"You drop the {myTI.ToTitleCase(selectedItem.nome)}.", "", "", "", "", "Press ESC to return");
                                                controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                controller.interactableItems.inventory.Remove(selectedItem);
                                            }
                                            controller.popUpMessage.font = controller.achievements.originalFont;
                                            dropUsed = true;
                                            yield return new WaitUntil(controller.EscPressed);
                                        }
                                        controller.ClosePopUpWindow();
                                        break;
                                    }
                                }
                            }
                        }
                        //boolean gatekeepers
                    if (useUsed)
                        {
                            useUsed = false;
                            itemUsed = true;
                            break;
                        }
                    if (dropUsed)
                        {
                            itemUsed = true;
                            break;
                        }
                    }
                }
                if (doubleBreak)
                {
                    doubleBreak = false;
                    break;
                }
                if (itemUsed)
                {
                    itemUsed = false;
                    if (dropUsed)
                    {
                        dropUsed = false;
                        StartCoroutine(DisplayBattleInventory());
                    }
                    else
                    {
                        actionSelected = true;
                        inventoryComplete = true;
                    }                    
                    break;
                }
            }
        }
        else
        {
            yield return new WaitUntil(controller.EscPressed);
            cursorCancel.Play();
            invDisplay.SetActive(false);
            invDisplayBorder.SetActive(false);
            egoDoneArrow.SetActive(false);
            arrow.SetActive(true);
            egoCombatOptions[5].color = Color.white;
            inventoryComplete = true;
        }
    }
    void InvStats(Item itemSelected)
    {
        //damage die & damage
        int egoDamageDie = (int)(ego.allStats[6].value + ego.allStats[6].effectValue);
        int withItemDamageDie = egoDamageDie;
        int egoDamage = (int)(ego.allStats[7].value + ego.allStats[7].effectValue);
        int withItemDamage = egoDamage;
        int egoCurrentWeaponDamage = 0;
        if (ego.equippedWeapon != null) { egoCurrentWeaponDamage = controller.ego.equippedWeapon.damage; }        
        if (itemSelected is Weapon)
        {
            Weapon weaponSelected = (Weapon)itemSelected;
            withItemDamage = egoDamage - egoCurrentWeaponDamage + weaponSelected.damage;
            withItemDamageDie = (int)ego.allStats[6].effectValue + weaponSelected.damageDie;
        }
        string dieColor = "white";
        string damageColor = "white";
        string damageSign = "+";
        if (egoDamageDie < withItemDamageDie) { dieColor = "green"; }
        if (egoDamageDie > withItemDamageDie) { dieColor = "red"; }
        if (egoDamage < withItemDamage) { damageColor = "green"; }
        if (egoDamage > withItemDamage) { damageColor = "red"; }
        if (withItemDamage < 0) { damageSign = ""; }
        combatInvDamage.text = $"<color={dieColor}>1d{withItemDamageDie}</color> <color={damageColor}>{damageSign}{withItemDamage}</color>";

        //critMultiplier
        float egoCritMultiplier = ego.allStats[8].value + ego.allStats[8].effectValue;
        float withItemCritMultiplier = egoCritMultiplier;
        if (itemSelected is Weapon)
        {
            Weapon weaponSelected = (Weapon)itemSelected;
            withItemCritMultiplier = ego.allStats[8].effectValue + weaponSelected.critMultiplier;
        }
        string critMultiplierColor = "white";
        if (egoCritMultiplier < withItemCritMultiplier) { critMultiplierColor = "green"; }
        if (egoCritMultiplier > withItemCritMultiplier) { critMultiplierColor = "red"; }

        combatInvCritMultiplier.text = $"<color={critMultiplierColor}>x{withItemCritMultiplier}</color>";

        //toHitMod
        int egoToHitMod = (int)(ego.allStats[9].value + ego.allStats[9].effectValue);
        int withItemToHitMod = egoToHitMod;
        int egoCurrentWeaponToHitMod = 0;
        if (ego.equippedWeapon != null) { egoCurrentWeaponToHitMod = controller.ego.equippedWeapon.toHitMod; }
        if (itemSelected is Weapon)
        {
            Weapon weaponSelected = (Weapon)itemSelected;
            withItemToHitMod = egoToHitMod - egoCurrentWeaponToHitMod + weaponSelected.toHitMod;
        }
        string toHitModColor = "white";
        if (egoToHitMod < withItemToHitMod) { toHitModColor = "green"; }
        if (egoToHitMod > withItemToHitMod) { toHitModColor = "red"; }

        if (withItemToHitMod < 0) { combatInvToHitMod.text = $"<color={toHitModColor}>{withItemToHitMod}</color>"; }
        else { combatInvToHitMod.text = $"<color={toHitModColor}>+{withItemToHitMod}</color>"; }

        

        //armorClass
        int egoArmorClass = (int)(ego.allStats[3].value + ego.allStats[3].effectValue);
        int withItemArmorClass = (int)egoArmorClass;
        int egoCurrentShieldArmorClass = 0;
        if (ego.equippedShield != null) { egoCurrentShieldArmorClass = controller.ego.equippedShield.armorClass; }
        if (itemSelected is Shield)
        {
            Shield shieldSelected = (Shield)itemSelected;
            withItemArmorClass = egoArmorClass - egoCurrentShieldArmorClass + shieldSelected.armorClass;
        }
        string armorClassColor = "white";
        if (egoArmorClass < withItemArmorClass) { armorClassColor = "green"; }
        if (egoArmorClass > withItemArmorClass) { armorClassColor = "red"; }

        combatInvArmorClass.text = $"<color={armorClassColor}>{withItemArmorClass}</color>";

        //critResist
        float egoCritResist = ego.allStats[4].value - ego.allStats[4].effectValue;
        float withItemCritResist = egoCritResist;
        float egoCurrentArmorCritResist = 1;
        float egoCurrentShieldCritResist = 1;
        if (ego.equippedArmor != null) { egoCurrentArmorCritResist = controller.ego.equippedArmor.critResist; }
        if (ego.equippedShield != null) { egoCurrentShieldCritResist = controller.ego.equippedShield.critResist; }
        if (itemSelected is Armor)
        {
            Armor armorSelected = (Armor)itemSelected;
            withItemCritResist = armorSelected.critResist - (1 - egoCurrentShieldCritResist) - ego.allStats[4].effectValue;
        }
        else if (itemSelected is Shield)
        {
            Shield shieldSelected = (Shield)itemSelected;
            withItemCritResist = shieldSelected.critResist - (1 - egoCurrentArmorCritResist) - ego.allStats[4].effectValue;
        }
        string critResistColor = "white";
        if (egoCritResist > withItemCritResist) { critResistColor = "green"; }
        if (egoCritResist < withItemCritResist) { critResistColor = "red"; }

        combatInvCritResist.text = $"<color={critResistColor}>x{withItemCritResist}</color>";

        //dmgReduction
        int egoDmgReduction = (int)(ego.allStats[5].value + ego.allStats[5].effectValue);
        int withItemDmgReduction = (int)egoDmgReduction;
        int egoCurrentArmorDmgReduction = 0;
        if (ego.equippedArmor != null) { egoCurrentArmorDmgReduction = controller.ego.equippedArmor.damageReduction; }
        if (itemSelected is Armor)
        {
            Armor armorSelected = (Armor)itemSelected;
            withItemDmgReduction = egoDmgReduction - egoCurrentArmorDmgReduction + armorSelected.damageReduction;
        }
        string dmgReductionColor = "white";
        if (egoDmgReduction < withItemDmgReduction) { dmgReductionColor = "green"; }
        if (egoDmgReduction > withItemDmgReduction) { dmgReductionColor = "red"; }

        if (withItemDmgReduction >= 0) { combatInvDmgReduction.text = $"<color={dmgReductionColor}>-{withItemDmgReduction}</color>"; }
        else { combatInvDmgReduction.text = $"<color={dmgReductionColor}>+{Mathf.Abs(withItemDmgReduction)}</color>"; }
    }
    void ActivateHPRoll(Character character)
    {
        if (character == ego)
        {
            StopCoroutine(egoHP);
            egoHP = HPRoll(ego);
            StartCoroutine(egoHP);
        }
        if (activeBadGuys[0] != null)
        {
            if (character == activeBadGuys[0])
            {
                StopCoroutine(badGuy0HP);
                if (activeBadGuys[0] != null) { badGuy0HP = HPRoll(activeBadGuys[0]); }
                StartCoroutine(badGuy0HP);
            }
        }
        if (activeBadGuys[1] != null)
        {
            if (character == activeBadGuys[1])
            {
                StopCoroutine(badGuy1HP);
                if (activeBadGuys[1] != null) { badGuy1HP = HPRoll(activeBadGuys[1]); }
                StartCoroutine(badGuy1HP);
            }
        }
        if (activeBadGuys[2] != null)
        {
            if (character == activeBadGuys[2])
            {
                StopCoroutine(badGuy2HP);
                if (activeBadGuys[2] != null) { badGuy2HP = HPRoll(activeBadGuys[2]); }
                StartCoroutine(badGuy2HP);
            }
        }
        if (activeBadGuys[3] != null)
        {
            if (character == activeBadGuys[3])
            {
                StopCoroutine(badGuy3HP);
                if (activeBadGuys[3] != null) { badGuy3HP = HPRoll(activeBadGuys[3]); }
                StartCoroutine(badGuy3HP);
            }
        }
        if (activeBadGuys[4] != null)
        {
            if (character == activeBadGuys[4])
            {
                StopCoroutine(badGuy4HP);
                if (activeBadGuys[4] != null) { badGuy4HP = HPRoll(activeBadGuys[4]); }
                StartCoroutine(badGuy4HP);
            }
        }
    }
    IEnumerator HPRoll(Character character)
    {
        while (character.allStats[0].value > character.allStats[1].value)
        {
            character.allStats[0].value--;
            yield return new WaitForSeconds(.2f);
        }
        while (character.allStats[0].value < character.allStats[1].value)
        {
            character.allStats[0].value++;
            yield return new WaitForSeconds(.1f);
        }
    }
    IEnumerator ActivateBattleLog(string text1, string text2 = null, float midPause = .5f, float endPause = 1.25f, AudioSource sound = null, Effect effect = null, Character target = null, Character caster = null)
    {
        battleLog.SetActive(true);
        //two parter with midpause
        battleText.maxVisibleCharacters = 0;
        if (text2 != null)
        {
            battleText.text = text1;
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(0));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(midPause);
            //second half with close
            if (effect != null)
            {
                AddEffect(target, caster.currentTurnOrder, effect);
                if (target.activeEffects[target.activeEffects.Count - 1].priorityEffect != null && sound != null)
                {
                    sound = blockEffect;
                    text2 = $"\n\nThe influence of {caster.nome}'s {caster.chosenAbility.title} ability is suppressed by a more powerful effect.";
                }
            }
            if (sound != null) { sound.Play(); }
            battleText.text += " " + text2;
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(endingCharacter));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(endPause);
            battleLogGreyScreen.SetActive(true);
            yield return new WaitForSeconds(.5f);
            battleLog.SetActive(false);
            yield return new WaitForSeconds(.5f);
            battleLogGreyScreen.SetActive(false);
        }
        //one parter
        else
        {
            if (effect != null) { AddEffect(target, caster.currentTurnOrder, effect); }
            battleText.text = text1;
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(0));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(endPause);
            battleLogGreyScreen.SetActive(true);
            yield return new WaitForSeconds(.5f);
            battleLog.SetActive(false);
            yield return new WaitForSeconds(.5f);
            battleLogGreyScreen.SetActive(false);
        }
        activateBattleLogComplete = true;
    }
    IEnumerator BattleMessage(int startingCharacter, float characterPause = 0.025f)
    {
        int totalVisibleCharacters = battleText.textInfo.characterCount;
        int counter = startingCharacter;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            battleText.maxVisibleCharacters = visibleCount;
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Escape))
            {
                battleText.maxVisibleCharacters = totalVisibleCharacters;
                visibleCount = totalVisibleCharacters;
                counter = totalVisibleCharacters;
            }

            if (visibleCount >= totalVisibleCharacters) { break; }
            counter += 1;

            yield return new WaitForSeconds(characterPause);
        }
        endingCharacter = counter;
        messageComplete = true;
    }
    IEnumerator SelectAnimation(GameObject border)
    {
        while (true)
        {
            border.SetActive(false);
            yield return new WaitForSeconds(.15f);
            border.SetActive(true);
            yield return new WaitForSeconds(.15f);
        }
    }
    IEnumerator SelectFlicker(GameObject border)
    {
        border.SetActive(false);
        yield return new WaitForSeconds(.15f);
        border.SetActive(true);
        yield return new WaitForSeconds(.2f);
        border.SetActive(false);
        yield return new WaitForSeconds(.15f);
        border.SetActive(true);
    }
    IEnumerator TextBlinker(TMP_Text text)
    {
        while (true)
        {
            text.color = darkGrey;
            yield return new WaitForSeconds(.15f);
            text.color = Color.white;
            yield return new WaitForSeconds(.15f);
        }        
    }
    IEnumerator EndTurn()
    {
        //Clear out the dead
        for (int i = 0; i < activeBadGuys.Length; i++)
        {
            if (deadThisRound.Contains(activeBadGuys[i]))
            {
                deadThisRound.Remove(activeBadGuys[i]);
                activeBadGuys[i] = null;
            }
        }
        //check if battle is over
        bool endBattle = true;
        for (int i = 0; i < activeBadGuys.Length; i++)
        {
            if (activeBadGuys[i] != null) { endBattle = false; }
        }
        yield return new WaitForSeconds(.01f);
        //Reset turn
        if (!endBattle)
        {
            turnOrderBlackScreen.SetActive(true);
            yield return new WaitForSeconds(.5f);
            WhiteWash();
            CalculateTurnOrder();
            DisplayTurnOrder();
        }    
        yield return new WaitForSeconds(.5f);
        if (endBattle)
        {
            currentTheme.Stop();
            winBattle.Play();
            //clear remaining baddie from turn list
            for (int i = 0; i < turnOrder.Length; i++)
            {
                if (turnOrder[i] != ego)
                {
                    turnOrderNames[i].text = "";
                    turnOrder[i] = null;
                }
            }
            fightOverFade.SetActive(true);
            battleLog.SetActive(true);
            battleText.text = $"\nYou won!";
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(0));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(.5f);
            battleText.text = $"You won!";
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(endingCharacter -2));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(.25f);
            fightOverFadedScreen.SetActive(true);
            fightOverFade.SetActive(false);
            yield return new WaitForSeconds(.25f);
            continueArrow.SetActive(true);
            yield return new WaitUntil(controller.EnterPressed);
            continueArrow.SetActive(false);
            yield return new WaitForSeconds(.1f);
            winCoda.Play();
            if (multipleCorpses)
            {
                if (lootPurse == 0) { battleText.text = $"Not a single crystal between all of them. Must've been the losers at last night's poker game."; }
                else if (lootPurse == 1) { battleText.text = $"You find a single measly blue crystal; what a way to make a living!"; }
                else { battleText.text = $"Searching the grotesque carcasses, you find {lootPurse} crystals, which you take."; }
            }
            else
            {
                if (lootPurse == 0) { battleText.text = $"Not a single crystal. Must've been the big loser at last night's poker game."; }
                else if (lootPurse == 1) { battleText.text = $"You find a single measly blue crystal; what a way to make a living!"; }
                else { battleText.text = $"Searching the grotesque carcass, you find {lootPurse} crystals, which you take."; }
            }
            ego.blueCrystals += lootPurse;
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(0));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(.15f);
            continueArrow.SetActive(true);
            yield return new WaitUntil(controller.EnterPressed);
            continueArrow.SetActive(false);
            yield return new WaitForSeconds(.15f);
            if (lootBox.Count > 0)
            {
                if (lootBox.Count == 1)
                {
                    if (multipleCorpses) { battleText.text = $"And the enemies left a present!"; }
                    else { battleText.text = $"And the enemy left a present!"; }
                    yield return new WaitForSeconds(.01f);
                    messageComplete = false;
                    StartCoroutine(BattleMessage(0));
                    yield return new WaitUntil(MessageComplete);
                    yield return new WaitForSeconds(.15f);
                    continueArrow.SetActive(true);
                    yield return new WaitUntil(controller.EnterPressed);
                    continueArrow.SetActive(false);
                    yield return new WaitForSeconds(.05f);
                    battleText.text += $"\n\nThe {myTI.ToTitleCase(lootBox[0].nome)} looks useful.";
                    yield return new WaitForSeconds(.01f);
                    messageComplete = false;
                    StartCoroutine(BattleMessage(endingCharacter));
                    yield return new WaitUntil(MessageComplete);
                    yield return new WaitForSeconds(.25f);
                    battleText.text += $" You take it.";
                    yield return new WaitForSeconds(.01f);
                    messageComplete = false;
                    StartCoroutine(BattleMessage(endingCharacter));
                    yield return new WaitUntil(MessageComplete);
                    yield return new WaitForSeconds(.15f);
                    continueArrow.SetActive(true);
                    yield return new WaitUntil(controller.EnterPressed);
                    continueArrow.SetActive(false);
                    yield return new WaitForSeconds(.05f);
                }
                else
                {
                    if (multipleCorpses) { battleText.text = $"And the enemies left some presents!"; }
                    else { battleText.text = $"And the enemy left some presents!"; }
                    yield return new WaitForSeconds(.01f);
                    messageComplete = false;
                    StartCoroutine(BattleMessage(0));
                    yield return new WaitUntil(MessageComplete);
                    yield return new WaitForSeconds(.15f);
                    continueArrow.SetActive(true);
                    yield return new WaitUntil(controller.EnterPressed);
                    continueArrow.SetActive(false);
                    yield return new WaitForSeconds(.1f);
                    battleText.text += $"\n\nThe ";
                    //determine syntax
                    List<Item> condensedLootList = new List<Item>();
                    List<int> condensedLootListNumbers = new List<int>();
                    bool skipList = false;
                    for (int i = 0; i < lootBox.Count; i++)
                    {
                        for (int j = 0; j < condensedLootList.Count; j++)
                        {
                            if (lootBox[i].nome == condensedLootList[j].nome) { skipList = true; }
                        }
                        if (skipList) { continue; }
                        else
                        {
                            skipList = false;
                            int counter = 0;
                            for (int j = i; j < lootBox.Count; j++)
                            {
                                if (lootBox[i].nome == lootBox[j].nome) { counter++; }
                            }
                            condensedLootList.Add(lootBox[i]);
                            condensedLootListNumbers.Add(counter);
                        }
                    }
                    //now apply syntax
                    if (condensedLootList.Count == 1)
                    {
                        Debug.Log("count 1");
                        if (lootBox[0].nome[lootBox[0].nome.Length - 1] == 's') { battleText.text += $"{myTI.ToTitleCase(lootBox[0].nome)} "; }
                        else { battleText.text += $"{myTI.ToTitleCase(lootBox[0].nome)}s "; }                        
                    }
                    else if (condensedLootList.Count == 2)
                    {
                        Debug.Log("count 2");
                        if (condensedLootListNumbers[0] == 1) { battleText.text += $"{myTI.ToTitleCase(condensedLootList[0].nome)} and "; }
                        else
                        {
                            if (condensedLootList[0].nome[condensedLootList[0].nome.Length - 1] == 's') { battleText.text += $"{myTI.ToTitleCase(condensedLootList[0].nome)} and "; }
                            else { battleText.text += $"{myTI.ToTitleCase(condensedLootList[0].nome)}s and "; }
                        }
                        if (condensedLootListNumbers[1] == 1) { battleText.text += $"{myTI.ToTitleCase(condensedLootList[1].nome)} "; }
                        else
                        {
                            if (condensedLootList[1].nome[condensedLootList[1].nome.Length - 1] == 's') { battleText.text += $"{myTI.ToTitleCase(condensedLootList[1].nome)} "; }
                            else { battleText.text += $"{myTI.ToTitleCase(condensedLootList[1].nome)}s "; }
                        }
                    }
                    else
                    {
                        Debug.Log("count 3+");
                        for (int i = 0; i < condensedLootList.Count; i++)
                        {
                            if (condensedLootListNumbers[i] == 1)
                            {
                                battleText.text += myTI.ToTitleCase(condensedLootList[0].nome);
                                if (i == condensedLootList.Count - 2) { battleText.text += ", and the "; }
                                else if (i == condensedLootList.Count - 1) { battleText.text += " "; }
                                else { battleText.text += ", the "; }
                            }
                            else
                            {
                                if (condensedLootList[i].nome[condensedLootList[i].nome.Length - 1] == 's')
                                {
                                    battleText.text += myTI.ToTitleCase(condensedLootList[i].nome);
                                    if (i == condensedLootList.Count - 2) { battleText.text += ", and the "; }
                                    else if (i == condensedLootList.Count - 1) { battleText.text += " "; }
                                    else { battleText.text += ", the "; }
                                }
                                else
                                {
                                    battleText.text += $"{myTI.ToTitleCase(condensedLootList[i].nome)}s";
                                    if (i == condensedLootList.Count - 2) { battleText.text += ", and the "; }
                                    else if (i == condensedLootList.Count - 1) { battleText.text += " "; }
                                    else { battleText.text += ", the "; }
                                }
                            }
                        }
                    }                    
                    battleText.text += $"look useful.";
                    yield return new WaitForSeconds(.01f);
                    messageComplete = false;
                    StartCoroutine(BattleMessage(endingCharacter));
                    yield return new WaitUntil(MessageComplete);
                    yield return new WaitForSeconds(.25f);
                    battleText.text += $" You take them.";
                    yield return new WaitForSeconds(.01f);
                    messageComplete = false;
                    StartCoroutine(BattleMessage(endingCharacter));
                    yield return new WaitUntil(MessageComplete);
                    yield return new WaitForSeconds(.15f);
                    continueArrow.SetActive(true);
                    yield return new WaitUntil(controller.EnterPressed);
                    continueArrow.SetActive(false);
                    yield return new WaitForSeconds(.05f);
                }
                for (int i = 0; i < lootBox.Count; i++)
                {
                    controller.interactableItems.inventory.Add(lootBox[i]);
                }
            }
            yield return new WaitForSeconds(.25f);

            battleLogGreyScreen.SetActive(true);
            yield return new WaitForSeconds(.5f);
            battleLog.SetActive(false);
            yield return new WaitForSeconds(.3f);
            fightOverWhiteScreen.SetActive(true);
            yield return new WaitForSeconds(.2f);
            battleLogGreyScreen.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            winCoda.Stop();
            combatScreen.SetActive(false);
            fadeFromWhite1.SetActive(true);
            fightOverWhiteScreen.SetActive(false);
            //return to game
            controller.demoScript.demoProceed = true;
            yield return new WaitForSeconds(1f);
            fadeFromWhite1.SetActive(false);
        }
        else
        {
            turnOrderBlackScreen.SetActive(false);
            StartCoroutine(TurnDistributor());
        }
    }
    IEnumerator CheckTheDead()
    {
        for (int i = 0; i < activeBadGuys.Length; i++)
        {
            if (activeBadGuys[i] != null)
            {
                if (activeBadGuys[i].allStats[0].value == 0 && !deadThisRound.Contains(activeBadGuys[i]))
                {
                    //remove flank effect if applicable
                    if (activeBadGuys[i].combatSlot == slotFlank1 || activeBadGuys[i].combatSlot == slotFlank2)
                    {
                        Effect removingFlank = null;
                        for (int j = 0; j < ego.activeEffects.Count; j++)
                        {
                            if (ego.activeEffects[j].title == "Flanked")
                            {
                                removingFlank = ego.activeEffects[j];
                                break;
                            }
                        }
                        RemoveEffect(ego, removingFlank);
                    } 
                    
                    StartCoroutine(ClearTheDead(activeBadGuys[i]));
                    battleLog.SetActive(true);
                    battleText.text = $"{activeBadGuys[i].nome} has been defeated!";
                    yield return new WaitForSeconds(.01f);
                    messageComplete = false;
                    StartCoroutine(BattleMessage(0));
                    yield return new WaitUntil(MessageComplete);
                    yield return new WaitForSeconds(.75f);
                    battleLogGreyScreen.SetActive(true);
                    yield return new WaitForSeconds(.5f);
                    battleLog.SetActive(false);
                    yield return new WaitForSeconds(.5f);
                    battleLogGreyScreen.SetActive(false);
                }
            }
        }
        deadCheckComplete = true;
        if (ego.allStats[0].value == 0)
        {
            deadCheckComplete = false;
            StartCoroutine(EgoDead());
        }
    }
    IEnumerator EgoDead()
    {
        yield return new WaitForSeconds(.25f);
        currentTheme.Stop();
        loseBattle.Play();
        fightOverFade.SetActive(true);
        battleLog.SetActive(true);
        battleText.text = $"You've fallen in battle.";
        yield return new WaitForSeconds(.01f);
        messageComplete = false;
        StartCoroutine(BattleMessage(0, 0.05f));
        yield return new WaitUntil(MessageComplete);
        yield return new WaitForSeconds(1f);
        fightOverFadedScreen.SetActive(true);
        fightOverFade.SetActive(false);
        yield return new WaitForSeconds(1.25f);
        continueArrow.SetActive(true);
        yield return new WaitUntil(controller.EnterPressed);
        continueArrow.SetActive(false);
        yield return new WaitForSeconds(.1f);
        battleLogGreyScreen.SetActive(true);
        yield return new WaitForSeconds(.5f);
        battleLog.SetActive(false);
        yield return new WaitForSeconds(.3f);
        fightOverWhiteScreen.SetActive(true);
        yield return new WaitForSeconds(.2f);
        battleLogGreyScreen.SetActive(false);
        yield return new WaitForSeconds(.5f);
        loseBattle.Stop();
        combatScreen.SetActive(false);
        fadeFromWhite1.SetActive(true);
        fightOverWhiteScreen.SetActive(false);
        //return to game
        yield return new WaitForSeconds(1f);
        fadeFromWhite1.SetActive(false);
        controller.demoScript.DemoDeath();
        StopAllCoroutines();
    }
    IEnumerator ClearTheDead(BadGuy deadGuy)
    {
        //add loot to end result
        for (int i = 0; i < deadGuy.itemLoot.Count; i++) { lootBox.Add(deadGuy.itemLoot[i]); }
        lootPurse += Random.Range(deadGuy.crystalLootMin, deadGuy.crystalLootMax + 1);
        //remove any effects lingering
        for (int i = 0; i < deadGuy.activeEffects.Count; i++) { RemoveEffect(deadGuy, deadGuy.activeEffects[i]); }
        //update defeated list
        bool addBaddie = false;
        for (int i = 0; i < ego.defeatedBadGuys.Count; i++)
        {
            if (ego.defeatedBadGuys[i].nome == deadGuy.nome) { break; }
            addBaddie = true;
        }
        if (addBaddie)
        {
            for (int i = 0; i < allBadGuys.Length; i++)
            {
                if (deadGuy.nome == allBadGuys[i].nome) { ego.defeatedBadGuys.Add(Instantiate(allBadGuys[i])); }
            }
        }
        //commence clear
        badGuyDie.Play();
        deadThisRound.Add(deadGuy);
        //check if all are dead
        deadCount++;
        if (deadCount == totalBaddiesToKill) { battleIsWon = true; }
        //continue with clear
        enemySlotGreyScreen.transform.position = deadGuy.combatSlot.transform.position;
        enemySlotGreyScreen.SetActive(true);
        yield return new WaitForSeconds(.5f);
        deadGuy.combatSlot.SetActive(false);
        yield return new WaitForSeconds(.5f);
        enemySlotGreyScreen.SetActive(false);
        deadGuy.activeEffects.Clear();
        deadGuy.chosenAction = "";
        deadGuy.displayAction = "Dead";
        deadGuy.chosenTarget = null;
    }
    void WhiteWash()
    {
        egoDoneArrow.SetActive(false);
        slot1DoneArrow.SetActive(false);
        slot2aDoneArrow.SetActive(false);
        slot2bDoneArrow.SetActive(false);
        slot3bDoneArrow.SetActive(false);
        slot3cDoneArrow.SetActive(false);
        slotFlank1DoneArrow.SetActive(false);
        slotFlank2DoneArrow.SetActive(false);
        for (int i = 0; i < egoCombatOptions.Length; i++) { egoCombatOptions[i].color = Color.white; }
        special1.color = Color.white;
        special2a.color = Color.white;
        special2b.color = Color.white;
        special3b.color = Color.white;
        special3c.color = Color.white;
        specialFlank1.color = Color.white;
        specialFlank2.color = Color.white;
        attack1.color = Color.white;
        attack2a.color = Color.white;
        attack2b.color = Color.white;
        attack3b.color = Color.white;
        attack3c.color = Color.white;
        attackFlank1.color = Color.white;
        attackFlank2.color = Color.white;
        defend1.color = Color.white;
        defend2a.color = Color.white;
        defend2b.color = Color.white;
        defend3b.color = Color.white;
        defend3c.color = Color.white;
        defendFlank1.color = Color.white;
        defendFlank2.color = Color.white;
        inventory1.color = Color.white;
        inventory2a.color = Color.white;
        inventory2b.color = Color.white;
        inventory3b.color = Color.white;
        inventory3c.color = Color.white;
        inventoryFlank1.color = Color.white;
        inventoryFlank2.color = Color.white;
    }
    string BoldText(string text) { return $"<b>>{text}<</b>"; }
    string DeBoldText(string boldText)
    {
        string text = boldText.Replace("<b>>", "");
        text = text.Replace("<</b>", "");
        return text;
    }


    bool ActionSelected() { return actionSelected; }
    bool ActionComplete() { return actionComplete; }
    bool MessageComplete() { return messageComplete; }
    bool ActivateBattleLogComplete() { return activateBattleLogComplete; }
    bool InventoryComplete() { return inventoryComplete; }
    bool DeadCheckComplete() { return deadCheckComplete; }
    bool PotionComplete() { return potionComplete; }
    bool EffectComplete() { return effectComplete; }






    // Update is called once per frame
    void Update()
    {
        if (battleIsWon) { actionSelected = true; }
        //equipment
        if (ego.equippedWeapon != null) { combatWeaponDisplay.text = ego.equippedWeapon.nome; }
        else { combatWeaponDisplay.text = "None"; }
        if (ego.equippedArmor != null) { combatArmorDisplay.text = ego.equippedArmor.nome; }
        else { combatArmorDisplay.text = "None"; }
        if (ego.equippedShield != null) { combatShieldDisplay.text = ego.equippedShield.nome; }
        else { combatShieldDisplay.text = "None"; }
        
        //HP totals
        for (int i = 0; i < activeBadGuys.Length; i++)
        {
            if (activeBadGuys[i] != null)
            {
                if (activeBadGuys[i].combatSlot == slot1) { curHP1.text = activeBadGuys[i].allStats[0].value.ToString(); }
                else if (activeBadGuys[i].combatSlot == slot2a) { curHP2a.text = activeBadGuys[i].allStats[0].value.ToString(); }
                else if (activeBadGuys[i].combatSlot == slot2b) { curHP2b.text = activeBadGuys[i].allStats[0].value.ToString(); }
                else if (activeBadGuys[i].combatSlot == slot3b) { curHP3b.text = activeBadGuys[i].allStats[0].value.ToString(); }
                else if (activeBadGuys[i].combatSlot == slot3c) { curHP3c.text = activeBadGuys[i].allStats[0].value.ToString(); }
                else if (activeBadGuys[i].combatSlot == slotFlank1) { curHPFlank1.text = activeBadGuys[i].allStats[0].value.ToString(); }
                else if (activeBadGuys[i].combatSlot == slotFlank2) { curHPFlank2.text = activeBadGuys[i].allStats[0].value.ToString(); }
            }            
        }
        curHPEgo.text = ego.allStats[0].value.ToString();
        //ego die
        //if (ego.allStats[0].value == 0 && !actionSelected) { StartCoroutine(EgoDead()); }

        //turn order actions
        for (int i = 0; i < turnOrderActions.Length; i++)
        {
            if (turnOrder[i] != null) { turnOrderActions[i].text = turnOrder[i].displayAction; }
            else { turnOrderActions[i].text = ""; }
        }

        //target HP between 0 and max
        for (int i = 0; i < activeBadGuys.Length; i++)
        {
            if (activeBadGuys[i] != null)
            {
                if (activeBadGuys[i].allStats[1].value < 0) { activeBadGuys[i].allStats[1].value = 0; }
                if (activeBadGuys[i].allStats[1].value > (activeBadGuys[i].allStats[2].value + activeBadGuys[i].allStats[2].effectValue)) { activeBadGuys[i].allStats[1].value = (activeBadGuys[i].allStats[2].value + activeBadGuys[i].allStats[2].effectValue); }
            }
        }
        if (ego.allStats[1].value < 0) { ego.allStats[1].value = 0; }
        if (ego.allStats[1].value > (ego.allStats[2].value + ego.allStats[2].effectValue)) { ego.allStats[1].value = (ego.allStats[2].value + ego.allStats[2].effectValue); }
    }
}
