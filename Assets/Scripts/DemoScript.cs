using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DemoScript : MonoBehaviour
{
    bool listenerListener = true;
    bool demoListener = true;
    GameController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
    }
    IEnumerator DemoListener()
    {
        demoListener = false;
        bool activate = false;
        while (!activate)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                listenerListener = false;
                activate = true;
            }
            yield return null;
        }
        StartCoroutine(Demo());
    }
    IEnumerator Demo()
    {
        controller.inputBox.SetActive(false);
        controller.npcInteraction.TurnOffOptionBackLights();
        controller.npcInteraction.WriteNPCName("");
        controller.npcInteraction.ActivateDialogueBox();
        List<string> demoDialogue = new List<string>();
        demoDialogue.Add($"Hey, you.");
        demoDialogue.Add($"Yeah, you over there!");
        if (controller.ego.equippedWeapon != null) { demoDialogue[1] = $"Yeah, you holding the {controller.ego.equippedWeapon.nome}!"; }
        else if (controller.ego.equippedShield != null) { demoDialogue[1] = $"Yeah, you holding the {controller.ego.equippedShield.nome}!"; }
        else if (controller.ego.equippedArmor != null) { demoDialogue[1] = $"Yeah, you in the {controller.ego.equippedArmor.nome}!"; }
        yield return new WaitForSeconds(.5f);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        demoDialogue.Clear();
        demoDialogue.Add($"Heeeey you found me!");
        demoDialogue.Add($"Well more like I found you. This is a short demo so I had to come to these few areas to which you can actually go.");
        demoDialogue.Add($"So here's how the rest of this is going to play out.");
        demoDialogue.Add($"I'm gonna provide you with supplies and you're going to fight a bunch of things. After each battle you'll be healed, and given access to new equipment.");
        demoDialogue.Add($"And it's gonna end like the blackhole in Star Fox. If you don't know what that means, well... you will.");
        yield return new WaitForSeconds(.25f);
        controller.npcInteraction.WriteNPCName("Jesse");
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (listenerListener && controller.roomNavigation.currentRoom.roomName == "G7" && controller.registerRooms.allRooms[32].visited)
        {
            EventSystem.current.SetSelectedGameObject(controller.displayBox);
            if (demoListener) { StartCoroutine(DemoListener()); }            
        }
    }
}
