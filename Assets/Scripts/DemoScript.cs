using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DemoScript : MonoBehaviour
{
    bool listenerListener = true;
    bool demoListener = true;
    public bool demoProceed = false;
    int martyrCounter = 0;
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
        demoDialogue.Add($"I'm gonna provide you with supplies and you're going to fight a bunch of things. After each battle you'll pick some bonus equipment, then be shipped off to the next fight.");
        demoDialogue.Add($"And it's gonna end like the blackhole in Star Fox. If you don't know what that means, well... you will.");
        demoDialogue.Add($"Whatever you got on will be fine for this first one.");
        yield return new WaitForSeconds(.25f);
        controller.npcInteraction.WriteNPCName("Jesse");
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        controller.npcInteraction.dialogueBox.SetActive(false);
        controller.npcInteraction.dialogueBoxBackground.SetActive(false);
        controller.npcInteraction.wholeScreenFadeBlack.SetActive(true);
        yield return new WaitForSeconds(2f);
        StartCoroutine(controller.combat.BeginBattle(controller.combat.allBadGuys[1], 1));
        demoProceed = false;
        yield return new WaitUntil(DemoProceed);
        demoProceed = false;
        controller.npcInteraction.ActivateDialogueBox();

        demoDialogue.Clear();
        demoDialogue.Add($"A little too easy, right? Let's spice it up a bit. You'll probably want to increase your damage output for this one.");
        demoDialogue.Add($"Take any one you want.");
        yield return new WaitForSeconds(.5f);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.GenericOptionSelection("Knife", "Garbage Can Lid", "Light Healing Potion"));
        controller.npcInteraction.genericOptionComplete = false;
        yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
        controller.npcInteraction.genericOptionComplete = false;
        if (controller.npcInteraction.genericOptionSelected == 0) { controller.GetEquipped(Instantiate(controller.registerObjects.allWeapons[5])); }
        else if (controller.npcInteraction.genericOptionSelected == 1) { controller.GetStrapped(Instantiate(controller.registerObjects.allShields[2])); }
        else if (controller.npcInteraction.genericOptionSelected == 2 && controller.ego.potionBelt.Count > 3) { controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[0])); }
        else { martyrCounter++; }

        controller.npcInteraction.dialogueBox.SetActive(false);
        controller.npcInteraction.dialogueBoxBackground.SetActive(false);
        controller.npcInteraction.wholeScreenFadeBlack.SetActive(true);
        yield return new WaitForSeconds(2f);
        StartCoroutine(controller.combat.BeginBattle(controller.combat.allBadGuys[1], 3));
        demoProceed = false;
        yield return new WaitUntil(DemoProceed);
        demoProceed = false;
    }
    bool DemoProceed() { return demoProceed; }

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
