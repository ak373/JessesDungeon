using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Characters/Ego")]
public class Ego : Character
{
    public int blueCrystals;
    public int fightClubRank;
    public int conversation;
    public string fleeLocation;
    public int bankedCrystals;
    public List<BadGuy> defeatedBadGuys = new List<BadGuy>();
}
