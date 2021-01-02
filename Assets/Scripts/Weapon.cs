using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Items/Weapon")]
public class Weapon : Item
{
    public string type;
    public int damage;
    public int damageDie;
    public float critMultiplier;
    public int toHitMod;
    public bool twoHanded;

    public Weapon(string aNoun, int aPrice, string aNome, string aType, int aDamage, float aCritMultiplier, int aToHitMod, bool aTwoHanded, bool aUnlocked, string aDescription)
    {
        noun = aNoun;
        price = aPrice;
        nome = aNome;
        type = aType;
        damage = aDamage;
        critMultiplier = aCritMultiplier;
        toHitMod = aToHitMod;
        twoHanded = aTwoHanded;
        unlocked = aUnlocked;
        description = aDescription;
    }

    public Weapon InitializeWeapon(Weapon weapon, string noun, int price, string nome, string type, int damage, float critMultiplier, int toHitMod, bool twoHanded, bool unlocked, string description)
    {
        weapon.noun = noun;
        weapon.price = price;
        weapon.nome = nome;
        weapon.type = type;
        weapon.damage = damage;
        weapon.critMultiplier = critMultiplier;
        weapon.toHitMod = toHitMod;
        weapon.twoHanded = twoHanded;
        weapon.unlocked = unlocked;
        weapon.description = description;
        return weapon;
    }
}
