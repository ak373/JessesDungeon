using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject
{
    public string nome;
    public Stat[] allStats;
    public List<Effect> activeEffects = new List<Effect>();
    //public int currentHitPoints;
    //public int targetHitPoints;
    //public int maxHitPoints;
    public Armor equippedArmor;
    public Weapon equippedWeapon;
    public Shield equippedShield;
    //public int armorClass;
    //public float critResist;
    //public int damageReduction;
    //public int damageDie;
    //public int damage;
    //public float critMultiplier;
    //public int flurry;
    //public int toHitMod;
    //public int initMod;
    [HideInInspector] public int currentInit;
    [HideInInspector] public string displayAction;
    [HideInInspector] public string chosenAction;
}
