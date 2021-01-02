using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Interactable Object")]
public class InteractableObject : ScriptableObject
{
    public string noun;
    public Item searchGeneratedItem;
    //public Weapon searchGeneratedWeapon;
    //public Armor searchGeneratedArmor;
    //public Shield searchGeneratedShield;
    //public Undroppable searchGeneratedUndroppable;
    [HideInInspector] public bool searched;
    public Interaction[] interactions;
}
