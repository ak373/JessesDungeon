using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/DialogueOption")]
public class DialogueOption : ScriptableObject
{
    public string reply;
    [TextArea(2, 10)]
    public List<string> response = new List<string>();
    public List<DialogueOption> additionalReplies = new List<DialogueOption>();
    public List<DialogueOption> parentReplyList;
    public bool availableToSay;
    [HideInInspector] public bool hasBeenSaid;
}
