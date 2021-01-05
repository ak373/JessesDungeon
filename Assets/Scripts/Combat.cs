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
    public TMP_Text curHPEgo, maxHPEgo, curHP1, maxHP1, curHP2a, maxHP2a, curHP2b, maxHP2b, curHP3b, maxHP3b, curHP3c, maxHP3c, curHPFlank1, maxHPFlank1, curHPFlank2, maxHPFlank2;
    public TMP_Text[] turnOrderNames;
    public TMP_Text[] turnOrderActions;
    public TMP_Text[] egoCombatOptions;

    BadGuy[] activeBadGuys = { null, null, null, null, null };
    BadGuy badGuy0, badGuy1, badGuy2, badGuy3, badGuy4;
    Stats[] turnOrder = { null, null, null, null, null, null };
    List<int> usedInitValues = new List<int>();
    int currentArrowPosition;
    GameController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
        currentArrowPosition = 0;

        //testing purposes
        InitiateCombat(testBadGuy, testNumber);
    }

    public void InitiateCombat(BadGuy badGuy, int numberOfBadGuys)
    {
        curHPEgo.text = ego.currentHitPoints.ToString();
        maxHPEgo.text = ego.maxHitPoints.ToString();

        //populate badguy array
        for (int i = 0; i < numberOfBadGuys; i++) { activeBadGuys[i] = Instantiate(badGuy); }
        for (int j = numberOfBadGuys; j < 4; j++) { activeBadGuys[j] = null; }

        //determine proper badguy UI layout
        if (numberOfBadGuys != 2)
        {
            slot1.SetActive(true);
            badGuy0 = activeBadGuys[0];
            badGuy0.combatSlot = slot1;
            curHP1.text = activeBadGuys[0].currentHitPoints.ToString();
            maxHP1.text = activeBadGuys[0].maxHitPoints.ToString();
            name1.text = activeBadGuys[0].nome;
        }
        else
        {
            slot2a.SetActive(true);
            badGuy0 = activeBadGuys[0];
            badGuy0.combatSlot = slot2a;
            curHP2a.text = activeBadGuys[0].currentHitPoints.ToString();
            maxHP2a.text = activeBadGuys[0].maxHitPoints.ToString();
            name2a.text = activeBadGuys[0].nome;
            slot2b.SetActive(true);
            badGuy1 = activeBadGuys[1];
            badGuy1.combatSlot = slot2b;
            curHP2b.text = activeBadGuys[1].currentHitPoints.ToString();
            maxHP2b.text = activeBadGuys[1].maxHitPoints.ToString();
            name2b.text = activeBadGuys[1].nome;
        }
        if (numberOfBadGuys > 2)
        {
            slot3b.SetActive(true);
            badGuy1 = activeBadGuys[1];
            badGuy1.combatSlot = slot3b;
            curHP3b.text = activeBadGuys[1].currentHitPoints.ToString();
            maxHP3b.text = activeBadGuys[1].maxHitPoints.ToString();
            name3b.text = activeBadGuys[1].nome;
            slot3c.SetActive(true);
            badGuy2 = activeBadGuys[2];
            badGuy2.combatSlot = slot3c;
            curHP3c.text = activeBadGuys[2].currentHitPoints.ToString();
            maxHP3c.text = activeBadGuys[2].maxHitPoints.ToString();
            name3c.text = activeBadGuys[2].nome;
        }
        if (numberOfBadGuys > 3)
        {
            slotFlank1.SetActive(true);
            badGuy3 = activeBadGuys[3];
            badGuy3.combatSlot = slotFlank1;
            curHPFlank1.text = activeBadGuys[3].currentHitPoints.ToString();
            maxHPFlank1.text = activeBadGuys[3].maxHitPoints.ToString();
            nameFlank1.text = activeBadGuys[3].nome;
        }
        if (numberOfBadGuys > 4)
        {
            slotFlank2.SetActive(true);
            badGuy4 = activeBadGuys[4];
            badGuy4.combatSlot = slotFlank2;
            curHPFlank2.text = activeBadGuys[4].currentHitPoints.ToString();
            maxHPFlank2.text = activeBadGuys[4].maxHitPoints.ToString();
            nameFlank2.text = activeBadGuys[4].nome;
        }

        CalculateTurnOrder();
        DisplayTurnOrder();
        TurnDistributor();
    }
    void CalculateTurnOrder()
    {
        usedInitValues.Clear();
        //ego roll
        ego.currentInit = Random.Range(0, 21) + ego.initMod;
        usedInitValues.Add(ego.currentInit);

        //badguy rolls
        for (int i = 0; i < activeBadGuys.Length; i++) { if (activeBadGuys[i] != null) { activeBadGuys[i].currentInit = GenerateUniqueRoll(activeBadGuys[i]); } }
        
        int GenerateUniqueRoll(BadGuy badGuy)
        {
            int roll = Random.Range(0, 21) + badGuy.initMod;
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
    void TurnDistributor()
    {
        for (int i = 0; i < turnOrder.Length; i++)
        {
            if (turnOrder[i] == badGuy0)
            {               
                badGuy0.currentAction = BadGuyLogic();
                StartCoroutine(BadGuyActionSelect(badGuy0));
            }
            else if (turnOrder[i] == badGuy1)
            {
                badGuy1.currentAction = BadGuyLogic();
                StartCoroutine(BadGuyActionSelect(badGuy1));
            }
            else if (turnOrder[i] == badGuy2)
            {
                badGuy2.currentAction = BadGuyLogic();
                StartCoroutine(BadGuyActionSelect(badGuy2));
            }
            else if (turnOrder[i] == badGuy3)
            {
                badGuy3.currentAction = BadGuyLogic();
                StartCoroutine(BadGuyActionSelect(badGuy3));
            }
            else if (turnOrder[i] == badGuy4)
            {
                badGuy4.currentAction = BadGuyLogic();
                StartCoroutine(BadGuyActionSelect(badGuy4));
            }
            else if (turnOrder[i] == ego) { StartCoroutine(EgoActionSelect()); }
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
        if (badGuy.currentAction == "Attack") { howManyMoves = 0; }
        else if (badGuy.currentAction == "Defend") { howManyMoves = 1; }
        else if (badGuy.currentAction == "Delay") { howManyMoves = 2; }
        else if (badGuy.currentAction == "Inventory") { howManyMoves = 3; }
        yield return new WaitForSeconds(.5f);
        while (howManyMoves != 0)
        {
            howManyMoves--;
            currentArrowPosition++;
            if (badGuy.combatSlot == slot1) { arrow.transform.position = slot1ArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slot2a) { arrow.transform.position = slot2aArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slot2b) { arrow.transform.position = slot2bArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slot3b) { arrow.transform.position = slot3bArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slot3c) { arrow.transform.position = slot3cArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slotFlank1) { arrow.transform.position = slotFlank1ArrowPositions[currentArrowPosition].transform.position; }
            else if (badGuy.combatSlot == slotFlank2) { arrow.transform.position = slotFlank2ArrowPositions[currentArrowPosition].transform.position; }
            yield return new WaitForSeconds(.5f);
        }
        if (badGuy.combatSlot == slot1)
        {
            slot1DoneArrow.transform.position = arrow.transform.position;
            slot1DoneArrow.SetActive(true);
        }
        else if (badGuy.combatSlot == slot2a)
        {
            slot2aDoneArrow.transform.position = arrow.transform.position;
            slot2aDoneArrow.SetActive(true);
        }
        else if (badGuy.combatSlot == slot2b)
        {
            slot2bDoneArrow.transform.position = arrow.transform.position;
            slot2bDoneArrow.SetActive(true);
        }
        else if (badGuy.combatSlot == slot3b)
        {
            slot3bDoneArrow.transform.position = arrow.transform.position;
            slot3bDoneArrow.SetActive(true);
        }
        else if (badGuy.combatSlot == slot3c)
        {
            slot3cDoneArrow.transform.position = arrow.transform.position;
            slot3cDoneArrow.SetActive(true);
        }
        else if (badGuy.combatSlot == slotFlank1)
        {
            slotFlank1DoneArrow.transform.position = arrow.transform.position;
            slotFlank1DoneArrow.SetActive(true);
        }
        else if (badGuy.combatSlot == slotFlank2)
        {
            slotFlank2DoneArrow.transform.position = arrow.transform.position;
            slotFlank2DoneArrow.SetActive(true);
        }
        arrow.SetActive(false);
    }
    string BadGuyLogic()
    {
        return "Inventory";
    }

    bool ActionSelected() { return Input.GetKeyDown(KeyCode.Return); }






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
