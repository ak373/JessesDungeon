using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
    public GameObject dialogueBox, dialogueBoxBackground, NPC1Border, NPC2Border, replyBox, replyBoxBackground, replyBoxFade, optionBox, option1Background, option2Background, option3Background, option4Background, option1Highlight, option2Highlight, option3Highlight, option4Highlight, optionBoxBackground, npcTextBackground, optionBoxGreyFilter, continueArrow;
    public GameObject reply0Background, reply1Background, reply2Background, reply3Background, reply4Background, reply5Background, reply6Background, reply7Background, reply8Background, reply9Background, reply10Background;
    public GameObject reply0Highlight, reply1Highlight, reply2Highlight, reply3Highlight, reply4Highlight, reply5Highlight, reply6Highlight, reply7Highlight, reply8Highlight, reply9Highlight, reply10Highlight;
    public TMP_Text NPC1Name, NPC2Name, NPCText, reply0, reply1, reply2, reply3, reply4, reply5, reply6, reply7, reply8, reply9, reply10, escToReturnReply, option1, option2, option3, option4;
    TMP_Text[] replyRay = { null, null, null, null, null, null, null, null, null, null, null };
    GameObject[] replyBackRay = { null, null, null, null, null, null, null, null, null, null, null };
    GameObject[] replyHighRay = { null, null, null, null, null, null, null, null, null, null, null };

    public GameObject shop, shopBackground, weaponBackground, armorBackground, shieldBackground, weaponHighlight, armorHighlight, shieldHighlight, weaponDescriptionBox, weaponDescriptionBoxBorder, armorDescriptionBox, armorDescriptionBoxBorder, shieldDescriptionBox, shieldDescriptionBoxBorder, currentTwoHanded, newTwoHanded, wholeScreenFadeBlack;
    public Image weaponTitleBackground, armorTitleBackground, shieldTitleBackground;
    Color titleUnselected = new Color(0.2941177f, 0f, 0f, 0.2941177f);
    Color titleSelected = new Color(0.4901961f, 0f, 0f, 0.4901961f);
    public TMP_Text weaponTitle, armorTitle, shieldTitle, weaponDescription, armorDescription, shieldDescription, weaponText, weaponPrice, armorText, armorPrice, shieldText, shieldPrice, adjustedDamage, adjustedCritical, adjustedToHit, adjustedArmorClass, adjustedCritResist, adjustedDamageReduction, equippedStat1Title, equippedStat2Title, equippedStat3Title, equippedStat1, equippedStat2, equippedStat3, equippedItemTitle, newItemTitle, newStat1Title, newStat2Title, newStat3Title, newStat1, newStat2, newStat3, currentType, newType;
    public TMP_Text egoWeaponText, egoArmorText, egoShieldText, currentCrystalsText, shopCrystalsText;

    public NPC[] allNPCs;
    public AudioSource purchase, error, rest;
    public Ego ego;

    int endingCharacter;
    [HideInInspector] public int genericOptionSelected;
    int saleDivider = 4;
    int askAboutMemoryElement = -1;
    int optionSelectMemoryElement = -1;
    [HideInInspector] public bool messageComplete, npcSpeechComplete, inventoryClosed, buyComplete, sellComplete, restComplete, genericOptionComplete;
    GameController controller;
    IEnumerator askAbout;
    Queue<string> sentences = new Queue<string>();
    Item selectedItem;

    //second quest pronouns
    [HideInInspector] public string bro = "bro";
    [HideInInspector] public string man = "man";
    [HideInInspector] public string dude = "dude";
    [HideInInspector] public string guy = "guy";

    //
    //allNPCs[0] = Badger
    //allNPCs[1] = Skinny Pete


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
        replyRay[0] = reply0;
        replyRay[1] = reply1;
        replyRay[2] = reply2;
        replyRay[3] = reply3;
        replyRay[4] = reply4;
        replyRay[5] = reply5;
        replyRay[6] = reply6;
        replyRay[7] = reply7;
        replyRay[8] = reply8;
        replyRay[9] = reply9;
        replyRay[10] = reply10;
        replyBackRay[0] = reply0Background;
        replyBackRay[1] = reply1Background;
        replyBackRay[2] = reply2Background;
        replyBackRay[3] = reply3Background;
        replyBackRay[4] = reply4Background;
        replyBackRay[5] = reply5Background;
        replyBackRay[6] = reply6Background;
        replyBackRay[7] = reply7Background;
        replyBackRay[8] = reply8Background;
        replyBackRay[9] = reply9Background;
        replyBackRay[10] = reply10Background;
        replyHighRay[0] = reply0Highlight;
        replyHighRay[1] = reply1Highlight;
        replyHighRay[2] = reply2Highlight;
        replyHighRay[3] = reply3Highlight;
        replyHighRay[4] = reply4Highlight;
        replyHighRay[5] = reply5Highlight;
        replyHighRay[6] = reply6Highlight;
        replyHighRay[7] = reply7Highlight;
        replyHighRay[8] = reply8Highlight;
        replyHighRay[9] = reply9Highlight;
        replyHighRay[10] = reply10Highlight;
        
        ResetHasBeenSaid();
        //badger dialogue
        allNPCs[0].openingGreeting.Clear();
        allNPCs[0].openingGreeting.Add($"Hey, {man}, is there something I can help you with?");
        allNPCs[0].giveItemResponse.Clear();
        allNPCs[0].giveItemResponse.Add($"Oh? What do you have for me?");
        allNPCs[0].initiateTradeResponse.Clear();
        allNPCs[0].initiateTradeResponse.Add($"Yeah, {man}, get some rest. Is 3 crystals OK?");
        allNPCs[0].closingRemark.Clear();
        allNPCs[0].closingRemark.Add($"Don't over-do it, yeah?");
        //skinny pete dialogue
        allNPCs[1].openingGreeting.Clear();
        allNPCs[1].openingGreeting.Add($"What's up, {bro}? Got somethin' to trade?");
        allNPCs[1].giveItemResponse.Clear();
        allNPCs[1].giveItemResponse.Add($"A freebie? You shouldn't have! What you got, {man}?");
        allNPCs[1].initiateTradeResponse.Clear();
        allNPCs[1].initiateTradeResponse.Add($"All right!! We buyin' or sellin'?");
        allNPCs[1].closingRemark.Clear();
        allNPCs[1].closingRemark.Add($"I know you gotta be careful and whatnot, but go bring me back some goodies, yeah?");
    }
    void ResetHasBeenSaid()
    {
        for (int i = 0; i < allNPCs.Length; i++)
        {
            if (allNPCs[i].askAbout.Count > 0)
            {
                for (int j = 0; j < allNPCs[i].askAbout.Count; j++)
                {
                    DigTreeDeeper(allNPCs[i].askAbout);                    
                }
            }            
        }

        void DigTreeDeeper(List<DialogueOption> currentTree)
        {
            for (int i = 0; i < currentTree.Count; i++)
            {
                currentTree[i].hasBeenSaid = false;
                if (currentTree[i].additionalReplies.Count > 0)
                {
                    DigTreeDeeper(currentTree[i].additionalReplies);
                }
            }
        }
    }
    public void WriteNPCName(string name, int boxNumber = 1)
    {
        if (boxNumber == 1) { NPC1Name.text = name; }
        else { NPC2Name.text = name; }
    }
    public void WriteDialogueOptions(string line1 = "Ask about...", string line2 = "Give item", string line3 = null, string line4 = "Enough already")
    {
        option1.text = line1;
        option2.text = line2;
        option3.text = line3;
        option4.text = line4;
    }
    void WriteDialogueReplies(List<DialogueOption> dialogueTree)
    {
        int counter = 0;
        for (int i = 0; i < dialogueTree.Count; i++)
        {
            if (dialogueTree[i].availableToSay)
            {
                replyRay[counter].text = dialogueTree[i].reply;
                replyBackRay[counter].SetActive(true);
                if (dialogueTree[i].hasBeenSaid) { replyRay[counter].color = Color.gray; }
                else { replyRay[counter].color = Color.white; }
                counter++;
            }
        }
        for (int i = counter; i < 11; i++)
        {
            replyRay[i].text = "";
            replyBackRay[i].SetActive(false);
        }
    }
    public void StartInitiateDialogue(NPC speaker)
    {
        StartCoroutine(InitiateDialogue(speaker));
    }
    public IEnumerator InitiateDialogue(NPC speaker)
    {
        controller.inputBox.SetActive(false);
        TurnOffOptionBackLights();
        WriteNPCName(speaker.nome);
        WriteDialogueOptions(null, null, null, null);
        ActivateDialogueBox();
        yield return new WaitForSeconds(.5f);
        npcSpeechComplete = false;
        StartCoroutine(NPCSpeech(speaker.openingGreeting));
        yield return new WaitUntil(NPCSpeechComplete);
        npcSpeechComplete = false;
        StartCoroutine(OptionSelect(speaker));
    }
    public void ActivateDialogueBox()
    {
        NPCText.text = "";
        dialogueBox.SetActive(true);
        dialogueBoxBackground.SetActive(true);
    }
    public void TurnOffOptionBackLights()
    {
        option1Background.SetActive(false);
        option2Background.SetActive(false);
        option3Background.SetActive(false);
        option4Background.SetActive(false);
        option1Highlight.SetActive(false);
        option2Highlight.SetActive(false);
        option3Highlight.SetActive(false);
        option4Highlight.SetActive(false);
    }
    public void ClearOptions()
    {
        option1.text = "";
        option2.text = "";
        option3.text = "";
        option4.text = "";
        TurnOffOptionBackLights();
    }
    public IEnumerator GenericOptionSelection(string opt1, string opt2 = null, string opt3 = null, string opt4 = null)
    {
        optionBoxGreyFilter.SetActive(false);
        WriteDialogueOptions(opt1, opt2, opt3, opt4);

        int selectedElement = 0;
        string plainOption1 = option1.text;
        string plainOption2 = option2.text;
        string plainOption3 = option3.text;
        string plainOption4 = option4.text;
        TurnOffOptionBackLights();
        if (opt1 != null) { option1Background.SetActive(true); }
        if (opt2 != null) { option2Background.SetActive(true); }
        if (opt3 != null) { option3Background.SetActive(true); }
        if (opt4 != null) { option4Background.SetActive(true); }

        while (true)
        {
            //if (optionSelectMemoryElement != -1)
            //{
            //    selectedElement = optionSelectMemoryElement;
            //    optionSelectMemoryElement = -1;
            //}
            if (selectedElement < 0) { selectedElement = 3; }
            if (selectedElement > 3) { selectedElement = 0; }

            option1.text = plainOption1;
            option2.text = plainOption2;
            option3.text = plainOption3;
            option4.text = plainOption4;
            option1Highlight.SetActive(false);
            option2Highlight.SetActive(false);
            option3Highlight.SetActive(false);
            option4Highlight.SetActive(false);

            if (selectedElement == 0)
            {
                option1.text = $"<color=yellow>{option1.text}</color>";
                option1Highlight.SetActive(true);
            }
            else if (selectedElement == 1)
            {
                option2.text = $"<color=yellow>{option2.text}</color>";
                option2Highlight.SetActive(true);
            }
            else if (selectedElement == 2)
            {
                option3.text = $"<color=yellow>{option3.text}</color>";
                option3Highlight.SetActive(true);
            }
            else if (selectedElement == 3)
            {
                option4.text = $"<color=yellow>{option4.text}</color>";
                option4Highlight.SetActive(true);
            }

            yield return new WaitUntil(controller.UpDownEnterEscPressed);
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                controller.interactableItems.cursorMove.Play();
                selectedElement--;
                if (opt4 == null && selectedElement == -1) { selectedElement = 2; }
                if (opt3 == null && selectedElement == 2) { selectedElement = 1; }
                if (opt2 == null && selectedElement == 1) { selectedElement = 0; }
                if (opt1 == null && selectedElement == 0) { selectedElement = -1; }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                controller.interactableItems.cursorMove.Play();
                selectedElement++;
                if (opt1 == null && selectedElement == 0) { selectedElement = 1; }
                if (opt2 == null && selectedElement == 1) { selectedElement = 2; }
                if (opt3 == null && selectedElement == 2) { selectedElement = 3; }
                if (opt4 == null && selectedElement == 3) { selectedElement = 0; }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                optionBoxGreyFilter.SetActive(true);
                option1.text = plainOption1;
                option2.text = plainOption2;
                option3.text = plainOption3;
                option4.text = plainOption4;
                genericOptionSelected = -1;
                optionSelectMemoryElement = -1;
                genericOptionComplete = true;
                break;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                optionBoxGreyFilter.SetActive(true);
                if (selectedElement == 0)
                {
                    optionSelectMemoryElement = selectedElement;
                    genericOptionSelected = 0;
                }
                else if (selectedElement == 1)
                {
                    optionSelectMemoryElement = selectedElement;
                    genericOptionSelected = 1;
                }
                else if (selectedElement == 2)
                {
                    optionSelectMemoryElement = selectedElement;
                    genericOptionSelected = 2;
                }
                else if (selectedElement == 3)
                {
                    optionSelectMemoryElement = selectedElement;
                    genericOptionSelected = 3;
                }
                //needs to move? but if 3 isn't cancel then where does it go?
                optionSelectMemoryElement = -1;
                //
                genericOptionComplete = true;
                break;
            }
        }
    }
    IEnumerator OptionSelect(NPC speaker, string opt1 = "default", string opt2 = "default", string opt3 = "default", string opt4 = "default")
    {
        if (opt1 == "default") { opt1 = speaker.options[0]; }
        if (opt2 == "default") { opt2 = speaker.options[1]; }
        if (opt3 == "default") { opt3 = speaker.options[2]; }
        if (opt4 == "default") { opt4 = speaker.options[3]; }

        optionBoxGreyFilter.SetActive(false);
        WriteDialogueOptions(opt1, opt2, opt3, opt4);

        int selectedElement = 0;
        string plainOption1 = option1.text;
        string plainOption2 = option2.text;
        string plainOption3 = option3.text;
        string plainOption4 = option4.text;
        TurnOffOptionBackLights();
        if (opt1 != null) { option1Background.SetActive(true); }
        if (opt2 != null) { option2Background.SetActive(true); }
        if (opt3 != null) { option3Background.SetActive(true); }
        if (opt4 != null) { option4Background.SetActive(true); }

        while (true)
        {
            if (optionSelectMemoryElement != -1)
            {
                selectedElement = optionSelectMemoryElement;
                optionSelectMemoryElement = -1;
            }
            if (selectedElement < 0) { selectedElement = 3; }
            if (selectedElement > 3) { selectedElement = 0; }

            option1.text = plainOption1;
            option2.text = plainOption2;
            option3.text = plainOption3;
            option4.text = plainOption4;
            option1Highlight.SetActive(false);
            option2Highlight.SetActive(false);
            option3Highlight.SetActive(false);
            option4Highlight.SetActive(false);

            if (selectedElement == 0)
            {
                option1.text = $"<color=yellow>{option1.text}</color>";
                option1Highlight.SetActive(true);
            }
            else if (selectedElement == 1)
            {
                option2.text = $"<color=yellow>{option2.text}</color>";
                option2Highlight.SetActive(true);
            }
            else if (selectedElement == 2)
            {
                option3.text = $"<color=yellow>{option3.text}</color>";
                option3Highlight.SetActive(true);
            }
            else if (selectedElement == 3)
            {
                option4.text = $"<color=yellow>{option4.text}</color>";
                option4Highlight.SetActive(true);
            }

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
                optionBoxGreyFilter.SetActive(true);
                controller.interactableItems.cursorCancel.Play();
                option1.text = plainOption1;
                option2.text = plainOption2;
                option3.text = plainOption3;
                option4.text = plainOption4;
                optionSelectMemoryElement = -1;
                ReturnToGame();
                break;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                optionBoxGreyFilter.SetActive(true);
                if (selectedElement == 0)
                {
                    askAboutMemoryElement = -1;
                    optionSelectMemoryElement = selectedElement;
                    controller.interactableItems.cursorSelect.Play();
                    StartCoroutine(ActivateAskAbout(speaker.askAbout, speaker));
                }
                else if (selectedElement == 1)
                {
                    optionSelectMemoryElement = selectedElement;
                    controller.interactableItems.cursorSelect.Play();
                    StartCoroutine(GiveItem(speaker));
                }
                else if (selectedElement == 2)
                {
                    optionSelectMemoryElement = selectedElement;
                    controller.interactableItems.cursorSelect.Play();
                    NPCTradeDistributor(speaker);
                }
                else if (selectedElement == 3)
                {
                    optionSelectMemoryElement = -1;
                    controller.interactableItems.cursorCancel.Play();
                    option1.text = plainOption1;
                    option2.text = plainOption2;
                    option3.text = plainOption3;
                    option4.text = plainOption4;
                    ReturnToGame();
                }
                break;
            }
        }
    }
    IEnumerator GiveItem(NPC receiver)
    {
        NPCText.text = "";
        yield return new WaitForSeconds(.2f);
        npcSpeechComplete = false;
        StartCoroutine(NPCSpeech(receiver.giveItemResponse));
        yield return new WaitUntil(NPCSpeechComplete);
        npcSpeechComplete = false;
        inventoryClosed = false;
        StartCoroutine(DisplayInventory());
        yield return new WaitUntil(InventoryClosed);
        inventoryClosed = false;
        NPCText.text = "";
        yield return new WaitForSeconds(.2f);
        if (selectedItem != null)
        {
            //good point. what happens??
            //if proper receiver and proper item
            //else
            npcSpeechComplete = false;
            List<string> returnItem = new List<string>();
            returnItem.Add("That's very thoughtful, but I think this'll be better off with you.");
            StartCoroutine(NPCSpeech(returnItem));
            yield return new WaitUntil(NPCSpeechComplete);
            npcSpeechComplete = false;
            StartCoroutine(OptionSelect(receiver));
        }
        else
        {
            npcSpeechComplete = false;
            List<string> nullItem = new List<string>();
            nullItem.Add("Oh, I'm wrong.");
            StartCoroutine(NPCSpeech(nullItem));
            yield return new WaitUntil(NPCSpeechComplete);
            npcSpeechComplete = false;
            StartCoroutine(OptionSelect(receiver));
        }
        NPCText.text = "";
    }
    void NPCTradeDistributor(NPC npc)
    {
        //if npc.trade == thing {activate correct coroutine
        if (npc.nome == "Skinny Pete") { StartCoroutine(PeteShop(npc)); }
        else if (npc.nome == "Badger") { StartCoroutine(BadgerRest(npc)); }
    }
    IEnumerator BadgerRest(NPC badger)
    {
        TurnOffOptionBackLights();
        npcSpeechComplete = false;
        yield return new WaitForSeconds(.25f);
        StartCoroutine(NPCSpeech(badger.initiateTradeResponse));
        yield return new WaitUntil(NPCSpeechComplete);
        npcSpeechComplete = false;
        genericOptionComplete = false;
        StartCoroutine(GenericOptionSelection("Yeah. I'll take a break.", "No! I must press on!", null, null));
        yield return new WaitUntil(GenericOptionComplete);
        genericOptionComplete = false;
        if (genericOptionSelected == 0)
        {
            if (ego.blueCrystals >= 3)
            {
                npcSpeechComplete = false;
                List<string> restChosen = new List<string>();
                restChosen.Add($"Don't worry for a second -- me and Pete got your back.");
                yield return new WaitForSeconds(.25f);
                controller.interactableItems.cursorSelect.Play();
                StartCoroutine(NPCSpeech(restChosen));
                yield return new WaitUntil(NPCSpeechComplete);
                npcSpeechComplete = false;
                yield return new WaitForSeconds(.5f);
                restComplete = false;
                StartCoroutine(Rest());
                yield return new WaitUntil(RestComplete);
                restComplete = false;
                yield return new WaitForSeconds(.5f);
                npcSpeechComplete = false;
                List<string> restFinish = new List<string>();
                restFinish.Add($"Don't you look like a million bucks? Now - up and at them!");
                StartCoroutine(NPCSpeech(restFinish));
                yield return new WaitUntil(NPCSpeechComplete);
                npcSpeechComplete = false;
            }
            else
            {
                npcSpeechComplete = false;
                List<string> restFail = new List<string>();
                restFail.Add($"You're looking a little light in the pockets -- I need to eat, too... you understand, right? I'll be here when you've got a few more to spend.");
                restFail.Add($"Can I help you with anything else?");
                yield return new WaitForSeconds(.25f);
                error.Play();
                StartCoroutine(NPCSpeech(restFail));
                yield return new WaitUntil(NPCSpeechComplete);
                npcSpeechComplete = false;
                StartCoroutine(OptionSelect(badger));
            }
        }
        else if (genericOptionSelected == 1 || genericOptionSelected == -1)
        {
            npcSpeechComplete = false;
            List<string> optionReturn = new List<string>();
            optionReturn.Add($"All right, that's cool... Maybe next time. Can I help you with anything else?");
            yield return new WaitForSeconds(.25f);
            controller.interactableItems.cursorSelect.Play();
            StartCoroutine(NPCSpeech(optionReturn));
            yield return new WaitUntil(NPCSpeechComplete);
            npcSpeechComplete = false;
            StartCoroutine(OptionSelect(badger));
        }

        IEnumerator Rest()
        {
            StartCoroutine(controller.roomNavigation.FadeAudioOut(controller.registerRooms.townTheme, .25f));
            StartCoroutine(controller.roomNavigation.FadeAudioIn(rest, .25f));
            yield return new WaitForSeconds(2f);
            wholeScreenFadeBlack.SetActive(true);
            StartCoroutine(controller.roomNavigation.FadeAudioOut(rest, 4f));
            yield return new WaitForSeconds(6f);
            ego.blueCrystals -= 3;
            ego.allStats[0].value = ego.allStats[2].value + ego.allStats[2].effectValue;
            ego.allStats[1].value = ego.allStats[0].value;
            while (true)
            {
                int counter = 0;
                for (int i = 0; i < ego.activeEffects.Count; i++)
                {
                    if (!ego.activeEffects[i].beneficial)
                    {
                        ego.activeEffects.Remove(ego.activeEffects[i]);
                        break;
                    }
                    else { counter++; }
                }
                if (counter == ego.activeEffects.Count) { break; }
            }
            StartCoroutine(controller.roomNavigation.FadeAudioIn(controller.registerRooms.townTheme, .5f));
            yield return new WaitForSeconds(1f);
            wholeScreenFadeBlack.SetActive(false);
            restComplete = true;
        }
    }
    IEnumerator PeteShop(NPC pete)
    {
        List<Item> weaponList = new List<Item>();
        List<Item> armorList = new List<Item>();
        List<Item> shieldList = new List<Item>();
        for (int i = 0; i < controller.registerObjects.allWeapons.Length; i++)
        {
            if (controller.registerObjects.allWeapons[i].unlocked) { weaponList.Add(controller.registerObjects.allWeapons[i]); }
        }
        for (int i = 0; i < controller.registerObjects.allArmor.Length; i++)
        {
            if (controller.registerObjects.allArmor[i].unlocked) { armorList.Add(controller.registerObjects.allArmor[i]); }
        }
        for (int i = 0; i < controller.registerObjects.allShields.Length; i++)
        {
            if (controller.registerObjects.allShields[i].unlocked) { shieldList.Add(controller.registerObjects.allShields[i]); }
        }
        weaponText.text = WriteShoppingList(weaponList);
        weaponPrice.text = WritePriceList(weaponList);
        armorText.text = WriteShoppingList(armorList);
        armorPrice.text = WritePriceList(armorList);
        shieldText.text = WriteShoppingList(shieldList);
        shieldPrice.text = WritePriceList(shieldList);
        string normalWeaponText = weaponText.text;
        string normalArmorText = armorText.text;
        string normalShieldText = shieldText.text;
        string normalWeaponPrice = weaponPrice.text;
        string normalArmorPrice = armorPrice.text;
        string normalShieldPrice = shieldPrice.text;
        int selectedElement = 0;
        int weaponMemory = -1;
        int armorMemory = -1;
        int shieldMemory = -1;
        int columnMemory = 1;

        npcSpeechComplete = false;
        yield return new WaitForSeconds(.25f);
        StartCoroutine(NPCSpeech(pete.initiateTradeResponse));
        yield return new WaitUntil(NPCSpeechComplete);
        npcSpeechComplete = false;
        genericOptionComplete = false;
        StartCoroutine(GenericOptionSelection("Buy", "Sell", null, "Nevermind"));
        yield return new WaitUntil(GenericOptionComplete);
        if (genericOptionSelected == 0 || genericOptionSelected == 1) { controller.interactableItems.cursorSelect.Play(); }
        else { controller.interactableItems.cursorCancel.Play(); }
        genericOptionComplete = false;
        yield return new WaitForSeconds(.25f);
        if (genericOptionSelected == 0)
        {
            npcSpeechComplete = false;
            List<string> buyChosen = new List<string>();
            buyChosen.Add($"Just let me know if you see something you like!");
            StartCoroutine(NPCSpeech(buyChosen));
            yield return new WaitUntil(NPCSpeechComplete);
            npcSpeechComplete = false;
            yield return new WaitForSeconds(.25f);
            buyComplete = false;
            StartCoroutine(Buy());
            yield return new WaitUntil(BuyComplete);
            buyComplete = false;
        }
        else if (genericOptionSelected == 1)
        {
            npcSpeechComplete = false;
            List<string> sellChosen = new List<string>();
            sellChosen.Add($"Whatcha got for me, {guy}?");
            StartCoroutine(NPCSpeech(sellChosen));
            yield return new WaitUntil(NPCSpeechComplete);
            npcSpeechComplete = false;
            yield return new WaitForSeconds(.5f);
            sellComplete = false;
            StartCoroutine(Sell());
            yield return new WaitUntil(SellComplete);
            sellComplete = false;
        }
        else if (genericOptionSelected == 3 || genericOptionSelected == -1)
        {
            npcSpeechComplete = false;
            List<string> optionReturn = new List<string>();            
            optionReturn.Add($"All right, {dude}, can I do anything else for ya?");
            StartCoroutine(NPCSpeech(optionReturn));
            yield return new WaitUntil(NPCSpeechComplete);
            npcSpeechComplete = false;
            StartCoroutine(OptionSelect(pete));
        }


        IEnumerator Buy()
        {
            IEnumerator weaponTeletype = TeletypeDescription(weaponDescription);
            IEnumerator armorTeletype = TeletypeDescription(armorDescription);
            IEnumerator shieldTeletype = TeletypeDescription(shieldDescription);

            TurnOffOptionBackLights();
            option1.text = "";
            option2.text = "";
            option3.text = "";
            option4.text = "";
            weaponHighlight.SetActive(false);
            armorHighlight.SetActive(false);
            shieldHighlight.SetActive(false);
            NPCText.text = "";
            shop.SetActive(true);
            shopBackground.SetActive(true);
            if (columnMemory == 1) { weaponHighlight.SetActive(true); }
            else if (columnMemory == 2) { armorHighlight.SetActive(true); }
            else if (columnMemory == 3) { shieldHighlight.SetActive(true); }

            while (true)
            {
                weaponText.text = normalWeaponText;
                armorText.text = normalArmorText;
                shieldText.text = normalShieldText;
                weaponPrice.text = normalWeaponPrice;
                armorPrice.text = normalArmorPrice;
                shieldPrice.text = normalShieldPrice;
                shopCrystalsText.text = $"Crystals: {ego.blueCrystals}";
                weaponTitleBackground.color = titleUnselected;
                armorTitleBackground.color = titleUnselected;
                shieldTitleBackground.color = titleUnselected;
                StopCoroutine(weaponTeletype);
                StopCoroutine(armorTeletype);
                StopCoroutine(shieldTeletype);
                weaponDescriptionBox.SetActive(false);
                armorDescriptionBox.SetActive(false);
                shieldDescriptionBox.SetActive(false);
                weaponDescriptionBoxBorder.SetActive(false);
                armorDescriptionBoxBorder.SetActive(false);
                shieldDescriptionBoxBorder.SetActive(false);
                weaponDescription.text = "";
                weaponDescription.maxVisibleCharacters = 0;
                armorDescription.text = "";
                armorDescription.maxVisibleCharacters = 0;
                shieldDescription.text = "";
                shieldDescription.maxVisibleCharacters = 0;
                int itemLength = 0;
                int itemIndex = 0;
                int priceLength = 0;
                int priceIndex = 0;
                string currentListText = "";
                string currentListPrice = "";
                if (weaponHighlight.activeInHierarchy)
                {
                    if (selectedElement < 0) { selectedElement = weaponList.Count - 1; }
                    if (selectedElement > weaponList.Count - 1) { selectedElement = 0; }
                    itemLength = weaponList[selectedElement].nome.Length;
                    priceLength = weaponList[selectedElement].price.ToString().Length;
                    //no item can complete encompass the name of another item in the same list
                    itemIndex = weaponText.text.IndexOf(myTI.ToTitleCase(weaponList[selectedElement].nome));
                    //done like this means no two items in the same list can have the same price
                    priceIndex = weaponPrice.text.IndexOf(weaponList[selectedElement].price.ToString());
                    currentListText = weaponText.text;
                    currentListPrice = weaponPrice.text;
                }
                else if (armorHighlight.activeInHierarchy)
                {
                    if (selectedElement < 0) { selectedElement = armorList.Count - 1; }
                    if (selectedElement > armorList.Count - 1) { selectedElement = 0; }
                    itemLength = armorList[selectedElement].nome.Length;
                    priceLength = armorList[selectedElement].price.ToString().Length;
                    //no item can complete encompass the name of another item in the same list
                    itemIndex = armorText.text.IndexOf(myTI.ToTitleCase(armorList[selectedElement].nome));
                    //done like this means no two items in the same list can have the same price
                    priceIndex = armorPrice.text.IndexOf(armorList[selectedElement].price.ToString());
                    currentListText = armorText.text;
                    currentListPrice = armorPrice.text;
                }
                else if (shieldHighlight.activeInHierarchy)
                {
                    if (selectedElement < 0) { selectedElement = shieldList.Count - 1; }
                    if (selectedElement > shieldList.Count - 1) { selectedElement = 0; }
                    itemLength = shieldList[selectedElement].nome.Length;
                    priceLength = shieldList[selectedElement].price.ToString().Length;
                    //no item can complete encompass the name of another item in the same list
                    itemIndex = shieldText.text.IndexOf(myTI.ToTitleCase(shieldList[selectedElement].nome));
                    //done like this means no two items in the same list can have the same price
                    priceIndex = shieldPrice.text.IndexOf(shieldList[selectedElement].price.ToString());
                    currentListText = shieldText.text;
                    currentListPrice = shieldPrice.text;
                }

                string newText = "";

                for (int i = 0; i < itemIndex; i++) { newText += currentListText[i]; }

                newText += "<color=yellow>";

                for (int i = itemIndex; i < itemIndex + itemLength; i++) { newText += currentListText[i]; }

                newText += "</color>";

                for (int i = itemIndex + itemLength; i < currentListText.Length; i++) { newText += currentListText[i]; }

                string newPrice = "";

                for (int i = 0; i < priceIndex; i++) { newPrice += currentListPrice[i]; }

                newPrice += "<color=yellow>";

                for (int i = priceIndex; i < priceIndex + priceLength; i++) { newPrice += currentListPrice[i]; }

                newPrice += "</color>";

                for (int i = priceIndex + priceLength; i < currentListPrice.Length; i++) { newPrice += currentListPrice[i]; }

                if (weaponHighlight.activeInHierarchy)
                {
                    weaponText.text = newText;
                    weaponPrice.text = newPrice;
                    weaponTitleBackground.color = titleSelected;
                    weaponDescriptionBox.SetActive(true);
                    weaponDescriptionBoxBorder.SetActive(true);
                    weaponDescription.text = weaponList[selectedElement].description;
                    weaponTeletype = TeletypeDescription(weaponDescription);
                    StartCoroutine(weaponTeletype);
                    ShopStats(weaponList[selectedElement]);
                }
                else if (armorHighlight.activeInHierarchy)
                {
                    armorText.text = newText;
                    armorPrice.text = newPrice;
                    armorTitleBackground.color = titleSelected;
                    armorDescriptionBox.SetActive(true);
                    armorDescriptionBoxBorder.SetActive(true);
                    armorDescription.text = armorList[selectedElement].description;
                    armorTeletype = TeletypeDescription(armorDescription);
                    StartCoroutine(armorTeletype);
                    ShopStats(armorList[selectedElement]);
                }
                else if (shieldHighlight.activeInHierarchy)
                {
                    shieldText.text = newText;
                    shieldPrice.text = newPrice;
                    shieldTitleBackground.color = titleSelected;
                    shieldDescriptionBox.SetActive(true);
                    shieldDescriptionBoxBorder.SetActive(true);
                    shieldDescription.text = shieldList[selectedElement].description;
                    shieldTeletype = TeletypeDescription(shieldDescription);
                    StartCoroutine(shieldTeletype);
                    ShopStats(shieldList[selectedElement]);
                }

                yield return new WaitUntil(controller.LeftRightUpDownEnterEscPressed);
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
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    controller.interactableItems.cursorMove.Play();
                    if (weaponHighlight.activeInHierarchy)
                    {
                        weaponHighlight.SetActive(false);
                        armorHighlight.SetActive(true);
                        weaponMemory = selectedElement;
                        if (armorMemory != -1) { selectedElement = armorMemory; }
                    }
                    else if (armorHighlight.activeInHierarchy)
                    {
                        armorHighlight.SetActive(false);
                        shieldHighlight.SetActive(true);
                        armorMemory = selectedElement;
                        if (shieldMemory != -1) { selectedElement = shieldMemory; }
                    }
                    else if (shieldHighlight.activeInHierarchy)
                    {
                        shieldHighlight.SetActive(false);
                        weaponHighlight.SetActive(true);
                        shieldMemory = selectedElement;
                        if (weaponMemory != -1) { selectedElement = weaponMemory; }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    controller.interactableItems.cursorMove.Play();
                    if (weaponHighlight.activeInHierarchy)
                    {
                        weaponHighlight.SetActive(false);
                        shieldHighlight.SetActive(true);
                        weaponMemory = selectedElement;
                        if (shieldMemory != -1) { selectedElement = shieldMemory; }
                    }
                    else if (armorHighlight.activeInHierarchy)
                    {
                        armorHighlight.SetActive(false);
                        weaponHighlight.SetActive(true);
                        armorMemory = selectedElement;
                        if (weaponMemory != -1) { selectedElement = weaponMemory; }
                    }
                    else if (shieldHighlight.activeInHierarchy)
                    {
                        shieldHighlight.SetActive(false);
                        armorHighlight.SetActive(true);
                        shieldMemory = selectedElement;
                        if (armorMemory != -1) { selectedElement = armorMemory; }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    controller.interactableItems.cursorCancel.Play();
                    shop.SetActive(false);
                    shopBackground.SetActive(false);
                    npcSpeechComplete = false;
                    yield return new WaitForSeconds(.25f);
                    List<string> buyExit = new List<string>();
                    buyExit.Add($"All right, {dude}, can I do anything else for ya?");
                    StartCoroutine(NPCSpeech(buyExit));
                    yield return new WaitUntil(NPCSpeechComplete);
                    npcSpeechComplete = false;
                    StartCoroutine(OptionSelect(pete));
                    buyComplete = true;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (weaponHighlight.activeInHierarchy)
                    {
                        selectedItem = weaponList[selectedElement];
                        columnMemory = 1;
                    }
                    else if (armorHighlight.activeInHierarchy)
                    {
                        selectedItem = armorList[selectedElement];
                        columnMemory = 2;
                    }
                    else if (shieldHighlight.activeInHierarchy)
                    {
                        selectedItem = shieldList[selectedElement];
                        columnMemory = 3;
                    }

                    if (ego.blueCrystals >= selectedItem.price)
                    {
                        controller.interactableItems.cursorSelect.Play();
                        option1.text = "";
                        option2.text = "";
                        option3.text = "";
                        option4.text = "";
                        shop.SetActive(false);
                        shopBackground.SetActive(false);
                        yield return new WaitForSeconds(.25f);
                        List<string> buyCheck = new List<string>();
                        buyCheck.Add($"The {myTI.ToTitleCase(selectedItem.nome)} for {selectedItem.price} crystals? No taksies-backsies!");
                        npcSpeechComplete = false;
                        StartCoroutine(NPCSpeech(buyCheck));
                        currentCrystalsText.text = $"Crystals: {ego.blueCrystals}";
                        yield return new WaitUntil(NPCSpeechComplete);
                        npcSpeechComplete = false;
                        optionBoxGreyFilter.SetActive(false);
                        option1.text = "Yep!";
                        option2.text = "Hmm. Perhaps not.";
                        option1Background.SetActive(true);
                        option2Background.SetActive(true);
                        bool yesSelected = true;
                        while (true)
                        {
                            option1Highlight.SetActive(false);
                            option2Highlight.SetActive(false);
                            if (yesSelected)
                            {
                                option1.color = Color.yellow;
                                option1Highlight.SetActive(true);
                                option2.color = Color.white;
                            }
                            else
                            {
                                option1.color = Color.white;
                                option2.color = Color.yellow;
                                option2Highlight.SetActive(true);
                            }
                            yield return new WaitUntil(controller.UpDownEnterEscPressed);
                            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                controller.interactableItems.cursorMove.Play();
                                yesSelected = !yesSelected;
                            }
                            else if (Input.GetKeyDown(KeyCode.Escape))
                            {
                                currentCrystalsText.text = $"";
                                optionBoxGreyFilter.SetActive(true);
                                controller.interactableItems.cursorCancel.Play();
                                yield return new WaitForSeconds(.25f);
                                List<string> buyCancel = new List<string>();
                                buyCancel.Add("Aw, man!");
                                buyCancel.Add("Anything else catch your eye?");
                                npcSpeechComplete = false;
                                StartCoroutine(NPCSpeech(buyCancel));
                                yield return new WaitUntil(NPCSpeechComplete);
                                npcSpeechComplete = false;
                                yield return new WaitForSeconds(.5f);
                                option1.color = Color.white;
                                option2.color = Color.white;
                                shop.SetActive(true);
                                shopBackground.SetActive(true);
                                option1.text = "Ask about";
                                option2.text = "Give item";
                                option3.text = "Shop";
                                option4.text = "Enough already";
                                StartCoroutine(Buy());
                                break;
                            }
                            else if (Input.GetKeyDown(KeyCode.Return))
                            {
                                currentCrystalsText.text = $"";
                                optionBoxGreyFilter.SetActive(true);
                                if (yesSelected)
                                {
                                    controller.interactableItems.cursorSelect.Play();
                                    purchase.Play();
                                    ego.blueCrystals -= selectedItem.price;
                                    controller.interactableItems.inventory.Add(selectedItem);
                                    yield return new WaitForSeconds(.25f);
                                    List<string> buyConfirm = new List<string>();
                                    buyConfirm.Add("Right on!");
                                    buyConfirm.Add("Equip it now?");
                                    //equip it now possibilities with 2hers
                                    bool unStrap = false;
                                    bool unEquip = false;
                                    if (selectedItem is Weapon)
                                    {
                                        Weapon selectedWeapon = (Weapon)selectedItem;
                                        if (selectedWeapon.twoHanded && ego.equippedShield != null)
                                        {
                                            unStrap = true;
                                            buyConfirm[1] = $"Equip it now? You'll need to take off your {myTI.ToTitleCase(ego.equippedShield.nome)}.";
                                        }
                                    }
                                    else if (selectedItem is Shield)
                                    {
                                        Shield selectedShield = (Shield)selectedItem;
                                        if (ego.equippedWeapon != null)
                                        {
                                            if (ego.equippedWeapon.twoHanded)
                                            {
                                                unEquip = true;
                                                buyConfirm[1] = $"Equip it now? You'll have to stow your {myTI.ToTitleCase(ego.equippedWeapon.nome)}.";
                                            }
                                        }
                                    }
                                    
                                    npcSpeechComplete = false;
                                    StartCoroutine(NPCSpeech(buyConfirm));
                                    yield return new WaitUntil(NPCSpeechComplete);
                                    npcSpeechComplete = false;
                                    genericOptionComplete = false;
                                    option1.color = Color.white;
                                    option2.color = Color.white;
                                    StartCoroutine(GenericOptionSelection("Of course!", "Nah - I'll hang onto it.", null, null));
                                    yield return new WaitUntil(GenericOptionComplete);
                                    genericOptionComplete = false;
                                    if (genericOptionSelected == 0)
                                    {
                                        controller.interactableItems.cursorSelect.Play();
                                        purchase.Play();
                                    }
                                    else { controller.interactableItems.cursorCancel.Play(); }
                                    List<string> contBuy = new List<string>();
                                    yield return new WaitForSeconds(.25f);
                                    if (genericOptionSelected == 0)
                                    {
                                        contBuy.Add($"Ah, it suits you!");
                                        if (unStrap) { controller.GetUnStrapped(); }
                                        if (unEquip) { controller.GetUnEquipped(); }
                                        if (selectedItem is Weapon) { controller.GetEquipped((Weapon)selectedItem); }
                                        else if (selectedItem is Armor) { controller.GetDressed((Armor)selectedItem); }
                                        else if (selectedItem is Shield) { controller.GetStrapped((Shield)selectedItem); }
                                    }
                                    else if (genericOptionSelected == 1 || genericOptionSelected == -1) { contBuy.Add($"I'll just put it in the bag for you."); }
                                    contBuy.Add("Anything else catch your eye?");
                                    npcSpeechComplete = false;
                                    StartCoroutine(NPCSpeech(contBuy));
                                }
                                else if (!yesSelected)
                                {
                                    controller.interactableItems.cursorCancel.Play();
                                    yield return new WaitForSeconds(.25f);
                                    List<string> buyCancel = new List<string>();
                                    buyCancel.Add("Aw, man!");
                                    buyCancel.Add("Anything else catch your eye?");
                                    npcSpeechComplete = false;
                                    StartCoroutine(NPCSpeech(buyCancel));
                                }
                                yield return new WaitUntil(NPCSpeechComplete);
                                npcSpeechComplete = false;
                                yield return new WaitForSeconds(.5f);
                                option1.color = Color.white;
                                option2.color = Color.white;
                                shop.SetActive(true);
                                shopBackground.SetActive(true);
                                option1.text = "Ask about";
                                option2.text = "Give item";
                                option3.text = "Shop";
                                option4.text = "Enough already";
                                StartCoroutine(Buy());
                                break;
                            }
                        }
                        break;
                    }
                    else
                    {
                        error.Play();
                        option1.text = "";
                        option2.text = "";
                        option3.text = "";
                        option4.text = "";
                        shop.SetActive(false);
                        shopBackground.SetActive(false);
                        yield return new WaitForSeconds(.25f);
                        List<string> buyError = new List<string>();
                        buyError.Add($"Shoot I don't think you have enough cash! Maybe next time, all right, {bro}?");
                        buyError.Add("Anything else catch your eye?");
                        npcSpeechComplete = false;
                        StartCoroutine(NPCSpeech(buyError));
                        yield return new WaitUntil(NPCSpeechComplete);
                        npcSpeechComplete = false;
                        yield return new WaitForSeconds(.5f);
                        shop.SetActive(true);
                        shopBackground.SetActive(true);
                        option1.text = "Ask about";
                        option2.text = "Give item";
                        option3.text = "Shop";
                        option4.text = "Enough already";
                        StartCoroutine(Buy());
                        break;
                    }                    
                }
            }
        }
        IEnumerator Sell()
        {
            inventoryClosed = false;
            StartCoroutine(DisplayInventory());
            NPCText.text = "";
            yield return new WaitUntil(InventoryClosed);
            inventoryClosed = false;
            if (selectedItem != null)
            {
                npcSpeechComplete = false;
                yield return new WaitForSeconds(.25f);
                List<string> sellCheck = new List<string>();
                sellCheck.Add($"The {myTI.ToTitleCase(selectedItem.nome)}? Is {Mathf.RoundToInt(selectedItem.price / saleDivider)} crystals cool?");
                StartCoroutine(NPCSpeech(sellCheck));
                currentCrystalsText.text = $"Crystals: {ego.blueCrystals}";
                yield return new WaitUntil(NPCSpeechComplete);
                npcSpeechComplete = false;
                genericOptionComplete = false;
                StartCoroutine(GenericOptionSelection("Let's do it!", "I don't think so.", null, null));
                yield return new WaitUntil(GenericOptionComplete);
                genericOptionComplete = false;
                currentCrystalsText.text = $"";
                if (genericOptionSelected == 0)
                {
                    controller.interactableItems.cursorSelect.Play();
                    purchase.Play();
                    ego.blueCrystals += Mathf.RoundToInt(selectedItem.price / saleDivider);
                    if (ego.equippedWeapon != null) { if (selectedItem == ego.equippedWeapon) { controller.GetUnEquipped(); } }
                    if (ego.equippedArmor != null) { if (selectedItem == ego.equippedArmor) { controller.GetUnDressed(); } }
                    if (ego.equippedShield != null) { if (selectedItem == ego.equippedShield) { controller.GetUnStrapped(); } }
                    controller.interactableItems.inventory.Remove(selectedItem);
                    npcSpeechComplete = false;
                    yield return new WaitForSeconds(.25f);
                    List<string> sellConfirm = new List<string>();
                    sellConfirm.Add($"Hey {man} good doing business with you.");
                    sellConfirm.Add($"Got anything else for me?");
                    StartCoroutine(NPCSpeech(sellConfirm));
                    yield return new WaitUntil(NPCSpeechComplete);
                    npcSpeechComplete = false;
                }
                else if (genericOptionSelected == 1 || genericOptionSelected == -1)
                {
                    controller.interactableItems.cursorCancel.Play();
                    npcSpeechComplete = false;
                    yield return new WaitForSeconds(.25f);
                    List<string> sellCancel = new List<string>();
                    sellCancel.Add($"Aw, all right. Maybe next time.");
                    sellCancel.Add($"Got anything else for me?");
                    StartCoroutine(NPCSpeech(sellCancel));
                    yield return new WaitUntil(NPCSpeechComplete);
                    npcSpeechComplete = false;
                }
                genericOptionComplete = false;
                StartCoroutine(GenericOptionSelection("Yes", "No", null, null));
                yield return new WaitUntil(GenericOptionComplete);
                genericOptionComplete = false;
                if (genericOptionSelected == 0)
                {
                    controller.interactableItems.cursorSelect.Play();
                    sellComplete = false;
                    StartCoroutine(Sell());
                    yield return new WaitUntil(SellComplete);
                    sellComplete = false;
                }
                else if (genericOptionSelected == 1 || genericOptionSelected == -1)
                {
                    controller.interactableItems.cursorCancel.Play();
                    npcSpeechComplete = false;
                    yield return new WaitForSeconds(.25f);
                    List<string> sellExit = new List<string>();
                    sellExit.Add($"Cool, {man} - anything else?");
                    StartCoroutine(NPCSpeech(sellExit));
                    yield return new WaitUntil(NPCSpeechComplete);
                    npcSpeechComplete = false;
                    StartCoroutine(OptionSelect(pete));
                }
            }
            else
            {
                npcSpeechComplete = false;
                yield return new WaitForSeconds(.25f);
                List<string> nullItem = new List<string>();
                nullItem.Add($"Cool, {man} - anything else?");
                StartCoroutine(NPCSpeech(nullItem));
                yield return new WaitUntil(NPCSpeechComplete);
                npcSpeechComplete = false;
                StartCoroutine(OptionSelect(pete));
            }
        }
    }
    IEnumerator TeletypeDescription(TMP_Text textBox, float characterPause = 0.02f)
    {
        int totalVisibleCharacters = textBox.textInfo.characterCount;
        int counter = 0;
        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            textBox.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters) { break; }
            counter += 1;

            yield return new WaitForSeconds(characterPause);
        }
    }
    string WriteShoppingList(List<Item> itemList)
    {
        string writtenList = "";
        for (int i = 0; i < itemList.Count; i++)
        {
            writtenList += $"{myTI.ToTitleCase(itemList[i].nome)}\n";
        }
        return writtenList;
    }
    string WritePriceList(List<Item> itemList)
    {
        string priceList = "";
        for (int i = 0; i < itemList.Count; i++)
        {
            priceList += $"{itemList[i].price}\n";
        }
        return priceList;
    }
    public void ShopStats(Item itemSelected)
    {
        currentTwoHanded.SetActive(false);
        newTwoHanded.SetActive(false);
        currentType.text = "";
        newType.text = "";
        string sign = "+";

        //update new item and equipped stat display
        newItemTitle.text = myTI.ToTitleCase(itemSelected.nome);
        if (itemSelected is Weapon)
        {
            Weapon weaponSelected = (Weapon)itemSelected;
            if (ego.equippedWeapon != null) { equippedItemTitle.text = myTI.ToTitleCase(ego.equippedWeapon.nome); }
            else { equippedItemTitle.text = "Unarmed"; }
            equippedStat1Title.text = "Damage:";
            equippedStat2Title.text = "Critical:";
            equippedStat3Title.text = "To Hit:";
            newStat1Title.text = "Damage:";
            newStat2Title.text = "Critical:";
            newStat3Title.text = "To Hit:";
            if (ego.equippedWeapon != null)
            {
                equippedStat1.text = $"1d{ego.equippedWeapon.damageDie} +{ego.equippedWeapon.damage}";
                equippedStat2.text = $"x{ego.equippedWeapon.critMultiplier}";
                if (ego.equippedWeapon.toHitMod < 0) { sign = "-"; }
                equippedStat3.text = $"{sign}{ego.equippedWeapon.toHitMod}";
                currentType.text = ego.equippedWeapon.type;
                if (ego.equippedWeapon.twoHanded) { currentTwoHanded.SetActive(true); }
            }
            else
            {
                equippedStat1.text = "1d4 +0";
                equippedStat2.text = "x1.5";
                equippedStat3.text = "+0";
            }
            //compare current to new item stats, highlight increase/decrease
            string color = "green";
            string colorTwo = "green";
            if (ego.equippedWeapon != null)
            {
                if (weaponSelected.damageDie < ego.equippedWeapon.damageDie) { color = "red"; }
                else if (weaponSelected.damageDie > ego.equippedWeapon.damageDie) { color = "green"; }
                else { color = "white"; }
                if (weaponSelected.damage < ego.equippedWeapon.damage) { colorTwo = "red"; }
                else if (weaponSelected.damage > ego.equippedWeapon.damage) { colorTwo = "green"; }
                else { colorTwo = "white"; }
            }
            else if (weaponSelected.damage == 0) { colorTwo = "white"; }
            newStat1.text = $"<color={color}>1d{weaponSelected.damageDie}</color> <color={colorTwo}>+{weaponSelected.damage}</color>";

            color = "green";
            if (ego.equippedWeapon != null)
            {
                if (weaponSelected.critMultiplier < ego.equippedWeapon.critMultiplier) { color = "red"; }
                else if (weaponSelected.critMultiplier > ego.equippedWeapon.critMultiplier) { color = "green"; }
                else { color = "white"; }
            }
            else if (weaponSelected.critMultiplier == 1.5) { color = "white"; }
            newStat2.text = $"<color={color}>x{weaponSelected.critMultiplier}</color>";

            color = "green";
            if (weaponSelected.toHitMod < 0) { sign = "-"; }
            if (ego.equippedWeapon != null)
            {
                if (weaponSelected.toHitMod < ego.equippedWeapon.toHitMod) { color = "red"; }
                else if (weaponSelected.toHitMod > ego.equippedWeapon.toHitMod) { color = "green"; }
                else { color = "white"; }
            }
            else if (weaponSelected.toHitMod == 0) { color = "white"; }
            newStat3.text = $"<color={color}>{sign}{weaponSelected.toHitMod}</color>";
            newType.text = weaponSelected.type;
            if (weaponSelected.twoHanded) { newTwoHanded.SetActive(true); }
        }
        else if (itemSelected is Armor)
        {
            Armor armorSelected = (Armor)itemSelected;
            if (ego.equippedArmor != null) { equippedItemTitle.text = myTI.ToTitleCase(ego.equippedArmor.nome); }
            else { equippedItemTitle.text = "None"; }
            equippedStat1Title.text = "Dmg Reduction:";
            equippedStat2Title.text = "Crit Resist:";
            equippedStat3Title.text = "";
            newStat1Title.text = "Dmg Reduction:";
            newStat2Title.text = "Crit Resist:";
            newStat3Title.text = "";
            if (ego.equippedArmor != null)
            {
                if (ego.equippedArmor.damageReduction >= 0) { sign = "-"; }
                equippedStat1.text = $"{sign}{ego.equippedArmor.damageReduction}";
                equippedStat2.text = $"x{ego.equippedArmor.critResist}";
                equippedStat3.text = $"";
            }
            else
            {
                equippedStat1.text = "-0";
                equippedStat2.text = "x1.00";
                equippedStat3.text = "";
            }
            //compare current to new item stats, highlight increase/decrease
            string color = "green";
            if (ego.equippedArmor != null)
            {
                if (armorSelected.damageReduction < ego.equippedArmor.damageReduction) { color = "red"; }
                else if (armorSelected.damageReduction > ego.equippedArmor.damageReduction) { color = "green"; }
                else { color = "white"; }
            }
            else if (armorSelected.damageReduction == 0) { color = "white"; }
            if (armorSelected.damageReduction >= 0) { sign = "-"; }
            newStat1.text = $"<color={color}>{sign}{armorSelected.damageReduction}</color>";

            color = "green";
            if (ego.equippedArmor != null)
            {
                if (armorSelected.critResist > ego.equippedArmor.critResist) { color = "red"; }
                else if (armorSelected.critResist < ego.equippedArmor.critResist) { color = "green"; }
                else { color = "white"; }
            }
            else if (armorSelected.critResist == 1) { color = "white"; }
            newStat2.text = $"<color={color}>x{armorSelected.critResist}</color>";

            newStat3.text = $"";
        }
        else if (itemSelected is Shield)
        {
            Shield shieldSelected = (Shield)itemSelected;
            if (ego.equippedShield != null) { equippedItemTitle.text = myTI.ToTitleCase(ego.equippedShield.nome); }
            else { equippedItemTitle.text = "None"; }
            equippedStat1Title.text = "Armor Class:";
            equippedStat2Title.text = "Crit Resist:";
            equippedStat3Title.text = "";
            newStat1Title.text = "Armor Class:";
            newStat2Title.text = "Crit Resist:";
            newStat3Title.text = "";
            if (ego.equippedShield != null)
            {
                equippedStat1.text = $"x{ego.equippedShield.armorClass}";
                equippedStat2.text = $"x{ego.equippedShield.critResist}";
                equippedStat3.text = $"";
            }
            else
            {
                equippedStat1.text = "0";
                equippedStat2.text = "x1.00";
                equippedStat3.text = "";
            }
            //compare current to new item stats, highlight increase/decrease
            string color = "green";
            if (ego.equippedShield != null)
            {
                if (shieldSelected.armorClass < ego.equippedShield.armorClass) { color = "red"; }
                else if (shieldSelected.armorClass > ego.equippedShield.armorClass) { color = "green"; }
                else { color = "white"; }
            }
            else if (shieldSelected.armorClass == 0) { color = "white"; }
            newStat1.text = $"<color={color}>{shieldSelected.armorClass}</color>";

            color = "green";
            if (ego.equippedShield != null)
            {
                if (shieldSelected.critResist > ego.equippedShield.critResist) { color = "red"; }
                else if (shieldSelected.critResist < ego.equippedShield.critResist) { color = "green"; }
                else { color = "white"; }
            }
            else if (shieldSelected.critResist == 1) { color = "white"; }
            newStat2.text = $"<color={color}>x{shieldSelected.critResist}</color>";

            newStat3.text = $"";
        }

        //update total adjusted stat display
        //damage die & damage
        int egoDamageDie = (int)(ego.allStats[6].value + ego.allStats[6].effectValue);
        int withItemDamageDie = egoDamageDie;
        int egoDamage = (int)(ego.allStats[7].value + ego.allStats[7].effectValue);
        int withItemDamage = egoDamage;
        int egoCurrentWeaponDamage = 0;
        if (ego.equippedWeapon != null) { egoCurrentWeaponDamage = controller.ego.equippedWeapon.damage; }
        if (itemSelected is Weapon)
        {
            Weapon weaponSelected = (Weapon)itemSelected;
            withItemDamage = egoDamage - egoCurrentWeaponDamage + weaponSelected.damage;
            withItemDamageDie = (int)ego.allStats[6].effectValue + weaponSelected.damageDie;
        }
        string dieColor = "white";
        string damageColor = "white";
        string damageSign = "+";
        if (egoDamageDie < withItemDamageDie) { dieColor = "green"; }
        if (egoDamageDie > withItemDamageDie) { dieColor = "red"; }
        if (egoDamage < withItemDamage) { damageColor = "green"; }
        if (egoDamage > withItemDamage) { damageColor = "red"; }
        if (withItemDamage < 0) { damageSign = ""; }
        adjustedDamage.text = $"<color={dieColor}>1d{withItemDamageDie}</color> <color={damageColor}>{damageSign}{withItemDamage}</color>";

        //critMultiplier
        float egoCritMultiplier = ego.allStats[8].value + ego.allStats[8].effectValue;
        float withItemCritMultiplier = egoCritMultiplier;
        if (itemSelected is Weapon)
        {
            Weapon weaponSelected = (Weapon)itemSelected;
            withItemCritMultiplier = ego.allStats[8].effectValue + weaponSelected.critMultiplier;
        }
        string critMultiplierColor = "white";
        if (egoCritMultiplier < withItemCritMultiplier) { critMultiplierColor = "green"; }
        if (egoCritMultiplier > withItemCritMultiplier) { critMultiplierColor = "red"; }

        adjustedCritical.text = $"<color={critMultiplierColor}>x{withItemCritMultiplier}</color>";

        //toHitMod
        int egoToHitMod = (int)(ego.allStats[9].value + ego.allStats[9].effectValue);
        int withItemToHitMod = egoToHitMod;
        int egoCurrentWeaponToHitMod = 0;
        if (ego.equippedWeapon != null) { egoCurrentWeaponToHitMod = controller.ego.equippedWeapon.toHitMod; }
        if (itemSelected is Weapon)
        {
            Weapon weaponSelected = (Weapon)itemSelected;
            withItemToHitMod = egoToHitMod - egoCurrentWeaponToHitMod + weaponSelected.toHitMod;
        }
        string toHitModColor = "white";
        if (egoToHitMod < withItemToHitMod) { toHitModColor = "green"; }
        if (egoToHitMod > withItemToHitMod) { toHitModColor = "red"; }

        if (withItemToHitMod < 0) { adjustedToHit.text = $"<color={toHitModColor}>{withItemToHitMod}</color>"; }
        else { adjustedToHit.text = $"<color={toHitModColor}>+{withItemToHitMod}</color>"; }



        //armorClass
        int egoArmorClass = (int)(ego.allStats[3].value + ego.allStats[3].effectValue);
        int withItemArmorClass = (int)egoArmorClass;
        int egoCurrentShieldArmorClass = 0;
        if (ego.equippedShield != null) { egoCurrentShieldArmorClass = controller.ego.equippedShield.armorClass; }
        if (itemSelected is Shield)
        {
            Shield shieldSelected = (Shield)itemSelected;
            withItemArmorClass = egoArmorClass - egoCurrentShieldArmorClass + shieldSelected.armorClass;
        }
        string armorClassColor = "white";
        if (egoArmorClass < withItemArmorClass) { armorClassColor = "green"; }
        if (egoArmorClass > withItemArmorClass) { armorClassColor = "red"; }

        adjustedArmorClass.text = $"<color={armorClassColor}>{withItemArmorClass}</color>";

        //critResist
        float egoCritResist = ego.allStats[4].value - ego.allStats[4].effectValue;
        float withItemCritResist = egoCritResist;
        float egoCurrentArmorCritResist = 1;
        float egoCurrentShieldCritResist = 1;
        if (ego.equippedArmor != null) { egoCurrentArmorCritResist = controller.ego.equippedArmor.critResist; }
        if (ego.equippedShield != null) { egoCurrentShieldCritResist = controller.ego.equippedShield.critResist; }
        if (itemSelected is Armor)
        {
            Armor armorSelected = (Armor)itemSelected;
            withItemCritResist = armorSelected.critResist - (1 - egoCurrentShieldCritResist) - ego.allStats[4].effectValue;
        }
        else if (itemSelected is Shield)
        {
            Shield shieldSelected = (Shield)itemSelected;
            withItemCritResist = shieldSelected.critResist - (1 - egoCurrentArmorCritResist) - ego.allStats[4].effectValue;
        }
        string critResistColor = "white";
        if (egoCritResist > withItemCritResist) { critResistColor = "green"; }
        if (egoCritResist < withItemCritResist) { critResistColor = "red"; }

        adjustedCritResist.text = $"<color={critResistColor}>x{withItemCritResist}</color>";

        //dmgReduction
        int egoDmgReduction = (int)(ego.allStats[5].value + ego.allStats[5].effectValue);
        int withItemDmgReduction = (int)egoDmgReduction;
        int egoCurrentArmorDmgReduction = 0;
        if (ego.equippedArmor != null) { egoCurrentArmorDmgReduction = controller.ego.equippedArmor.damageReduction; }
        if (itemSelected is Armor)
        {
            Armor armorSelected = (Armor)itemSelected;
            withItemDmgReduction = egoDmgReduction - egoCurrentArmorDmgReduction + armorSelected.damageReduction;
        }
        string dmgReductionColor = "white";
        if (egoDmgReduction < withItemDmgReduction) { dmgReductionColor = "green"; }
        if (egoDmgReduction > withItemDmgReduction) { dmgReductionColor = "red"; }

        if (withItemDmgReduction >= 0) { adjustedDamageReduction.text = $"<color={dmgReductionColor}>-{withItemDmgReduction}</color>"; }
        else { adjustedDamageReduction.text = $"<color={dmgReductionColor}>+{Mathf.Abs(withItemDmgReduction)}</color>"; }
    }
    IEnumerator DisplayInventory()
    {
        string toPassIn;
        string plainEgoWeapon = egoWeaponText.text;
        string plainEgoArmor = egoArmorText.text;
        string plainEgoShield = egoShieldText.text;
        List<Item> alreadyListed = new List<Item>();
        selectedItem = null;

        controller.interactableItems.invDisplay.SetActive(true);
        controller.interactableItems.invDisplayBorder.SetActive(true);
        DisplayPotionBelt();
        DisplayEquippedItems();
        controller.interactableItems.InvStats(null);
        alreadyListed.Clear();
        toPassIn = "";
        if (controller.interactableItems.inventory.Count == 0) { toPassIn += "Your inventory is empty! How sad.\n"; }
        else
        {
            for (int i = 0; i < controller.interactableItems.inventory.Count; i++)
            {
                if (alreadyListed.Contains(controller.interactableItems.inventory[i])) { continue; }
                else
                {
                    int counter = 0;
                    for (int j = i; j < controller.interactableItems.inventory.Count; j++)
                    {
                        if (controller.interactableItems.inventory[i] == controller.interactableItems.inventory[j]) { counter++; }
                    }
                    alreadyListed.Add(controller.interactableItems.inventory[i]);
                    string total = counter.ToString();
                    toPassIn += total + " " + myTI.ToTitleCase(controller.interactableItems.inventory[i].nome) + "\n";
                }
            }
        }
        controller.interactableItems.invText.text = toPassIn;


        if (controller.interactableItems.inventory.Count > 0 || ego.equippedWeapon != null || ego.equippedArmor != null || ego.equippedShield != null || ego.potionBelt.Count > 0)
        {
            bool skipToEquipment = false;
            bool blockEquipment = false;
            if (ego.equippedWeapon == null && ego.equippedArmor == null && ego.equippedShield == null && ego.potionBelt.Count == 0) { blockEquipment = true; }
            string normalInvText = controller.interactableItems.invText.text;
            if (controller.interactableItems.inventory.Count > 0) { selectedItem = alreadyListed[0]; }
            else { selectedItem = null; }
            int selectedElement = 0;
            int memoryElement = 0;
            bool doubleBreak = false;
            while (true)
            {
                DisplayPotionBelt();
                DisplayEquippedItems();
                if (controller.interactableItems.inventory.Count <= 0) { skipToEquipment = true; }
                if (!skipToEquipment)
                {
                    controller.interactableItems.invText.text = normalInvText;
                    if (selectedElement < 0) { selectedElement = alreadyListed.Count - 1; }
                    if (selectedElement > alreadyListed.Count - 1) { selectedElement = 0; }
                    int itemLength = alreadyListed[selectedElement].nome.Length;
                    int invIndex = 0;
                    invIndex = controller.interactableItems.invText.text.IndexOf(myTI.ToTitleCase(alreadyListed[selectedElement].nome));

                    string newText = "";

                    for (int i = 0; i < invIndex; i++) { newText += controller.interactableItems.invText.text[i]; }

                    newText += "<color=yellow>";

                    for (int i = invIndex; i < invIndex + itemLength; i++) { newText += controller.interactableItems.invText.text[i]; }

                    newText += "</color>";

                    for (int i = invIndex + itemLength; i < controller.interactableItems.invText.text.Length; i++) { newText += controller.interactableItems.invText.text[i]; }

                    controller.interactableItems.invText.text = newText;

                    controller.interactableItems.InvStats(alreadyListed[selectedElement]);

                    yield return new WaitUntil(controller.RightUpDownEnterEscPressed);
                }

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
                //Right Side
                else if (Input.GetKeyDown(KeyCode.RightArrow) && blockEquipment) { controller.interactableItems.cursorMove.Play(); }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || skipToEquipment)
                {
                    if (skipToEquipment) { skipToEquipment = false; }
                    else { controller.interactableItems.cursorMove.Play(); }
                    controller.interactableItems.invText.text = normalInvText;
                    memoryElement = selectedElement;
                    int equipmentElement = 0;
                    string plainPotion0 = controller.interactableItems.potion0Text.text;
                    string plainPotion1 = controller.interactableItems.potion1Text.text;
                    string plainPotion2 = controller.interactableItems.potion2Text.text;
                    string plainWeapon = controller.interactableItems.weaponText.text;
                    string plainArmor = controller.interactableItems.armorText.text;
                    string plainShield = controller.interactableItems.shieldText.text;

                    while (true)
                    {
                        if (equipmentElement < 0) { equipmentElement = 5; }
                        if (equipmentElement > 5) { equipmentElement = 0; }

                        controller.interactableItems.potion0Text.text = plainPotion0;
                        controller.interactableItems.potion1Text.text = plainPotion1;
                        controller.interactableItems.potion2Text.text = plainPotion2;
                        controller.interactableItems.weaponText.text = plainWeapon;
                        controller.interactableItems.armorText.text = plainArmor;
                        controller.interactableItems.shieldText.text = plainShield;

                        if (equipmentElement == 0) { controller.interactableItems.weaponText.text = $"<color=yellow>{controller.interactableItems.weaponText.text}</color>"; }
                        else if (equipmentElement == 1) { controller.interactableItems.armorText.text = $"<color=yellow>{controller.interactableItems.armorText.text}</color>"; }
                        else if (equipmentElement == 2) { controller.interactableItems.shieldText.text = $"<color=yellow>{controller.interactableItems.shieldText.text}</color>"; }
                        else if (equipmentElement == 3) { controller.interactableItems.potion0Text.text = $"<color=yellow>{controller.interactableItems.potion0Text.text}</color>"; }
                        else if (equipmentElement == 4) { controller.interactableItems.potion1Text.text = $"<color=yellow>{controller.interactableItems.potion1Text.text}</color>"; }
                        else if (equipmentElement == 5) { controller.interactableItems.potion2Text.text = $"<color=yellow>{controller.interactableItems.potion2Text.text}</color>"; }

                        yield return new WaitUntil(controller.LeftUpDownEnterEscPressed);
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            controller.interactableItems.cursorMove.Play();
                            equipmentElement--;
                            if (equipmentElement == -1) { equipmentElement = 5; }
                            if (ego.potionBelt.Count <= 2 && equipmentElement == 5) { equipmentElement = 2; }
                            if (ego.potionBelt.Count <= 1 && equipmentElement == 4) { equipmentElement = 2; }
                            if (ego.potionBelt.Count == 0 && equipmentElement == 3) { equipmentElement = 2; }
                            if (ego.equippedShield == null && equipmentElement == 2) { equipmentElement--; }
                            if (ego.equippedArmor == null && equipmentElement == 1) { equipmentElement--; }
                            if (ego.equippedWeapon == null && equipmentElement == 0) { equipmentElement--; }
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            controller.interactableItems.cursorMove.Play();
                            equipmentElement++;
                            if (equipmentElement == 6) { equipmentElement = 0; }
                            if (ego.equippedWeapon == null && equipmentElement == 0) { equipmentElement++; }
                            if (ego.equippedArmor == null && equipmentElement == 1) { equipmentElement++; }
                            if (ego.equippedShield == null && equipmentElement == 2) { equipmentElement++; }
                            if (ego.potionBelt.Count == 0 && equipmentElement == 3) { equipmentElement = 0; }
                            if (ego.potionBelt.Count == 1 && equipmentElement == 4) { equipmentElement = 0; }
                            if (ego.potionBelt.Count == 2 && equipmentElement == 5) { equipmentElement = 0; }
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            controller.interactableItems.cursorMove.Play();
                            if (controller.interactableItems.inventory.Count > 0)
                            {
                                controller.interactableItems.potion0Text.text = plainPotion0;
                                controller.interactableItems.potion1Text.text = plainPotion1;
                                controller.interactableItems.potion2Text.text = plainPotion2;
                                controller.interactableItems.weaponText.text = plainWeapon;
                                controller.interactableItems.armorText.text = plainArmor;
                                controller.interactableItems.shieldText.text = plainShield;
                                selectedElement = memoryElement;
                                break;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            selectedItem = null;
                            controller.interactableItems.cursorCancel.Play();
                            controller.interactableItems.invDisplay.SetActive(false);
                            controller.interactableItems.invDisplayBorder.SetActive(false);
                            controller.interactableItems.potion0Text.text = plainPotion0;
                            controller.interactableItems.potion1Text.text = plainPotion1;
                            controller.interactableItems.potion2Text.text = plainPotion2;
                            controller.interactableItems.weaponText.text = plainWeapon;
                            controller.interactableItems.armorText.text = plainArmor;
                            controller.interactableItems.shieldText.text = plainShield;
                            doubleBreak = true;
                            break;
                        }
                        else if (Input.GetKeyDown(KeyCode.Return))
                        {
                            controller.interactableItems.cursorSelect.Play();
                            controller.interactableItems.invDisplay.SetActive(false);
                            controller.interactableItems.invDisplayBorder.SetActive(false);
                            controller.interactableItems.potion0Text.text = plainPotion0;
                            controller.interactableItems.potion1Text.text = plainPotion1;
                            controller.interactableItems.potion2Text.text = plainPotion2;
                            controller.interactableItems.weaponText.text = plainWeapon;
                            controller.interactableItems.armorText.text = plainArmor;
                            controller.interactableItems.shieldText.text = plainShield;
                            if (equipmentElement == 0) { selectedItem = ego.equippedWeapon; }
                            else if (equipmentElement == 1) { selectedItem = ego.equippedArmor; }
                            else if (equipmentElement == 2) { selectedItem = ego.equippedShield; }
                            else { selectedItem = ego.potionBelt[equipmentElement]; }
                            doubleBreak = true;
                            break;
                        }
                        if (doubleBreak)
                        {
                            doubleBreak = false;
                            break;
                        }
                    }
                }
                //End Right Side
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    selectedItem = null;
                    controller.interactableItems.cursorCancel.Play();
                    controller.interactableItems.invDisplay.SetActive(false);
                    controller.interactableItems.invDisplayBorder.SetActive(false);
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    controller.interactableItems.cursorSelect.Play();
                    controller.interactableItems.invDisplay.SetActive(false);
                    controller.interactableItems.invDisplayBorder.SetActive(false);
                    selectedItem = alreadyListed[selectedElement];
                    break;
                }
                if (doubleBreak)
                {
                    doubleBreak = false;
                    break;
                }
            }
        }
        else
        {
            yield return new WaitUntil(controller.EscPressed);
            controller.interactableItems.cursorCancel.Play();
            controller.interactableItems.invDisplay.SetActive(false);
            controller.interactableItems.invDisplayBorder.SetActive(false);
        }
        inventoryClosed = true;


        void DisplayPotionBelt()
        {
            controller.interactableItems.potion0Text.text = "";
            controller.interactableItems.potion1Text.text = "";
            controller.interactableItems.potion2Text.text = "";
            if (ego.potionBelt.Count > 0) { controller.interactableItems.potion0Text.text = controller.interactableItems.myTI.ToTitleCase(ego.potionBelt[0].nome); }
            if (ego.potionBelt.Count > 1) { controller.interactableItems.potion1Text.text = controller.interactableItems.myTI.ToTitleCase(ego.potionBelt[1].nome); }
            if (ego.potionBelt.Count > 2) { controller.interactableItems.potion2Text.text = controller.interactableItems.myTI.ToTitleCase(ego.potionBelt[2].nome); }
        }
        void DisplayEquippedItems()
        {
            if (ego.equippedWeapon != null) { egoWeaponText.text = myTI.ToTitleCase(ego.equippedWeapon.nome); }
            else { egoWeaponText.text = "None"; }
            if (ego.equippedArmor != null) { egoArmorText.text = myTI.ToTitleCase(ego.equippedArmor.nome); }
            else { egoArmorText.text = "None"; }
            if (ego.equippedShield != null) { egoShieldText.text = myTI.ToTitleCase(ego.equippedShield.nome); }
            else { egoShieldText.text = "None"; }
            plainEgoWeapon = egoWeaponText.text;
            plainEgoArmor = egoArmorText.text;
            plainEgoShield = egoShieldText.text;
        }
    }
    IEnumerator ActivateAskAbout(List<DialogueOption> replyList, NPC speaker, bool internalTree = false)
    {
        WriteDialogueReplies(replyList);
        int selectedElement = 0;
        int lastElement = 0;

        if (!internalTree) { escToReturnReply.text = "Enough Already"; }
        else { escToReturnReply.text = "Something Else"; }
        string plainReply0 = reply0.text;
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
        string plainEscToReturn = escToReturnReply.text;

        replyBoxFade.SetActive(true);
        replyBox.SetActive(true);
        replyBoxBackground.SetActive(true);
        if (!internalTree) { yield return new WaitForSeconds(.6f); }
        while (true)
        {
            if (askAboutMemoryElement != -1)
            {
                selectedElement = askAboutMemoryElement;
                askAboutMemoryElement = -1;
            }
            int availableReplies = 0;
            int escToReturnIndex = 0;
            for (int i = 0; i < replyList.Count; i++)
            {
                if (replyList[i].availableToSay) { availableReplies++; }
            }
            escToReturnIndex = availableReplies;

            // -1 for proper index, +1 for escToReturn
            if (selectedElement < 0) { selectedElement = availableReplies; }
            if (selectedElement > availableReplies) { selectedElement = 0; }

            reply0.text = plainReply0;
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
            escToReturnReply.text = plainEscToReturn;
            if (lastElement == selectedElement) { yield return new WaitForSeconds(.05f); }
            lastElement = selectedElement;

            //grey out done asks
            for (int i = 0; i < replyList.Count; i++)
            {
                if (replyList[i].hasBeenSaid) { replyRay[i].color = Color.gray; }
                else { replyRay[i].color = Color.white; }
            }
            //highlight selection
            if (selectedElement == escToReturnIndex) { escToReturnReply.text = $"<color=yellow>{escToReturnReply.text}</color>"; }
            else
            {
                for (int i = 0; i < replyList.Count; i++)
                {
                    if (selectedElement == i)
                    {
                        replyRay[i].text = $"<color=yellow>{replyRay[i].text}</color>";
                        replyHighRay[i].SetActive(true);
                    }
                }
            }

            yield return new WaitUntil(controller.UpDownEnterEscPressed);
            for (int i = 0; i < replyHighRay.Length; i++) { replyHighRay[i].SetActive(false); }

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
                reply0.text = plainReply0;
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
                escToReturnReply.text = plainEscToReturn;
                if (replyList[0].parentReplies.Count == 0)
                {
                    replyBoxFade.SetActive(false);
                    replyBox.SetActive(false);
                    replyBoxBackground.SetActive(false);
                    StartCoroutine(OptionSelect(speaker));
                }
                else
                {
                    bool treeInsert = true;
                    if (replyList[0].parentReplies[0].parentReplies.Count == 0) { treeInsert = false; }
                    StartCoroutine(ActivateAskAbout(replyList[0].parentReplies, speaker, treeInsert));
                }
                break;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (selectedElement == escToReturnIndex)
                {
                    controller.interactableItems.cursorCancel.Play();
                    reply0.text = plainReply0;
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
                    escToReturnReply.text = plainEscToReturn;
                    if (replyList[0].parentReplies.Count == 0)
                    {
                        replyBoxFade.SetActive(false);
                        replyBox.SetActive(false);
                        replyBoxBackground.SetActive(false);
                        StartCoroutine(OptionSelect(speaker));
                    }
                    else
                    {
                        bool treeInsert = true;
                        if (replyList[0].parentReplies[0].parentReplies.Count == 0) { treeInsert = false; }
                        StartCoroutine(ActivateAskAbout(replyList[0].parentReplies, speaker, treeInsert));
                    }
                }
                else
                {
                    askAboutMemoryElement = selectedElement;
                    replyList[selectedElement].hasBeenSaid = true;
                    controller.interactableItems.cursorSelect.Play();
                    replyBoxFade.SetActive(false);
                    replyBox.SetActive(false);
                    replyBoxBackground.SetActive(false);
                    yield return new WaitForSeconds(.25f);
                    npcSpeechComplete = false;
                    StartCoroutine(NPCSpeech(replyList[selectedElement].response));
                    yield return new WaitUntil(NPCSpeechComplete);
                    npcSpeechComplete = false;
                    NPCText.text = "";
                    if (replyList[selectedElement].additionalReplies.Count == 0)
                    {
                        bool treeInsert = true;
                        if (replyList[selectedElement].parentReplies.Count == 0) { treeInsert = false; }
                        StartCoroutine(ActivateAskAbout(replyList, speaker, treeInsert));
                    }
                    else
                    {
                        askAboutMemoryElement = -1;
                        StartCoroutine(ActivateAskAbout(replyList[selectedElement].additionalReplies, speaker, true));
                    }
                }                
                break;
            }
        }
    }
    public IEnumerator NPCSpeech(List<string> responseList, float endPause = .5f)
    {
        sentences.Clear();
        foreach (string sentence in responseList) { sentences.Enqueue(sentence); }
        while (sentences.Count > 0)
        {
            NPCText.maxVisibleCharacters = 0;
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
            yield return new WaitForSeconds(.25f);
        }
        npcSpeechComplete = true;
    }
    IEnumerator TeletypeMessage(int startingCharacter, float characterPause = 0.02f)
    {
        int totalVisibleCharacters = NPCText.textInfo.characterCount;
        int counter = startingCharacter;
        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            NPCText.maxVisibleCharacters = visibleCount;
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Escape))
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
    IEnumerator FadeAudio(AudioSource audio, float fadeTime)
    {
        float startVolume = audio.volume;
        while (audio.volume > 0)
        {
            audio.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audio.Stop();
        audio.volume = startVolume;
    }
    void ReturnToGame()
    {
        dialogueBox.SetActive(false);
        dialogueBoxBackground.SetActive(false);
        controller.UnlockUserInput();
    }
    public bool NPCSpeechComplete() { return npcSpeechComplete; }
    bool MessageComplete() { return messageComplete; }
    bool InventoryClosed() { return inventoryClosed; }
    bool BuyComplete() { return buyComplete; }
    bool SellComplete() { return sellComplete; }
    bool RestComplete() { return restComplete; }
    public bool GenericOptionComplete() { return genericOptionComplete; }




    // Update is called once per frame
    void Update()
    {
        
    }
}
