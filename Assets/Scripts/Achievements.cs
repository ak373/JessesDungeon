using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Achievements : MonoBehaviour
{
    List<Deed> doneDeeds = new List<Deed>();
    List<Deed> toDoDeeds = new List<Deed>();
    public Deed[] allDeeds;
    string doneDeedsDisplayed = "";
    string toDoDeedsDisplayed = "";

    public GameObject deedpopUpWindow, deedWhiteScreen, deedGreyScreen;
    public TMP_Text deedTitle, deedBlurb;
    public TMP_FontAsset deedDescriptionFont;
    public TMP_FontAsset originalFont;
    //public Animator fadeIn, fadeOut;

    GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController>();
    }
    void Start()
    {
        doneDeeds.Clear();
        toDoDeeds.Clear();
        for (int i = 0; i < allDeeds.Length; i++)
        {
            if (allDeeds[i].achieved) { doneDeeds.Add(allDeeds[i]); }
            else { toDoDeeds.Add(allDeeds[i]); }
        }
    }
    public void SnatchInput(string fromTextInput)
    {
        if (fromTextInput == "achievements" && controller.currentActiveInput == "inventory")
        {
            controller.textInput.textIsGood = true;
            InitiateDisplayAchievements();
        }
    }
    public void InitiateDisplayAchievements()
    {
        controller.interactableItems.inventoryStats.SetActive(false);
        doneDeedsDisplayed = "";
        toDoDeedsDisplayed = "";
        for (int i = 0; i < doneDeeds.Count; i++) { doneDeedsDisplayed += $"[{doneDeeds[i].title}]\n"; }
        for (int i = 0; i < toDoDeeds.Count; i++)
        {
            if (allDeeds[1].achieved) { toDoDeedsDisplayed += $"[{toDoDeeds[i].title}]\n"; }
            else { toDoDeedsDisplayed += "<color=#AFAFAF>????</color>\n"; }            
        }
        if (doneDeeds.Count == 0) { doneDeedsDisplayed = "You've achieved nothing. Step up your game, hero."; }
        if (toDoDeeds.Count == 0) { toDoDeedsDisplayed = "You've done it all! Congratulations! You're really a hero!"; }
        if (doneDeeds.Count < 6)
        {
            int difference = 5 - doneDeeds.Count;
            for (int i = 0; i < difference; i++) { doneDeedsDisplayed += "\n"; }
        }
        if (toDoDeeds.Count < 6)
        {
            int difference = 5 - toDoDeeds.Count;
            for (int i = 0; i < difference; i++) { toDoDeedsDisplayed += "\n"; }
        }
        StartCoroutine(DisplayDoneAchievements());
    }
    public IEnumerator DisplayDoneAchievements()
    {
        controller.escToContinue = false;
        controller.currentActiveInput = "yesno";
        controller.OverwriteMainWindow($"<size=40><b>Done Deeds</size></b>\n\n{doneDeedsDisplayed}\n\n-------------------------------------\n[Deeds] to do\n[Erase] all deeds\n\nPress ESC to return.");
        while (true)
        {
            controller.userInput = null;
            while (true)
            {
                yield return new WaitForSeconds(.25f);
                break;
            }
            yield return new WaitUntil(controller.InputGivenOrEscPressed);
            if (controller.userInput == null)
            {
                controller.interactableItems.inventoryStats.SetActive(true);
                controller.interactableItems.DisplayInventory();
                controller.textInput.inputField.ActivateInputField();
                controller.textInput.inputField.text = null;
                while (true)
                {
                    yield return new WaitForSeconds(.25f);
                    break;
                }
                controller.escToContinue = true;
                controller.currentActiveInput = "inventory";
                break;
            }
            else
            {
                Deed inspectDeed = null;
                for (int i = 0; i < doneDeeds.Count; i++) { if (controller.userInput == doneDeeds[i].title.ToLower()) { inspectDeed = doneDeeds[i]; } }

                if (controller.userInput == "deeds" || controller.userInput == "erase")
                {
                    if (controller.userInput == "deeds") { StartCoroutine(DisplayToDoAchievements()); }
                    else if (controller.userInput == "erase") { StartCoroutine(EraseDeeds()); }
                    controller.userInput = null;
                    break;
                }
                else if (inspectDeed != null)
                {
                    controller.inputBox.SetActive(false);
                    controller.OpenPopUpWindow(inspectDeed.title, "", inspectDeed.description, "", "", "", "", "Press ESC to return");
                    originalFont = controller.popUpMessage.font;
                    controller.popUpMessage.font = deedDescriptionFont;
                    yield return new WaitUntil(controller.EscPressed);
                    controller.popUpMessage.font = originalFont;
                    controller.ClosePopUpWindow();
                    controller.inputBox.SetActive(true);
                    controller.userInput = null;
                }
                else if (controller.userInput != null) { controller.AddToMainWindow("\n\nDid you done do a dum-dum?"); }
            }
            
        }
    }
    public IEnumerator DisplayToDoAchievements()
    {
        controller.escToContinue = false;
        controller.currentActiveInput = "yesno";
        controller.OverwriteMainWindow($"<size=40><b>To Do Deeds</size></b>\n\n{toDoDeedsDisplayed}\n\n-------------------------------------\n[Deeds] done\n[Erase] all deeds\n\nPress ESC to return.");
        while (true)
        {
            controller.userInput = null;
            while (true)
            {
                yield return new WaitForSeconds(.25f);
                break;
            }
            yield return new WaitUntil(controller.InputGivenOrEscPressed);
            if (controller.userInput == null)
            {
                controller.interactableItems.inventoryStats.SetActive(true);
                controller.interactableItems.DisplayInventory();
                controller.textInput.inputField.ActivateInputField();
                controller.textInput.inputField.text = null;
                while (true)
                {
                    yield return new WaitForSeconds(.25f);
                    break;
                }
                controller.escToContinue = true;
                controller.currentActiveInput = "inventory";
                break;
            }
            else
            {
                Deed inspectDeed = null;
                for (int i = 0; i < toDoDeeds.Count; i++) { if (controller.userInput == toDoDeeds[i].title.ToLower()) { inspectDeed = toDoDeeds[i]; } }

                if (controller.userInput == "deeds" || controller.userInput == "erase")
                {
                    if (controller.userInput == "deeds") { StartCoroutine(DisplayDoneAchievements()); }
                    else if (controller.userInput == "erase") { StartCoroutine(EraseDeeds()); }
                    controller.userInput = null;
                    break;
                }
                else if (inspectDeed != null)
                {
                    controller.inputBox.SetActive(false);
                    controller.OpenPopUpWindow(inspectDeed.title, "", inspectDeed.description, "", "", "", "", "Press ESC to continue");
                    originalFont = controller.popUpMessage.font;
                    controller.popUpMessage.font = deedDescriptionFont;
                    yield return new WaitUntil(controller.EscPressed);
                    controller.popUpMessage.font = originalFont;
                    controller.ClosePopUpWindow();
                    controller.inputBox.SetActive(true);
                    controller.userInput = null;
                }
                else if (controller.userInput != null) { controller.AddToMainWindow("\n\nDid you done do a dum-dum?"); }
            }            
        }
    }
    public IEnumerator EraseDeeds()
    {
        controller.currentActiveInput = "yesno";
        controller.OpenPopUpWindow("Reset accomplished deeds?", "", "This action cannot be undone.", "", "[Yes]. Wipe it clean!", "", "[No] Wait! Don't!", "");
        while (true)
        {
            controller.userInput = null;
            while (true)
            {
                yield return new WaitForSeconds(.25f);
                break;
            }
            yield return new WaitUntil(controller.InputGiven);
            if (controller.userInput == "yes" || controller.userInput == "no")
            {
                controller.escToContinue = true;
                controller.currentActiveInput = "inventory";
                controller.popUpText.text = null;
                controller.popUpBox.SetActive(false);
                if (controller.userInput == "yes")
                {
                    ClearDeeds();
                    while (true)
                    {
                        yield return new WaitForSeconds(.25f);
                        break;
                    }
                    yield return new WaitUntil(controller.EnterPressed);
                    controller.interactableItems.inventoryStats.SetActive(true);
                }
                else if (controller.userInput == "no")
                {
                    controller.DisplayNarratorResponse("You're right. What's the point of doing good deeds if no one knows about it?");
                    while (true)
                    {
                        yield return new WaitForSeconds(.25f);
                        break;
                    }
                    yield return new WaitUntil(controller.EnterPressed);
                    controller.interactableItems.inventoryStats.SetActive(true);
                }
                controller.userInput = null;
                break;
            }
            else if (controller.userInput != null) { controller.AddToMainWindow($"\n\nYes. {controller.userInput}! No?"); }
        }
    }
    public void ClearDeeds()
    {
        doneDeeds.Clear();
        toDoDeeds.Clear();
        for (int i = 0; i < allDeeds.Length; i++)
        {
            allDeeds[i].achieved = false;
            toDoDeeds.Add(allDeeds[i]);
        }
        controller.DisplayNarratorResponse("The deed is done. Your deeds are gone.");
    }
    public IEnumerator DisplayDeedPopUp(Deed deed)
    {
        if (!deed.achieved)
        {
            deed.achieved = true;
            toDoDeeds.Remove(deed);
            doneDeeds.Add(deed);
        }
        deedTitle.text = deed.title;
        deedBlurb.text = deed.blurb;
        deedpopUpWindow.SetActive(true);
        yield return new WaitForSeconds(5f);
        deedGreyScreen.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        deedpopUpWindow.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        deedGreyScreen.SetActive(false);

    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
