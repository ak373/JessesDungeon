using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Characters/BadGuyCombatActions")]
public class BadGuyCombatActions : ScriptableObject
{
    public string title;
    public int likelihood;
    [TextArea (1,5)]
    public List<string> messages = new List<string>();
    public Effect effect;
    public string target;
    public bool beneficial;
}