using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Combat : MonoBehaviour
{
    //testing purposes
    public BadGuy testBadGuy;
    public int testNumber;

    public Ego ego;
    public GameObject arrow, egoDoneArrow, slot1DoneArrow, slot2aDoneArrow, slot2bDoneArrow, slot3bDoneArrow, slot3cDoneArrow, slotFlank1DoneArrow, slotFlank2DoneArrow;
    public GameObject[] egoArrowPositions, slot1ArrowPositions, slot2aArrowPositions, slot2bArrowPositions, slot3bArrowPositions, slot3cArrowPositions, slotFlank1ArrowPositions, slotFlank2ArrowPositions;
    public TMP_Text combatWeaponDisplay, combatArmorDisplay, combatShieldDisplay;
    public GameObject slotEgo, slot1, slot2a, slot2b, slot3b, slot3c, slotFlank1, slotFlank2;
    public TMP_Text name1, name2a, name2b, name3b, name3c, nameFlank1, nameFlank2;
    public TMP_Text special1, special2a, special2b, special3b, special3c, specialFlank1, specialFlank2;
    public TMP_Text attack1, attack2a, attack2b, attack3b, attack3c, attackFlank1, attackFlank2;
    public TMP_Text defend1, defend2a, defend2b, defend3b, defend3c, defendFlank1, defendFlank2;
    public TMP_Text inventory1, inventory2a, inventory2b, inventory3b, inventory3c, inventoryFlank1, inventoryFlank2;
    public TMP_Text curHPEgo, maxHPEgo, curHP1, maxHP1, curHP2a, maxHP2a, curHP2b, maxHP2b, curHP3b, maxHP3b, curHP3c, maxHP3c, curHPFlank1, maxHPFlank1, curHPFlank2, maxHPFlank2;
    public TMP_Text[] turnOrderNames;
    public TMP_Text[] turnOrderActions;
    public TMP_Text[] egoCombatOptions;

    BadGuy[] activeBadGuys = { null, null, null, null, null };
    BadGuy badGuy0, badGuy1, badGuy2, badGuy3, badGuy4;
    Character[] turnOrder = { null, null, null, null, null, null };
    List<int> usedInitValues = new List<int>();
    int currentArrowPosition;
    bool actionSelected;

    GameController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
        currentArrowPosition = 0;


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

    public void InitiateCombat(BadGuy badGuy, int numberOfBadGuys)
    {
        curHPEgo.text = ego.allStats[0].value.ToString();
        maxHPEgo.text = ego.allStats[2].value.ToString();

        //populate badguy array
        for (int i = 0; i < numberOfBadGuys; i++) { activeBadGuys[i] = Instantiate(badGuy); }
        for (int j = numberOfBadGuys; j < 4; j++) { activeBadGuys[j] = null; }

        //determine proper badguy UI layout
        if (numberOfBadGuys != 2)
        {
            slot1.SetActive(true);
            badGuy0 = activeBadGuys[0];
            badGuy0.combatSlot = slot1;
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
            curHP2a.text = activeBadGuys[0].allStats[0].value.ToString();
            maxHP2a.text = activeBadGuys[0].allStats[2].value.ToString();
            name2a.text = activeBadGuys[0].nome;
            special2a.text = activeBadGuys[0].specialAbility;
            slot2b.SetActive(true);
            badGuy1 = activeBadGuys[1];
            badGuy1.combatSlot = slot2b;
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
            curHP3b.text = activeBadGuys[1].allStats[0].value.ToString();
            maxHP3b.text = activeBadGuys[1].allStats[2].value.ToString();
            name3b.text = activeBadGuys[1].nome;
            special3b.text = activeBadGuys[1].specialAbility;
            slot3c.SetActive(true);
            badGuy2 = activeBadGuys[2];
            badGuy2.combatSlot = slot3c;
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
        for (int i = 0; i < 6; i++)
        {
            turnOrderNames[i].text = null;
            turnOrderActions[i].text = null;
        }

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
            if (activeBadGuys[0] != null && turnOrder[i] == badGuy0)
            {
                badGuy0.pendingAction = BadGuyLogic(badGuy0);
                StartCoroutine(BadGuyActionSelect(badGuy0));
            }
            else if (activeBadGuys[1] != null && turnOrder[i] == badGuy1)
            {
                badGuy1.pendingAction = BadGuyLogic(badGuy1);
                StartCoroutine(BadGuyActionSelect(badGuy1));
            }
            else if (activeBadGuys[2] != null && turnOrder[i] == badGuy2)
            {
                badGuy2.pendingAction = BadGuyLogic(badGuy2);
                StartCoroutine(BadGuyActionSelect(badGuy2));
            }
            else if (activeBadGuys[3] != null && turnOrder[i] == badGuy3)
            {
                badGuy3.pendingAction = BadGuyLogic(badGuy3);
                StartCoroutine(BadGuyActionSelect(badGuy3));
            }
            else if (activeBadGuys[4] != null && turnOrder[i] == badGuy4)
            {
                badGuy4.pendingAction = BadGuyLogic(badGuy4);
                StartCoroutine(BadGuyActionSelect(badGuy4));
            }
            else if (turnOrder[i] == ego) { StartCoroutine(EgoActionSelect()); }
            yield return new WaitUntil(ActionSelected);
        }
    }
    IEnumerator EgoActionSelect()
    {
        arrow.transform.position = egoArrowPositions[0].transform.position;
        arrow.SetActive(true);
        while (true)
        {
            if (currentArrowPosition < 0) { currentArrowPosition = 5; }
            if (currentArrowPosition > 5) { currentArrowPosition = 0; }
            arrow.transform.position = egoArrowPositions[currentArrowPosition].transform.position;
            yield return new WaitUntil(controller.ArrowOrEnterPressed);
            if (Input.GetKeyDown(KeyCode.UpArrow)) { currentArrowPosition--; }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) { currentArrowPosition++; }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                actionSelected = true;
                egoDoneArrow.transform.position = arrow.transform.position;
                egoDoneArrow.SetActive(true);
                arrow.SetActive(false);
                if (currentArrowPosition == 0)
                {
                    ego.currentAction = "Attack";
                }
                else if (currentArrowPosition == 1)
                {
                    ego.currentAction = "Defend";
                }
                else if (currentArrowPosition == 2)
                {
                    ego.currentAction = "Delay";
                }
                else if (currentArrowPosition == 3)
                {
                    ego.currentAction = "Flee";                    
                }
                else if (currentArrowPosition == 4)
                {
                    
                }
                else if (currentArrowPosition == 5)
                {
                   
                }
                egoCombatOptions[currentArrowPosition].color = Color.grey;
                break;
            }
        }
    }
    IEnumerator BadGuyActionSelect(BadGuy badGuy)
    {
        Debug.Log("badguy action select");
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
        if (badGuy.pendingAction == "Attack") { howManyMoves = 0; }
        else if (badGuy.pendingAction == "Defend") { howManyMoves = 1; }
        else if (badGuy.pendingAction == "Special") { howManyMoves = 2; }
        else if (badGuy.pendingAction == "Inventory") { howManyMoves = 3; }
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
            }
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
            if (badGuy.pendingAction == "Attack") { attack1.color = Color.grey; }
            else if (badGuy.pendingAction == "Defend") { defend1.color = Color.grey; }
            else if (badGuy.pendingAction == "Special") { special1.color = Color.grey; }
            else if (badGuy.pendingAction == "Inventory") { inventory1.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slot2a)
        {
            slot2aDoneArrow.transform.position = arrow.transform.position;
            slot2aDoneArrow.SetActive(true);
            if (badGuy.pendingAction == "Attack") { attack2a.color = Color.grey; }
            else if (badGuy.pendingAction == "Defend") { defend2a.color = Color.grey; }
            else if (badGuy.pendingAction == "Special") { special2a.color = Color.grey; }
            else if (badGuy.pendingAction == "Inventory") { inventory2a.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slot2b)
        {
            slot2bDoneArrow.transform.position = arrow.transform.position;
            slot2bDoneArrow.SetActive(true);
            if (badGuy.pendingAction == "Attack") { attack2b.color = Color.grey; }
            else if (badGuy.pendingAction == "Defend") { defend2b.color = Color.grey; }
            else if (badGuy.pendingAction == "Special") { special2b.color = Color.grey; }
            else if (badGuy.pendingAction == "Inventory") { inventory2b.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slot3b)
        {
            slot3bDoneArrow.transform.position = arrow.transform.position;
            slot3bDoneArrow.SetActive(true);
            if (badGuy.pendingAction == "Attack") { attack3b.color = Color.grey; }
            else if (badGuy.pendingAction == "Defend") { defend3b.color = Color.grey; }
            else if (badGuy.pendingAction == "Special") { special3b.color = Color.grey; }
            else if (badGuy.pendingAction == "Inventory") { inventory3b.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slot3c)
        {
            slot3cDoneArrow.transform.position = arrow.transform.position;
            slot3cDoneArrow.SetActive(true);
            if (badGuy.pendingAction == "Attack") { attack3c.color = Color.grey; }
            else if (badGuy.pendingAction == "Defend") { defend3c.color = Color.grey; }
            else if (badGuy.pendingAction == "Special") { special3c.color = Color.grey; }
            else if (badGuy.pendingAction == "Inventory") { inventory3c.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slotFlank1)
        {
            slotFlank1DoneArrow.transform.position = arrow.transform.position;
            slotFlank1DoneArrow.SetActive(true);
            if (badGuy.pendingAction == "Attack") { attackFlank1.color = Color.grey; }
            else if (badGuy.pendingAction == "Defend") { defendFlank1.color = Color.grey; }
            else if (badGuy.pendingAction == "Special") { specialFlank1.color = Color.grey; }
            else if (badGuy.pendingAction == "Inventory") { inventoryFlank1.color = Color.grey; }
        }
        else if (badGuy.combatSlot == slotFlank2)
        {
            slotFlank2DoneArrow.transform.position = arrow.transform.position;
            slotFlank2DoneArrow.SetActive(true);
            if (badGuy.pendingAction == "Attack") { attackFlank2.color = Color.grey; }
            else if (badGuy.pendingAction == "Defend") { defendFlank2.color = Color.grey; }
            else if (badGuy.pendingAction == "Special") { specialFlank2.color = Color.grey; }
            else if (badGuy.pendingAction == "Inventory") { inventoryFlank2.color = Color.grey; }
        }
        arrow.SetActive(false);
        badGuy.currentAction = badGuy.pendingAction;
        yield return new WaitForSeconds(.75f);
        actionSelected = true;
    }
    string BadGuyLogic(BadGuy badGuy)
    {
        Debug.Log("badguy logic");



        return "Inventory";
    }

    bool ActionSelected() { return actionSelected; }






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

        //turn order actions
        for (int i = 0; i < turnOrderActions.Length; i++)
        {
            if (turnOrder[i] != null) { turnOrderActions[i].text = turnOrder[i].currentAction; }            
        }
    }
}
