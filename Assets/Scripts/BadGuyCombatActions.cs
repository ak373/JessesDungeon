using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Characters/BadGuyCombatActions")]
public class BadGuyCombatActions : ScriptableObject
{
    public string title;
    public int likelihood;
    [TextArea (1,5)]
    public string message;
    public Effect effect;
}