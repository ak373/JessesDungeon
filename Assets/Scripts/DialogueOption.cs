using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/DialogueOption")]
public class DialogueOption : ScriptableObject
{
    public string reply;
    public List<string> response = new List<string>();
    public List<DialogueOption> additionalReplies = new List<DialogueOption>();
    public List<DialogueOption> parentReplyList;
    public bool availableToSay;
    public bool hasBeenSaid;
}
