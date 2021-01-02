using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Items/Potions")]
public class Potion : Item
{
    public int potency;
    public Stats statAffected;
    //public float statAffected; I don't remember what this was supposed to be. I guess terrible way of labeling stats.
    public int duration;
    //-1 instant, 0 through next battle, >0 minutes?
}
