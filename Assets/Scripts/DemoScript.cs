using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DemoScript : MonoBehaviour
{
    public AudioSource interimTheme;
    bool listenerListener = true;
    bool demoListener = true;
    [HideInInspector] public bool demoProceed = false;
    int martyrCounter = 0;
    GameController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
    }
    public void SnatchInput(string fromTextInput)
    {
        if (fromTextInput == "go north" || fromTextInput == "go south")
        {
            bool raseiredRoom = false;
            for (int i = 0; i < controller.roomNavigation.currentRoom.exits.Length; i++)
            {
                if (("go " + controller.roomNavigation.currentRoom.exits[i].keyString) == fromTextInput) { raseiredRoom = true; }
            }
            if (raseiredRoom)
            {
                controller.textInput.textIsGood = true;
                StartCoroutine(controller.Narrator("Not a chance. Room unavailable in Spawned Jesse's Dungeon."));
            }            
        }
    }
    IEnumerator DemoListener()
    {
        demoListener = false;
        yield return new WaitForSeconds(.25f);
        yield return new WaitUntil(controller.EnterPressed);
        yield return new WaitForSeconds(.25f);
        yield return new WaitUntil(controller.EnterPressed);
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
        demoDialogue.Add($"Heeey! You found me!");
        demoDialogue.Add($"Well more like I found you. This is a short demo so I had to come to these few areas to which you can actually go.");
        demoDialogue.Add($"So here's how the rest of this is going to play out.");
        demoDialogue.Add($"I will provide you with supplies and you're going to fight a bunch of things.");
        demoDialogue.Add($"After each battle you'll pick some equipment, then be shipped off to the next fight.");
        demoDialogue.Add($"And it's gonna end like the blackhole in Star Fox. If you don't know what that means, well... you will.");
        demoDialogue.Add($"Whatever you got on will be fine for this first one.");
        yield return new WaitForSeconds(.25f);
        controller.npcInteraction.WriteNPCName("Jesse");
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        //controller.npcInteraction.dialogueBox.SetActive(false);
        //controller.npcInteraction.dialogueBoxBackground.SetActive(false);
        StartCoroutine(controller.roomNavigation.FadeAudioOut(interimTheme, .25f));
        StartCoroutine(controller.combat.BeginBattle(controller.combat.allBadGuys[1], 1));
        demoProceed = false;
        yield return new WaitUntil(DemoProceed);
        demoProceed = false;
        //controller.combat.fightOverWhiteScreen.SetActive(false);

        //  battle 2 --
        //controller.npcInteraction.ActivateDialogueBox();
        controller.npcInteraction.NPCText.text = "";
        StartCoroutine(controller.roomNavigation.FadeAudioIn(interimTheme, .25f));
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
        if (controller.npcInteraction.genericOptionSelected == 0)
        {
            controller.combat.cursorSelect.Play();
            Weapon choice = Instantiate(controller.registerObjects.allWeapons[5]);
            controller.interactableItems.inventory.Add(choice);
            controller.GetEquipped(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 1)
        {
            controller.combat.cursorSelect.Play();
            Shield choice = Instantiate(controller.registerObjects.allShields[2]);
            controller.interactableItems.inventory.Add(choice);
            controller.GetStrapped(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 2 && controller.ego.potionBelt.Count < 3)
        {
            controller.combat.cursorSelect.Play();
            controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[0]));
        }
        else
        {
            controller.interactableItems.cursorCancel.Play();
            martyrCounter++;
        }

        //controller.npcInteraction.dialogueBox.SetActive(false);
        //controller.npcInteraction.dialogueBoxBackground.SetActive(false);
        StartCoroutine(controller.roomNavigation.FadeAudioOut(interimTheme, .25f));
        StartCoroutine(controller.combat.BeginBattle(controller.combat.allBadGuys[1], 3));
        demoProceed = false;
        yield return new WaitUntil(DemoProceed);
        demoProceed = false;
        //controller.combat.fightOverWhiteScreen.SetActive(false);

        //  battle 3 --
        //controller.npcInteraction.ActivateDialogueBox();
        controller.npcInteraction.ClearOptions();
        controller.npcInteraction.NPCText.text = "";
        StartCoroutine(controller.roomNavigation.FadeAudioIn(interimTheme, .25f));
        demoDialogue.Clear();
        demoDialogue.Add($"A bit more to deal with that time, right? Let's up it a little bit more. You might want to look into some protective equipment.");
        demoDialogue.Add($"Take any one you want.");
        yield return new WaitForSeconds(.5f);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.GenericOptionSelection("Hunting Knife", "Jagged Metal Pole", "Healing Potion"));
        controller.npcInteraction.genericOptionComplete = false;
        yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
        controller.npcInteraction.genericOptionComplete = false;
        if (controller.npcInteraction.genericOptionSelected == 0)
        {
            controller.combat.cursorSelect.Play();
            Weapon choice = Instantiate(controller.registerObjects.allWeapons[6]);
            controller.interactableItems.inventory.Add(choice);
            controller.GetEquipped(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 1)
        {
            controller.combat.cursorSelect.Play();
            Weapon choice = Instantiate(controller.registerObjects.allWeapons[8]);
            controller.interactableItems.inventory.Add(choice);
            if (controller.ego.equippedShield != null) { controller.GetUnStrapped(); }
            controller.GetEquipped(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 2 && controller.ego.potionBelt.Count < 3)
        {
            controller.combat.cursorSelect.Play();
            controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[1]));
        }
        else
        {
            controller.interactableItems.cursorCancel.Play();
            martyrCounter++;
        }
        // item 2
        demoDialogue.Clear();
        demoDialogue.Add($"Take any one you want.");
        yield return new WaitForSeconds(.5f);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.GenericOptionSelection("Leather Armor", "Cabinet Door", "Healing Potion"));
        controller.npcInteraction.genericOptionComplete = false;
        yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
        controller.npcInteraction.genericOptionComplete = false;
        if (controller.npcInteraction.genericOptionSelected == 0)
        {
            controller.combat.cursorSelect.Play();
            Armor choice = Instantiate(controller.registerObjects.allArmor[2]);
            controller.interactableItems.inventory.Add(choice);
            controller.GetDressed(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 1)
        {
            controller.combat.cursorSelect.Play();
            Shield choice = Instantiate(controller.registerObjects.allShields[3]);
            controller.interactableItems.inventory.Add(choice);
            if (!controller.ego.equippedWeapon.twoHanded) { controller.GetStrapped(choice); }
        }
        else if (controller.npcInteraction.genericOptionSelected == 2 && controller.ego.potionBelt.Count < 3)
        {
            controller.combat.cursorSelect.Play();
            controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[1]));
        }
        else
        {
            controller.interactableItems.cursorCancel.Play();
            martyrCounter++;
        }

        //controller.npcInteraction.dialogueBox.SetActive(false);
        //controller.npcInteraction.dialogueBoxBackground.SetActive(false);
        StartCoroutine(controller.roomNavigation.FadeAudioOut(interimTheme, .25f));
        StartCoroutine(controller.combat.BeginBattle(controller.combat.allBadGuys[2], 2));
        demoProceed = false;
        yield return new WaitUntil(DemoProceed);
        demoProceed = false;
        //controller.combat.fightOverWhiteScreen.SetActive(false);

        //  battle 4 --
        //controller.npcInteraction.ActivateDialogueBox();
        controller.npcInteraction.ClearOptions();
        controller.npcInteraction.NPCText.text = "";
        StartCoroutine(controller.roomNavigation.FadeAudioIn(interimTheme, .25f));
        demoDialogue.Clear();
        demoDialogue.Add($"But can you handle these guys?");
        demoDialogue.Add($"Take any one you want.");
        yield return new WaitForSeconds(.5f);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.GenericOptionSelection("Gun", "Club", "Light Healing Elixir"));
        controller.npcInteraction.genericOptionComplete = false;
        yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
        controller.npcInteraction.genericOptionComplete = false;
        if (controller.npcInteraction.genericOptionSelected == 0)
        {
            controller.combat.cursorSelect.Play();
            Weapon choice = Instantiate(controller.registerObjects.allWeapons[12]);
            controller.interactableItems.inventory.Add(choice);
            controller.GetEquipped(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 1)
        {
            controller.combat.cursorSelect.Play();
            Weapon choice = Instantiate(controller.registerObjects.allWeapons[9]);
            controller.interactableItems.inventory.Add(choice);
            if (controller.ego.equippedShield != null) { controller.GetUnStrapped(); }
            controller.GetEquipped(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 2 && controller.ego.potionBelt.Count < 3)
        {
            controller.combat.cursorSelect.Play();
            controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[3]));
        }
        else
        {
            controller.interactableItems.cursorCancel.Play();
            martyrCounter++;
        }
        // item 2
        demoDialogue.Clear();
        demoDialogue.Add($"Take any one you want.");
        yield return new WaitForSeconds(.5f);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.GenericOptionSelection("Ringmail", "Greater Healing Potion", "Healing Elixir"));
        controller.npcInteraction.genericOptionComplete = false;
        yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
        controller.npcInteraction.genericOptionComplete = false;
        if (controller.npcInteraction.genericOptionSelected == 0)
        {
            controller.combat.cursorSelect.Play();
            Armor choice = Instantiate(controller.registerObjects.allArmor[4]);
            controller.interactableItems.inventory.Add(choice);
            controller.GetDressed(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 1 && controller.ego.potionBelt.Count < 3)
        {
            controller.combat.cursorSelect.Play();
            controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[2]));
        }
        else if (controller.npcInteraction.genericOptionSelected == 2 && controller.ego.potionBelt.Count < 3)
        {
            controller.combat.cursorSelect.Play();
            controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[4]));
        }
        else
        {
            controller.interactableItems.cursorCancel.Play();
            martyrCounter++;
        }

        //controller.npcInteraction.dialogueBox.SetActive(false);
        //controller.npcInteraction.dialogueBoxBackground.SetActive(false);
        StartCoroutine(controller.roomNavigation.FadeAudioOut(interimTheme, .25f));
        StartCoroutine(controller.combat.BeginBattle(controller.combat.allBadGuys[3], 3));
        demoProceed = false;
        yield return new WaitUntil(DemoProceed);
        demoProceed = false;
        //controller.combat.fightOverWhiteScreen.SetActive(false);

        //  battle 5 --
        //controller.npcInteraction.ActivateDialogueBox();
        controller.npcInteraction.ClearOptions();
        controller.npcInteraction.NPCText.text = "";
        StartCoroutine(controller.roomNavigation.FadeAudioIn(interimTheme, .25f));
        demoDialogue.Clear();
        demoDialogue.Add($"Hope you have some HP left.");
        demoDialogue.Add($"Take any one you want.");
        yield return new WaitForSeconds(.5f);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.GenericOptionSelection("Sword", "Pogo Stick", "Healing Elixir"));
        controller.npcInteraction.genericOptionComplete = false;
        yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
        controller.npcInteraction.genericOptionComplete = false;
        if (controller.npcInteraction.genericOptionSelected == 0)
        {
            controller.combat.cursorSelect.Play();
            Weapon choice = Instantiate(controller.registerObjects.allWeapons[13]);
            controller.interactableItems.inventory.Add(choice);
            controller.GetEquipped(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 1)
        {
            controller.combat.cursorSelect.Play();
            Weapon choice = Instantiate(controller.registerObjects.allWeapons[10]);
            controller.interactableItems.inventory.Add(choice);
            if (controller.ego.equippedShield != null) { controller.GetUnStrapped(); }
            controller.GetEquipped(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 2 && controller.ego.potionBelt.Count < 3)
        {
            controller.combat.cursorSelect.Play();
            controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[4]));
        }
        else
        {
            controller.interactableItems.cursorCancel.Play();
            martyrCounter++;
        }
        // item 2
        demoDialogue.Clear();
        demoDialogue.Add($"Take any one you want.");
        yield return new WaitForSeconds(.5f);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.GenericOptionSelection("Chainmail", "Shield", "Healing Elixir"));
        controller.npcInteraction.genericOptionComplete = false;
        yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
        controller.npcInteraction.genericOptionComplete = false;
        if (controller.npcInteraction.genericOptionSelected == 0)
        {
            controller.combat.cursorSelect.Play();
            Armor choice = Instantiate(controller.registerObjects.allArmor[5]);
            controller.interactableItems.inventory.Add(choice);
            controller.GetDressed(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 1)
        {
            controller.combat.cursorSelect.Play();
            Shield choice = Instantiate(controller.registerObjects.allShields[4]);
            controller.interactableItems.inventory.Add(choice);
            if (!controller.ego.equippedWeapon.twoHanded) { controller.GetStrapped(choice); }            
        }
        else if (controller.npcInteraction.genericOptionSelected == 2 && controller.ego.potionBelt.Count < 3)
        {
            controller.combat.cursorSelect.Play();
            controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[4]));
        }
        else
        {
            controller.interactableItems.cursorCancel.Play();
            martyrCounter++;
        }

        //controller.npcInteraction.dialogueBox.SetActive(false);
        //controller.npcInteraction.dialogueBoxBackground.SetActive(false);
        StartCoroutine(controller.roomNavigation.FadeAudioOut(interimTheme, .25f));
        StartCoroutine(controller.combat.BeginBattle(controller.combat.allBadGuys[4], 2));
        demoProceed = false;
        yield return new WaitUntil(DemoProceed);
        demoProceed = false;
        //controller.combat.fightOverWhiteScreen.SetActive(false);

        //  battle 6 --
        //controller.npcInteraction.ActivateDialogueBox();
        controller.npcInteraction.ClearOptions();
        controller.npcInteraction.NPCText.text = "";
        StartCoroutine(controller.roomNavigation.FadeAudioIn(interimTheme, .25f));
        demoDialogue.Clear();
        demoDialogue.Add($"All right -- I've got things to do. I'm just going to put in an automated system to finish for me... while I set it up, HERE!");
        demoDialogue.Add($"Take any one you want.");
        yield return new WaitForSeconds(.5f);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.GenericOptionSelection("Mace", "Ganon's Trident", "Greater Healing Elixir"));
        controller.npcInteraction.genericOptionComplete = false;
        yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
        controller.npcInteraction.genericOptionComplete = false;
        if (controller.npcInteraction.genericOptionSelected == 0)
        {
            controller.combat.cursorSelect.Play();
            Weapon choice = Instantiate(controller.registerObjects.allWeapons[14]);
            controller.interactableItems.inventory.Add(choice);
            controller.GetEquipped(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 1)
        {
            controller.combat.cursorSelect.Play();
            Weapon choice = Instantiate(controller.registerObjects.allWeapons[15]);
            controller.interactableItems.inventory.Add(choice);
            if (controller.ego.equippedShield != null) { controller.GetUnStrapped(); }
            controller.GetEquipped(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 2 && controller.ego.potionBelt.Count < 3)
        {
            controller.combat.cursorSelect.Play();
            controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[5]));
        }
        else
        {
            controller.interactableItems.cursorCancel.Play();
            martyrCounter++;
        }
        // item 2
        demoDialogue.Clear();
        demoDialogue.Add($"Take any one you want.");
        yield return new WaitForSeconds(.5f);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.GenericOptionSelection("Hazmat Suit", "The Mirror Shield", "Greater Healing Elixir"));
        controller.npcInteraction.genericOptionComplete = false;
        yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
        controller.npcInteraction.genericOptionComplete = false;
        if (controller.npcInteraction.genericOptionSelected == 0)
        {
            controller.combat.cursorSelect.Play();
            Armor choice = Instantiate(controller.registerObjects.allArmor[6]);
            controller.interactableItems.inventory.Add(choice);
            controller.GetDressed(choice);
        }
        else if (controller.npcInteraction.genericOptionSelected == 1)
        {
            controller.combat.cursorSelect.Play();
            Shield choice = Instantiate(controller.registerObjects.allShields[5]);
            controller.interactableItems.inventory.Add(choice);
            if (!controller.ego.equippedWeapon.twoHanded) { controller.GetStrapped(choice); }
        }
        else if (controller.npcInteraction.genericOptionSelected == 2 && controller.ego.potionBelt.Count < 3)
        {
            controller.combat.cursorSelect.Play();
            controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[5]));
        }
        else
        {
            controller.interactableItems.cursorCancel.Play();
            martyrCounter++;
        }

        //controller.npcInteraction.dialogueBox.SetActive(false);
        //controller.npcInteraction.dialogueBoxBackground.SetActive(false);
        StartCoroutine(controller.roomNavigation.FadeAudioOut(interimTheme, .25f));
        StartCoroutine(controller.combat.BeginBattle(controller.combat.allBadGuys[0], 5));
        demoProceed = false;
        yield return new WaitUntil(DemoProceed);
        demoProceed = false;
        //controller.combat.fightOverWhiteScreen.SetActive(false);
        controller.npcInteraction.WriteNPCName("Botte");
        StartCoroutine(AutomatedSystem());

        IEnumerator AutomatedSystem()
        {
            //controller.npcInteraction.ActivateDialogueBox();
            controller.npcInteraction.ClearOptions();
            controller.npcInteraction.NPCText.text = "";
            StartCoroutine(controller.roomNavigation.FadeAudioIn(interimTheme, .25f));
            demoDialogue.Clear();
            demoDialogue.Add($"Take any one you want.");
            yield return new WaitForSeconds(.5f);
            controller.npcInteraction.npcSpeechComplete = false;
            StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
            yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
            controller.npcInteraction.npcSpeechComplete = false;
            StartCoroutine(controller.npcInteraction.GenericOptionSelection("Mace", "Ganon's Trident", "Assassin's Dagger", "Greater Healing Potion"));
            controller.npcInteraction.genericOptionComplete = false;
            yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
            controller.npcInteraction.genericOptionComplete = false;
            if (controller.npcInteraction.genericOptionSelected == 0)
            {
                controller.combat.cursorSelect.Play();
                Weapon choice = Instantiate(controller.registerObjects.allWeapons[14]);
                controller.interactableItems.inventory.Add(choice);
                controller.GetEquipped(choice);
            }
            else if (controller.npcInteraction.genericOptionSelected == 1)
            {
                controller.combat.cursorSelect.Play();
                Weapon choice = Instantiate(controller.registerObjects.allWeapons[15]);
                controller.interactableItems.inventory.Add(choice);
                if (controller.ego.equippedShield != null) { controller.GetUnStrapped(); }
                controller.GetEquipped(choice);
            }
            else if (controller.npcInteraction.genericOptionSelected == 2)
            {
                controller.combat.cursorSelect.Play();
                Weapon choice = Instantiate(controller.registerObjects.allWeapons[11]);
                controller.interactableItems.inventory.Add(choice);
                if (controller.ego.equippedShield != null) { controller.GetUnStrapped(); }
                controller.GetEquipped(choice);
            }
            else if (controller.npcInteraction.genericOptionSelected == 3 && controller.ego.potionBelt.Count < 3)
            {
                controller.combat.cursorSelect.Play();
                controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[2]));
            }
            else { controller.interactableItems.cursorCancel.Play(); }
            // item 2
            demoDialogue.Clear();
            demoDialogue.Add($"Take any one you want.");
            yield return new WaitForSeconds(.5f);
            controller.npcInteraction.npcSpeechComplete = false;
            StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
            yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
            controller.npcInteraction.npcSpeechComplete = false;
            StartCoroutine(controller.npcInteraction.GenericOptionSelection("Hazmat Suit", "The Mirror Shield", "Greater Healing Elixir"));
            controller.npcInteraction.genericOptionComplete = false;
            yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
            controller.npcInteraction.genericOptionComplete = false;
            if (controller.npcInteraction.genericOptionSelected == 0)
            {
                controller.combat.cursorSelect.Play();
                Armor choice = Instantiate(controller.registerObjects.allArmor[6]);
                controller.interactableItems.inventory.Add(choice);
                controller.GetDressed(choice);
            }
            else if (controller.npcInteraction.genericOptionSelected == 1)
            {
                controller.combat.cursorSelect.Play();
                Shield choice = Instantiate(controller.registerObjects.allShields[5]);
                controller.interactableItems.inventory.Add(choice);
                if (!controller.ego.equippedWeapon.twoHanded) { controller.GetStrapped(choice); }
            }
            else if (controller.npcInteraction.genericOptionSelected == 2 && controller.ego.potionBelt.Count < 3)
            {
                controller.combat.cursorSelect.Play();
                controller.ego.potionBelt.Add(Instantiate(controller.registerObjects.allPotions[5]));
            }
            else { controller.interactableItems.cursorCancel.Play(); }

            // enemy choice
            BadGuy badGuyChosen = Instantiate(controller.combat.allBadGuys[0]);
            demoDialogue.Clear();
            demoDialogue.Add($"Choose your foe.");
            yield return new WaitForSeconds(.5f);
            controller.npcInteraction.npcSpeechComplete = false;
            StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
            yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
            controller.npcInteraction.npcSpeechComplete = false;
            StartCoroutine(controller.npcInteraction.GenericOptionSelection("The Creeper in the Dark", "An Aggressive Peace Monger", "A Bold Yet Discretionary Strategist", "A Big Burly Brute"));
            controller.npcInteraction.genericOptionComplete = false;
            yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
            controller.npcInteraction.genericOptionComplete = false;
            if (controller.npcInteraction.genericOptionSelected == 0)
            {
                controller.combat.cursorSelect.Play();
                badGuyChosen = Instantiate(controller.combat.allBadGuys[0]);
            }
            else if (controller.npcInteraction.genericOptionSelected == 1)
            {
                controller.combat.cursorSelect.Play();
                badGuyChosen = Instantiate(controller.combat.allBadGuys[1]);
            }
            else if (controller.npcInteraction.genericOptionSelected == 2)
            {
                controller.combat.cursorSelect.Play();
                badGuyChosen = Instantiate(controller.combat.allBadGuys[2]);
            }
            else if (controller.npcInteraction.genericOptionSelected == 3)
            {
                controller.combat.cursorSelect.Play();
                badGuyChosen = Instantiate(controller.combat.allBadGuys[3]);
            }
            else { controller.interactableItems.cursorCancel.Play(); }

            // number choice
            int numberChosen = 5;
            demoDialogue.Clear();
            demoDialogue.Add($"Choose their numbers.");
            yield return new WaitForSeconds(.5f);
            controller.npcInteraction.npcSpeechComplete = false;
            StartCoroutine(controller.npcInteraction.NPCSpeech(demoDialogue));
            yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
            controller.npcInteraction.npcSpeechComplete = false;
            StartCoroutine(controller.npcInteraction.GenericOptionSelection("1", "2", "3", "4"));
            controller.npcInteraction.genericOptionComplete = false;
            yield return new WaitUntil(controller.npcInteraction.GenericOptionComplete);
            controller.npcInteraction.genericOptionComplete = false;
            if (controller.npcInteraction.genericOptionSelected == 0)
            {
                controller.combat.cursorSelect.Play();
                numberChosen = 1;
            }
            else if (controller.npcInteraction.genericOptionSelected == 1)
            {
                controller.combat.cursorSelect.Play();
                numberChosen = 2;
            }
            else if (controller.npcInteraction.genericOptionSelected == 2)
            {
                controller.combat.cursorSelect.Play();
                numberChosen = 3;
            }
            else if (controller.npcInteraction.genericOptionSelected == 3)
            {
                controller.combat.cursorSelect.Play();
                numberChosen = 4;
            }
            else { controller.interactableItems.cursorCancel.Play(); }

            //controller.npcInteraction.dialogueBox.SetActive(false);
            //controller.npcInteraction.dialogueBoxBackground.SetActive(false);
            StartCoroutine(controller.roomNavigation.FadeAudioOut(interimTheme, .25f));
            StartCoroutine(controller.combat.BeginBattle(badGuyChosen, numberChosen));
            demoProceed = false;
            yield return new WaitUntil(DemoProceed);
            demoProceed = false;
            //controller.combat.fightOverWhiteScreen.SetActive(false);
            StartCoroutine(AutomatedSystem());
        }
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
