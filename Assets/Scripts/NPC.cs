using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/NPC")]
public class NPC : ScriptableObject
{
    public string nome;
    public List<string> openingGreeting = new List<string>();
    public List<DialogueOption> askAbout = new List<DialogueOption>();
    public string trade;
}
