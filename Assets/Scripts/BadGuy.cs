using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Characters/BadGuy")]
public class BadGuy : Character
{
    public List<Item> itemLoot = new List<Item>();
    public Potion[] potionBelt;
    public int crystalLootMin;
    public int crystalLootMax;
    [TextArea(2,5)]
    public string battleCry;
    [HideInInspector] public GameObject combatSlot;
    [HideInInspector] public GameObject combatBorder;
    public string specialAbility;
    public BadGuyCombatActions[] normalAIRay;
}
