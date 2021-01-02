using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//fairly certain this script is obsolete.

public class DialogueManager : MonoBehaviour
{
    //public TMP_Text nameText;
    //public GameObject conversation1;
    //float timeDelay;
    //bool enterToContinueDialogue;
    //Queue<int> pauses;
    //Queue<string> sentences;
    //GameController controller;
           
    // Start is called before the first frame update
    void Start()
    {
        //timeDelay = 1;
        //sentences = new Queue<string>();
        //pauses = new Queue<int>();
        //controller = GetComponent<GameController>();
    }

    //public void StartDialogue(Dialogue dialogue)
    //{
    //    //nameText.text = dialogue.NPCName;
    //    sentences.Clear();
    //    pauses.Clear();
    //    foreach (string sentence in dialogue.sentences) { sentences.Enqueue(sentence); }
    //    foreach (int pause in dialogue.pauses) { pauses.Enqueue(pause); }

    //    if (dialogue.timed) { StartCoroutine(TimedDialogue()); }
    //    else { EnterToContinueDialogue(); }
        
    //}

    //IEnumerator TimedDialogue()
    //{
    //    int pause = pauses.Dequeue();
    //    string sentence = sentences.Dequeue();
    //    if (sentences.Count == 1)
    //    {
    //        controller.DisplayNarratorResponse(sentence);
    //        yield return null;
    //    }
    //    else
    //    {
    //        controller.AddToMainWindowWithLine(sentence);
    //        yield return new WaitForSeconds(pause);
    //    }
    //}
    //public void EnterToContinueDialogue()
    //{
    //    timeDelay = Time.time;
    //    string sentence = sentences.Dequeue();
    //    if (sentences.Count == 1) { controller.DisplayNarratorResponse(sentence); }
    //    else
    //    {
    //        enterToContinueDialogue = true;
    //        controller.AddToMainWindowWithLine(sentence);
    //    }
    //}




    // Update is called once per frame
    void Update()
    {
        //if (enterToContinueDialogue && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    enterToContinueDialogue = false;
        //    EnterToContinueDialogue();
        //}
    }
}
