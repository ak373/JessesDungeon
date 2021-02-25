using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Effects")]
public class Effect : ScriptableObject
{
    public string title;
    public string abbreviation;
    public string priorityLine;
    public int allStatsNumber;
    public int allStatsNumber2;
    public int potency;
    public int potency2;
    public Color color;
    public int duration;
    [HideInInspector] public int delayedDuration;
    public bool beneficial;
    public bool compounding;
    public string compoundMessage;
    public string compoundMessage2;
    [TextArea(3,10)]
    public string description;
    [HideInInspector] public int turnOrderTick;
}
