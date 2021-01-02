using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Characters/BadGuy")]
public class BadGuy : Stats
{
    public List<Item> itemLoot = new List<Item>();
    public Potion[] potionBelt;
    public int crystalLootMin;
    public int crystalLootMax;
    [TextArea(2,5)]
    public string battleCry;
}
