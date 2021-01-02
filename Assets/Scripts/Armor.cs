using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Items/Armor")]
public class Armor : Item
{
    public int damageReduction;
    public float critResist;


    public void InitializeArmor(Armor armor, string noun, int price, string nome, int damageReduction, float critResist, bool unlocked, string description)
    {
        armor.noun = noun;
        armor.price = price;
        armor.nome = nome;
        armor.damageReduction = damageReduction;
        armor.critResist = critResist;
        armor.unlocked = unlocked;
        armor.description = description;
    }
}
