using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//cd c:\Users\Andrew\Desktop\Unity Projects\JDMW
//git add .
//git commit -m "commit message"
//git push


public class GameController : MonoBehaviour
{
    [HideInInspector] public bool enterToContinue;
    [HideInInspector] public bool fakeEnterToContinue;
    [HideInInspector] public bool escToContinue;
    [HideInInspector] public bool toResumeEscToContinue;
    [HideInInspector] public bool exitPopUp;
    [HideInInspector] public float timeDelay;
    [HideInInspector] public bool dropIt;
    [HideInInspector] public bool dropItNot;
    [HideInInspector] public Item droppingItem;
    [HideInInspector] public bool pauseIt;
    [HideInInspector] public bool pauseItForUndroppable;
    [HideInInspector] public bool pauseItForDrop;
    [HideInInspector] public bool hasBubbleLead;
    [HideInInspector] public bool secondQuestActive;
    [HideInInspector] public float scrollRectValue;
    public bool debugMode;
    public TMP_Text displayText;
    public TMP_Text actionLogTextTMP;
    //public Text actionLogText;
    public Text popUpText;
    public TMP_Text popUpTitle;
    public TMP_Text popUpEsc;
    public TMP_Text popUpItemDescription;
    public TMP_Text popUpItemStatOne;
    public TMP_Text popUpItemStatTwo;
    public TMP_Text popUpMessage;
    public TMP_Text popUpYes;
    public TMP_Text popUpNo;
    public TMP_Text equippedWeapon;
    public TMP_Text equippedArmor;
    public TMP_Text equippedShield;
    public TMP_Text currentHP;
    public TMP_Text blueCrystals;
    public TMP_Text invDisplayDamage;
    public TMP_Text invDisplayCritical;
    public TMP_Text invDisplayToHit;
    public TMP_Text invDisplayArmorClass;
    public TMP_Text invDisplayCritResist;
    public TMP_Text invDisplayDmgReduction;
    public GameObject inputBox, displayBox;
    public GameObject popUpBox;
    public Scrollbar scrollBar;
    public Button scrollUpArrow, scrollDownArrow, scrollNonArrow;
    public GameObject scrollArrows;
    public Ego ego;
    public InputAction[] inputActions;
    [HideInInspector] public InputAction inputAction;
    [HideInInspector] public InteractableObject interactableObject;
    [HideInInspector] public RoomNavigation roomNavigation;
    [HideInInspector] public Room room;
    [HideInInspector] public TextInput textInput;
    [HideInInspector] public InteractableItems interactableItems;
    [HideInInspector] public RegisterObjects registerObjects;
    [HideInInspector] public RegisterRooms registerRooms;
    [HideInInspector] public MiniMap map;
    [HideInInspector] public DebugMode debugClass;
    [HideInInspector] public AdditionalNarrations additionalNarrations;
    [HideInInspector] public SecondQuest secondQuest;
    [HideInInspector] public NPCTalk npcTalk;
    [HideInInspector] public Achievements achievements;
    [HideInInspector] public TeleType teleType;
    [HideInInspector] public string currentActiveInput;
    [HideInInspector] public string userInput;
    Color orange = new Color(1.0f, 0.64f, 0.0f);
    bool colorItRed = false;
    bool colorItOrange = false;
    bool colorItYellow = false;
    bool fireTheColor = false;
    string lastAction;
    [HideInInspector] public int lastActionHero = 1;
    //bool enterToContinueDialogue;
    bool earlyConvoBail = false;
    [HideInInspector] public bool stupidEnterGlitch = true;

    public GameObject conversation1, conversation2, conversation3, conversation4, conversation5, conversation6, conversation7, jokeConversation1, jokeConversation2, jokeConversation3, jokeConversation4;
    public GameObject conversation1Repeat, conversation2Repeat, conversation3Repeat, conversation4Repeat, conversation5Repeat, conversation6Repeat, conversation7Repeat;

    Queue<int> pauses;
    Queue<string> sentences;
    List<string> actionLog = new List<string>();


    // Start is called before the first frame update
    void Awake()
    {
        roomNavigation = GetComponent<RoomNavigation>();
        textInput = GetComponent<TextInput>();
        interactableItems = GetComponent<InteractableItems>();
        registerObjects = GetComponent<RegisterObjects>();
        registerRooms = GetComponent<RegisterRooms>();
        map = GetComponent<MiniMap>();
        debugClass = GetComponent<DebugMode>();
        additionalNarrations = GetComponent<AdditionalNarrations>();
        npcTalk = GetComponent<NPCTalk>();
        achievements = GetComponent<Achievements>();
        teleType = GetComponent<TeleType>();
        GameObject.Find("ScrollRect").GetComponent<ScrollRect>().verticalNormalizedPosition = 0.5f;
        enterToContinue = false;
        escToContinue = false;
        toResumeEscToContinue = false;
        exitPopUp = false;
        debugMode = false;
        secondQuestActive = false;
        currentActiveInput = "main";
        ego.equippedWeapon = null;
        ego.equippedArmor = null;
        ego.equippedShield = null;
        ego.chosenAction = "";
        ego.allStats[0].value = 100;
        ego.allStats[1].value = 100;
        ego.allStats[2].value = 100;
        ego.allStats[3].value = 0;
        ego.allStats[4].value = 1;
        ego.allStats[5].value = 0;
        ego.allStats[6].value = 4;
        ego.allStats[7].value = 0;
        ego.allStats[8].value = 1.5f;
        ego.allStats[9].value = 0;
        ego.allStats[10].value = 0;
        for (int i = 0; i < ego.allStats.Length; i++) { ego.allStats[i].effectValue = 0; }        
        ego.activeEffects.Clear();
        ego.defeatedBadGuys.Clear();
        ego.blueCrystals = 0;
        ego.bankedCrystals = 0;
        ego.fightClubRank = 0;
        ego.fleeLocation = "";
        ego.conversation = 0;
        ego.currentInit = 0;
        ego.displayAction = "";
        ego.chosenAction = "";
        ego.chosenItem = null;
        ego.chosenItem2 = null;
        ego.chosenTarget = null;
        ego.potionBelt.Clear();
    //enterToContinueDialogue = false;
    timeDelay = 1;
        sentences = new Queue<string>();
        pauses = new Queue<int>();
        //ego = Instantiate(ego);
    }

