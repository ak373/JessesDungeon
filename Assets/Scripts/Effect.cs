using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Effects")]
public class Effect : ScriptableObject
{
    public string title;
    public string abbreviation;
    public string stat;
    public string stat2;
    public int potency;
    public int potency2;
    public Color color;
    public int duration;
    [TextArea(3,10)]
    public string description;
    [HideInInspector] public int remainingRounds;
}
