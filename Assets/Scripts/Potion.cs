using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Items/Potions")]
public class Potion : Item
{
    public int allStatsNumber;
    public int potency;
    public int duration;
    //-1 instant, 0 through next battle, >0 minutes?
}
