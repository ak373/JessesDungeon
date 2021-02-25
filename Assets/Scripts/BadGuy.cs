using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Characters/BadGuy")]
public class BadGuy : Character
{
    public List<Item> itemLoot = new List<Item>();
    public int crystalLootMin;
    public int crystalLootMax;
    [TextArea(2,5)]
    public string battleCry;
    [HideInInspector] public GameObject combatSlot;
    [HideInInspector] public GameObject combatBorder;
    //idk what I was thinking when I made this
    //[HideInInspector] public TMP_Text combatEffects;
    public string specialAbility;
    public BadGuyCombatActions[] normalAIRay;
}
