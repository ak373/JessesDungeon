using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class NPCTalk : MonoBehaviour
{
    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
    GameController controller;
    string userInput;
    bool endCoCoroutine = false;
    List<Item> inventory = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
    }
    public void SnatchInput(string fromTextInput)
    {
        userInput = fromTextInput;

        if (controller.currentActiveInput == "badger")
        {
            controller.textInput.textIsGood = true;
            if (fromTextInput == "talk") { StartCoroutine(BadgerTalk()); }
            else if (fromTextInput == "rest") { StartCoroutine(BadgerRest()); }
            else if (fromTextInput == "exit") { BadgerExit(); }
            else { controller.AddToMainWindow("\n\n\"Hey, yeah -- what?\""); }
        }
        else if (controller.currentActiveInput == "skinny pete")
        {
            controller.textInput.textIsGood = true;
            if (fromTextInput == "talk") { StartCoroutine(SkinnyPeteTalk()); }
            else if (fromTextInput == "shop") { StartCoroutine(SkinnyPeteShop()); }
            else if (fromTextInput == "exit") { SkinnyPeteExit(); }
            else { controller.AddToMainWindow("\n\n\"Huh? What's that now?\""); }
        }
        else if (controller.currentActiveInput == "geoff")
        {
            controller.textInput.textIsGood = true;
            if (fromTextInput == "talk") { StartCoroutine(GeoffTalk()); }
            else if (fromTextInput == "save") { StartCoroutine(SkinnyPeteShop()); }
            else if (fromTextInput == "exit") { GeoffExit(); }
            else { controller.AddToMainWindow("\n\n\"Get it together. Or I shall fong you.\""); }
        }
    }

    public void BadgerMain()
    {
        controller.textInput.textIsGood = true;
        if (!controller.secondQuestActive) { controller.OverwriteMainWindow("<size=40><b>Badger</b></size>\n-------------------------------------\n\n\n\"Greetings, good master! Welcome to the inn of Jesse's Dungeon!\"\n\n\nTalk\nRest\n\nExit"); }
        else { controller.OverwriteMainWindow("<size=40><b>Badger</b></size>\n-------------------------------------\n\n\n\"Greetings, good mistress! Welcome to the inn of Jesse's Dungeon!\"\n\n\nTalk\nRest\n\nExit"); }
    }
    public IEnumerator BadgerTalk()
    {
        controller.textInput.textIsGood = true;
        controller.LockInputForEnter();
        if (!controller.secondQuestActive) { controller.OverwriteMainWindow("<size=40><b>Badger</b></size>\n-------------------------------------\n\n\n\"Sup, guy? Be careful when you're walkin' around - especially at the body pile west of town. I'm not sure if you can catch chlamydia from a toilet seat or not, but you definitely can there.\"\n\n\nPress ENTER to continue."); }
        else { controller.OverwriteMainWindow("<size=40><b>Badger</b></size>\n-------------------------------------\n\n\n\"Sup, gal? Be careful when you're walkin' around - this place is crawlin' with creeps that I'm sure would love to take you down. I'm not sure if you can catch chlamydia from a toilet seat or not, but you definitely can out there.\"\n\n\nPress ENTER to continue."); }
        while (true)
        {
            yield return new WaitForSeconds(.25f);
            break;
        }
        yield return new WaitUntil(controller.EnterPressed);
        BadgerMain();
    }
    public IEnumerator BadgerRest()
    {
        controller.textInput.textIsGood = true;
        controller.currentActiveInput = "yesno";
        if (!controller.secondQuestActive) { controller.OverwriteMainWindow("<size=40><b>Badger</b></size>\n-------------------------------------\n\n\n\"Yeah, man - get some rest. Is 3 crystals OK?\"\n\n\n[Yes], let's get some rest.\n[No]! We must press on!"); }
        else { controller.OverwriteMainWindow("<size=40><b>Badger</b></size>\n-------------------------------------\n\n\n\"Yeah, girl - get some rest. Is 3 crystals OK?\"\n\n\n[Yes], let's get some rest.\n[No]! We must press on!"); }
        while (true)
        {
            controller.userInput = null;
            while (true)
            {
                yield return new WaitForSeconds(.25f);
                break;
            }
            yield return new WaitUntil(controller.InputGiven);
            controller.textInput.textIsGood = true;
            if (userInput == "yes" || userInput == "no")
            {
                controller.LockInputForEnter();
                if (userInput == "yes")
                {
                    if (controller.ego.blueCrystals >= 3)
                    {
                        controller.AddToMainWindowWithLine("\"Don't worry for a second -- me and Pete got your back.\"\n\n\nPress ENTER to continue.");
                        while (true)
                        {
                            yield return new WaitForSeconds(.25f);
                            break;
                        }
                        yield return new WaitUntil(controller.EnterPressed);
                        controller.LockInputForEnter();
                        controller.AddToMainWindowWithLine("You stretch out on the plank of wood and go to sleep.\n\n\nPress ENTER to continue.");
                        while (true)
                        {
                            yield return new WaitForSeconds(.25f);
                            break;
                        }
                        yield return new WaitUntil(controller.EnterPressed);
                        controller.ego.blueCrystals = controller.ego.blueCrystals - 3;
                        controller.ego.allStats[0].value = controller.ego.allStats[2].value + controller.ego.allStats[2].effectValue;
                        controller.LockInputForEnter();
                        controller.AddToMainWindowWithLine("\"Don't you look like a million bucks? Now - up and at them!\"\n\n\nPress ENTER to continue.");
                        while (true)
                        {
                            yield return new WaitForSeconds(.25f);
                            break;
                        }
                        yield return new WaitUntil(controller.EnterPressed);
                        controller.currentActiveInput = "main";
                    }
                    else
                    {
                        controller.AddToMainWindowWithLine("\"You're looking a little light in the pockets -- I need to eat too... you understand, right? I'll be here when you've got a few more to spend.\"\n\n\nPress ENTER to continue.");
                        while (true)
                        {
                            yield return new WaitForSeconds(.25f);
                            break;
                        }
                        yield return new WaitUntil(controller.EnterPressed);
                        controller.currentActiveInput = "badger";
                    }
                }
                else if (userInput == "no")
                {
                    controller.AddToMainWindowWithLine("\"All right, that's cool... Maybe next time.\"\n\n\nPress ENTER to continue.");
                    while (true)
                    {
                        yield return new WaitForSeconds(.25f);
                        break;
                    }
                    yield return new WaitUntil(controller.EnterPressed);
                    controller.currentActiveInput = "badger";
                }
                userInput = "";
                break;
            }
            else { controller.AddToMainWindow("\n\n\"Hey, yeah -- what?\""); }
        }
        if (controller.currentActiveInput == "main") { controller.DisplayRoomText(); }
        else { BadgerMain(); }        
    }
    public void BadgerExit()
    {
        controller.textInput.textIsGood = true;
        controller.LockInputForEnter();
        controller.OverwriteMainWindow("<size=40><b>Badger</b></size>\n-------------------------------------\n\n\n\"Don't over-do it, yeah?\"\n\n\nPress ENTER to continue.");
        controller.currentActiveInput = "main";
    }
    public void SkinnyPeteMain()
    {
        controller.textInput.textIsGood = true;
        controller.OverwriteMainWindow("<size=40><b>Skinny Pete</b></size>\n-------------------------------------\n\n\n\"Whoa! What can I do for ya?\"\n\n\nTalk\nShop\n\nExit");
    }
    public IEnumerator SkinnyPeteTalk()
    {
        controller.textInput.textIsGood = true;
        controller.LockInputForEnter();
        if (false) { }
        //if (converseCount == 7 && readLogbook == true && tookPeteCash == false)
        //else if (lydiaQuestGiven == true && peteQuestGiven == false)
        //else if (peteQuestGiven == true && peteQuestCompleted == true)
        //else if (peteQuestGiven == true && undroppables.Contains(mezcal))
        else { controller.OverwriteMainWindow("<size=40><b>Skinny Pete</b></size>\n-------------------------------------\n\n\n\"What kind of adventures have you been on? Damn I'm so jealous!\"\n\n\nPress ENTER to continue."); }
        while (true)
        {
            yield return new WaitForSeconds(.25f);
            break;
        }
        yield return new WaitUntil(controller.EnterPressed);
        SkinnyPeteMain();
    }
    public IEnumerator SkinnyPeteShop()
    {
        controller.textInput.textIsGood = true;
        controller.currentActiveInput = "yesno";
        if (!controller.secondQuestActive) { controller.OverwriteMainWindow("<size=40><b>Skinny Pete</b></size>\n-------------------------------------\n\n\n\"All right! We buyin' or sellin', brother?!\"\n\n\n[Buy]! Accumulate! Congregate!\n[Sell]! Liquidate! Consolidate!\n\n[Exit]! Amalgamation complete!"); }
        else { controller.OverwriteMainWindow("<size=40><b>Skinny Pete</b></size>\n-------------------------------------\n\n\n\"All right! We buyin' or sellin', sister?!\"\n\n\n[Buy]! Accumulate! Congregate!\n[Sell]! Liquidate! Consolidate!\n\n[Exit]! Amalgamation complete!"); }
        
        while (true)
        {
            controller.userInput = null;
            while (true)
            {
                yield return new WaitForSeconds(.25f);
                break;
            }
            yield return new WaitUntil(controller.InputGiven);
            controller.textInput.textIsGood = true;
            if (userInput == "buy" || userInput == "sell" || userInput == "exit")
            {
                if (userInput == "buy")
                {
                    List<Item> unlockedItems = new List<Item>();
                    for (int i = 0; i < controller.registerObjects.allItems.Length; i++) { if (controller.registerObjects.allItems[i].unlocked) { unlockedItems.Add(controller.registerObjects.allItems[i]); } }
                    Item[] itemStock = unlockedItems.ToArray();

                    
                    string weapons = "";
                    string armors = "";
                    string shields = "";
                    for (int i = 0; i < itemStock.Length; i++)
                    {
                        string pricePadding = "";
                        if (itemStock[i].price < 10) { pricePadding = "    "; }//4 spaces
                        else if (itemStock[i].price < 100) { pricePadding = "   "; }//3 spaces
                        else if (itemStock[i].price < 1000) { pricePadding = "  "; }//2 spaces
                        else if (itemStock[i].price < 10000) { pricePadding = " "; }//1 space

                        if (itemStock[i] is Weapon) { weapons += itemStock[i].price + pricePadding + itemStock[i].nome + "\n"; }
                        if (itemStock[i] is Armor) { armors += itemStock[i].price + pricePadding + itemStock[i].nome + "\n"; }
                        if (itemStock[i] is Shield) { shields += itemStock[i].price + pricePadding + itemStock[i].nome + "\n"; }
                    }
                    if (weapons == "") { weapons = "-\n"; }
                    if (armors == "") { armors = "-\n"; }
                    if (shields == "") { shields = "-\n"; }
                    string stock = "<size=40><u><b>Weapons</b></u></size>\n" + weapons;
                    stock += "<size=40><u><b>Armor</b></u></size>\n" + armors;
                    stock += "<size=40><u><b>Shields</b></u></size>\n" + shields;

                    controller.OverwriteMainWindow(stock + "\nExit\n\n\n\"Anything catch your eye?\"");

                    controller.userInput = null;
                    while (true)
                    {
                        yield return new WaitForSeconds(.25f);
                        break;
                    }
                    yield return new WaitUntil(controller.InputGiven);
                    controller.textInput.textIsGood = true;
                    if (userInput == "buy") { userInput = ""; }

                    for (int i = 0; i < itemStock.Length; i++)
                    {
                        if (userInput == itemStock[i].nome)
                        {
                            controller.LockInputForEnter();
                            if (controller.ego.blueCrystals >= itemStock[i].price)
                            {
                                controller.ego.blueCrystals -= itemStock[i].price;
                                controller.interactableItems.inventory.Add(itemStock[i]);
                                controller.AddToMainWindowWithLine("\"Right on!\"\n\n\nPress ENTER to continue.");
                                while (true)
                                {
                                    yield return new WaitForSeconds(.25f);
                                    break;
                                }
                                yield return new WaitUntil(controller.EnterPressed);
                                userInput = "buy";
                            }
                            else
                            {
                                if (!controller.secondQuestActive) { controller.AddToMainWindowWithLine("\"Shoot I don't think you got enough cash! Maybe next time, all right, bro?\"\n\n\nPress ENTER to continue."); }
                                else { controller.AddToMainWindowWithLine("\"Shoot I don't think you got enough cash! Maybe next time, all right, sis?\"\n\n\nPress ENTER to continue."); }
                                while (true)
                                {
                                    yield return new WaitForSeconds(.25f);
                                    break;
                                }
                                yield return new WaitUntil(controller.EnterPressed);
                                userInput = "buy";
                            }
                            break;
                        }
                    }
                    if (userInput == "exit")
                    {
                        controller.LockInputForEnter();
                        userInput = null;
                        if (!controller.secondQuestActive) { controller.AddToMainWindowWithLine("\"Cool, man.\"\n\n\nPress ENTER to continue."); }
                        else { controller.AddToMainWindowWithLine("\"Cool, girl.\"\n\n\nPress ENTER to continue."); }
                        while (true)
                        {
                            yield return new WaitForSeconds(.25f);
                            break;
                        }
                        yield return new WaitUntil(controller.EnterPressed);
                        //controller.currentActiveInput = "skinny pete";
                        StartCoroutine(SkinnyPeteShop());
                        yield return new WaitUntil(EndCoCoroutine);
                        endCoCoroutine = false;
                        break;
                    }
                    else if (userInput != "buy") { controller.AddToMainWindow("\n\n\"Huh? What's that now?\""); }
                }
                else if (userInput == "sell")
                {
                    inventory = controller.interactableItems.inventory;
                    List<Item> alreadyListed = new List<Item>();
                    string weapons = "";
                    string armors = "";
                    string shields = "";

                    for (int i = 0; i < inventory.Count; i++)
                    {
                        if (alreadyListed.Contains(inventory[i])) { continue; }
                        else
                        {
                            int counter = 0;
                            for (int j = i; j < inventory.Count; j++)
                            {
                                if (inventory[i] == inventory[j]) { counter++; }
                            }
                            alreadyListed.Add(inventory[i]);
                            string total = counter.ToString();
                            string pricePadding = "";
                            if (inventory[i].price < 10) { pricePadding = "    "; }//4 spaces
                            else if (inventory[i].price < 100) { pricePadding = "   "; }//3 spaces
                            else if (inventory[i].price < 1000) { pricePadding = "  "; }//2 spaces
                            else if (inventory[i].price < 10000) { pricePadding = " "; }//1 space

                            if (inventory[i] is Weapon) { weapons += $"{inventory[i].price / 4}{pricePadding}{myTI.ToTitleCase(inventory[i].nome)} (x{total})\n"; }
                            if (inventory[i] is Armor) { armors += $"{inventory[i].price / 4}{pricePadding}{myTI.ToTitleCase(inventory[i].nome)} (x{total})\n"; }
                            if (inventory[i] is Shield) { shields += $"{inventory[i].price / 4}{pricePadding}{myTI.ToTitleCase(inventory[i].nome)} (x{total})\n"; }
                        }
                    }
                    if (weapons == "") { weapons = "-\n"; }
                    if (armors == "") { armors = "-\n"; }
                    if (shields == "") { shields = "-\n"; }
                    string stock = "<size=40><u><b>Weapons</b></u></size>\n" + weapons;
                    stock += "<size=40><u><b>Armor</b></u></size>\n" + armors;
                    stock += "<size=40><u><b>Shields</b></u></size>\n" + shields;

                    controller.OverwriteMainWindow(stock + "\nExit\n\n\n\"Whatcha got for me?\"");

                    controller.userInput = null;
                    while (true)
                    {
                        yield return new WaitForSeconds(.25f);
                        break;
                    }
                    yield return new WaitUntil(controller.InputGiven);
                    controller.textInput.textIsGood = true;
                    if (userInput == "sell") { userInput = ""; }

                    for (int i = 0; i < inventory.Count; i++)
                    {
                        if (userInput == inventory[i].nome)
                        {
                            controller.LockInputForEnter();
                            controller.ego.blueCrystals += inventory[i].price;
                            controller.interactableItems.inventory.Remove(inventory[i]);
                            if (!controller.secondQuestActive) { controller.AddToMainWindowWithLine("\"Hey man good doing business with you.\"\n\n\nPress ENTER to continue."); }
                            else { controller.AddToMainWindowWithLine("\"Hey girl good doing business with you.\"\n\n\nPress ENTER to continue."); ; }                            
                            while (true)
                            {
                                yield return new WaitForSeconds(.25f);
                                break;
                            }
                            yield return new WaitUntil(controller.EnterPressed);
                            userInput = "sell";
                            break;
                        }
                    }
                    if (userInput == "exit")
                    {
                        controller.LockInputForEnter();
                        userInput = null;
                        if (!controller.secondQuestActive) { controller.AddToMainWindowWithLine("\"Cool, man.\"\n\n\nPress ENTER to continue."); }
                        else { controller.AddToMainWindowWithLine("\"Cool, girl.\"\n\n\nPress ENTER to continue."); }
                        while (true)
                        {
                            yield return new WaitForSeconds(.25f);
                            break;
                        }
                        yield return new WaitUntil(controller.EnterPressed);
                        //controller.currentActiveInput = "skinny pete";
                        StartCoroutine(SkinnyPeteShop());
                        yield return new WaitUntil(EndCoCoroutine);
                        endCoCoroutine = false;
                        break;
                    }
                    else if (userInput != "sell") { controller.AddToMainWindow("\n\n\"Huh? What's that now?\""); }
                }
                else if (userInput == "exit")
                {
                    controller.LockInputForEnter();
                    userInput = null;
                    if (!controller.secondQuestActive) { controller.AddToMainWindowWithLine("\"Cool, man.\"\n\n\nPress ENTER to continue."); }
                    else { controller.AddToMainWindowWithLine("\"Cool, girl.\"\n\n\nPress ENTER to continue."); }
                    while (true)
                    {
                        yield return new WaitForSeconds(.25f);
                        break;
                    }
                    yield return new WaitUntil(controller.EnterPressed);
                    endCoCoroutine = true;
                    controller.currentActiveInput = "skinny pete";
                    break;
                }
            }
            else { controller.AddToMainWindow("\n\n\"Huh? What's that now?\""); }
        }
        SkinnyPeteMain();
    }
    public void SkinnyPeteExit()
    {
        controller.textInput.textIsGood = true;
        controller.LockInputForEnter();
        if (!controller.secondQuestActive) { controller.OverwriteMainWindow("<size=40><b>Skinny Pete</b></size>\n-------------------------------------\n\n\n\"Yo, man - I know you gotta be careful and whatnot, but go bring me back some goodies, yeah?\"\n\n\nPress ENTER to continue."); }
        else if (controller.secondQuestActive) { controller.OverwriteMainWindow("<size=40><b>Skinny Pete</b></size>\n-------------------------------------\n\n\n\"Yo, girl - I know you gotta be careful and whatnot, but go bring me back some goodies, yeah?\"\n\n\nPress ENTER to continue."); }
        controller.currentActiveInput = "main";
    }
    public void GeoffMain()
    {
        controller.textInput.textIsGood = true;
        controller.OverwriteMainWindow("<size=40><b>Geoff</b></size>\n-------------------------------------\n\n\n\"Hello, my friend! Stay a while and listen!\"\n\n\nTalk\nSave\n\nExit");
    }
    public IEnumerator GeoffTalk()
    {
        controller.textInput.textIsGood = true;
        controller.LockInputForEnter();
        if (!controller.secondQuestActive) { controller.OverwriteMainWindow("<size=40><b>Geoff</b></size>\n-------------------------------------\n\n\n\"I look forward to writing of your exploits! A strapping lad such as yourself ought to make for a great story.\"\n\n\nPress ENTER to continue."); }
        else { controller.OverwriteMainWindow("<size=40><b>Geoff</b></size>\n-------------------------------------\n\n\n\"I look forward to writing of your exploits! A gorgeous and daring dame like you ought to make for a great story.\"\n\n\nPress ENTER to continue."); }
        while (true)
        {
            yield return new WaitForSeconds(.25f);
            break;
        }
        yield return new WaitUntil(controller.EnterPressed);
        GeoffMain();
    }
    public void GeoffExit()
    {
        controller.textInput.textIsGood = true;
        controller.LockInputForEnter();
        controller.OverwriteMainWindow("<size=40><b>Geoff</b></size>\n-------------------------------------\n\n\n\"That's it! Go on out there and live! Experience is power, and power will bring you to new levels you've never imagined!\"\n\n\nPress ENTER to continue.");
        controller.currentActiveInput = "main";
    }


    bool EndCoCoroutine() { return endCoCoroutine; }

    // Update is called once per frame
    void Update()
    {
        
    }
}
