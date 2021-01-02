using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combat : MonoBehaviour
{
    public GameObject arrow, doneArrow;
    public GameObject[] egoArrowPositions;
    public TMP_Text combatWeaponDisplay, combatArmorDisplay, combatShieldDisplay;
    public GameObject slotEgo, slot1, slot2a, slot2b, slot3b, slot3c, slotFlank1, slotFlank2;
    public TMP_Text curHPEgo, maxHPEgo, curHP1, maxHP1, curHP2a, maxHP2a, curHP2b, maxHP2b, curHP3b, maxHP3b, curHP3c, maxHP3c, curHPFlank1, maxHPFlank1, curHPFlank2, maxHPFlank2;

    BadGuy[] activeBadGuys = { null, null, null, null, null };
    int currentArrowPosition;
    GameController controller;
    DieRoll dieRoll;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
        currentArrowPosition = 0;

        //testing purposes: skip to actionselect
        StartCoroutine(ActionSelect());
    }

    public void InitiateCombat(Ego ego, BadGuy badGuy, int numberOfBadGuys)
    {
        curHPEgo.text = ego.currentHitPoints.ToString();
        maxHPEgo.text = ego.maxHitPoints.ToString();
        if (ego.equippedWeapon != null) { combatWeaponDisplay.text = ego.equippedWeapon.nome; }
        else { combatWeaponDisplay.text = "None"; }
        if (ego.equippedArmor != null) { combatArmorDisplay.text = ego.equippedArmor.nome; }
        else { combatArmorDisplay.text = "None"; }
        if (ego.equippedShield != null) { combatShieldDisplay.text = ego.equippedShield.nome; }
        else { combatShieldDisplay.text = "None"; }

        for (int i = 0; i < numberOfBadGuys; i++) { activeBadGuys[i] = badGuy; }
        for (int j = numberOfBadGuys; j < 6; j++) { activeBadGuys[j] = null; }

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
        
        ego.currentInit = dieRoll.Rolld20() + ego.initMod;
        for (int i = 0; i < numberOfBadGuys; i++) { activeBadGuys[i].currentInit = dieRoll.Rolld20() + activeBadGuys[i].initMod; }
    }
    public IEnumerator ActionSelect()
    {
        Debug.Log("ActionSelect()");
        arrow.SetActive(true);
        while (true)
        {
            Debug.Log("Loop");
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
                if (currentArrowPosition == 0) { }
                else if (currentArrowPosition == 1) { }
                else if (currentArrowPosition == 2) { }
                else if (currentArrowPosition == 3) { }
                else if (currentArrowPosition == 4) { }
                else if (currentArrowPosition == 5) { }
                break;
            }
        }
    }







    // Update is called once per frame
    void Update()
    {
        
    }
}
