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
    void WriteNPCText(string text)
    {
        NPCText.text = text;
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
