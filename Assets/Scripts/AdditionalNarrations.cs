using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalNarrations : MonoBehaviour
{
    string userInput;
    //public Room currentRoom;
    //public InteractableObject dinnerTray;
    GameController controller;
    //float timeDelay = 1;
    //bool traySearched;
    [HideInInspector] public bool townVoicesSearched = false;
    [HideInInspector] public bool unlockListenTo;
    [HideInInspector] public bool listenToUnlocked;
    //[HideInInspector] public string activateStep;
    bool newConverse;
    

    // Start is called before the first frame update
    void Start()
    {
        //traySearched = false;
        townVoicesSearched = false;
        unlockListenTo = false;
        listenToUnlocked = false;
        newConverse = true;
        controller = GetComponent<GameController>();
        //activateStep = "";
        userInput = "";
    }
    public void SnatchInput(string fromTextInput)
    {
        userInput = fromTextInput;
        //listen to town voices
        if (!townVoicesSearched && controller.roomNavigation.currentRoom.roomName == "G7" && userInput == "search voices")
        {
            townVoicesSearched = true;
            unlockListenTo = true;
        }
        else if (unlockListenTo && userInput == "listen to voices" && controller.roomNavigation.currentRoom.roomName == "G7")
        {
            controller.textInput.textIsGood = true;
            unlockListenTo = false;
            listenToUnlocked = true;
            controller.ChangeRoomDescription(controller.roomNavigation.currentRoom, 2);
            //controller.DisplayNarratorResponse("Fine.");
            StartCoroutine(controller.Narrator("Fine."));
        }
        else if (listenToUnlocked && userInput == "listen to voices" && controller.roomNavigation.currentRoom.roomName == "G7")
        {
            controller.textInput.textIsGood = true;
            //additionalNarrations.activateStep = "conversations";
            Conversation(controller.ego);
        }
    }
    public void SnatchRoom(Room fromRoomNavigation)
    {
        
    }
    public IEnumerator InitiateFirstTownVisit()
    {
        controller.inputBox.SetActive(false);
        controller.registerRooms.allRooms[32].visited = true;

        List<string> townInitial = new List<string>();
        townInitial.Add("Hey. You must be new? Haven't seen you around before. You can call me Badger, and this here is Skinny Pete. Welcome to your new home!");
        townInitial.Add("If you wanna do some trading, Skinny Pete is your man. He's interested in pretty much anything - and that's how he's also got pretty much anything! Otherwise, if you need to rest -- I'm your guy. For just a small fee, you can get a full night's rest and (rest assured!) I'll watch out for ya.");
        townInitial.Add("And if you want to \"make something\" of yourself, you can go pay your respects to the cartel just out and to the north. I'd be careful getting involved with them, though.");
        townInitial.Add("It's good to see a friendly face around here. If there's anything we can do to help just let us know. Working together is the best way to survive down here!");
        if (controller.secondQuestActive) { townInitial[0] = "Hey. You must be new? Gee -- aren't you a looker! Best keep that mace handy. You can call me Badger, and this here is Skinny Pete. Welcome to your new home!\""; }
        
        controller.narratorComplete = false;
        controller.OverwriteMainWindow("There are two men conversing, who stop and look as you enter. They are each standing behind a crudely built podium; one has a plank of wood lying beside him, while the other conversely has a large, filled sack.\n\nThe man with the wooden plank speaks.\n\n\nPress ENTER to continue.");
        yield return new WaitForSeconds(.25f);
        yield return new WaitUntil(controller.EnterPressed);

        controller.npcInteraction.WriteNPCName("Badger");
        controller.npcInteraction.WriteDialogueOptions(null, null, null, null);
        controller.npcInteraction.ActivateDialogueBox();
        yield return new WaitForSeconds(.5f);
        controller.npcInteraction.npcSpeechComplete = false;
        StartCoroutine(controller.npcInteraction.NPCSpeech(townInitial));
        yield return new WaitUntil(controller.npcInteraction.NPCSpeechComplete);
        controller.npcInteraction.npcSpeechComplete = false;
        controller.UnlockUserInput();
        controller.npcInteraction.dialogueBox.SetActive(false);
        controller.npcInteraction.dialogueBoxBackground.SetActive(false);
        //string[] townScript =
        //    {
        //        "There are two men conversing, who stop and look as you enter. A third sits in a corner and writes in a book, seemingly unaware of your presence. The first two are each standing behind a crudely built podium; one has a [plank] of wood lying beside him, while the other conversely has a large, filled [sack].",
        //        "The man with the wooden plank speaks.\n\n\"Hey. You must be new? Haven't seen you around before. You can call me Badger, and this here is Skinny Pete. That one doodling in the corner is Jeff. Welcome to your new home!\"",
        //        "The man in the corner looks up.\n\n\"That'd be 'Geoff.'\"",
        //        "<size=15><b>Badger</b></size>\n-------------------------------------\n\n\n\"Right. What'd I say? Anyways...\"\n\n\"If you wanna do some trading, Skinny Pete is your man. He's interested in pretty much anything - and that's how he's also got pretty much anything! Otherwise, if you need to rest -- I'm your guy. For just a small fee, you can get a full night's rest and (rest assured!) I'll watch out for ya.\"",
        //        "<size=15><b>Badger</b></size>\n-------------------------------------\n\n\n\"And Jeff there likes to draw caricatures.\"",
        //        "<size=15><b>Geoff</b></size>\n-------------------------------------\n\n\n\"I'm a WRITER! You know - with ink and parchment?\"\n\n\"I would be happy to create a log of your journey. Just have a seat at your leisure and tell me of your exploits so we can write down your progress.\"",
        //        "<size=15><b>Badger</b></size>\n-------------------------------------\n\n\n\"Otherwise, if you want to 'make something' of yourself down here, you can go pay your respects to the cartel just out and to the north. I'd be careful getting involved with them, though.\""
        //    };
        //if (controller.secondQuestActive) { townScript[1] = "The man with the wooden plank speaks.\n\n\"Hey. You must be new? Gee -- aren't you a looker! Best keep that mace handy. You can call me Badger, and this here is Skinny Pete. That one doodling in the corner is Jeff. Welcome to your new home!\""; }
        //float[] townPauses = { 0, 0, 0, 0, 0, 0, 0 };
        //controller.InitiateMultiLineDialogue(townScript, townPauses);
    }
    public void Conversation(Ego ego)
    {
        controller.inputBox.SetActive(false);
        if (ego.conversation == 0)
        {
            ego.conversation++;
            newConverse = false;
            //activateStep = "firstConvo1";
            controller.conversation1.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
        else if (ego.conversation == 1)
        {
            if (newConverse)
            {
                ego.conversation++;
                newConverse = false;
                //activateStep = "secondConvo1";
                controller.conversation2.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
            else { controller.conversation1Repeat.GetComponent<DialogueTrigger>().TriggerDialogue(); }
            //else { activateStep = "firstConvoSkip1"; }
        }
        else if (ego.conversation == 2)
        {
            if (newConverse)
            {
                ego.conversation++;
                newConverse = false;
                //activateStep = "thirdConvo1";
                controller.conversation3.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
            else { controller.conversation1Repeat.GetComponent<DialogueTrigger>().TriggerDialogue(); }
            //else { activateStep = "secondConvoSkip1"; }
        }
        else if (ego.conversation == 3)
        {
            if (newConverse)
            {
                ego.conversation++;
                newConverse = false;
                //activateStep = "fourthConvo1";
                controller.conversation4.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
            else { controller.conversation2Repeat.GetComponent<DialogueTrigger>().TriggerDialogue(); }
            //else { activateStep = "thirdConvoSkip1"; }
        }
        else if (ego.conversation == 4)
        {
            if (newConverse)
            {
                ego.conversation++;
                newConverse = false;
                //activateStep = "fifthConvo1";
                controller.conversation5.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
            else { controller.conversation3Repeat.GetComponent<DialogueTrigger>().TriggerDialogue(); }
            //else { activateStep = "fourthConvoSkip1"; }
        }
        else if (ego.conversation == 5)
        {
            if (newConverse)
            {
                ego.conversation++;
                newConverse = false;
                //activateStep = "sixthConvo1";
                controller.conversation6.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
            else { controller.conversation4Repeat.GetComponent<DialogueTrigger>().TriggerDialogue(); }
            //else { activateStep = "fifthConvoSkip1"; }
        }
        else if (ego.conversation == 6)
        {
            if (newConverse)
            {
                ego.conversation++;
                newConverse = false;
                //activateStep = "seventhConvo1";
                controller.conversation7.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
            else { controller.conversation5Repeat.GetComponent<DialogueTrigger>().TriggerDialogue(); }
            //else { activateStep = "sixthConvoSkip1"; }
        }
        else if (ego.conversation == 7) { controller.conversation7Repeat.GetComponent<DialogueTrigger>().TriggerDialogue(); }
        //else if (ego.conversation == 7) { activateStep = "seventhConvoSkip1"; }
        else if (ego.conversation == 255)
        {
            ego.conversation++;
            //activateStep = "firstJoke1";
            controller.jokeConversation1.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
        else if (ego.conversation == 256)
        {
            ego.conversation++;
            //activateStep = "secondJoke1";
            controller.jokeConversation2.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
        else if (ego.conversation == 257)
        {
            ego.conversation++;
            //activateStep = "thirdJoke1";
            controller.jokeConversation3.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
        else if (ego.conversation == 258)
        {
            ego.conversation = 255;
            //activateStep = "fourthJoke1";
            controller.jokeConversation4.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (controller.pauseIt) //All narrator delays
        //{
        //    if (controller.pauseItForUndroppable && (Time.time - timeDelay >= 2))
        //    {
        //        controller.pauseItForUndroppable = false;
        //        controller.pauseIt = false;
        //        controller.DisplayNarratorResponse("Then you think better of it and pick it back up.");
        //    }
        //    if (controller.pauseItForDrop && (Time.time - timeDelay >= 4))
        //    {
        //        controller.pauseIt = false;
        //        controller.pauseItForDrop = false;
        //        controller.DisplayNarratorResponse("Wait. What game is this?");
        //    }
        //}
        //dinner tray
        //if (!traySearched && currentRoom.roomName == "I7" && userInput == "search tray")
        //{
        //    controller.enterToContinue = false;
        //    traySearched = true;
        //    timeDelay = Time.time;
        //    activateStep = "tray2";
        //}
        //if (activateStep == "tray2" && (Time.time - timeDelay >= 4))
        //{
        //    timeDelay = Time.time;
        //    activateStep = "tray3";
        //    controller.AddToMainWindowWithLine("Wait.");
        //}
        //if (activateStep == "tray3" && (Time.time - timeDelay >= 2))
        //{
        //    controller.currentActiveInput = "yesnodelays";
        //    activateStep = "tray4";
        //    userInput = null;
        //    controller.inputBox.SetActive(true);
        //    controller.textInput.inputField.ActivateInputField();
        //    controller.textInput.inputField.text = null;
        //    controller.OpenPopUpWindow("", "", "You don't want to, like, try and use these... do you?", "", "[Yes]", "", "[No]", "");
        //}
        //if (activateStep == "tray4")
        //{
        //    if (userInput == "yes")
        //    {
        //        controller.inputBox.SetActive(false);
        //        activateStep = "tray5";
        //        timeDelay = Time.time;
        //        controller.ClosePopUpWindow();
        //        controller.AddToMainWindow("\n\nOooookay");
        //    }
        //    else if (userInput == "no")
        //    {
        //        activateStep = "";
        //        userInput = "";
        //        controller.currentActiveInput = "main";
        //        controller.ClosePopUpWindow();
        //        controller.registerObjects.allObjects[0].searched = false;
        //        traySearched = false;
        //        controller.DisplayNarratorResponse("Well good. That's very sensible of you.");
        //    }
        //    else if (userInput != null)
        //    {
        //        userInput = null;
        //        controller.AddToMainWindowWithLine("Yes no maybe so?");
        //        controller.textInput.inputField.ActivateInputField();
        //        controller.textInput.inputField.text = null;
        //    }
        //}
        //if (activateStep == "tray5" && (Time.time - timeDelay >= .5))
        //{
        //    activateStep = "tray6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindow(".");
        //}
        //if (activateStep == "tray6" && (Time.time - timeDelay >= .5))
        //{
        //    activateStep = "tray7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindow(".");
        //}
        //if (activateStep == "tray7" && (Time.time - timeDelay >= .5))
        //{
        //    activateStep = "tray8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindow(".");
        //}
        //if (activateStep == "tray8" && (Time.time - timeDelay >= 2))
        //{
        //    activateStep = "tray9";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("You pick up the spoon, and brandish it high into the air. You swing it around several times, running battle drills with yourself. \"Voosh! voooosh!\" You begin to hear the sounds of your glorious blade whizzing through the air; though you soon realize that you were just getting a little too into it and began making sound effects.\n\nPress ENTER to continue.");
        //}
        //if (activateStep == "tray9" && Input.GetKeyDown(KeyCode.Return) && (Time.time - timeDelay >= .25))
        //{
        //    activateStep = "tray10";
        //    timeDelay = Time.time;
        //    controller.GetEquipped(controller.registerObjects.rustyWoodenSpoon);
        //    controller.AddToMainWindowWithLine("You equip the Rusty Wooden Spoon.\n\nPress ENTER to continue.");
        //}
        //if (activateStep == "tray10" && Input.GetKeyDown(KeyCode.Return) && (Time.time - timeDelay >= .25))
        //{
        //    activateStep = "tray11";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("You pick up the bowl, and juggle it around in your hand a few times. Fitting your fingers up against the inside walls, you push outward until friction causes the bowl to remain on the end of your hand. Extending your arm furiously, you repel many floating particles of dust which were certainly a choking hazard.\n\nPress ENTER to continue.");
        //}
        //if (activateStep == "tray11" && Input.GetKeyDown(KeyCode.Return) && (Time.time - timeDelay >= .25))
        //{
        //    activateStep = "";
        //    userInput = "";
        //    controller.currentActiveInput = "main";
        //    controller.GetStrapped(controller.registerObjects.soupBowl);
        //    controller.DisplayNarratorResponse("You equip the Soup Bowl.");
        //}

        //dust tracks
        //if (!tracksSearched && (currentRoom.roomName == "H7" || currentRoom.roomName == "G7") && userInput == "search tracks")
        //{
        //    if (activateStep == "") { activateStep = "tracks1"; }
        //    if (activateStep == "tracks1" && ((Time.time - controller.textInput.keyPressDelay) >= .1))
        //    {
        //        controller.enterToContinue = false;
        //        timeDelay = Time.time;
        //        activateStep = "tracks2";
        //    }
        //    if (activateStep == "tracks2" && Input.GetKeyDown(KeyCode.Return) && (Time.time - timeDelay >= .15))
        //    {
        //        activateStep = "tracks3";
        //        timeDelay = Time.time;
        //        controller.AddToMainWindowWithLine("After returning to status erectus, you realize you're now wearing the tracks, erasing anything they may have told you. I certainly hope you \"looked at\" them first!\n\nPress ENTER to continue.");
        //    }
        //    if (activateStep == "tracks3" && Input.GetKeyDown(KeyCode.Return) && (Time.time - timeDelay >= .25))
        //    {
        //        activateStep = "";
        //        userInput = "";
        //        tracksSearched = true;
        //        controller.enterToContinue = true;
        //        controller.textInput.keyPressDelay = Time.time;
        //        controller.AddToMainWindowWithLine("Totally messing with you. You're dusty but they're still there. It's cool.\n\nPress ENTER to (for real) continue.");
        //    }
        //}
        //else if (tracksSearched && (currentRoom.roomName == "H7" || currentRoom.roomName == "G7") && userInput == "search tracks")
        //{
        //    if (activateStep == "") { activateStep = "tracks1"; }
        //    if (activateStep == "tracks1" && ((Time.time - controller.textInput.keyPressDelay) >= .1))
        //    {
        //        controller.enterToContinue = false;
        //        timeDelay = Time.time;
        //        activateStep = "tracks2";
        //    }
        //    if (activateStep == "tracks2" && Input.GetKeyDown(KeyCode.Return) && (Time.time - timeDelay >= .25))
        //    {
        //        activateStep = "";
        //        userInput = "";
        //        controller.textInput.keyPressDelay = Time.time;
        //        controller.enterToContinue = true;
        //        controller.DisplayNarratorResponse("...That's how Aragorn does it isn't it?");
        //    }
        //}

        //listen to town voices
        //if (!townVoicesSearched && currentRoom.roomName == "G7" && userInput == "search voices")
        //{
        //    userInput = "";
        //    controller.enterToContinue = false;
        //    townVoicesSearched = true;
        //    timeDelay = Time.time;
        //    activateStep = "voices2";
        //}
        //if (activateStep == "voices2" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    controller.textInput.keyPressDelay = Time.time;
        //    activateStep = "";
        //    unlockListenTo = true;
        //    controller.inputBox.SetActive(false);
        //    controller.DisplayNarratorResponse("Oh this is on me, isn't it? No \"listen to\" command?... Well we don't have the budget to provide you with EVERY command prompt.");
        //}

        //conversations

        ////first convo with pauses
        //if (activateStep == "firstConvo1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "firstConvo2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"No, man! No one will ever - CAN ever - be better than the original!\"");
        //}
        //if (activateStep == "firstConvo2" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "firstConvo3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Listen, bro. Life is change. Records are made to be broken. Adapt or die. New things will always come along and it is their very purpose to best what came before.\"");
        //}
        //if (activateStep == "firstConvo3" && (Time.time - timeDelay >= 8))
        //{
        //    activateStep = "firstConvo4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Yeah but that don't mean that just because a newer one is good that it's better. Think about the time period Spock existed in and the fact that he had no original material to work off of.\"");
        //}
        //if (activateStep == "firstConvo4" && (Time.time - timeDelay >= 10))
        //{
        //    activateStep = "firstConvo5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Yeah but do those handicaps change the score? It's maybe 'unfair' that Tim Russ didn't have to reinvent the wheel and could build on Nimoy's work, but that's how it goes. As Newton said: we stand on the shoulders of giants, bro.\"");
        //}
        //if (activateStep == "firstConvo5" && (Time.time - timeDelay >= 11))
        //{
        //    activateStep = "firstConvo6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Oh, dude, Mighty Ducks 3? I love that movie too.\"");
        //}
        //if (activateStep == "firstConvo6" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.OverwriteMainWindow("The remainder of this conversation takes a dark turn.\n\n\n\n\n\n\nPress ENTER to continue.");
        //}
        ////first convo with skip
        //if (activateStep == "firstConvoSkip1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "firstConvoSkip2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"No, man! No one will ever - CAN ever - be better than the original!\"");
        //}
        //if (activateStep == "firstConvoSkip2" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "firstConvoSkip3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Listen, bro. Life is change. Records are made to be broken. Adapt or die. New things will always come along and it is their very purpose to best what came before.\"");
        //}
        //if (activateStep == "firstConvoSkip3" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "firstConvoSkip4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Yeah but that don't mean that just because a newer one is good that it's better. Think about the time period Spock existed in and the fact that he had no original material to work off of.\"");
        //}
        //if (activateStep == "firstConvoSkip4" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "firstConvoSkip5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Yeah but do those handicaps change the score? It's maybe 'unfair' that Tim Russ didn't have to reinvent the wheel and could build on Nimoy's work, but that's how it goes. As Newton said: we stand on the shoulders of giants, bro.\"");
        //}
        //if (activateStep == "firstConvoSkip5" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "firstConvoSkip6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Oh, dude, Mighty Ducks 3? I love that movie too.\"");
        //}
        //if (activateStep == "firstConvoSkip6" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.AddToMainWindowWithLine("The remainder of this conversation takes a dark turn.\n\nPress ENTER to continue.");
        //}

        ////second convo with pauses
        //if (activateStep == "secondConvo1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "secondConvo2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"No way, man. How could she be better? She's not even likeable on her own.\"");
        //}
        //if (activateStep == "secondConvo2" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "secondConvo3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Hey, man. She's strong, she's independent, and she lets everyone know it. At least she's not Picard's coddled little pet.\"");
        //}
        //if (activateStep == "secondConvo3" && (Time.time - timeDelay >= 8))
        //{
        //    activateStep = "secondConvo4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Love isn't a weakness. Their relationship is understated and it doesn't take away from Crusher's strength as a woman. Pulaski is just mean -- do you remember when she just kept telling Data he's just a machine and his dreams and aspirations of being human are impossible? Bitch is just nasty.\"");
        //}
        //if (activateStep == "secondConvo4" && (Time.time - timeDelay >= 12))
        //{
        //    activateStep = "secondConvo5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"That's how it is with a confident, independent person. It's how anyone like her would really act, and she has many otherwise redeeming qualities related to that behavior.\"");
        //}
        //if (activateStep == "secondConvo5" && (Time.time - timeDelay >= 10))
        //{
        //    activateStep = "secondConvo6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Sure -- but this isn't reality. TV is a form of art, and as such is a representation of reality - NOT reality itself. Art is lies that tells the truth.\"");
        //}
        //if (activateStep == "secondConvo6" && (Time.time - timeDelay >= 10))
        //{
        //    activateStep = "secondConvo7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"TV is art? Next you're going to tell me those scribbles Picasso made are art, too.\"");
        //}
        //if (activateStep == "secondConvo7" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.OverwriteMainWindow("The remainder of this conversation takes a dark turn.\n\n\n\n\n\n\nPress ENTER to continue.");
        //}
        ////second convo with skip
        //if (activateStep == "secondConvoSkip1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "secondConvoSkip2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"No way, man. How could she be better? She's not even likeable on her own.\"");
        //}
        //if (activateStep == "secondConvoSkip2" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "secondConvoSkip3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Hey, man. She's strong, she's independent, and she lets everyone know it. At least she's not Picard's coddled little pet.\"");
        //}
        //if (activateStep == "secondConvoSkip3" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "secondConvoSkip4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Love isn't a weakness. Their relationship is understated and it doesn't take away from Crusher's strength as a woman. Pulaski is just mean -- do you remember when she just kept telling Data he's just a machine and his dreams and aspirations of being human are impossible? Bitch is just nasty.\"");
        //}
        //if (activateStep == "secondConvoSkip4" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "secondConvoSkip5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"That's how it is with a confident, independent person. It's how anyone like her would really act, and she has many otherwise redeeming qualities related to that behavior.\"");
        //}
        //if (activateStep == "secondConvoSkip5" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "secondConvoSkip6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Sure -- but this isn't reality. TV is a form of art, and as such is a representation of reality - NOT reality itself. Art is lies that tells the truth.\"");
        //}
        //if (activateStep == "secondConvoSkip6" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "secondConvoSkip7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"TV is art? Next you're going to tell me those scribbles Picasso made are art, too.\"");
        //}
        //if (activateStep == "secondConvoSkip7" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.AddToMainWindowWithLine("The remainder of this conversation takes a dark turn.\n\nPress ENTER to continue.");
        //}
        
        ////third convo with pauses
        //if (activateStep == "thirdConvo1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "thirdConvo2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"Bro, it got a 93% Rotten Tomatoes score and a 89% audience approval score.\"");
        //}
        //if (activateStep == "thirdConvo2" && (Time.time - timeDelay >= 6))
        //{
        //    activateStep = "thirdConvo3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Dude, should I even list movies that were terrible but also 'scored' well with critics and audiences? Not to mention the corruption rumors behind the ratings on Star Wars IX.\"");
        //}
        //if (activateStep == "thirdConvo3" && (Time.time - timeDelay >= 10))
        //{
        //    activateStep = "thirdConvo4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"But what makes you think you'd be right in the face of so much differing opinion?\"");
        //}
        //if (activateStep == "thirdConvo4" && (Time.time - timeDelay >= 8))
        //{
        //    activateStep = "thirdConvo5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"The script was contrived and heavy-handed. The love story was shoe-horned near the end from non-existence. And don't even get me started on Data.\"");
        //}
        //if (activateStep == "thirdConvo5" && (Time.time - timeDelay >= 9))
        //{
        //    activateStep = "thirdConvo6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"...Let's not say things we can't take back, bro.\"");
        //}
        //if (activateStep == "thirdConvo6" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "thirdConvo7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"OK, you're right on that. But come on -- that whole thing about firing the torpedos at the Phoenix and everyone just stands there until they miss? Data obviously misses on purpose, and as he doesn't capitalize on his decoy it only serves as fabricated drama for a lacking movie.\"");
        //}
        //if (activateStep == "thirdConvo7" && (Time.time - timeDelay >= 15))
        //{
        //    activateStep = "thirdConvo8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"DATA CAN DO NO WRONG!!\"");
        //}
        //if (activateStep == "thirdConvo8" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.OverwriteMainWindow("The remainder of this conversation takes a dark turn.\n\n\n\n\n\n\nPress ENTER to continue.");
        //}
        ////third convo with skip
        //if (activateStep == "thirdConvoSkip1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "thirdConvoSkip2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"Bro, it got a 93% Rotten Tomatoes score and a 89% audience approval score.\"");
        //}
        //if (activateStep == "thirdConvoSkip2" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "thirdConvoSkip3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Dude, should I even list movies that were terrible but also 'scored' well with critics and audiences? Not to mention the corruption rumors behind the ratings on Star Wars IX.\"");
        //}
        //if (activateStep == "thirdConvoSkip3" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "thirdConvoSkip4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"But what makes you think you'd be right in the face of so much differing opinion?\"");
        //}
        //if (activateStep == "thirdConvoSkip4" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "thirdConvoSkip5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"The script was contrived and heavy-handed. The love story was shoe-horned near the end from non-existence. And don't even get me started on Data.\"");
        //}
        //if (activateStep == "thirdConvoSkip5" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "thirdConvoSkip6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"...Let's not say things we can't take back, bro.\"");
        //}
        //if (activateStep == "thirdConvoSkip6" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "thirdConvoSkip7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"OK, you're right on that. But come on -- that whole thing about firing the torpedos at the Phoenix and everyone just stands there until they miss? Data obviously misses on purpose, and as he doesn't capitalize on his decoy it only serves as fabricated drama for a lacking movie.\"");
        //}
        //if (activateStep == "thirdConvoSkip7" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "thirdConvoSkip8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"DATA CAN DO NO WRONG!!\"");
        //}
        //if (activateStep == "thirdConvoSkip8" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.AddToMainWindowWithLine("The remainder of this conversation takes a dark turn.\n\nPress ENTER to continue.");
        //}

        ////fourth convo with pauses
        //if (activateStep == "fourthConvo1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "fourthConvo2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"There's nothing that can get in the way of love, bro.\"");
        //}
        //if (activateStep == "fourthConvo2" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "fourthConvo3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"It's even visited several times throughout the series; the very point of their relationship is that they can never be.\"");
        //}
        //if (activateStep == "fourthConvo3" && (Time.time - timeDelay >= 8))
        //{
        //    activateStep = "fourthConvo4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"But they did end up togeth --\"");
        //}
        //if (activateStep == "fourthConvo4" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "fourthConvo5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Bro, don't even get me started on the movies again. They were trash and that was just pandering to audiences so to fill their pockets on a lackluster movie.\"");
        //}
        //if (activateStep == "fourthConvo5" && (Time.time - timeDelay >= 10))
        //{
        //    activateStep = "fourthConvo6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Oh, come on, bro, everyone deserves a happy ending.\"");
        //}
        //if (activateStep == "fourthConvo6" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "fourthConvo7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Riker chose his career over love. His ambition flourished but at the cost of his personal life. There's even tangential themes of him 'ending up like Picard' but - not only accepting it - relishing it.\"");
        //}
        //if (activateStep == "fourthConvo7" && (Time.time - timeDelay >= 14))
        //{
        //    activateStep = "fourthConvo8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"I've got faith... Faith of the heart...\"");
        //}
        //if (activateStep == "fourthConvo8" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.OverwriteMainWindow("The remainder of this conversation takes a dark turn.\n\n\n\n\n\n\nPress ENTER to continue.");
        //}
        ////fourth convo with skip
        //if (activateStep == "fourthConvoSkip1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "fourthConvoSkip2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"There's nothing that can get in the way of love, bro.\"");
        //}
        //if (activateStep == "fourthConvoSkip2" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fourthConvoSkip3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"It's even visited several times throughout the series; the very point of their relationship is that they can never be.\"");
        //}
        //if (activateStep == "fourthConvoSkip3" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fourthConvoSkip4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"But they did end up togeth --\"");
        //}
        //if (activateStep == "fourthConvoSkip4" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fourthConvoSkip5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Bro, don't even get me started on the movies again. They were trash and that was just pandering to audiences so to fill their pockets on a lackluster movie.\"");
        //}
        //if (activateStep == "fourthConvoSkip5" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fourthConvoSkip6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Oh, come on, bro, everyone deserves a happy ending.\"");
        //}
        //if (activateStep == "fourthConvoSkip6" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fourthConvoSkip7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Riker chose his career over love. His ambition flourished but at the cost of his personal life. There's even tangential themes of him 'ending up like Picard' but - not only accepting it - relishing it.\"");
        //}
        //if (activateStep == "fourthConvoSkip7" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fourthConvoSkip8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"I've got faith... Faith of the heart...\"");
        //}
        //if (activateStep == "fourthConvoSkip8" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.AddToMainWindowWithLine("The remainder of this conversation takes a dark turn.\n\nPress ENTER to continue.");
        //}

        ////fifth convo with pauses
        //if (activateStep == "fifthConvo1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "fifthConvo2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"It was just ahead of its time, bro.\"");
        //}
        //if (activateStep == "fifthConvo2" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "fifthConvo3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Did you even watch the original series? Yes, it was certainly ahead of its time -- but it also got pretty 'out there.'\"");
        //}
        //if (activateStep == "fifthConvo3" && (Time.time - timeDelay >= 8))
        //{
        //    activateStep = "fifthConvo4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"What do you mean?\"");
        //}
        //if (activateStep == "fifthConvo4" && (Time.time - timeDelay >= 3))
        //{
        //    activateStep = "fifthConvo5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"It got extremely heavy-handed and preachy, and just straight up bizarre sometimes. The show became like a sketch show, with the crew ending up on some world coincidentally identical to some previous period in human history. Even Abraham Lincoln showed up!\"");
        //}
        //if (activateStep == "fifthConvo5" && (Time.time - timeDelay >= 14))
        //{
        //    activateStep = "fifthConvo6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Oh, come on, bro, all the 'Star Trek' shows just threw the crew into different scenarios like that.\"");
        //}
        //if (activateStep == "fifthConvo6" && (Time.time - timeDelay >= 8))
        //{
        //    activateStep = "fifthConvo7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"There's a fine line between 'themes' in the later series and 'reality' in the original. Roddenberry was a visionary and the original series was groundbreaking, but the show was a mess.\"");
        //}
        //if (activateStep == "fifthConvo7" && (Time.time - timeDelay >= 14))
        //{
        //    activateStep = "fifthConvo8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Yeah you're right! 'Star Trek' and 'The Visionaries' were both unjustly canceled!\"");
        //}
        //if (activateStep == "fifthConvo8" && (Time.time - timeDelay >= 6))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.OverwriteMainWindow("The remainder of this conversation takes a dark turn.\n\n\n\n\n\n\nPress ENTER to continue.");
        //}
        ////fifth convo with skip
        //if (activateStep == "fifthConvoSkip1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "fifthConvoSkip2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"It was just ahead of its time, bro.\"");
        //}
        //if (activateStep == "fifthConvoSkip2" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fifthConvoSkip3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Did you even watch the original series? Yes, it was certainly ahead of its time -- but it also got pretty 'out there.'\"");
        //}
        //if (activateStep == "fifthConvoSkip3" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fifthConvoSkip4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"What do you mean?\"");
        //}
        //if (activateStep == "fifthConvoSkip4" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fifthConvoSkip5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"It got extremely heavy-handed and preachy, and just straight up bizarre sometimes. The show became like a sketch show, with the crew ending up on some world coincidentally identical to some previous period in human history. Even Abraham Lincoln showed up!\"");
        //}
        //if (activateStep == "fifthConvoSkip5" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fifthConvoSkip6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Oh, come on, bro, all the 'Star Trek' shows just threw the crew into different scenarios like that.\"");
        //}
        //if (activateStep == "fifthConvoSkip6" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fifthConvoSkip7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"There's a fine line between 'themes' in the later series and 'reality' in the original. Roddenberry was a visionary and the original series was groundbreaking, but the show was a mess.\"");
        //}
        //if (activateStep == "fifthConvoSkip7" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fifthConvoSkip8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Yeah you're right! 'Star Trek' and 'The Visionaries' were both unjustly canceled!\"");
        //}
        //if (activateStep == "fifthConvoSkip8" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.AddToMainWindowWithLine("The remainder of this conversation takes a dark turn.\n\nPress ENTER to continue.");
        //}

        ////sixth convo with pauses
        //if (activateStep == "sixthConvo1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "sixthConvo2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"It had time travel! It had Borg! It had pew pew bang and a happy ending! I liked it!\"");
        //}
        //if (activateStep == "sixthConvo2" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "sixthConvo3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Bro, I'm just gonna slap those each out of the air in turn.\"");
        //}
        //if (activateStep == "sixthConvo3" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "sixthConvo4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"There are no constraints on the time travel, so that whole thing is just a disaster. It doesn't even matter if I could (and I could) say 'she should've done this or that.' Omnipotence and omnipresence simply cheapen the struggle.\"");
        //}
        //if (activateStep == "sixthConvo4" && (Time.time - timeDelay >= 7.5))
        //{
        //    activateStep = "sixthConvo5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindow("\n\n\"The 'Voyager' series treats the Borg wrong, and the finale is no exception. The borg are an unending army where a single ship can decimate the entire alpha quadrant, and Voyager just like blows some of them up. You ever take calculus, bro? Subtract 'a lot' from infinity and you still have at least one ship that can end the alpha quadrant whenever it wants.\"");
        //}
        //if (activateStep == "sixthConvo5" && (Time.time - timeDelay >= 7.5))
        //{
        //    activateStep = "sixthConvo6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindow("\n\n\"And your 'pew pew bang happy ending,' bro? The whole thing was about deciding between an act of self-interest and an ethical act of self-sacrifice -- mirroring the series premiere, mind you. But - unlike the premiere - Janeway just has her cake and eats it too, which is a poor ending to any story.\"");
        //}
        //if (activateStep == "sixthConvo6" && (Time.time - timeDelay >= 35))
        //{
        //    activateStep = "sixthConvo7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"... And 'Endgame' is the most rewatched episode of the series on Netflix.\"");
        //}
        //if (activateStep == "sixthConvo7" && (Time.time - timeDelay >= 6))
        //{
        //    activateStep = "sixthConvo8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Yeah and they'll probably release some Netflix-original movie as an extension just to please fans, too. In what world would something like that be any good?\"");
        //}
        //if (activateStep == "sixthConvo8" && (Time.time - timeDelay >= 10))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.OverwriteMainWindow("The remainder of this conversation takes a dark turn.\n\n\n\n\n\n\nPress ENTER to continue.");
        //}
        ////sixth convo with skip
        //if (activateStep == "sixthConvoSkip1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "sixthConvoSkip2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"It had time travel! It had Borg! It had pew pew bang and a happy ending! I liked it!\"");
        //}
        //if (activateStep == "sixthConvoSkip2" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "sixthConvoSkip3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Bro, I'm just gonna slap those each out of the air in turn.\"");
        //}
        //if (activateStep == "sixthConvoSkip3" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "sixthConvoSkip4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"There are no constraints on the time travel, so that whole thing is just a disaster. It doesn't even matter if I could (and I could) say 'she should've done this or that.' Omnipotence and omnipresence simply cheapen the struggle.\"");
        //}
        //if (activateStep == "sixthConvoSkip4" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "sixthConvoSkip5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindow("\n\n\"The 'Voyager' series treats the Borg wrong, and the finale is no exception. The borg are an unending army where a single ship can decimate the entire alpha quadrant, and Voyager just like blows some of them up. You ever take calculus, bro? Subtract 'a lot' from infinity and you still have at least one ship that can end the alpha quadrant whenever it wants.\"");
        //}
        //if (activateStep == "sixthConvoSkip5" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "sixthConvoSkip6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindow("\n\n\"And your 'pew pew bang happy ending,' bro? The whole thing was about deciding between an act of self-interest and an ethical act of self-sacrifice -- mirroring the series premiere, mind you. But - unlike the premiere - Janeway just has her cake and eats it too, which is a poor ending to any story.\"");
        //}
        //if (activateStep == "sixthConvoSkip6" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "sixthConvoSkip7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"... And 'Endgame' is the most rewatched episode of the series on Netflix.\"");
        //}
        //if (activateStep == "sixthConvoSkip7" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "sixthConvoSkip8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Yeah and they'll probably release some Netflix-original movie as an extension just to please fans, too. In what world would something like that be any good?\"");
        //}
        //if (activateStep == "sixthConvoSkip8" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.AddToMainWindowWithLine("The remainder of this conversation takes a dark turn.\n\nPress ENTER to continue.");
        //}

        ////seventh convo with pauses
        //if (activateStep == "seventhConvo1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "seventhConvo2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"...There's no way that's true, man.\"");
        //}
        //if (activateStep == "seventhConvo2" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "seventhConvo3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"It totally is, bro, I'm tellin' you! You pay him and just 'vwip!' he'll get you outta here! Anywhere you wanna go and any kind of life you want!\"");
        //}
        //if (activateStep == "seventhConvo3" && (Time.time - timeDelay >= 10))
        //{
        //    activateStep = "seventhConvo4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Oh yeah? Then why hasn't everyone done it? Why haven't you?\"");
        //}
        //if (activateStep == "seventhConvo4" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "seventhConvo5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Bro, you're not getting me. When I say 'a lot,' I mean *a lot.*\"");
        //}
        //if (activateStep == "seventhConvo5" && (Time.time - timeDelay >= 7))
        //{
        //    activateStep = "seventhConvo6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"What kind of business plan is that, then?\"");
        //}
        //if (activateStep == "seventhConvo6" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "seventhConvo7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"I don't know, man. I think it's what he used to charge and he just keeps the same price out of principle or whatever.\"");
        //}
        //if (activateStep == "seventhConvo7" && (Time.time - timeDelay >= 10))
        //{
        //    activateStep = "seventhConvo8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"So, what, do you just drag a huge bag of cash up there and say 'here you go' or something?\"");
        //}
        //if (activateStep == "seventhConvo8" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "seventhConvo9";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Nah, dude, it's like some super spy stuff with like a secret passcode. It's weird, but you say, 'I need a new dust filter for my Hoover Maxextract Pressurepro Model 60 - can you help me with that?'\"");
        //}
        //if (activateStep == "seventhConvo9" && (Time.time - timeDelay >= 13))
        //{
        //    activateStep = "seventhConvo10";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Woah. Feel like I'd need to write that down. What was it again?\"");
        //}
        //if (activateStep == "seventhConvo10" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "seventhConvo11";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"'I need a new dust filter for my Hoover Maxextract Pressurepro Model 60 - can you help me with that?'\"");
        //}
        //if (activateStep == "seventhConvo11" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "seventhConvo12";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindow("\n\nPress ENTER to continue.");
        //}
        //if (activateStep == "seventhConvo12" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "seventhConvo13";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"So, like, do you think anyone will ever do it?\"");
        //}
        //if (activateStep == "seventhConvo13" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "seventhConvo14";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"If there's even enough money down here at all, you'd have to pool everyone's stash together just to get one guy out. And I really don't think anyone's gonna go for that.\"");
        //}
        //if (activateStep == "seventhConvo14" && (Time.time - timeDelay >= 11))
        //{
        //    activateStep = "seventhConvo15";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"They'll probably need your whole sack of goodies, too!\"");
        //}
        //if (activateStep == "seventhConvo15" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.OverwriteMainWindow("The remainder of this conversation takes a dark turn.\n\n\n\n\n\n\nPress ENTER to continue.");
        //}
        ////seventh convo with skip
        //if (activateStep == "seventhConvoSkip1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "seventhConvoSkip2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"...There's no way that's true, man.\"");
        //}
        //if (activateStep == "seventhConvoSkip2" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "seventhConvoSkip3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"It totally is, bro, I'm tellin' you! You pay him and just 'vwip!' he'll get you outta here! Anywhere you wanna go and any kind of life you want!\"");
        //}
        //if (activateStep == "seventhConvoSkip3" && (Time.time - timeDelay >= 10))
        //{
        //    activateStep = "seventhConvoSkip4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Oh yeah? Then why hasn't everyone done it? Why haven't you?\"");
        //}
        //if (activateStep == "seventhConvoSkip4" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "seventhConvoSkip5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Bro, you're not getting me. When I say 'a lot,' I mean *a lot.*\"");
        //}
        //if (activateStep == "seventhConvoSkip5" && (Time.time - timeDelay >= 7))
        //{
        //    activateStep = "seventhConvoSkip6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"What kind of business plan is that, then?\"");
        //}
        //if (activateStep == "seventhConvoSkip6" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "seventhConvoSkip7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"I don't know, man. I think it's what he used to charge and he just keeps the same price out of principle or whatever.\"");
        //}
        //if (activateStep == "seventhConvoSkip7" && (Time.time - timeDelay >= 10))
        //{
        //    activateStep = "seventhConvoSkip8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"So, what, do you just drag a huge bag of cash up there and say 'here you go' or something?\"");
        //}
        //if (activateStep == "seventhConvoSkip8" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "seventhConvoSkip9";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Nah, dude, it's like some super spy stuff with like a secret passcode. It's weird, but you say, 'I need a new dust filter for my Hoover Maxextract Pressurepro Model 60 - can you help me with that?'\"");
        //}
        //if (activateStep == "seventhConvoSkip9" && (Time.time - timeDelay >= 13))
        //{
        //    activateStep = "seventhConvoSkip10";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Woah. Feel like I'd need to write that down. What was it again?\"");
        //}
        //if (activateStep == "seventhConvoSkip10" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "seventhConvoSkip12";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"'I need a new dust filter for my Hoover Maxextract Pressurepro Model 60 - can you help me with that?'\"");
        //}
        //if (activateStep == "seventhConvoSkip12" && (Time.time - timeDelay >= .25))
        //{
        //    activateStep = "seventhConvoSkip13";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"So, like, do you think anyone will ever do it?\"");
        //}
        //if (activateStep == "seventhConvoSkip13" && (Time.time - timeDelay >= 4))
        //{
        //    activateStep = "seventhConvoSkip14";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"If there's even enough money down here at all, you'd have to pool everyone's stash together just to get one guy out. And I really don't think anyone's gonna go for that.\"");
        //}
        //if (activateStep == "seventhConvoSkip14" && (Time.time - timeDelay >= 11))
        //{
        //    activateStep = "seventhConvoSkip15";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"They'll probably need your whole sack of goodies, too!\"");
        //}
        //if (activateStep == "seventhConvoSkip15" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.AddToMainWindowWithLine("The remainder of this conversation takes a dark turn.\n\nPress ENTER to continue.");
        //}

        ////joke convos

        ////first joke convo
        //if (activateStep == "firstJoke1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "firstJoke2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"PICAAAAAAARD!\"");
        //}
        //if (activateStep == "firstJoke2" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "firstJoke3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"DATA!\"");
        //}
        //if (activateStep == "firstJoke3" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "firstJoke4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Q!\"");
        //}
        //if (activateStep == "firstJoke4" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "firstJoke5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"...THE DOCTOR!\"");
        //}
        //if (activateStep == "firstJoke5" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "firstJoke6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Janeway.\"");
        //}
        //if (activateStep == "firstJoke6" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "firstJoke7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Sweeting?\"");
        //}
        //if (activateStep == "firstJoke7" && (Time.time - timeDelay >= 1.5))
        //{
        //    activateStep = "firstJoke8";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("");
        //}
        //if (activateStep == "firstJoke8" && (Time.time - timeDelay >= 1.5))
        //{
        //    activateStep = "firstJoke9";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"KHAAAAAAAAN!\"\n\n\n\n\n\n\n");
        //}
        //if (activateStep == "firstJoke9" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "firstJoke10";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\n\nPress ENTER to continue.");
        //}
        //if (activateStep == "firstJoke10" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "firstJoke11";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("");
        //}
        //if (activateStep == "firstJoke11" && (Time.time - timeDelay >= 1.5))
        //{
        //    activateStep = "firstJoke12";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"KHAAAAAAAAAAAAAAAAAAAAAAAN!\"\n\n\n\n\n\n\n");
        //}
        //if (activateStep == "firstJoke12" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "";
        //    controller.DisplayRoomText();
        //    controller.enterToContinue = false;
        //    controller.inputBox.SetActive(true);
        //    controller.textInput.inputField.ActivateInputField();
        //    controller.textInput.inputField.text = null;
        //}
        ////second joke convo
        //if (activateStep == "secondJoke1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "secondJoke2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"Come on, Fox! Let's kick some ass!\"");
        //}
        //if (activateStep == "secondJoke2" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "secondJoke3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Shit! He's right behind me!\"");
        //}
        //if (activateStep == "secondJoke3" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "secondJoke4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Hey, idiot! I'm on your side!\"");
        //}
        //if (activateStep == "secondJoke4" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "secondJoke5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Shit! We were so close to Venom.\"");
        //}
        //if (activateStep == "secondJoke5" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "secondJoke6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Jesus Christ! What is that?\"");
        //}
        //if (activateStep == "secondJoke6" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "secondJoke7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Are you gonna listen to that --\"");
        //}
        //if (activateStep == "secondJoke7" && (Time.time - timeDelay >= 1.5))
        //{
        //    activateStep = "secondJoke8";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("");
        //}
        //if (activateStep == "secondJoke8" && (Time.time - timeDelay >= 1.5))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.OverwriteMainWindow("\"Do a barrel roll!\"\n\nPress ENTER to continue.\n\n\n\n\n");
        //}
        ////third joke convo
        //if (activateStep == "thirdJoke1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "thirdJoke2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"You know, Heisenberg has some pretty badass lines.\"");
        //}
        //if (activateStep == "thirdJoke2" && (Time.time - timeDelay >= 3))
        //{
        //    activateStep = "thirdJoke3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Yeah? Like what?\"");
        //}
        //if (activateStep == "thirdJoke3" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "thirdJoke4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"There are so many, bro! But I bet the best ones made their way into this game.\"");
        //}
        //if (activateStep == "thirdJoke4" && (Time.time - timeDelay >= 3.5))
        //{
        //    activateStep = "thirdJoke5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Why would they do that?\"");
        //}
        //if (activateStep == "thirdJoke5" && (Time.time - timeDelay >= 1.5))
        //{
        //    activateStep = "thirdJoke6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"If it was me, I'd make debug codes out of them.\"");
        //}
        //if (activateStep == "thirdJoke6" && (Time.time - timeDelay >= 3))
        //{
        //    activateStep = "thirdJoke7";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("");
        //}
        //if (activateStep == "thirdJoke7" && (Time.time - timeDelay >= 1.5))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.OverwriteMainWindow("Eh? ehhhhh?\n\nPress ENTER to continue.\n\n\n\n\n");
        //}
        ////fourth joke convo
        //if (activateStep == "fourthJoke1" && (Time.time - timeDelay >= .25) && Input.GetKeyDown(KeyCode.Return))
        //{
        //    activateStep = "fourthJoke2";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("\"There are 362 references to retro games and miscellaneous pop culture in this game.\"");
        //}
        //if (activateStep == "fourthJoke2" && (Time.time - timeDelay >= 5))
        //{
        //    activateStep = "fourthJoke3";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Wow! Is there really?\"");
        //}
        //if (activateStep == "fourthJoke3" && (Time.time - timeDelay >= 1.5))
        //{
        //    activateStep = "fourthJoke4";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Nah. Well I mean maybe -- probably woulda been smart of the designer to keep count as he made them.\"");
        //}
        //if (activateStep == "fourthJoke4" && (Time.time - timeDelay >= 6))
        //{
        //    activateStep = "fourthJoke5";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"You think anyone will ever count them?\"");
        //}
        //if (activateStep == "fourthJoke5" && (Time.time - timeDelay >= 2.5))
        //{
        //    activateStep = "fourthJoke6";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"I don't know, bro. Maybe one day someone playing it will count, but then you'd never know if you got every single one. There're references to James Bond and even Seinfeld in there somewhere.\"");
        //}
        //if (activateStep == "fourthJoke6" && (Time.time - timeDelay >= 10))
        //{
        //    activateStep = "fourthJoke7";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Gotta catch 'em all!\"");
        //}
        //if (activateStep == "fourthJoke7" && (Time.time - timeDelay >= 1.5))
        //{
        //    activateStep = "fourthJoke8";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"Actually there aren't any Pokemon references.\"");
        //}
        //if (activateStep == "fourthJoke8" && (Time.time - timeDelay >= 3))
        //{
        //    activateStep = "fourthJoke9";
        //    timeDelay = Time.time;
        //    controller.AddToMainWindowWithLine("\"...There is now.\"");
        //}
        //if (activateStep == "fourthJoke9" && (Time.time - timeDelay >= 1.5))
        //{
        //    activateStep = "fourthJoke10";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("");
        //}
        //if (activateStep == "fourthJoke10" && (Time.time - timeDelay >= 1))
        //{
        //    activateStep = "fourthJoke11";
        //    timeDelay = Time.time;
        //    controller.OverwriteMainWindow("Ba-doom shhh!\n\n\n\n\n\n\n");
        //}
        //if (activateStep == "fourthJoke11" && (Time.time - timeDelay >= .5))
        //{
        //    activateStep = "";
        //    controller.textInput.keyPressDelay = Time.time;
        //    controller.enterToContinue = true;
        //    controller.OverwriteMainWindow("Ba-doom shhh!\n\nPress ENTER to continue.\n\n\n\n\n");
        //}
    }
}