    // Stat elements in character arrays
    //
    // CurrentHP = allStats[0]
    // TargetHP = allStats[1]
    // MaxHP = allStats[2]
    // ArmorClass = allStats[3]
    // CritResist = allStats[4]
    // DamageReduction = allStats[5]
    // DamageDie = allStats[6]
    // Damage = allStats[7]
    // CritMultiplier = allStats[8]
    // ToHitMod = allStats[9]
    // InitMod = allStats[10]

    private void Start()
    {
        DisplayRoomText();
    }

    public void DisplayRoomText()
    {
        ClearRoom();
        //Initial Town Entrance Script
        if (roomNavigation.currentRoom.roomName == "F7" && !registerRooms.allRooms[32].visited) { additionalNarrations.InitiateFirstTownVisit(); }
        else
        {
            UnpackRoom();
            string roomText = "";
            switch (roomNavigation.currentRoom.currentDescription)
            {
                case 1:
                    roomText = roomNavigation.currentRoom.description1;
                    break;
                case 2:
                    roomText = roomNavigation.currentRoom.description2;
                    break;
                case 3:
                    roomText = roomNavigation.currentRoom.description3;
                    break;
            }
            displayText.text = roomText;
            ForceTextWindowUp();
        }        
    }

    public void AddToActionLog(string latestAction)
    {
        if (actionLog.Count >= 10) { actionLog.RemoveAt(0); }
        actionLog.Add(latestAction);
        //actionLogText.text = string.Join("\n\n\n", actionLog.ToArray());
        actionLogTextTMP.text = string.Join("\n", actionLog.ToArray());
    }
    public void ForceTextWindowUp()
    {
        Canvas.ForceUpdateCanvases();
        GameObject.Find("ScrollRect").GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        Canvas.ForceUpdateCanvases();
    }
    public void ForceTextWindowDown()
    {
        Canvas.ForceUpdateCanvases();
        GameObject.Find("ScrollRect").GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }
    IEnumerator Narrator(string response)
    {
        inputBox.SetActive(false);
        displayText.text += "\n\n\n-------------------------------------\n\n" + response + "\n\n\nPress ENTER to continue.";
        ForceTextWindowDown();
        yield return new WaitForSeconds(.25f);
        yield return new WaitUntil(EnterPressed);
        inputBox.SetActive(true);
        textInput.inputField.ActivateInputField();
        textInput.inputField.text = null;
        DisplayRoomText();
    }
    public void DisplayNarratorResponse(string response)
    {
        escToContinue = false;
        enterToContinue = true;
        inputBox.SetActive(false);
        displayText.text += "\n\n\n-------------------------------------\n\n" + response + "\n\n\nPress ENTER to continue.";
        ForceTextWindowDown();
    }
    public void InitiateMultiLineResponse(string[] responseLines, float[] responsePauses)
    {
        inputBox.SetActive(false);
        sentences.Clear();
        pauses.Clear();
        foreach (string sentence in responseLines) { sentences.Enqueue(sentence); }
        foreach (int pause in responsePauses) { pauses.Enqueue(pause); }

        StartCoroutine(DisplayMultiLineResponse());
    }
    public void InitiateMultiLineDialogue(string[] responseLines, float[] responsePauses)
    {
        inputBox.SetActive(false);
        sentences.Clear();
        pauses.Clear();
        foreach (string sentence in responseLines) { sentences.Enqueue(sentence); }
        foreach (int pause in responsePauses) { pauses.Enqueue(pause); }

        StartCoroutine(DisplayMultiLineDialogue());
    }
    public void InitiateInputActionResponse(string[] responseLines)
    {
        inputBox.SetActive(false);
        sentences.Clear();
        pauses.Clear();
        foreach (string sentence in responseLines)
        {
            sentences.Enqueue(sentence);
            pauses.Enqueue(0);
        }
        StartCoroutine(DisplayMultiLineResponse());
    }
    public void InitiateScriptedResponse(string[] responseLines, string scriptName)
    {
        inputBox.SetActive(false);
        sentences.Clear();
        pauses.Clear();
        foreach (string sentence in responseLines) { sentences.Enqueue(sentence); }

        if (scriptName == "tray")
        {
            pauses.Enqueue(1);
            pauses.Enqueue(0);
            pauses.Enqueue(2);
            pauses.Enqueue(0);
            pauses.Enqueue(3);
        }
        StartCoroutine(ScriptedResponseDinnerTray());
    }
    IEnumerator DisplayMultiLineResponse()
    {
        bool stupidEnterGlitch = true;
        while (sentences.Count >= 0)
        {
            if (sentences.Count == 0)
            {
                if (currentActiveInput == "main") { DisplayRoomText(); }
                else if (currentActiveInput == "inventory")
                {
                    escToContinue = true;
                    interactableItems.DisplayInventory();
                }
                inputBox.SetActive(true);
                textInput.inputField.ActivateInputField();
                textInput.inputField.text = null;
                break;
            }
            int pause = pauses.Dequeue();
            string sentence = sentences.Dequeue();
            AddToMainWindowWithLine(sentence);
            
            if (pause == 0)
            {
                AddToMainWindow("\n\n\nPress ENTER to continue.");
                while (stupidEnterGlitch)
                {
                    yield return new WaitForSeconds(.25f);
                    break;
                }
                
                yield return new WaitUntil(EnterPressed);
            }
            else { yield return new WaitForSeconds(pause); }
        }
    }
    IEnumerator DisplayMultiLineDialogue()
    {
        bool stupidEnterGlitch = true;
        while (sentences.Count >= 0)
        {
            if (sentences.Count == 0)
            {
                inputBox.SetActive(true);
                textInput.inputField.ActivateInputField();
                textInput.inputField.text = null;

                if (currentActiveInput == "main") { DisplayRoomText(); }
                else if (currentActiveInput == "inventory")
                {
                    escToContinue = true;
                    interactableItems.DisplayInventory();
                }
                break;
            }
            int pause = pauses.Dequeue();
            string sentence = sentences.Dequeue();
            OverwriteMainWindow(sentence);

            if (pause == 0)
            {
                AddToMainWindow("\n\n\nPress ENTER to continue.");
                while (stupidEnterGlitch)
                {
                    yield return new WaitForSeconds(.25f);
                    break;
                }

                yield return new WaitUntil(EnterPressed);
            }
            else { yield return new WaitForSeconds(pause); }
        }
    }
    IEnumerator ScriptedResponseDinnerTray()
    {
        bool choseNo = false;
        while (sentences.Count >= 0)
        {
            if (sentences.Count == 0)
            {
                DisplayRoomText();
                inputBox.SetActive(true);
                textInput.inputField.ActivateInputField();
                textInput.inputField.text = null;
                if (!choseNo) { StartCoroutine(achievements.DisplayDeedPopUp(achievements.allDeeds[0])); }
                break;
            }
            int pause = pauses.Dequeue();
            string sentence = sentences.Dequeue();
            //use pause number to differentiate which code block to execute            
            if (pause == 0) //regular line
            {
                AddToMainWindowWithLine(sentence + "\n\n\nPress ENTER to continue.");
                while (stupidEnterGlitch)
                {
                    yield return new WaitForSeconds(.25f);
                    break;
                }
                yield return new WaitUntil(EnterPressed);
            }
            else if (pause == 1) //popup box
            {
                AddToMainWindowWithLine(sentence);
                while (true)
                {
                    yield return new WaitForSeconds(4f);
                    AddToMainWindow("\n\nWait.");
                    break;
                }
                while (true)
                {
                    yield return new WaitForSeconds(1.5f);
                    userInput = null;
                    inputBox.SetActive(true);
                    textInput.inputField.ActivateInputField();
                    textInput.inputField.text = null;
                    OpenPopUpWindow("", "", "You don't want to, like, try and use these... do you?", "", "[Yes]", "", "[No]", "");
                    break;
                }
                while (true)
                {
                    currentActiveInput = "yesno";
                    userInput = null;
                    yield return new WaitUntil(InputGiven);
                    inputBox.SetActive(false);
                    if (userInput == "yes" || userInput == "no")
                    {
                        currentActiveInput = "main";
                        ClosePopUpWindow();
                        if (userInput == "yes")
                        {
                            userInput = "";
                            AddToMainWindowWithLine("Oooookay");
                            while (true)
                            {
                                yield return new WaitForSeconds(.75f);
                                break;
                            }
                            int counter = 0;
                            while (counter < 3)
                            {
                                AddToMainWindow(".");
                                counter++;
                                yield return new WaitForSeconds(.5f);
                            }
                        }
                        else
                        {
                            choseNo = true;
                            userInput = "";
                            registerObjects.allObjects[0].searched = false;
                            AddToMainWindowWithLine("Well good. That's very sensible of you.\n\n\nPress ENTER to continue.");
                            while (stupidEnterGlitch)
                            {
                                yield return new WaitForSeconds(.25f);
                                break;
                            }
                            yield return new WaitUntil(EnterPressed);
                            sentences.Clear();
                            pauses.Clear();
                            break;
                        }
                        break;
                    }
                    else
                    {
                        AddToMainWindowWithLine("Yes no maybe so?");
                        inputBox.SetActive(true);
                        textInput.inputField.ActivateInputField();
                        textInput.inputField.text = null;
                    }
                }                
            }
            else if (pause == 2)//equip spoon
            {
                GetEquipped((Weapon)registerObjects.allItems[1]);
                AddToMainWindowWithLine(sentence + "\n\n\nPress ENTER to continue.");
                while (stupidEnterGlitch)
                {
                    yield return new WaitForSeconds(.25f);
                    break;
                }
                yield return new WaitUntil(EnterPressed);
            }
            else if (pause == 3)//equip bowl
            {                
                GetStrapped((Shield)registerObjects.allItems[2]);
                AddToMainWindowWithLine(sentence + "\n\n\nPress ENTER to continue.");
                while (stupidEnterGlitch)
                {
                    yield return new WaitForSeconds(.25f);
                    break;
                }
                yield return new WaitUntil(EnterPressed);
            }
            else { yield return new WaitForSeconds(pause); }
        }
    }

