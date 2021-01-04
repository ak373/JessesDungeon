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
    public GameObject arrow, doneArrow;
    public GameObject[] egoArrowPositions;
    public TMP_Text combatWeaponDisplay, combatArmorDisplay, combatShieldDisplay;
    public GameObject slotEgo, slot1, slot2a, slot2b, slot3b, slot3c, slotFlank1, slotFlank2;
    public TMP_Text curHPEgo, maxHPEgo, curHP1, maxHP1, curHP2a, maxHP2a, curHP2b, maxHP2b, curHP3b, maxHP3b, curHP3c, maxHP3c, curHPFlank1, maxHPFlank1, curHPFlank2, maxHPFlank2;
    public TMP_Text[] turnOrderNames;
    public TMP_Text[] turnOrderActions;
    public TMP_Text[] egoCombatOptions;

    BadGuy[] activeBadGuys = { null, null, null, null, null };
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
        //for (int i = 0; i < numberOfBadGuys; i++) { activeBadGuys[i] = badGuy; }
        for (int j = numberOfBadGuys; j < 4; j++) { activeBadGuys[j] = null; }

        //determine proper badguy UI layout
        if (numberOfBadGuys != 2)
        {
            slot1.SetActive(true);
            curHP1.text = badGuy.currentHitPoints.ToString();
            maxHP1.text = badGuy.maxHitPoints.ToString();
        }
        else
        {
            slot2a.SetActive(true);
            curHP2a.text = badGuy.currentHitPoints.ToString();
            maxHP2a.text = badGuy.maxHitPoints.ToString();
            slot2b.SetActive(true);
            curHP2b.text = badGuy.currentHitPoints.ToString();
            maxHP2b.text = badGuy.maxHitPoints.ToString();
        }
        if (numberOfBadGuys > 2)
        {
            slot3b.SetActive(true);
            curHP3b.text = badGuy.currentHitPoints.ToString();
            maxHP3b.text = badGuy.maxHitPoints.ToString();
            slot3c.SetActive(true);
            curHP3c.text = badGuy.currentHitPoints.ToString();
            maxHP3c.text = badGuy.maxHitPoints.ToString();
        }
        if (numberOfBadGuys > 3)
        {
            slotFlank1.SetActive(true);
            curHPFlank1.text = badGuy.currentHitPoints.ToString();
            maxHPFlank1.text = badGuy.maxHitPoints.ToString();
        }
        if (numberOfBadGuys > 4)
        {
            slotFlank2.SetActive(true);
            curHPFlank2.text = badGuy.currentHitPoints.ToString();
            maxHPFlank2.text = badGuy.maxHitPoints.ToString();
        }

        CalculateTurnOrder();
        DisplayTurnOrder();
        StartCoroutine(ActionSelect());
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

    public IEnumerator ActionSelect()
    {
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
                doneArrow.transform.position = arrow.transform.position;
                doneArrow.SetActive(true);
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
