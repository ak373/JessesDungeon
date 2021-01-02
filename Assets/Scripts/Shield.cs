using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Items/Shield")]
public class Shield : Item
{
    public int armorClass;
    public float critResist;

    public Shield(string aNoun, int aPrice, string aNome, int aArmorClass, float aCritResist, bool aUnlocked, string aDescription)
    {
        noun = aNoun;
        price = aPrice;
        nome = aNome;
        armorClass = aArmorClass;
        critResist = aCritResist;
        unlocked = aUnlocked;
        description = aDescription;
    }

    public Shield InitializeShield(Shield shield, string noun, int price, string nome, int armorClass, float critResist, bool unlocked, string description)
    {
        shield.noun = noun;
        shield.price = price;
        shield.nome = nome;
        shield.armorClass = armorClass;
        shield.critResist = critResist;
        shield.unlocked = unlocked;
        shield.description = description;
        return shield;
    }
}
