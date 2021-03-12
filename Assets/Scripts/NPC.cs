using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/NPC")]
public class NPC : ScriptableObject
{
    public string nome;
    public string openingGreeting;
    public List<DialogueOption> askAbout = new List<DialogueOption>();
    public string trade;
}
