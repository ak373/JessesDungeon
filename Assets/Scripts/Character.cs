using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject
{
    public string nome;
    public Stat[] allStats;
    public List<Effect> activeEffects = new List<Effect>();
    public Armor equippedArmor;
    public Weapon equippedWeapon;
    public Shield equippedShield;
    [HideInInspector] public int currentInit;
    [HideInInspector] public string displayAction;
    [HideInInspector] public string chosenAction;
    [HideInInspector] public Character chosenTarget;
    [HideInInspector] public Item chosenItem;
}