    public void LockInputForEnter()
    {
        escToContinue = false;
        enterToContinue = true;
        inputBox.SetActive(false);
    }
    public void OverwriteMainWindow(string toDisplay)
    {
        displayText.text = toDisplay;
        ForceTextWindowUp();
    }
    public void AddToMainWindow(string toDisplay)
    {
        displayText.text += toDisplay;
        ForceTextWindowDown();
    }
    public void AddToMainWindowWithLine(string toDisplay)
    {
        displayText.text += "\n\n\n-------------------------------------\n\n" + toDisplay;
        ForceTextWindowDown();
    }
    public void WriteItemToPopUpWindow(Item item)
    {
        popUpMessage.text = "";
        popUpYes.text = "";
        popUpNo.text = "";
        popUpTitle.text = item.nome;
        popUpItemDescription.text = item.description;
        if (item is Weapon) { WriteWeaponStats((Weapon)item); }    
        if (item is Armor) { WriteArmorStats((Armor)item); }
        if (item is Shield) { WriteShieldStats((Shield)item); }
        popUpEsc.text = "Press ESC to return";

        void WriteWeaponStats(Weapon weapon)
        {
            popUpNo.text = $"Type: {weapon.type}";
            popUpItemStatOne.text = "Damage: 1d" + weapon.damageDie + " +" + weapon.damage;
            popUpItemStatTwo.text = "Crit Multiplier: x" + weapon.critMultiplier;
        }
        void WriteArmorStats(Armor armor)
        {
            popUpItemStatOne.text = "Damage Reduction: -" + armor.damageReduction.ToString();
            popUpItemStatTwo.text = "Crit Resist: x" + armor.critResist.ToString();
        }
        void WriteShieldStats(Shield shield)
        {
            popUpItemStatOne.text = "Armor Class: +" + shield.armorClass.ToString();
            popUpItemStatTwo.text = "Crit Resist: x" + shield.critResist.ToString();
        }
    }
    public void OpenPopUpWindow(string title, string description, string message, string opt1, string yes, string opt2, string no, string closer)
    {
        popUpBox.SetActive(true);
        popUpTitle.text = title;
        popUpItemDescription.text = description;
        popUpMessage.text = message;
        popUpItemStatOne.text = opt1;
        popUpYes.text = yes;
        popUpItemStatTwo.text = opt2;
        popUpNo.text = no;
        popUpEsc.text = closer;
    }
    public void ClosePopUpWindow()
    {
        popUpBox.SetActive(false);
        popUpTitle.text = "";
        popUpItemDescription.text = "";
        popUpMessage.text = "";
        popUpItemStatOne.text = "";
        popUpYes.text = "";
        popUpItemStatTwo.text = "";
        popUpNo.text = "";
        popUpEsc.text = "";
    }

