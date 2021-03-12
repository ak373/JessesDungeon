using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public GameObject dialogueBox, dialogueBoxBackground, NPC1Border, NPC2Border, replyBox, replyBoxBackground, optionBox, continueArrow;
    public TMP_Text NPC1Name, NPC2Name, NPCText, reply1, reply2, reply3, reply4, reply5, reply6, reply7, reply8, reply9, reply10, option1, option2, option3, option4;

    int endingCharacter;
    bool messageComplete;
    GameController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
    }

    void WriteNPCName(string name, int boxNumber = 1)
    {
        if (boxNumber == 1) { NPC1Name.text = name; }
        else { NPC2Name.text = name; }
    }
    void WriteDialogueOptions(string line1 = "Ask about...", string line2 = "Give item", string line3 = null, string line4 = "Enough already")
    {
        option1.text = line1;
        option2.text = line2;
        option3.text = line3;
        option4.text = line4;
    }
    void WriteDialogueReplies(List<DialogueOption> dialogueTree)
    {
        if (dialogueTree[0] != null) { reply1.text = dialogueTree[0].reply; }
        if (dialogueTree[1] != null) { reply2.text = dialogueTree[1].reply; }
        if (dialogueTree[2] != null) { reply3.text = dialogueTree[2].reply; }
        if (dialogueTree[3] != null) { reply4.text = dialogueTree[3].reply; }
        if (dialogueTree[4] != null) { reply5.text = dialogueTree[4].reply; }
        if (dialogueTree[5] != null) { reply6.text = dialogueTree[5].reply; }
        if (dialogueTree[6] != null) { reply7.text = dialogueTree[6].reply; }
        if (dialogueTree[7] != null) { reply8.text = dialogueTree[7].reply; }
        if (dialogueTree[8] != null) { reply9.text = dialogueTree[8].reply; }
        if (!dialogueTree[0].isInternalTree) { reply10.text = "Enough already"; }
        else { reply10.text = "Something else"; }
    }
    IEnumerator InitiateDialogue(NPC npc)
    {
        WriteNPCName(npc.nome);
        WriteDialogueOptions(null, null, null, null);
        dialogueBox.SetActive(true);
        dialogueBoxBackground.SetActive(true);
        yield return new WaitForSeconds(.5f);
        NPCSpeech(npc.openingGreeting);
    }
    void AddNPCText(string text)
    {
        NPCText.text += text;
    }
    IEnumerator NPCSpeech(string text, bool withContinueCarrot = false, float endPause = .5f)
    {
        NPCText.text = text;
        yield return new WaitForSeconds(.01f);
        messageComplete = false;
        StartCoroutine(TeletypeMessage(0));
        yield return new WaitUntil(MessageComplete);
        yield return new WaitForSeconds(endPause);
        if (withContinueCarrot)
        {
            continueArrow.SetActive(true);
            yield return new WaitUntil(controller.EnterPressed);
            continueArrow.SetActive(false);
            yield return new WaitForSeconds(.1f);
            //will need refinement for a proper looping
        }        
    }
    IEnumerator TeletypeMessage(int startingCharacter, float characterPause = 0.025f)
    {
        int totalVisibleCharacters = NPCText.textInfo.characterCount;
        int counter = startingCharacter;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            NPCText.maxVisibleCharacters = visibleCount;
            if (Input.GetKey(KeyCode.Return))
            {
                NPCText.maxVisibleCharacters = totalVisibleCharacters;
                visibleCount = totalVisibleCharacters;
                counter = totalVisibleCharacters;
            }

            if (visibleCount >= totalVisibleCharacters) { break; }
            counter += 1;

            yield return new WaitForSeconds(characterPause);
        }
        endingCharacter = counter;
        messageComplete = true;
    }

    bool MessageComplete() { return messageComplete; }




    // Update is called once per frame
    void Update()
    {
        
    }
}
