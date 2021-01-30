﻿using System.Collections;
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
    public GameObject arrow, egoDoneArrow, slot1DoneArrow, slot2aDoneArrow, slot2bDoneArrow, slot3bDoneArrow, slot3cDoneArrow, slotFlank1DoneArrow, slotFlank2DoneArrow;
    public GameObject[] egoArrowPositions, slot1ArrowPositions, slot2aArrowPositions, slot2bArrowPositions, slot3bArrowPositions, slot3cArrowPositions, slotFlank1ArrowPositions, slotFlank2ArrowPositions;
    public GameObject borderEgo, border1, border2a, border2b, border3b, border3c, borderFlank1, borderFlank2;
    public TMP_Text combatWeaponDisplay, combatArmorDisplay, combatShieldDisplay;
    public GameObject slotEgo, slot1, slot2a, slot2b, slot3b, slot3c, slotFlank1, slotFlank2;
    public GameObject effects1, effects2a, effects2b, effects3b, effects3c, effectsFlank1, effectsFlank2;
    public TMP_Text name1, name2a, name2b, name3b, name3c, nameFlank1, nameFlank2;
    public TMP_Text special1, special2a, special2b, special3b, special3c, specialFlank1, specialFlank2;
    public TMP_Text attack1, attack2a, attack2b, attack3b, attack3c, attackFlank1, attackFlank2;
    public TMP_Text defend1, defend2a, defend2b, defend3b, defend3c, defendFlank1, defendFlank2;
    public TMP_Text inventory1, inventory2a, inventory2b, inventory3b, inventory3c, inventoryFlank1, inventoryFlank2;
    public TMP_Text curHPEgo, maxHPEgo, curHP1, maxHP1, curHP2a, maxHP2a, curHP2b, maxHP2b, curHP3b, maxHP3b, curHP3c, maxHP3c, curHPFlank1, maxHPFlank1, curHPFlank2, maxHPFlank2;
    public TMP_Text[] turnOrderNames;
    public TMP_Text[] turnOrderActions;
    public TMP_Text[] egoCombatOptions;
    public GameObject battleLog, battleLogGreyScreen, turnOrderBlackScreen, enemySlotGreyScreen, fightOverFade, fightOverFadedScreen;
    public TMP_Text battleText, effectsText, invText;
    public GameObject invDisplay, invDisplayBorder, invOptions, invOptionsBorder, continueArrow;
    public TMP_Text combatInvDamage, combatInvCritMultiplier, combatInvToHitMod, combatInvArmorClass, combatInvCritResist, combatInvDmgReduction;
    public Scrollbar scrollBar;
    public List<TMP_Text> invActions = new List<TMP_Text>();
    public Effect[] allEffects;
    [HideInInspector] public float scrollRectValue;

    BadGuy[] activeBadGuys = { null, null, null, null, null };
    BadGuy badGuy0, badGuy1, badGuy2, badGuy3, badGuy4;
    Character[] turnOrder = { null, null, null, null, null, null };
    List<int> usedInitValues = new List<int>();
    List<BadGuy> deadThisRound = new List<BadGuy>();
    List<Item> lootBox = new List<Item>();
    int lootPurse = 0;
    int currentArrowPosition, endingCharacter;
    bool actionSelected, actionComplete, messageComplete, inventoryComplete, deadCheckComplete, multipleCorpses;
    bool unstrap = false;
    Color darkGrey = new Color(0.09411765f, 0.09411765f, 0.09411765f);
    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

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
        InitiateCombat(testBadGuy, testNumber);
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


    public void InitiateCombat(BadGuy badGuy, int numberOfBadGuys)
    {
        curHPEgo.text = ego.allStats[0].value.ToString();
        maxHPEgo.text = ego.allStats[2].value.ToString();
        turnOrderBlackScreen.SetActive(false);
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

        //determine proper badguy UI layout
        if (numberOfBadGuys != 2)
        {
            slot1.SetActive(true);
            badGuy0 = activeBadGuys[0];
            badGuy0.combatSlot = slot1;
            badGuy0.combatBorder = border1;
            curHP1.text = activeBadGuys[0].allStats[0].value.ToString();
            maxHP1.text = activeBadGuys[0].allStats[2].value.ToString();
            name1.text = activeBadGuys[0].nome;
            special1.text = activeBadGuys[0].specialAbility;
        }
        else
        {
            slot2a.SetActive(true);
            badGuy0 = activeBadGuys[0];
            badGuy0.combatSlot = slot2a;
            badGuy0.combatBorder = border2a;
            curHP2a.text = activeBadGuys[0].allStats[0].value.ToString();
            maxHP2a.text = activeBadGuys[0].allStats[2].value.ToString();
            name2a.text = activeBadGuys[0].nome;
            special2a.text = activeBadGuys[0].specialAbility;
            slot2b.SetActive(true);
            badGuy1 = activeBadGuys[1];
            badGuy1.combatSlot = slot2b;
            badGuy1.combatBorder = border2b;
            curHP2b.text = activeBadGuys[1].allStats[0].value.ToString();
            maxHP2b.text = activeBadGuys[1].allStats[2].value.ToString();
            name2b.text = activeBadGuys[1].nome;
            special2b.text = activeBadGuys[1].specialAbility;
        }
        if (numberOfBadGuys > 2)
        {
            slot3b.SetActive(true);
            badGuy1 = activeBadGuys[1];
            badGuy1.combatSlot = slot3b;
            badGuy1.combatBorder = border3b;
            curHP3b.text = activeBadGuys[1].allStats[0].value.ToString();
            maxHP3b.text = activeBadGuys[1].allStats[2].value.ToString();
            name3b.text = activeBadGuys[1].nome;
            special3b.text = activeBadGuys[1].specialAbility;
            slot3c.SetActive(true);
            badGuy2 = activeBadGuys[2];
            badGuy2.combatSlot = slot3c;
            badGuy2.combatBorder = border3c;
            curHP3c.text = activeBadGuys[2].allStats[0].value.ToString();
            maxHP3c.text = activeBadGuys[2].allStats[2].value.ToString();
            name3c.text = activeBadGuys[2].nome;
            special3c.text = activeBadGuys[2].specialAbility;
        }
        if (numberOfBadGuys > 3)
        {
            slotFlank1.SetActive(true);
            badGuy3 = activeBadGuys[3];
            badGuy3.combatSlot = slotFlank1;
            badGuy3.combatBorder = borderFlank1;
            curHPFlank1.text = activeBadGuys[3].allStats[0].value.ToString();
            maxHPFlank1.text = activeBadGuys[3].allStats[2].value.ToString();
            nameFlank1.text = activeBadGuys[3].nome;
            specialFlank1.text = activeBadGuys[3].specialAbility;
        }
        if (numberOfBadGuys > 4)
        {
            slotFlank2.SetActive(true);
            badGuy4 = activeBadGuys[4];
            badGuy4.combatSlot = slotFlank2;
            badGuy4.combatBorder = borderFlank2;
            curHPFlank2.text = activeBadGuys[4].allStats[0].value.ToString();
            maxHPFlank2.text = activeBadGuys[4].allStats[2].value.ToString();
            nameFlank2.text = activeBadGuys[4].nome;
            specialFlank2.text = activeBadGuys[4].specialAbility;
        }

        CalculateTurnOrder();
        DisplayTurnOrder();
        StartCoroutine(TurnDistributor());
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
            actionSelected = false;
            if (deadThisRound.Contains(turnOrder[i])) { actionSelected = true; }
            else if (activeBadGuys[0] != null && turnOrder[i] == badGuy0)
            {
                badGuy0.chosenAction = BadGuyLogic(badGuy0);
                StartCoroutine(BadGuyActionSelect(badGuy0));
            }
            else if (activeBadGuys[1] != null && turnOrder[i] == badGuy1)
            {
                badGuy1.chosenAction = BadGuyLogic(badGuy1);
                StartCoroutine(BadGuyActionSelect(badGuy1));
            }
            else if (activeBadGuys[2] != null && turnOrder[i] == badGuy2)
            {
                badGuy2.chosenAction = BadGuyLogic(badGuy2);
                StartCoroutine(BadGuyActionSelect(badGuy2));
            }
            else if (activeBadGuys[3] != null && turnOrder[i] == badGuy3)
            {
                badGuy3.chosenAction = BadGuyLogic(badGuy3);
                StartCoroutine(BadGuyActionSelect(badGuy3));
            }
            else if (activeBadGuys[4] != null && turnOrder[i] == badGuy4)
            {
                badGuy4.chosenAction = BadGuyLogic(badGuy4);
                StartCoroutine(BadGuyActionSelect(badGuy4));
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
        for (int i = 0; i < turnOrder.Length; i++)
        {
            //lower spell effect duration at turn order i
            for (int j = 0; j < ego.activeEffects.Count; j++)
            {
                if (ego.activeEffects[j].turnOrderTick == i) { ego.activeEffects[j].duration--; }
                if (ego.activeEffects[j].duration == 0) { RemoveEffect(ego, ego.activeEffects[j]); }
            }
            for (int j = 0; j < activeBadGuys.Length; j++)
            {
                if (activeBadGuys[j] != null)
                {
                    for (int k = 0; k < activeBadGuys[j].activeEffects.Count; k++)
                    {
                        if (activeBadGuys[j].activeEffects[k].turnOrderTick == i) { activeBadGuys[j].activeEffects[k].duration--; }
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
                if (turnOrder[i] == ego)
                {
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
                        else if (ego.chosenItem is Potion) { StartCoroutine(UsePotion()); }
                        actionComplete = true;
                    }
                }
                else //if (turnOrder[i] != jesse)
                {
                    BadGuy currentTurn = (BadGuy)turnOrder[i];
                    StartCoroutine(SelectFlicker(currentTurn.combatBorder));                    
                    if (turnOrder[i].displayAction == "Attack") { StartCoroutine(ExecuteBadGuyAttack((BadGuy)turnOrder[i])); }
                    else if (turnOrder[i].displayAction == "Inventory") { StartCoroutine(UsePotion()); }
                    else { StartCoroutine(SpecialAbility()); }
                }
                yield return new WaitUntil(ActionComplete);
                actionComplete = false;
            }
        }
        StartCoroutine(EndTurn());
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
            if (message[message.Length -1] == '!') { battleText.text += " A"; }
            else if (message[message.Length - 1] == ',') { battleText.text += " a"; }
            battleText.text += $"nd hits for {rolledDamage} damage!";

            ego.allStats[1].value = ego.allStats[0].value - rolledDamage;
            StopCoroutine(HPRoll(ego));
            StartCoroutine(HPRoll(ego));            
        }
        else if (d20 == 20)
        {
            if (message[message.Length - 1] == '!') { battleText.text += " A"; }
            else if (message[message.Length - 1] == ',') { battleText.text += " a"; }
            battleText.text += $"nd critically hits for {rolledDamage} damage!";

            ego.allStats[1].value = ego.allStats[0].value - rolledDamage;
            StopCoroutine(HPRoll(ego));
            StartCoroutine(HPRoll(ego));
        }
        else if (attackRoll == (egoArmorClass -1))
        {
            if (message[message.Length - 1] == '!') { battleText.text += " Just missed!"; }
            else if (message[message.Length - 1] == ',') { battleText.text += " and just missed!"; }
        }
        else
        {
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
    IEnumerator UsePotion()
    {
        yield return new WaitForSeconds(1f);
        actionComplete = true;
    }
    IEnumerator SpecialAbility()
    {
        yield return new WaitForSeconds(1f);
        actionComplete = true;
    }
    void AddEffect(Character target, int turnOrderOfCaster, Effect effect)
    {
        string color = "white";
        if (effect.color == Color.green) { color = "green"; }
        else if (effect.color == Color.red) { color = "red"; }

        //write on screen
        if (target == ego) { effectsText.text += $"<color={color}>{effect.title}</color>\n"; }
        else
        {
            BadGuy badTarget = (BadGuy)target;
            badTarget.combatEffects.text += $"<color={effect.color}>{effect.abbreviation}</color> ";
        }

        //modify stats
        for (int i = 0; i < target.allStats.Length; i++)
        {
            if (effect.stat == target.allStats[i].title) { target.allStats[i].effectValue += effect.potency; }
            if (effect.stat2 != null) { if (effect.stat2 == target.allStats[i].title) { target.allStats[i].effectValue += effect.potency2; } }
        }

        //add effect
        effect.turnOrderTick = turnOrderOfCaster;
        target.activeEffects.Add(effect);
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
            string replaceText = badTarget.combatEffects.text.Remove(badTarget.combatEffects.text.IndexOf($"<color={color}>{effect.abbreviation}</color> "), $"<color={color}>{effect.abbreviation}</color> ".Length);
            badTarget.combatEffects.text = replaceText;
        }

        //modify stats
        for (int i = 0; i < target.allStats.Length; i++)
        {
            if (effect.stat == target.allStats[i].title) { target.allStats[i].effectValue -= effect.potency; }
            if (effect.stat2 != null) { if (effect.stat2 == target.allStats[i].title) { target.allStats[i].effectValue -= effect.potency2; } }
        }

        //remove effect
        target.activeEffects.Remove(effect);
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
        
        battleText.text = $"You {verb} at {badGuy.nome},";
        yield return new WaitForSeconds(.01f);
        messageComplete = false;
        StartCoroutine(BattleMessage(0));
        yield return new WaitUntil(MessageComplete);
        yield return new WaitForSeconds(.5f);

        if (attackRoll >= badGuyArmorClass && (d20 < 20))
        {
            battleText.text += $" and hit for {rolledDamage} damage!";
            
            badGuy.allStats[1].value = badGuy.allStats[0].value - rolledDamage;
            StopCoroutine(HPRoll(badGuy));
            StartCoroutine(HPRoll(badGuy));
        }
        else if (d20 == 20)
        {
            battleText.text += $" and critically hit for {rolledDamage} damage!";

            badGuy.allStats[1].value = badGuy.allStats[0].value - rolledDamage;
            StopCoroutine(HPRoll(badGuy));
            StartCoroutine(HPRoll(badGuy));
        }
        else if (attackRoll == (badGuyArmorClass - 1)) { battleText.text += " and just miss!"; }
        else { battleText.text += " but miss!"; }
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
        while (true)
        {
            if (currentArrowPosition < 0) { currentArrowPosition = 5; }
            if (currentArrowPosition > 5) { currentArrowPosition = 0; }
            arrow.transform.position = egoArrowPositions[currentArrowPosition].transform.position;
            yield return new WaitUntil(controller.UpDownEnterPressed);
            if (Input.GetKeyDown(KeyCode.UpArrow)) { currentArrowPosition--; }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) { currentArrowPosition++; }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
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
                        if (activeBadGuys[i] != null) { activeBadGuyBorders.Add(activeBadGuys[i].combatBorder); }
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
                            StopCoroutine(selection);
                            activeBadGuyBorders[borderSelected].SetActive(true);
                            turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                            borderSelected--;
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            StopCoroutine(selection);
                            activeBadGuyBorders[borderSelected].SetActive(true);
                            turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                            borderSelected++;
                        }
                        else if (Input.GetKeyDown(KeyCode.Escape))
                        {
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
                    ego.displayAction = "Defend";
                    ego.chosenAction = "Defend";
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
                    AddEffect(ego, 1, allEffects[0]);
                    AddEffect(ego, 1, allEffects[1]);
                    AddEffect(ego, 1, allEffects[2]);
                    AddEffect(ego, 1, allEffects[3]);
                    AddEffect(ego, 1, allEffects[4]);
                    controller.interactableItems.inventory.Add(controller.registerObjects.allItems[0]);
                    controller.interactableItems.inventory.Add(controller.registerObjects.allItems[1]);
                    controller.interactableItems.inventory.Add(controller.registerObjects.allItems[2]);
                    controller.interactableItems.inventory.Add(controller.registerObjects.allItems[3]);
                    controller.interactableItems.inventory.Add(controller.registerObjects.allItems[4]);
                    for (int i = 0; i < activeBadGuys.Length; i++)
                    {
                        if (activeBadGuys[i] != null) { activeBadGuys[i].allStats[0].value = 1; }
                    }
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
                            //check if repeated effect for correct selection
                            for (int i = 0; i < selectedElement; i++)
                            {
                                if (ego.activeEffects[selectedElement] == ego.activeEffects[i]) { doublesCounter++; }
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

                            newText += "<b><size=15>[";

                            for (int i = effectIndex; i < effectIndex + effectLength; i++) { newText += effectsText.text[i]; }
                            
                            newText += "]</size></b>";

                            for (int i = effectIndex + effectLength; i < effectsText.text.Length; i++) { newText += effectsText.text[i]; }
                            
                            effectsText.text = newText;


                            yield return new WaitUntil(controller.UpDownEnterEscPressed);
                            if (Input.GetKeyDown(KeyCode.UpArrow)) { selectedElement--; }
                            else if (Input.GetKeyDown(KeyCode.DownArrow)) { selectedElement++; }
                            else if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                effectsText.text = normalEffectsText;
                                egoDoneArrow.SetActive(false);
                                arrow.SetActive(true);
                                egoCombatOptions[4].color = Color.white;
                                break;
                            }
                            else if (Input.GetKeyDown(KeyCode.Return))
                            {
                                selectedEffect = ego.activeEffects[selectedElement];
                                controller.OpenPopUpWindow(selectedEffect.title, "", selectedEffect.description, "", "", "", "", "Press ESC to return");
                                //copying font from achievements for simplicity
                                controller.achievements.originalFont = controller.popUpMessage.font;
                                controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                yield return new WaitUntil(controller.EscPressed);
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
        else if (currentHP >= (.25 * combinedMaxHP))
        {
            if (badGuy.potionBelt.Length > 0) { return "Inventory"; }
            else
            {   //strategist call for help
                if (badGuy.nome == "A Bold Yet Discretionary Strategist") { return "Call"; }
                else { return RegularAction(); }
            }
        }
        return "Attack";

        string RegularAction()
        {
            int d100 = Random.Range(1, 101);
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

                    return badGuy.normalAIRay[i].title;
                }
            }
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


        if (controller.interactableItems.inventory.Count > 0)
        {
            string normalInvText = invText.text;
            Item selectedItem = alreadyListed[0];
            int selectedElement = 0;
            bool itemUsed = false;
            bool dropUsed = false;
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

                newText += "<b><size=40>[";

                for (int i = invIndex; i < invIndex + itemLength; i++) { newText += invText.text[i]; }

                newText += "]</size></b>";

                for (int i = invIndex + itemLength; i < invText.text.Length; i++) { newText += invText.text[i]; }

                invText.text = newText;

                InvStats(alreadyListed[selectedElement]);

                yield return new WaitUntil(controller.UpDownEnterEscPressed);
                if (Input.GetKeyDown(KeyCode.UpArrow)) { selectedElement--; }
                else if (Input.GetKeyDown(KeyCode.DownArrow)) { selectedElement++; }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {                    
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
                            StopCoroutine(blinker);
                            invActions[option].color = Color.white;
                            option--;
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            StopCoroutine(blinker);
                            invActions[option].color = Color.white;
                            option++;
                        }
                        else if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            StopCoroutine(blinker);
                            invActions[option].color = Color.white;
                            invOptions.SetActive(false);
                            invOptionsBorder.SetActive(false);
                            break;
                        }
                        else if (Input.GetKeyDown(KeyCode.Return))
                        {
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
                                                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) { yesSelected = !yesSelected; }
                                                else if (Input.GetKeyDown(KeyCode.Return))
                                                {
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
                                                            if (Input.GetKeyDown(KeyCode.UpArrow)) { selectedShieldElement--; }
                                                            else if (Input.GetKeyDown(KeyCode.DownArrow)) { selectedShieldElement++; }
                                                            else if (Input.GetKeyDown(KeyCode.Escape))
                                                            {
                                                                selectedShield = null;
                                                                break;
                                                            }
                                                            else if (Input.GetKeyDown(KeyCode.Return))
                                                            {
                                                                if (alreadyListed[selectedShieldElement] is Shield)
                                                                {
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
                                            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) { yesSelected = !yesSelected; }
                                            else if (Input.GetKeyDown(KeyCode.Return))
                                            {
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
                                if (selectedItem.beneficial) { borderSelected = activeBorders.IndexOf(borderEgo); }

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
                                        StopCoroutine(selection);
                                        activeBorders[borderSelected].SetActive(true);
                                        turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                        borderSelected--;
                                    }
                                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                                    {
                                        StopCoroutine(selection);
                                        activeBorders[borderSelected].SetActive(true);
                                        turnOrderNames[nameToHighlight].text = DeBoldText(turnOrderNames[nameToHighlight].text);
                                        borderSelected++;
                                    }
                                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                                    {
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
                                    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) { yesSelected = !yesSelected; }
                                    else if (Input.GetKeyDown(KeyCode.Return))
                                    {
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
            yield return new WaitForSeconds(.2f);
        }
    }
    IEnumerator BattleMessage(int startingCharacter)
    {
        int totalVisibleCharacters = battleText.textInfo.characterCount;
        int counter = startingCharacter;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            battleText.maxVisibleCharacters = visibleCount;
            if (Input.GetKey(KeyCode.Return))
            {
                battleText.maxVisibleCharacters = totalVisibleCharacters;
                visibleCount = totalVisibleCharacters;
                counter = totalVisibleCharacters;
            }

            if (visibleCount >= totalVisibleCharacters) { break; }
            counter += 1;

            yield return new WaitForSeconds(0.025f);
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
            yield return new WaitForSeconds(.25f);
            battleText.text = $"You won!";
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(BattleMessage(endingCharacter -2));
            yield return new WaitUntil(MessageComplete);
            yield return new WaitForSeconds(.5f);
            fightOverFadedScreen.SetActive(true);
            fightOverFade.SetActive(false);
            yield return new WaitForSeconds(.25f);
            continueArrow.SetActive(true);
            yield return new WaitUntil(controller.EnterPressed);
            continueArrow.SetActive(false);
            yield return new WaitForSeconds(.15f);
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
            if (lootBox.Count > 0)
            {
                yield return new WaitForSeconds(1f);
                if (lootBox.Count == 1)
                {
                    if (multipleCorpses) { battleText.text = $"And the enemies left a present!\n\n"; }
                    else { battleText.text = $"And the enemy left a present!\n\n"; }
                    yield return new WaitForSeconds(.01f);
                    messageComplete = false;
                    StartCoroutine(BattleMessage(0));
                    yield return new WaitUntil(MessageComplete);
                    yield return new WaitForSeconds(.25f);
                    continueArrow.SetActive(true);
                    yield return new WaitUntil(controller.EnterPressed);
                    continueArrow.SetActive(false);
                    yield return new WaitForSeconds(.15f);
                    battleText.text += $"The {myTI.ToTitleCase(lootBox[0].nome)} looks useful.";
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
                    yield return new WaitForSeconds(.25f);
                    continueArrow.SetActive(true);
                    yield return new WaitUntil(controller.EnterPressed);
                    continueArrow.SetActive(false);
                    yield return new WaitForSeconds(.15f);
                }
                else
                {
                    if (multipleCorpses) { battleText.text = $"And the enemies left some presents!\n\n"; }
                    else { battleText.text = $"And the enemy left some presents!\n\n"; }
                    messageComplete = false;
                    StartCoroutine(BattleMessage(0));
                    yield return new WaitUntil(MessageComplete);
                    yield return new WaitForSeconds(.25f);
                    continueArrow.SetActive(true);
                    yield return new WaitUntil(controller.EnterPressed);
                    continueArrow.SetActive(false);
                    yield return new WaitForSeconds(.15f);
                    battleText.text += $" The ";
                    //determine syntax
                    List<LootCounter> transientLootList = new List<LootCounter>();
                    LootCounter transientLootCounter = null;
                    transientLootCounter.item = lootBox[0];
                    transientLootCounter.count = 1;
                    transientLootList.Add(transientLootCounter);
                    for (int i = 1; i < lootBox.Count; i++)
                    {
                        for (int j = 0; j < transientLootList.Count; j++)
                        {
                            if (transientLootList[j].item.nome == lootBox[i].nome) { transientLootList[j].count++; }
                            else
                            {
                                transientLootCounter.item = lootBox[i];
                                transientLootCounter.count = 1;
                                transientLootList.Add(transientLootCounter);
                            }
                        }
                    }
                    //now apply syntax
                    if (transientLootList.Count == 1)
                    {
                        if (lootBox[0].nome[lootBox[0].nome.Length - 1] == 's') { battleText.text += $"{myTI.ToTitleCase(lootBox[0].nome)} "; }
                        else { battleText.text += $"{myTI.ToTitleCase(lootBox[0].nome)}s "; }                        
                    }
                    else if (transientLootList.Count == 2)
                    {
                        if (transientLootList[0].count == 1) { battleText.text += $"{myTI.ToTitleCase(transientLootList[0].item.nome)} and "; }
                        else
                        {
                            if (transientLootList[0].item.nome[transientLootList[0].item.nome.Length - 1] == 's') { battleText.text += $"{myTI.ToTitleCase(transientLootList[0].item.nome)} and "; }
                            else { battleText.text += $"{myTI.ToTitleCase(transientLootList[0].item.nome)}s and "; }
                        }
                        if (transientLootList[1].count == 1) { battleText.text += $"{myTI.ToTitleCase(transientLootList[0].item.nome)} "; }
                        else
                        {
                            if (transientLootList[1].item.nome[transientLootList[1].item.nome.Length - 1] == 's') { battleText.text += $"{myTI.ToTitleCase(transientLootList[0].item.nome)} "; }
                            else { battleText.text += $"{myTI.ToTitleCase(transientLootList[1].item.nome)}s "; }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < transientLootList.Count; i++)
                        {
                            if (transientLootList[i].count == 1)
                            {
                                battleText.text += myTI.ToTitleCase(transientLootList[0].item.nome);
                                if (i == transientLootList.Count - 2) { battleText.text += ", and the "; }
                                else if (i == transientLootList.Count - 1) { battleText.text += " "; }
                                else { battleText.text += ", the "; }
                            }
                            else
                            {
                                if (transientLootList[0].item.nome[transientLootList[0].item.nome.Length - 1] == 's')
                                {
                                    battleText.text += myTI.ToTitleCase(transientLootList[0].item.nome);
                                    if (i == transientLootList.Count - 2) { battleText.text += ", and the "; }
                                    else if (i == transientLootList.Count - 1) { battleText.text += " "; }
                                    else { battleText.text += ", the "; }
                                }
                                else
                                {
                                    battleText.text += $"{myTI.ToTitleCase(transientLootList[0].item.nome)}s";
                                    if (i == transientLootList.Count - 2) { battleText.text += ", and the "; }
                                    else if (i == transientLootList.Count - 1) { battleText.text += " "; }
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
                    yield return new WaitForSeconds(.25f);
                    continueArrow.SetActive(true);
                    yield return new WaitUntil(controller.EnterPressed);
                    continueArrow.SetActive(false);
                    yield return new WaitForSeconds(.15f);
                }
                for (int i = 0; i < lootBox.Count; i++)
                {
                    controller.interactableItems.inventory.Add(lootBox[i]);
                }                
            }
            
            //battleLogGreyScreen.SetActive(true);
            //yield return new WaitForSeconds(.5f);
            //battleLog.SetActive(false);
            //yield return new WaitForSeconds(.5f);
            //battleLogGreyScreen.SetActive(false);
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
    }
    IEnumerator ClearTheDead(BadGuy deadGuy)
    {
        for (int i = 0; i < deadGuy.itemLoot.Count; i++) { lootBox.Add(deadGuy.itemLoot[i]); }
        lootPurse += Random.Range(deadGuy.crystalLootMin, deadGuy.crystalLootMax + 1);

        deadThisRound.Add(deadGuy);
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
    bool InventoryComplete() { return inventoryComplete; }
    bool DeadCheckComplete() { return deadCheckComplete; }






    // Update is called once per frame
    void Update()
    {
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

        //turn order actions
        for (int i = 0; i < turnOrderActions.Length; i++)
        {
            if (turnOrder[i] != null) { turnOrderActions[i].text = turnOrder[i].displayAction; }
            else { turnOrderActions[i].text = ""; }
        }

        //target HP non-negative
        for (int i = 0; i < activeBadGuys.Length; i++)
        {
            if (activeBadGuys[i] != null)
            {
                if (activeBadGuys[i].allStats[1].value < 0) { activeBadGuys[i].allStats[1].value = 0; }
            }
        }
        if (ego.allStats[1].value < 0) { ego.allStats[1].value = 0; }
    }
}
