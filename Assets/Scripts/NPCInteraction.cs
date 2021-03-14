using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public GameObject dialogueBox, dialogueBoxBackground, NPC1Border, NPC2Border, replyBox, replyBoxBackground, replyBoxFade, optionBox, continueArrow;
    public TMP_Text NPC1Name, NPC2Name, NPCText, reply1, reply2, reply3, reply4, reply5, reply6, reply7, reply8, reply9, reply10, option1, option2, option3, option4;

    int endingCharacter;
    bool messageComplete;
    GameController controller;
    IEnumerator askAbout;
    Queue<string> sentences;

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
    }
    IEnumerator InitiateDialogue(NPC speaker)
    {
        WriteNPCName(speaker.nome);
        WriteDialogueOptions(null, null, null, null);
        dialogueBox.SetActive(true);
        dialogueBoxBackground.SetActive(true);
        yield return new WaitForSeconds(.5f);
        NPCSpeech(speaker.openingGreeting);
        StartCoroutine(OptionSelect(speaker));
    }
    IEnumerator OptionSelect(NPC speaker)
    {
        WriteDialogueOptions(speaker.options[0], speaker.options[1], speaker.options[2], speaker.options[3]);

        int selectedElement = 0;
        int memoryElement = -1;
        string plainOption1 = option1.text;
        string plainOption2 = option2.text;
        string plainOption3 = option3.text;
        string plainOption4 = option4.text;
        
        while (true)
        {
            if (memoryElement != -1) { selectedElement = memoryElement; }
            if (selectedElement < 0) { selectedElement = 3; }
            if (selectedElement > 3) { selectedElement = 0; }

            option1.text = plainOption1;
            option2.text = plainOption2;
            option3.text = plainOption3;
            option4.text = plainOption4;

            if (selectedElement == 0) { option1.text = $"<color=yellow>{option1.text}</color>"; }
            else if (selectedElement == 1) { option2.text = $"<color=yellow>{option2.text}</color>"; }
            else if (selectedElement == 2) { option3.text = $"<color=yellow>{option3.text}</color>"; }
            else if (selectedElement == 3) { option4.text = $"<color=yellow>{option4.text}</color>"; }

            yield return new WaitUntil(controller.UpDownEnterEscPressed);
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                controller.interactableItems.cursorMove.Play();
                selectedElement--;
                if (option4.text == null && selectedElement == 3) { selectedElement = 2; }
                if (option3.text == null && selectedElement == 2) { selectedElement = 1; }
                if (option2.text == null && selectedElement == 1) { selectedElement = 0; }
                if (option1.text == null && selectedElement == 0) { selectedElement = 3; }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                controller.interactableItems.cursorMove.Play();
                selectedElement++;
                if (option1.text == null && selectedElement == 0) { selectedElement = 1; }
                if (option2.text == null && selectedElement == 1) { selectedElement = 2; }
                if (option3.text == null && selectedElement == 2) { selectedElement = 3; }
                if (option4.text == null && selectedElement == 3) { selectedElement = 0; }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                controller.interactableItems.cursorCancel.Play();
                option1.text = plainOption1;
                option2.text = plainOption2;
                option3.text = plainOption3;
                option4.text = plainOption4;
                controller.UnlockUserInput();
                break;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (selectedElement == 0)
                {
                    memoryElement = selectedElement;
                    controller.interactableItems.cursorSelect.Play();
                    StartCoroutine(ActivateAskAbout(speaker.askAbout, speaker));
                }
                else if (selectedElement == 1)
                {
                    memoryElement = selectedElement;
                    controller.interactableItems.cursorSelect.Play();
                    /*give item*/
                }
                else if (selectedElement == 2)
                {
                    memoryElement = selectedElement;
                    controller.interactableItems.cursorSelect.Play();
                    /*npc trade*/
                }
                else if (selectedElement == 3)
                {
                    controller.interactableItems.cursorCancel.Play();
                    option1.text = plainOption1;
                    option2.text = plainOption2;
                    option3.text = plainOption3;
                    option4.text = plainOption4;
                    controller.UnlockUserInput();
                }
                break;
            }
        }
    }
    IEnumerator GiveItem(NPC receiver)
    IEnumerator ActivateAskAbout(List<DialogueOption> replyList, NPC speaker)
    {
        WriteDialogueReplies(replyList);
        int selectedElement = 0;
        int memoryElement = -1;
        string plainReply1 = reply1.text;
        string plainReply2 = reply2.text;
        string plainReply3 = reply3.text;
        string plainReply4 = reply4.text;
        string plainReply5 = reply5.text;
        string plainReply6 = reply6.text;
        string plainReply7 = reply7.text;
        string plainReply8 = reply8.text;
        string plainReply9 = reply9.text;
        string plainReply10 = reply10.text;

        replyBoxFade.SetActive(true);
        replyBox.SetActive(true);
        replyBoxBackground.SetActive(true);
        yield return new WaitForSeconds(.6f);
        while (true)
        {
            if (memoryElement != -1) { selectedElement = memoryElement; }
            if (selectedElement < 0) { selectedElement = replyList.Count - 1; }
            if (selectedElement > replyList.Count - 1) { selectedElement = 0; }

            reply1.text = plainReply1;
            reply2.text = plainReply2;
            reply3.text = plainReply3;
            reply4.text = plainReply4;
            reply5.text = plainReply5;
            reply6.text = plainReply6;
            reply7.text = plainReply7;
            reply8.text = plainReply8;
            reply9.text = plainReply9;
            reply10.text = plainReply10;

            if (selectedElement == 0) { reply1.text = $"<color=yellow>{reply1.text}</color>"; }
            else if (selectedElement == 1) { reply2.text = $"<color=yellow>{reply2.text}</color>"; }
            else if (selectedElement == 2) { reply3.text = $"<color=yellow>{reply3.text}</color>"; }
            else if (selectedElement == 3) { reply4.text = $"<color=yellow>{reply4.text}</color>"; }
            else if (selectedElement == 4) { reply5.text = $"<color=yellow>{reply5.text}</color>"; }
            else if (selectedElement == 5) { reply6.text = $"<color=yellow>{reply6.text}</color>"; }
            else if (selectedElement == 6) { reply7.text = $"<color=yellow>{reply7.text}</color>"; }
            else if (selectedElement == 7) { reply8.text = $"<color=yellow>{reply8.text}</color>"; }
            else if (selectedElement == 8) { reply9.text = $"<color=yellow>{reply9.text}</color>"; }
            else if (selectedElement == 9) { reply10.text = $"<color=yellow>{reply10.text}</color>"; }

            yield return new WaitUntil(controller.UpDownEnterEscPressed);
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                controller.interactableItems.cursorMove.Play();
                selectedElement--;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                controller.interactableItems.cursorMove.Play();
                selectedElement++;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                controller.interactableItems.cursorCancel.Play();
                reply1.text = plainReply1;
                reply2.text = plainReply2;
                reply3.text = plainReply3;
                reply4.text = plainReply4;
                reply5.text = plainReply5;
                reply6.text = plainReply6;
                reply7.text = plainReply7;
                reply8.text = plainReply8;
                reply9.text = plainReply9;
                reply10.text = plainReply10;
                if (replyList[selectedElement].parentReplyList != null)
                {
                    replyBoxFade.SetActive(false);
                    replyBox.SetActive(false);
                    replyBoxBackground.SetActive(false);
                    StartCoroutine(OptionSelect(speaker));
                }
                else { StartCoroutine(ActivateAskAbout(replyList[selectedElement].parentReplyList, speaker)); }
                break;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                memoryElement = selectedElement;
                controller.interactableItems.cursorSelect.Play();
                StartCoroutine(NPCSpeech(replyList[selectedElement].response));
                if (replyList[selectedElement].additionalReplies.Count == 0)
                {
                    StartCoroutine(ActivateAskAbout(replyList, speaker));
                }
                else
                {
                    memoryElement = -1;
                    StartCoroutine(ActivateAskAbout(replyList[selectedElement].additionalReplies, speaker));
                }
                break;
            }
        }
    }
    IEnumerator NPCSpeech(List<string> responseList, float endPause = .5f)
    {
        sentences.Clear();
        foreach (string sentence in responseList) { sentences.Enqueue(sentence); }
        while (sentences.Count > 0)
        {
            string reply = sentences.Dequeue();
            NPCText.text = reply;
            yield return new WaitForSeconds(.01f);
            messageComplete = false;
            StartCoroutine(TeletypeMessage(0));
            yield return new WaitUntil(MessageComplete);
            messageComplete = false;
            yield return new WaitForSeconds(endPause);
            continueArrow.SetActive(true);
            yield return new WaitUntil(controller.EnterPressed);
            continueArrow.SetActive(false);
            yield return new WaitForSeconds(.1f);
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
    void SwitchAskAboutTree(List<DialogueOption> tree, NPC speaker)
    {
        StopCoroutine(askAbout);
        askAbout = ActivateAskAbout(tree, speaker);
        StartCoroutine(askAbout);
    }
    void AddNPCText(string text)
    {
        NPCText.text += text;
    }

    bool MessageComplete() { return messageComplete; }




    // Update is called once per frame
    void Update()
    {
        
    }
}
