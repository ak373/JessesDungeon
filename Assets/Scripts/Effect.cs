using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Effects")]
public class Effect : ScriptableObject
{
    public string title;
    public string stat;
    public int potency;
    public int duration;
    [TextArea(3,10)]
    public string description;
    [HideInInspector] public int remainingRounds;
}