    public void InspectItem(string itemName)
    {
        Item itemToInspect = null;
        for (int i = 0; i < interactableItems.inventory.Count; i++)
        {
            if (itemName == interactableItems.inventory[i].nome)
            {
                itemToInspect = interactableItems.inventory[i];
                break;
            }
        }
        if (itemToInspect == null && ego.equippedWeapon != null && ego.equippedWeapon.nome == itemName) { itemToInspect = ego.equippedWeapon; }
        if (itemToInspect == null && ego.equippedArmor != null && ego.equippedArmor.nome == itemName) { itemToInspect = ego.equippedArmor; }
        if (itemToInspect == null && ego.equippedShield != null && ego.equippedShield.nome == itemName) { itemToInspect = ego.equippedShield; }
        if (itemToInspect == null) { DisplayNarratorResponse("On closer inspection, you see it's not there."); }
        else
        {
            WriteItemToPopUpWindow(itemToInspect);
            toResumeEscToContinue = true;
            exitPopUp = true;
            inputBox.SetActive(false);
            popUpBox.SetActive(true);
        }
    }
    public void GetEquipped(Weapon newWeapon)
    {
        if (ego.equippedWeapon != null) { interactableItems.inventory.Add(ego.equippedWeapon); }
        interactableItems.inventory.Remove(newWeapon);
        ego.equippedWeapon = newWeapon;
        ego.allStats[6].value = ego.equippedWeapon.damageDie;
        ego.allStats[7].value = ego.equippedWeapon.damage;
        ego.allStats[8].value = ego.equippedWeapon.critMultiplier;
        ego.allStats[9].value = ego.equippedWeapon.toHitMod;
    }
    public void GetDressed(Armor newArmor)
    {
        if (ego.equippedArmor != null) { interactableItems.inventory.Add(ego.equippedArmor); }
        interactableItems.inventory.Remove(newArmor);
        ego.equippedArmor = newArmor;
        ego.allStats[5].value = ego.equippedArmor.damageReduction;
        ego.allStats[4].value -= (1 - ego.equippedArmor.critResist);
    }
    public void GetStrapped(Shield newShield)
    {
        if (ego.equippedShield != null) { interactableItems.inventory.Add(ego.equippedShield); }
        interactableItems.inventory.Remove(newShield);
        ego.equippedShield = newShield;
        ego.allStats[3].value = ego.equippedShield.armorClass;
        ego.allStats[4].value -= (1 - ego.equippedShield.critResist);
    }
    public void GetUnEquipped()
    {
        interactableItems.inventory.Add(ego.equippedWeapon);
        ego.allStats[6].value = 4;
        ego.allStats[7].value = 0;
        ego.allStats[8].value = 1.5f;
        ego.equippedWeapon = null;
    }
    public void GetUnDressed()
    {
        interactableItems.inventory.Add(ego.equippedArmor);
        ego.allStats[5].value = 0;
        ego.allStats[4].value += (1 - ego.equippedArmor.critResist);
        ego.equippedArmor = null;
    }
    public void GetUnStrapped()
    {
        interactableItems.inventory.Add(ego.equippedShield);
        ego.allStats[3].value = 0;
        ego.allStats[4].value += (1 - ego.equippedShield.critResist);
        ego.equippedShield = null;
    }
    public void GetStripped()
    {
        GetUnEquipped();
        GetUnDressed();
        GetUnStrapped();
    }

