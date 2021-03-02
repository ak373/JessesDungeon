﻿using System.Collections;
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
    public List<Potion> potionBelt = new List<Potion>();
    [HideInInspector] public int currentInit;
    [HideInInspector] public int currentTurnOrder;
    [HideInInspector] public string displayAction;
    [HideInInspector] public string chosenAction;
    [HideInInspector] public BadGuyCombatActions chosenAbility;
    [HideInInspector] public Character chosenTarget;
    [HideInInspector] public Item chosenItem;
    [HideInInspector] public Item chosenItem2;
}
