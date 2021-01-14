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
    public GameObject battleLog, battleLogGreyScreen, turnOrderBlackScreen;
    public TMP_Text battleText, effectsText;
    public Effect[] allEffects;

    BadGuy[] activeBadGuys = { null, null, null, null, null };
    BadGuy badGuy0, badGuy1, badGuy2, badGuy3, badGuy4;
    Character[] turnOrder = { null, null, null, null, null, null };
    List<int> usedInitValues = new List<int>();
    int currentArrowPosition, endingCharacter;
    bool actionSelected, actionComplete, messageComplete;

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
        for (int i = 0; i < turnOrderNames.Length; i++)
        {
            turnOrderNames[i].text = "";            
        }
        for (int i = 0; i < activeBadGuys.Length; i++)
        {
            if (activeBadGuys[i] != null) { activeBadGuys[i].displayAction = ""; }
        }
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
        }
        yield return new WaitForSeconds(.2f);
        StartCoroutine(ExecuteActions());
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
            if (turnOrder[i] != null)
            {
                //remove guarded on own turn
                for (int j = 0; j < turnOrder[i].activeEffects.Count; j++)
                {
                    if (turnOrder[i].activeEffects[j].title == "Guarded") { RemoveEffect(turnOrder[i], turnOrder[i].activeEffects[j]); }
                }
                if (turnOrder[i] == ego)
                {
                    StartCoroutine(SelectFlicker(borderEgo));
                    if (ego.chosenAction == "Attack") { StartCoroutine(ExecuteEgoAttack((BadGuy)ego.chosenTarget)); }
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
                    else if (ego.chosenAction == "Delay")
                    {

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

                if (currentArrowPosition == 0)//Attack
                {
                    List<GameObject> activeBadGuyBorders = new List<GameObject>();
                    int borderSelected = 0;

                    //construct target cycle
                    for (int i = 0; i < activeBadGuys.Length; i++)
                    {
                        if (activeBadGuys[i] != null) { activeBadGuyBorders.Add(activeBadGuys[i].combatBorder); }
                    }

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
                        yield return new WaitUntil(controller.LeftRightEnterEscPressed);
                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            StopCoroutine(selection);
                            activeBadGuyBorders[borderSelected].SetActive(true);
                            borderSelected--;
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            StopCoroutine(selection);
                            activeBadGuyBorders[borderSelected].SetActive(true);
                            borderSelected++;
                        }
                        else if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            egoDoneArrow.SetActive(false);
                            arrow.SetActive(true);
                            egoCombatOptions[currentArrowPosition].color = Color.white;
                            StopCoroutine(selection);
                            activeBadGuyBorders[borderSelected].SetActive(true);
                            attackSelectionMemory = borderSelected;
                            break;
                        }
                        else if (Input.GetKeyDown(KeyCode.Return))
                        {
                            StopCoroutine(selection);
                            activeBadGuyBorders[borderSelected].SetActive(true);
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
                else if (currentArrowPosition == 1)
                {
                    ego.displayAction = "Defend";
                    ego.chosenAction = "Defend";
                    actionSelected = true;
                    break;
                }
                else if (currentArrowPosition == 2)
                {
                    ego.displayAction = "Delay";
                }
                else if (currentArrowPosition == 3)
                {
                    ego.displayAction = "Flee";
                }
                else if (currentArrowPosition == 4)
                {

                }
                else if (currentArrowPosition == 5)
                {

                }
                if (actionSelected) { break; }



                //delete or redistribute
                //actionSelected = true;
                //egoDoneArrow.transform.position = arrow.transform.position;
                //egoDoneArrow.SetActive(true);
                //arrow.SetActive(false);
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
    IEnumerator EndTurn()
    {
        turnOrderBlackScreen.SetActive(true);
        yield return new WaitForSeconds(.5f);
        WhiteWash();
        //for (int i = 0; i < ego.activeEffects.Count; i++)
        //{
        //    Debug.Log(ego.activeEffects[i].duration);
        //    ego.activeEffects[i].duration--;
        //    if (ego.activeEffects[i].duration == 0) { RemoveEffect(ego, ego.activeEffects[i]); }
        //}
        //for (int i = 0; i < activeBadGuys.Length; i++)
        //{
        //    if (activeBadGuys[i] != null)
        //    {
        //        activeBadGuys[i].hasGoneThisTurn = false;
        //        for (int j = 0; j < activeBadGuys[i].activeEffects.Count; j++)
        //        {
        //            activeBadGuys[i].activeEffects[j].duration--;
        //            if (activeBadGuys[i].activeEffects[j].duration == 0) { RemoveEffect(activeBadGuys[i], activeBadGuys[i].activeEffects[j]); }
        //        }
        //    }
        //}
        CalculateTurnOrder();
        DisplayTurnOrder();
        yield return new WaitForSeconds(.5f);
        turnOrderBlackScreen.SetActive(false);
        StartCoroutine(TurnDistributor());
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


    bool ActionSelected() { return actionSelected; }
    bool ActionComplete() { return actionComplete; }
    bool MessageComplete() { return messageComplete; }






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
        }
    }
}
