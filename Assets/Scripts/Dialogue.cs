using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string NPCName;
    [TextArea(3,20)]
    public string[] sentences;
    public int[] pauses;
    public bool timed;
    public bool townRepeat;
    public bool jokeConversation;

    //public Dialogue(string aName, string[] aSentences, float[] aPauses, bool aTimed)
    //{
    //    name = aName;
    //    sentences = aSentences;
    //    pauses = aPauses;
    //    timed = aTimed;
    //}
}