    public Item ExtractItem(string itemName)
    {
        if (itemName == "bubble lead") { return registerObjects.allItems[0]; }
        //if (ego.equippedWeapon == registerObjects.bubbleLead) { ego.equippedWeapon = null; }
        //if (!interactableItems.inventory.Contains(registerObjects.bubbleLead)) { interactableItems.inventory.Add(registerObjects.bubbleLead); }
        for (int i = 0; i < interactableItems.inventory.Count; i++)
        {
            if (itemName == interactableItems.inventory[i].nome) { return interactableItems.inventory[i]; }            
        }
        if (ego.equippedWeapon != null)
        {
            if (ego.equippedWeapon.nome == itemName) { return ego.equippedWeapon; }
        }
        if (ego.equippedArmor != null)
        {
            if (ego.equippedArmor.nome == itemName) { return ego.equippedArmor; }
        }
        if (ego.equippedShield != null)
        {
            if (ego.equippedShield.nome == itemName) { return ego.equippedShield; }
        }
        return null;
    }
    public void InitiateDropItem(Item itemToDrop) { StartCoroutine(DropItem(itemToDrop)); }
    IEnumerator DropItem(Item itemToDrop)
    {
        currentActiveInput = "yesno";
        OpenPopUpWindow("Drop " + itemToDrop.nome + "?", "", "This action cannot be undone.", "", "[Yes]. I'm not afraid.", "", "[No]! Take me back!", "");
        while (true)
        {
            userInput = null;
            while (stupidEnterGlitch)
            {
                yield return new WaitForSeconds(.25f);
                break;
            }
            yield return new WaitUntil(InputGiven);
            if (userInput == "yes" || userInput == "no")
            {
                currentActiveInput = "inventory";
                popUpText.text = null;
                popUpBox.SetActive(false);
                if (userInput == "yes")
                {               
                    if (!interactableItems.inventory.Contains(itemToDrop))
                    {
                        if (ego.equippedArmor == itemToDrop) { ego.equippedArmor = null; }
                        else if (ego.equippedWeapon == itemToDrop) { ego.equippedWeapon = null; }
                        else if (ego.equippedShield == itemToDrop) { ego.equippedShield = null; }
                    }
                    else { interactableItems.inventory.Remove(itemToDrop); }
                    DisplayNarratorResponse("You drop the " + itemToDrop.nome + ".");
                }
                else if (userInput == "no") { DisplayNarratorResponse("Discretion is the better part of valor. Packrat."); }
                userInput = "";
                break;
            }
            else { AddToMainWindow("\n\nWe don't need a filibuster."); }
        }
    }

    void PrepareInteractablesInRoom(Room currentRoom)
    {
        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Length; i++)
        {
            interactableItems.UnpackInteractables(currentRoom, i);
            InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i];
            for (int j = 0; j < interactableInRoom.interactions.Length; j++)
            {
                Interaction interaction = interactableInRoom.interactions[j];
                if (interaction.inputAction.keyWord == "look")
                {   
                    if (secondQuestActive && interaction.secondQuestTextResponseAlternate != null && interactableInRoom.searched) { interactableItems.lookAtDictionary.Add(interactableInRoom.noun, interaction.secondQuestTextResponseAlternate); }
                    else if (secondQuestActive && interaction.secondQuestTextResponse != null) { interactableItems.lookAtDictionary.Add(interactableInRoom.noun, interaction.secondQuestTextResponse); }
                    else if (interaction.textResponseAlternate != null && interactableInRoom.searched) { interactableItems.lookAtDictionary.Add(interactableInRoom.noun, interaction.textResponseAlternate); }
                    else { interactableItems.lookAtDictionary.Add(interactableInRoom.noun, interaction.textResponse); }
                }
                if (interaction.inputAction.keyWord == "search")
                {
                    if (secondQuestActive && interaction.secondQuestTextResponseAlternate != null && interactableInRoom.searched) { interactableItems.searchDictionary.Add(interactableInRoom.noun, interaction.secondQuestTextResponseAlternate); }
                    else if (secondQuestActive && interaction.secondQuestTextResponse != null && !interactableInRoom.searched) { interactableItems.searchDictionary.Add(interactableInRoom.noun, interaction.secondQuestTextResponse); }
                    else if (interactableInRoom.searched && interaction.textResponseAlternate != null) { interactableItems.searchDictionary.Add(interactableInRoom.noun, interaction.textResponseAlternate); }
                    else { interactableItems.searchDictionary.Add(interactableInRoom.noun, interaction.textResponse); }                    
                }
                if (interaction.inputAction.keyWord == "listen")
                {                    
                    if (secondQuestActive && interaction.secondQuestTextResponseAlternate != null && interactableInRoom.searched) { interactableItems.listenToDictionary.Add(interactableInRoom.noun, interaction.secondQuestTextResponseAlternate); }
                    else if (secondQuestActive && interaction.secondQuestTextResponse != null && !interactableInRoom.searched) { interactableItems.listenToDictionary.Add(interactableInRoom.noun, interaction.secondQuestTextResponse); }
                    else if (interaction.textResponseAlternate != null && interactableInRoom.searched) { interactableItems.listenToDictionary.Add(interactableInRoom.noun, interaction.textResponseAlternate); }
                    else if (interaction.textResponse != null) { interactableItems.listenToDictionary.Add(interactableInRoom.noun, interaction.textResponse); }
                    //else { interactableItems.listenToDictionary.Add(interactableInRoom.noun,  "The only thing you hear is the beating of your own heart."); }
                }
            }
        }
    }

    public void FlipSearchedText(Room currentRoom, string noun)
    {
        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Length; i++)
        {
            InteractableObject iObject = currentRoom.interactableObjectsInRoom[i];
            if (!iObject.searched)
            {
                if (iObject.searchGeneratedItem != null) { interactableItems.inventory.Add(iObject.searchGeneratedItem); }
                //if (iObject.searchGeneratedWeapon != null)
                //{
                //    interactableItems.inventory.Add(iObject.searchGeneratedWeapon);
                //    //iObject.searchGeneratedWeapon = null;
                //}
                //if (iObject.searchGeneratedArmor != null)
                //{
                //    interactableItems.inventory.Add(iObject.searchGeneratedArmor);
                //    //iObject.searchGeneratedArmor = null;
                //}
                //if (iObject.searchGeneratedShield != null)
                //{
                //    interactableItems.inventory.Add(iObject.searchGeneratedShield);
                //    //iObject.searchGeneratedShield = null;
                //}
                //if (iObject.searchGeneratedUndroppable != null)
                //{
                //    interactableItems.inventory.Add(iObject.searchGeneratedUndroppable);
                //    //iObject.searchGeneratedUndroppable = null;
                //}
            }            
            if (iObject.noun == noun) { iObject.searched = true; }
        }  
    }

    public string[] TestVerbDictionaryWithNoun(Dictionary<string, string[]> verbDictionary, string verb, string noun)
    {
        string[] lookError = { "It looks pretty much as you'd expect." };
        string[] searchError = { "Unfortunately, searching that did not aid you in your quest. A waste of time even to describe it!" };
        string[] talkError = { "Hmm. No one seems to be listening." };
        string[] inspectError = { "On closer inspection, you see that's not there." };
        string[] listenError = { "All you hear is the beating of your own heart." };
        string[] errorError = { "That didn't do anything useful." };

        if (verbDictionary.ContainsKey(noun)) { return verbDictionary[noun]; }
        if (verb == "look") { return lookError; }
        if (verb == "search") { return searchError; }
        if (verb == "talk") { return talkError; }
        if (verb == "inspect") { return inspectError; }
        if (verb == "listen") { return listenError; }
        return errorError;
    }

    public void ChangeRoomDescription(Room room, int newDescriptionNumber)
    {
        room.currentDescription = newDescriptionNumber;        
    }

    void UnpackRoom()
    {
        roomNavigation.UnpackExitsInRoom();
        roomNavigation.currentRoom.visited = true;
        PrepareInteractablesInRoom(roomNavigation.currentRoom);
    }

    void ClearRoom()
    {
        roomNavigation.ClearExits();
        interactableItems.ClearInteractablesInRoom();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //nameText.text = dialogue.NPCName;
        inputBox.SetActive(false);
        sentences.Clear();
        pauses.Clear();
        foreach (string sentence in dialogue.sentences) { sentences.Enqueue(sentence); }
        foreach (int pause in dialogue.pauses) { pauses.Enqueue(pause); }

        if (dialogue.jokeConversation) { StartCoroutine(JokeConversation()); }
        else if (dialogue.timed) { StartCoroutine(TownConversation(dialogue)); }
        //else { EnterToContinueDialogue(); }
    }
    IEnumerator JokeConversation()
    {
        int totalLines = sentences.Count;
        while (sentences.Count >= 0)
        {
            if (sentences.Count == totalLines)
            {
                AddToMainWindowWithLine("Sidling up against the closest wall and holding your breath, you can make out the conversation.");
                yield return new WaitForSeconds(6f);
            }
            if (earlyConvoBail)
            {
                DisplayRoomText();
                inputBox.SetActive(true);
                textInput.inputField.ActivateInputField();
                textInput.inputField.text = null;
                earlyConvoBail = false;
                break;
            }
            int pause = pauses.Dequeue();
            string sentence = sentences.Dequeue();
            if (sentences.Count == totalLines - 1)
            {
                OverwriteMainWindow(sentence);
                yield return new WaitForSeconds(pause);
            }
            //ugly manipulation for special cases
            else if (sentence == " " || sentence.StartsWith("\"KHAAAAAAAA") || sentence == "Ba-doom shhh!")
            {
                if (sentence.StartsWith("\"KHAAAAAAAAAAAAAAAAAAAAAAAN!")) { earlyConvoBail = true; }
                OverwriteMainWindow(sentence);
                yield return new WaitForSeconds(pause);
            }
            else if (sentences.Count == 0)
            {
                enterToContinue = true;
                OverwriteMainWindow(sentence);
                break;
            }
            else
            {
                //ugly manipulation for special cases
                if (sentence.StartsWith("Press ENTER to")) { AddToMainWindow("\n\n" + sentence); }
                else { AddToMainWindowWithLine(sentence); }

                if (sentence.Contains("Press ENTER to continue")) { yield return new WaitUntil(EnterPressed); }
                else { yield return new WaitForSeconds(pause); }
            }
        }
    }

    IEnumerator TownConversation(Dialogue dialogue)
    {
        int totalLines = sentences.Count;
        while (sentences.Count >= 0)
        {
            if (sentences.Count == totalLines)
            {
                AddToMainWindowWithLine("Sidling up against the closest wall and holding your breath, you can make out the conversation.");
                yield return new WaitForSeconds(6f);
            }
            if (sentences.Count == 0)
            {
                enterToContinue = true;
                if (dialogue.townRepeat) { AddToMainWindowWithLine("The remainder of this conversation takes a dark turn.\n\n\nPress ENTER to continue."); }
                else { OverwriteMainWindow("The remainder of this conversation takes a dark turn.\n\n\n\n\n\n\nPress ENTER to continue."); }               
                break;
            }
            int pause = pauses.Dequeue();
            string sentence = sentences.Dequeue();
            if (sentences.Count == totalLines -1)
            {
                OverwriteMainWindow(sentence);
                yield return new WaitForSeconds(pause);
            }
            else
            {
                //ugly manipulation for special cases
                if (sentence.StartsWith("\"The 'Voyager' series") || sentence.StartsWith("\"And your 'pew") || sentence.StartsWith("Press ENTER to")) { AddToMainWindow("\n\n" + sentence); }
                else { AddToMainWindowWithLine(sentence); }

                if (sentence.Contains("Press ENTER to continue")) { yield return new WaitUntil(EnterPressed); }
                else { yield return new WaitForSeconds(pause); }
            }
        }        
    }

    public bool EnterPressed() { return Input.GetKeyDown(KeyCode.Return); }
    public bool EscPressed() { return Input.GetKeyDown(KeyCode.Escape); }
    public bool YesOrNo() { return (userInput == "yes" || userInput == "no"); }
    public bool YesSelected() { return (userInput == "yes"); }
    public bool NoSelected() { return (userInput == "no"); }
    public bool InputGiven() { return (userInput != null); }
    public bool InputGivenOrEscPressed() { return (userInput != null || Input.GetKeyDown(KeyCode.Escape)); }
    public bool UpDownEnterPressed() { return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow); }
    public bool LeftRightEnterEscPressed() { return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow); }
    public bool LeftRightUpDownEnterEscPressed() { return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow); }
    public bool RightUpDownEnterEscPressed() { return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow); }
    public bool LeftUpDownEnterEscPressed() { return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow); }
    public bool LeftRightEnterPressed() { return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow); }
    public bool UpDownEnterEscPressed() { return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow); }

    public void SnatchInput(string fromTextInput) { userInput = fromTextInput; }



    // Update is called once per frame
    void Update()
    {
        //equipment, HP, and money display
        blueCrystals.text = ego.blueCrystals.ToString();
        if (ego.equippedWeapon != null) { equippedWeapon.text = ego.equippedWeapon.nome; }
        else { equippedWeapon.text = "None"; }
        if (ego.equippedArmor != null) { equippedArmor.text = ego.equippedArmor.nome; }
        else { equippedArmor.text = "None"; }
        if (ego.equippedShield != null) { equippedShield.text = ego.equippedShield.nome; }
        else { equippedShield.text = "None"; }
        currentHP.text = ego.allStats[0].value.ToString() + "/" + ego.allStats[2].value.ToString();

        //inventory stats display
        //damage
        if (ego.allStats[6].effectValue > 0) { invDisplayDamage.text = $"1d{ego.allStats[6].value} +{ego.allStats[7].value} <color=green>+{ego.allStats[7].effectValue}</color>"; }
        else if (ego.allStats[6].effectValue < 0) { invDisplayDamage.text = $"1d{ego.allStats[6].value} +{ego.allStats[7].value} <color=red>{ego.allStats[7].effectValue}</color>"; }
        else { invDisplayDamage.text = $"1d{ego.allStats[6].value} +{ego.allStats[7].value}"; }

        //critMultiplier
        if (ego.allStats[8].effectValue > 0) { invDisplayCritical.text = "x" + ego.allStats[8].value + $" <color=green>+{ego.allStats[8].effectValue}</color>"; }
        else if (ego.allStats[8].effectValue < 0) { invDisplayCritical.text = "x" + ego.allStats[8].value + $" <color=red>{ego.allStats[8].effectValue}</color>"; }
        else { invDisplayCritical.text = "x" + ego.allStats[8].value; }
        
        //toHitMod
        if (ego.allStats[9].value >= 0)
        {
            if (ego.allStats[9].effectValue > 0) { invDisplayToHit.text = "+" + ego.allStats[9].value + $" <color=green>+{ego.allStats[9].effectValue}</color>"; }
            else if (ego.allStats[9].effectValue < 0) { invDisplayToHit.text = "+" + ego.allStats[9].value + $" <color=red>{ego.allStats[9].effectValue}</color>"; }
            else { invDisplayToHit.text = "+" + ego.allStats[9].value; }
        }
        else if (ego.allStats[9].value < 0)
        {
            if (ego.allStats[9].effectValue > 0) { invDisplayToHit.text = ego.allStats[9].value + $" <color=green>+{ego.allStats[9].effectValue}</color>"; }
            else if (ego.allStats[9].effectValue < 0) { invDisplayToHit.text = ego.allStats[9].value + $" <color=red>{ego.allStats[9].effectValue}</color>"; }
            else { invDisplayToHit.text = ego.allStats[9].value.ToString(); }
        }

        //armorClass
        if (ego.allStats[3].effectValue > 0) { invDisplayArmorClass.text = ego.allStats[3].value + $" <color=green>+{ego.allStats[3].effectValue}</color>"; }
        else if (ego.allStats[3].effectValue < 0) { invDisplayArmorClass.text = ego.allStats[3].value + $" <color=red>{ego.allStats[3].effectValue}</color>"; }
        else { invDisplayArmorClass.text = ego.allStats[3].value.ToString(); }

        //critResist
        if (ego.allStats[4].effectValue > 0) { invDisplayCritResist.text = "x" + ego.allStats[4].value + $" <color=green>+{ego.allStats[4].effectValue}</color>"; }
        else if (ego.allStats[4].effectValue < 0) { invDisplayCritResist.text = "x" + ego.allStats[4].value + $" <color=red>{ego.allStats[4].effectValue}</color>"; }
        else { invDisplayCritResist.text = "x" + ego.allStats[4].value; }

        //dmgReduction
        if (ego.allStats[5].effectValue > 0) { invDisplayDmgReduction.text = "-" + ego.allStats[5].value + $" <color=green>-{ego.allStats[5].effectValue}</color>"; }
        else if (ego.allStats[5].effectValue < 0) { invDisplayDmgReduction.text = "+" + ego.allStats[5].value + $" <color=red>+{Mathf.Abs(ego.allStats[5].effectValue)}</color>"; }
        else { invDisplayDmgReduction.text = "-" + ego.allStats[5].value; }

        //low HP color change
        if (fireTheColor)
        {
            if (!colorItRed && !colorItOrange && !colorItYellow) { colorItRed = true; }
            if (colorItRed)
            {
                currentHP.color = Color.red;
                colorItRed = false;
                colorItOrange = true;
            }
            else if (colorItOrange)
            {
                currentHP.color = orange;
                colorItOrange = false;
                colorItYellow = true;
            }
            else if (colorItYellow)
            {
                currentHP.color = Color.yellow;
                colorItYellow = false;
                colorItRed = true;
            }
        }
        if (ego.allStats[0].value <= (.1 * (ego.allStats[2].value + ego.allStats[2].effectValue))) { fireTheColor = true; }
        else if (ego.allStats[0].value <= (.25 * (ego.allStats[2].value + ego.allStats[2].effectValue))) { currentHP.color = Color.red; }
        else if (ego.allStats[0].value <= (.5 * (ego.allStats[2].value + ego.allStats[2].effectValue))) { currentHP.color = orange; }
        else if (ego.allStats[0].value <= (.75 * (ego.allStats[2].value + ego.allStats[2].effectValue))) { currentHP.color = Color.yellow; }
        else if (ego.allStats[0].value > (.75 * (ego.allStats[2].value + ego.allStats[2].effectValue))) { currentHP.color = Color.white; }
        if (ego.allStats[0].value > (.1 * (ego.allStats[2].value + ego.allStats[2].effectValue))) { fireTheColor = false; }

        if (enterToContinue && (Time.time - textInput.keyPressDelay >= 0.25))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                enterToContinue = false;
                inputBox.SetActive(true);
                textInput.inputField.ActivateInputField();
                textInput.inputField.text = null;

                if (currentActiveInput == "main") { DisplayRoomText(); }
                else if (currentActiveInput == "inventory")
                {
                    escToContinue = true;
                    interactableItems.DisplayInventory();
                }
            }
        }
        else if (!enterToContinue && Input.GetKeyDown(KeyCode.Return)) { EventSystem.current.SetSelectedGameObject(inputBox); }

        if (escToContinue && (Time.time - textInput.keyPressDelay >= 0.25))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                map.mapWindow.SetActive(false);
                interactableItems.inventoryStats.SetActive(false);
                inputBox.SetActive(true);
                DisplayRoomText();
                currentActiveInput = "main";
                escToContinue = false;
                textInput.inputField.ActivateInputField();
                textInput.inputField.text = null;
            }
        }
        if (exitPopUp && (Time.time - textInput.keyPressDelay >= 0.25))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                popUpText.text = null;
                popUpBox.SetActive(false);
                if (toResumeEscToContinue)
                {
                    escToContinue = true;
                    toResumeEscToContinue = false;
                }
                exitPopUp = false;
                inputBox.SetActive(true);
                textInput.inputField.ActivateInputField();
                textInput.inputField.text = null;
            }
        }
        //if (enterToContinueDialogue && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    enterToContinueDialogue = false;
        //    EnterToContinueDialogue();
        //}
        if (Input.GetKeyDown(KeyCode.Tab) && actionLog.Count > 0)
        {
            lastAction = actionLog[actionLog.Count - lastActionHero];
            if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                if (textInput.inputField.text == lastAction && actionLog.Count > lastActionHero)
                {
                    lastActionHero++;
                    lastAction = actionLog[actionLog.Count - lastActionHero];
                }
                else { lastActionHero = 1; }
            } 
            else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (textInput.inputField.text == lastAction && lastActionHero > 1)
                {
                    lastActionHero--;
                    lastAction = actionLog[actionLog.Count - lastActionHero];
                }
                else { lastActionHero = actionLog.Count; }
            }
            textInput.inputField.text = lastAction;
        }

        //scroll arrows
        if (scrollBar.size < 1) { scrollArrows.SetActive(true); }
        else { scrollArrows.SetActive(false); }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            scrollRectValue = GameObject.Find("ScrollRect").GetComponent<ScrollRect>().verticalNormalizedPosition;
            if (scrollRectValue <= 0.9)
            {
                scrollUpArrow.Select();
                Canvas.ForceUpdateCanvases();
                GameObject.Find("ScrollRect").GetComponent<ScrollRect>().verticalNormalizedPosition += 0.1f;
                Canvas.ForceUpdateCanvases();
                scrollNonArrow.Select();
            }
            else if (scrollRectValue > 0.9)
            {
                scrollUpArrow.Select();
                Canvas.ForceUpdateCanvases();
                GameObject.Find("ScrollRect").GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
                Canvas.ForceUpdateCanvases();
                scrollNonArrow.Select();
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            scrollRectValue = GameObject.Find("ScrollRect").GetComponent<ScrollRect>().verticalNormalizedPosition;
            if (scrollRectValue >= 0.1)
            {
                scrollDownArrow.Select();
                Canvas.ForceUpdateCanvases();
                GameObject.Find("ScrollRect").GetComponent<ScrollRect>().verticalNormalizedPosition -= 0.1f;
                Canvas.ForceUpdateCanvases();
                scrollNonArrow.Select();
            }
            else if (scrollRectValue < 0.1)
            {
                scrollDownArrow.Select();
                Canvas.ForceUpdateCanvases();
                GameObject.Find("ScrollRect").GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
                Canvas.ForceUpdateCanvases();
                scrollNonArrow.Select();
            }
        }
        //if (dropIt)
        //{
        //    popUpText.text = null;
        //    popUpBox.SetActive(false);
        //    dropIt = false;
        //    DropItem(droppingItem);
        //}
        //if (dropItNot)
        //{
        //    popUpText.text = null;
        //    popUpBox.SetActive(false);
        //    dropItNot = false;
        //    DisplayNarratorResponse("Discretion is the better part of valor.\n\nPackrat.");
        //}
        if (ego.equippedWeapon == registerObjects.allItems[0] || interactableItems.inventory.Contains(registerObjects.allItems[0])) { hasBubbleLead = true; }
        else { hasBubbleLead = false; }
    }
}
