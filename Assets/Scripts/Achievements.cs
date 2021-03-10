using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using TMPro;

public class Achievements : MonoBehaviour
{
    public TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
    List<Deed> doneDeeds = new List<Deed>();
    List<Deed> toDoDeeds = new List<Deed>();
    public Deed[] allDeeds;
    string doneDeedsDisplayed = "";
    string toDoDeedsDisplayed = "";

    public GameObject deedpopUpWindow, deedWindow, deedWhiteScreen, deedGreyScreen;
    public TMP_Text deedTitle, deedBlurb, deedSwitch, deedErase;
    public TMP_FontAsset deedDescriptionFont;
    public TMP_FontAsset originalFont;
    //public Animator fadeIn, fadeOut;

    GameController controller;
    GameObject deedList, deedScrollRect;
    TMP_Text deedListText;
    IEnumerator doneDeedsCoroutine, toDoDeedsCoroutine;

    private void Awake()
    {
        controller = GetComponent<GameController>();

        deedScrollRect = deedWindow.transform.Find("ScrollRect").gameObject;
        deedList = deedScrollRect.transform.Find("MainText").gameObject;
        deedListText = deedList.transform.GetComponent<TMP_Text>();
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
    //public void SnatchInput(string fromTextInput)
    //{
    //    if (fromTextInput == "achievements" && controller.currentActiveInput == "inventory")
    //    {
    //        controller.textInput.textIsGood = true;
    //        InitiateDisplayAchievements();
    //    }
    //}
    public void InitiateDisplayAchievements()
    {        
        doneDeedsDisplayed = "";
        toDoDeedsDisplayed = "";
        for (int i = 0; i < doneDeeds.Count; i++) { doneDeedsDisplayed += $"[{doneDeeds[i].title}]\n"; }
        for (int i = 0; i < toDoDeeds.Count; i++)
        {
            if (allDeeds[1].achieved) { toDoDeedsDisplayed += $"[{toDoDeeds[i].title}]\n"; }
            else { toDoDeedsDisplayed += "<color=#AFAFAF>????</color>\n"; }            
        }
        //if (doneDeeds.Count == 0) { doneDeedsDisplayed = "You've achieved nothing. Step up your game, hero."; }
        //if (toDoDeeds.Count == 0) { toDoDeedsDisplayed = "You've done it all! Congratulations! You're really a hero!"; }
        //if (doneDeeds.Count < 6)
        //{
        //    int difference = 5 - doneDeeds.Count;
        //    for (int i = 0; i < difference; i++) { doneDeedsDisplayed += "\n"; }
        //}
        //if (toDoDeeds.Count < 6)
        //{
        //    int difference = 5 - toDoDeeds.Count;
        //    for (int i = 0; i < difference; i++) { toDoDeedsDisplayed += "\n"; }
        //}
        doneDeedsCoroutine = DisplayDoneAchievements();
        StartCoroutine(doneDeedsCoroutine);
    }
    public IEnumerator DisplayDoneAchievements()
    {
        if (toDoDeedsCoroutine != null) { StopCoroutine(toDoDeedsCoroutine); }
        WriteDeedScreen($"<size=40><b>Done Deeds</size></b>\n\n{doneDeedsDisplayed}");
        deedSwitch.text = "To Do Deeds";
        while (true)
        {
            string normalDeedListText = deedListText.text;
            Deed selectedDeed = doneDeeds[0];
            int selectedElement = 0;
            int memoryElement = 0;
            bool skipToOptions = false;
            if (doneDeeds.Count == 0) { skipToOptions = true; }

            deedListText.text = normalDeedListText;
            if (selectedElement < 0) { selectedElement = doneDeeds.Count - 1; }
            if (selectedElement > doneDeeds.Count - 1) { selectedElement = 0; }
            int deedLength = doneDeeds[selectedElement].title.Length;
            int deedListIndex = 0;
            deedListIndex = deedListText.text.IndexOf(myTI.ToTitleCase(doneDeeds[selectedElement].title));

            string newText = "";

            for (int i = 0; i < deedListIndex; i++) { newText += deedListText.text[i]; }

            newText += "<color=yellow>";

            for (int i = deedListIndex; i < deedListIndex + deedLength; i++) { newText += deedListText.text[i]; }

            newText += "</color>";

            for (int i = deedListIndex + deedLength; i < deedListText.text.Length; i++) { newText += deedListText.text[i]; }

            deedListText.text = newText;

            yield return new WaitUntil(controller.RightUpDownEnterEscPressed);

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
            else if (Input.GetKeyDown(KeyCode.RightArrow) || skipToOptions)
            {
                if (skipToOptions) { skipToOptions = false; }
                else { controller.interactableItems.cursorMove.Play(); }
                deedListText.text = normalDeedListText;
                memoryElement = selectedElement;
                int optionElement = 0;
                string plainSwitch = deedSwitch.text;
                string plainErase = deedErase.text;

                while (true)
                {
                    if (optionElement < 0) { optionElement = 1; }
                    if (optionElement > 1) { optionElement = 0; }

                    deedSwitch.text = plainSwitch;
                    deedErase.text = plainErase;

                    yield return new WaitUntil(controller.LeftUpDownEnterEscPressed);
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        controller.interactableItems.cursorMove.Play();
                        optionElement--;
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        controller.interactableItems.cursorMove.Play();
                        optionElement++;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        controller.interactableItems.cursorMove.Play();
                        if (doneDeeds.Count > 0)
                        {
                            deedSwitch.text = plainSwitch;
                            deedErase.text = plainErase;
                            selectedElement = memoryElement;
                            break;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        controller.interactableItems.cursorCancel.Play();
                        deedSwitch.text = plainSwitch;
                        deedErase.text = plainErase;
                        deedWindow.SetActive(false);
                        //doubleBreak = true;
                        break;
                    }
                    else if (Input.GetKeyDown(KeyCode.Return))
                    {
                        if (optionElement == 0)
                        {
                            controller.interactableItems.cursorSelect.Play();
                            toDoDeedsCoroutine = DisplayToDoAchievements();
                            StartCoroutine(toDoDeedsCoroutine);
                        }
                        else if (optionElement == 1)
                        {
                            bool yesSelected = false;
                            while (true)
                            {
                                if (yesSelected) { controller.OpenPopUpWindow("Reset accomplished deeds?", "", "This action cannot be undone.", "", "<color=yellow>[Yes]</color>. Wipe it clean!", "", "[No] Wait! Don't!", ""); }
                                else { controller.OpenPopUpWindow("Reset accomplished deeds?", "", "This action cannot be undone.", "", "[Yes]. Wipe it clean!", "", "<color=yellow>[No]</color> Wait! Don't!", ""); }
                                yield return new WaitUntil(controller.LeftRightEnterPressed);
                                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                                {
                                    controller.interactableItems.cursorMove.Play();
                                    yesSelected = !yesSelected;
                                }
                                else if (Input.GetKeyDown(KeyCode.Return))
                                {
                                    if (yesSelected)
                                    {
                                        controller.interactableItems.cursorSelect.Play();
                                        ClearDeeds();
                                        controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                        yield return new WaitUntil(controller.EscPressed);
                                        controller.popUpMessage.font = controller.achievements.originalFont;
                                        //dropUsed = true;
                                    }
                                    else
                                    {
                                        controller.interactableItems.cursorCancel.Play();
                                        controller.OpenPopUpWindow("", "", "You're right. What's the point of doing good deeds if no one knows about it?", "", "", "", "", "Press ESC to return");
                                        controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                        yield return new WaitUntil(controller.EscPressed);
                                        controller.popUpMessage.font = controller.achievements.originalFont;
                                    }
                                    controller.ClosePopUpWindow();
                                    break;
                                }
                            }
                        }
                    }
                    //if (doubleBreak)
                    //{
                    //    doubleBreak = false;
                    //    break;
                    //}
                }
            }
            //End Right Side
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                controller.interactableItems.cursorCancel.Play();
                deedListText.text = normalDeedListText;
                deedWindow.SetActive(false);
                break;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                controller.interactableItems.cursorSelect.Play();
                selectedDeed = doneDeeds[selectedElement];
                controller.OpenPopUpWindow(selectedDeed.title, "", selectedDeed.description, "", "", "", "", "Press ESC to return");
                originalFont = controller.popUpMessage.font;
                controller.popUpMessage.font = deedDescriptionFont;
                yield return new WaitUntil(controller.EscPressed);
                controller.popUpMessage.font = originalFont;
                controller.ClosePopUpWindow();
            }

            //
            //
            //old code
            //
            //

            //    if (controller.userInput == null)
            //    {
            //        controller.interactableItems.inventoryStats.SetActive(true);
            //        controller.interactableItems.DisplayInventory();
            //        controller.textInput.inputField.ActivateInputField();
            //        controller.textInput.inputField.text = null;
            //        while (true)
            //        {
            //            yield return new WaitForSeconds(.25f);
            //            break;
            //        }
            //        controller.escToContinue = true;
            //        controller.currentActiveInput = "inventory";
            //        break;
            //    }
            //    else
            //    {
            //        Deed inspectDeed = null;
            //        for (int i = 0; i < doneDeeds.Count; i++) { if (controller.userInput == doneDeeds[i].title.ToLower()) { inspectDeed = doneDeeds[i]; } }

            //        if (controller.userInput == "deeds" || controller.userInput == "erase")
            //        {
            //            if (controller.userInput == "deeds") { StartCoroutine(DisplayToDoAchievements()); }
            //            else if (controller.userInput == "erase") { StartCoroutine(EraseDeeds()); }
            //            controller.userInput = null;
            //            break;
            //        }
            //        else if (inspectDeed != null)
            //        {
            //            controller.inputBox.SetActive(false);
            //            controller.OpenPopUpWindow(inspectDeed.title, "", inspectDeed.description, "", "", "", "", "Press ESC to return");
            //            originalFont = controller.popUpMessage.font;
            //            controller.popUpMessage.font = deedDescriptionFont;
            //            yield return new WaitUntil(controller.EscPressed);
            //            controller.popUpMessage.font = originalFont;
            //            controller.ClosePopUpWindow();
            //            controller.inputBox.SetActive(true);
            //            controller.userInput = null;
            //        }
            //        else if (controller.userInput != null) { controller.AddToMainWindow("\n\nDid you done do a dum-dum?"); }
            //    }
        }
    }
    public IEnumerator DisplayToDoAchievements()
    {
        if (doneDeedsCoroutine != null) { StopCoroutine(doneDeedsCoroutine); }
        WriteDeedScreen($"<size=40><b>To Do Deeds</size></b>\n\n{toDoDeedsDisplayed}");
        deedSwitch.text = "Done Deeds";
        while (true)
        {
            string normalDeedListText = deedListText.text;
            Deed selectedDeed = toDoDeeds[0];
            int selectedElement = 0;
            int memoryElement = 0;
            bool skipToOptions = false;
            if (toDoDeeds.Count == 0) { skipToOptions = true; }

            deedListText.text = normalDeedListText;
            if (selectedElement < 0) { selectedElement = toDoDeeds.Count - 1; }
            if (selectedElement > toDoDeeds.Count - 1) { selectedElement = 0; }
            int deedLength = toDoDeeds[selectedElement].title.Length;
            int deedListIndex = 0;
            deedListIndex = deedListText.text.IndexOf(myTI.ToTitleCase(toDoDeeds[selectedElement].title));

            string newText = "";

            for (int i = 0; i < deedListIndex; i++) { newText += deedListText.text[i]; }

            newText += "<color=yellow>";

            for (int i = deedListIndex; i < deedListIndex + deedLength; i++) { newText += deedListText.text[i]; }

            newText += "</color>";

            for (int i = deedListIndex + deedLength; i < deedListText.text.Length; i++) { newText += deedListText.text[i]; }

            deedListText.text = newText;

            yield return new WaitUntil(controller.RightUpDownEnterEscPressed);

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
            else if (Input.GetKeyDown(KeyCode.RightArrow) || skipToOptions)
            {
                if (skipToOptions) { skipToOptions = false; }
                else { controller.interactableItems.cursorMove.Play(); }
                deedListText.text = normalDeedListText;
                memoryElement = selectedElement;
                int optionElement = 0;
                string plainSwitch = deedSwitch.text;
                string plainErase = deedErase.text;

                while (true)
                {
                    if (optionElement < 0) { optionElement = 1; }
                    if (optionElement > 1) { optionElement = 0; }

                    deedSwitch.text = plainSwitch;
                    deedErase.text = plainErase;

                    yield return new WaitUntil(controller.LeftUpDownEnterEscPressed);
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        controller.interactableItems.cursorMove.Play();
                        optionElement--;
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        controller.interactableItems.cursorMove.Play();
                        optionElement++;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        controller.interactableItems.cursorMove.Play();
                        if (toDoDeeds.Count > 0)
                        {
                            deedSwitch.text = plainSwitch;
                            deedErase.text = plainErase;
                            selectedElement = memoryElement;
                            break;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        controller.interactableItems.cursorCancel.Play();
                        deedSwitch.text = plainSwitch;
                        deedErase.text = plainErase;
                        deedWindow.SetActive(false);
                        //doubleBreak = true;
                        break;
                    }
                    else if (Input.GetKeyDown(KeyCode.Return))
                    {
                        if (optionElement == 0)
                        {
                            controller.interactableItems.cursorSelect.Play();
                            doneDeedsCoroutine = DisplayDoneAchievements();
                            StartCoroutine(doneDeedsCoroutine);
                        }
                        else if (optionElement == 1)
                        {
                            bool yesSelected = false;
                            while (true)
                            {
                                if (yesSelected) { controller.OpenPopUpWindow("Reset accomplished deeds?", "", "This action cannot be undone.", "", "<color=yellow>[Yes]</color>. Wipe it clean!", "", "[No] Wait! Don't!", ""); }
                                else { controller.OpenPopUpWindow("Reset accomplished deeds?", "", "This action cannot be undone.", "", "[Yes]. Wipe it clean!", "", "<color=yellow>[No]</color> Wait! Don't!", ""); }
                                yield return new WaitUntil(controller.LeftRightEnterPressed);
                                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                                {
                                    controller.interactableItems.cursorMove.Play();
                                    yesSelected = !yesSelected;
                                }
                                else if (Input.GetKeyDown(KeyCode.Return))
                                {
                                    if (yesSelected)
                                    {
                                        controller.interactableItems.cursorSelect.Play();
                                        ClearDeeds();
                                        controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                        yield return new WaitUntil(controller.EscPressed);
                                        controller.popUpMessage.font = controller.achievements.originalFont;
                                        //dropUsed = true;
                                    }
                                    else
                                    {
                                        controller.interactableItems.cursorCancel.Play();
                                        controller.OpenPopUpWindow("", "", "You're right. What's the point of doing good deeds if no one knows about it?", "", "", "", "", "Press ESC to return");
                                        controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                        yield return new WaitUntil(controller.EscPressed);
                                        controller.popUpMessage.font = controller.achievements.originalFont;
                                    }
                                    controller.ClosePopUpWindow();
                                    break;
                                }
                            }
                        }
                    }
                    //if (doubleBreak)
                    //{
                    //    doubleBreak = false;
                    //    break;
                    //}
                }
            }
            //End Right Side
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                controller.interactableItems.cursorCancel.Play();
                deedListText.text = normalDeedListText;
                deedWindow.SetActive(false);
                break;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                controller.interactableItems.cursorSelect.Play();
                selectedDeed = doneDeeds[selectedElement];
                if (controller.secondQuestActive) { controller.OpenPopUpWindow(selectedDeed.title, "", selectedDeed.description, "", "", "", "", "Press ESC to return"); }
                else { controller.OpenPopUpWindow("<color=grey>???</color>", "", "<color=grey>???</color>", "", "", "", "", "Press ESC to return"); }
                originalFont = controller.popUpMessage.font;
                controller.popUpMessage.font = deedDescriptionFont;
                yield return new WaitUntil(controller.EscPressed);
                controller.popUpMessage.font = originalFont;
                controller.ClosePopUpWindow();
            }
        }


            //
            //
            //old code
            //
            //

        //    controller.escToContinue = false;
        //controller.currentActiveInput = "yesno";
        //controller.OverwriteMainWindow($"<size=40><b>To Do Deeds</size></b>\n\n{toDoDeedsDisplayed}\n\n-------------------------------------\n[Deeds] done\n[Erase] all deeds\n\nPress ESC to return.");
        //while (true)
        //{
        //    controller.userInput = null;
        //    while (true)
        //    {
        //        yield return new WaitForSeconds(.25f);
        //        break;
        //    }
        //    yield return new WaitUntil(controller.InputGivenOrEscPressed);
        //    if (controller.userInput == null)
        //    {
        //        controller.interactableItems.inventoryStats.SetActive(true);
        //        controller.interactableItems.DisplayInventory();
        //        controller.textInput.inputField.ActivateInputField();
        //        controller.textInput.inputField.text = null;
        //        while (true)
        //        {
        //            yield return new WaitForSeconds(.25f);
        //            break;
        //        }
        //        controller.escToContinue = true;
        //        controller.currentActiveInput = "inventory";
        //        break;
        //    }
        //    else
        //    {
        //        Deed inspectDeed = null;
        //        for (int i = 0; i < toDoDeeds.Count; i++) { if (controller.userInput == toDoDeeds[i].title.ToLower()) { inspectDeed = toDoDeeds[i]; } }

        //        if (controller.userInput == "deeds" || controller.userInput == "erase")
        //        {
        //            if (controller.userInput == "deeds") { StartCoroutine(DisplayDoneAchievements()); }
        //            else if (controller.userInput == "erase") { StartCoroutine(EraseDeeds()); }
        //            controller.userInput = null;
        //            break;
        //        }
        //        else if (inspectDeed != null)
        //        {
        //            controller.inputBox.SetActive(false);
        //            controller.OpenPopUpWindow(inspectDeed.title, "", inspectDeed.description, "", "", "", "", "Press ESC to continue");
        //            originalFont = controller.popUpMessage.font;
        //            controller.popUpMessage.font = deedDescriptionFont;
        //            yield return new WaitUntil(controller.EscPressed);
        //            controller.popUpMessage.font = originalFont;
        //            controller.ClosePopUpWindow();
        //            controller.inputBox.SetActive(true);
        //            controller.userInput = null;
        //        }
        //        else if (controller.userInput != null) { controller.AddToMainWindow("\n\nDid you done do a dum-dum?"); }
        //    }
        //}
    }
    void WriteDeedScreen(string text)
    {
        deedListText.text = text;
    }


    //public IEnumerator DisplayDoneAchievements()
    //{
    //    //controller.escToContinue = false;
    //    //controller.currentActiveInput = "yesno";
    //    controller.OverwriteMainWindow($"<size=40><b>Done Deeds</size></b>\n\n{doneDeedsDisplayed}\n\n-------------------------------------\n[Deeds] to do\n[Erase] all deeds\n\nPress ESC to return.");
    //    while (true)
    //    {
    //        controller.userInput = null;
    //        while (true)
    //        {
    //            yield return new WaitForSeconds(.25f);
    //            break;
    //        }
    //        yield return new WaitUntil(controller.InputGivenOrEscPressed);
    //        if (controller.userInput == null)
    //        {
    //            controller.interactableItems.inventoryStats.SetActive(true);
    //            controller.interactableItems.DisplayInventory();
    //            controller.textInput.inputField.ActivateInputField();
    //            controller.textInput.inputField.text = null;
    //            while (true)
    //            {
    //                yield return new WaitForSeconds(.25f);
    //                break;
    //            }
    //            controller.escToContinue = true;
    //            controller.currentActiveInput = "inventory";
    //            break;
    //        }
    //        else
    //        {
    //            Deed inspectDeed = null;
    //            for (int i = 0; i < doneDeeds.Count; i++) { if (controller.userInput == doneDeeds[i].title.ToLower()) { inspectDeed = doneDeeds[i]; } }

    //            if (controller.userInput == "deeds" || controller.userInput == "erase")
    //            {
    //                if (controller.userInput == "deeds") { StartCoroutine(DisplayToDoAchievements()); }
    //                else if (controller.userInput == "erase") { StartCoroutine(EraseDeeds()); }
    //                controller.userInput = null;
    //                break;
    //            }
    //            else if (inspectDeed != null)
    //            {
    //                controller.inputBox.SetActive(false);
    //                controller.OpenPopUpWindow(inspectDeed.title, "", inspectDeed.description, "", "", "", "", "Press ESC to return");
    //                originalFont = controller.popUpMessage.font;
    //                controller.popUpMessage.font = deedDescriptionFont;
    //                yield return new WaitUntil(controller.EscPressed);
    //                controller.popUpMessage.font = originalFont;
    //                controller.ClosePopUpWindow();
    //                controller.inputBox.SetActive(true);
    //                controller.userInput = null;
    //            }
    //            else if (controller.userInput != null) { controller.AddToMainWindow("\n\nDid you done do a dum-dum?"); }
    //        }

    //    }
    //}
    //public IEnumerator DisplayToDoAchievements()
    //{
    //    controller.escToContinue = false;
    //    controller.currentActiveInput = "yesno";
    //    controller.OverwriteMainWindow($"<size=40><b>To Do Deeds</size></b>\n\n{toDoDeedsDisplayed}\n\n-------------------------------------\n[Deeds] done\n[Erase] all deeds\n\nPress ESC to return.");
    //    while (true)
    //    {
    //        controller.userInput = null;
    //        while (true)
    //        {
    //            yield return new WaitForSeconds(.25f);
    //            break;
    //        }
    //        yield return new WaitUntil(controller.InputGivenOrEscPressed);
    //        if (controller.userInput == null)
    //        {
    //            controller.interactableItems.inventoryStats.SetActive(true);
    //            controller.interactableItems.DisplayInventory();
    //            controller.textInput.inputField.ActivateInputField();
    //            controller.textInput.inputField.text = null;
    //            while (true)
    //            {
    //                yield return new WaitForSeconds(.25f);
    //                break;
    //            }
    //            controller.escToContinue = true;
    //            controller.currentActiveInput = "inventory";
    //            break;
    //        }
    //        else
    //        {
    //            Deed inspectDeed = null;
    //            for (int i = 0; i < toDoDeeds.Count; i++) { if (controller.userInput == toDoDeeds[i].title.ToLower()) { inspectDeed = toDoDeeds[i]; } }

    //            if (controller.userInput == "deeds" || controller.userInput == "erase")
    //            {
    //                if (controller.userInput == "deeds") { StartCoroutine(DisplayDoneAchievements()); }
    //                else if (controller.userInput == "erase") { StartCoroutine(EraseDeeds()); }
    //                controller.userInput = null;
    //                break;
    //            }
    //            else if (inspectDeed != null)
    //            {
    //                controller.inputBox.SetActive(false);
    //                controller.OpenPopUpWindow(inspectDeed.title, "", inspectDeed.description, "", "", "", "", "Press ESC to continue");
    //                originalFont = controller.popUpMessage.font;
    //                controller.popUpMessage.font = deedDescriptionFont;
    //                yield return new WaitUntil(controller.EscPressed);
    //                controller.popUpMessage.font = originalFont;
    //                controller.ClosePopUpWindow();
    //                controller.inputBox.SetActive(true);
    //                controller.userInput = null;
    //            }
    //            else if (controller.userInput != null) { controller.AddToMainWindow("\n\nDid you done do a dum-dum?"); }
    //        }            
    //    }
    //}
    //public IEnumerator EraseDeeds()
    //{
    //    controller.currentActiveInput = "yesno";
    //    controller.OpenPopUpWindow("Reset accomplished deeds?", "", "This action cannot be undone.", "", "[Yes]. Wipe it clean!", "", "[No] Wait! Don't!", "");
    //    while (true)
    //    {
    //        controller.userInput = null;
    //        while (true)
    //        {
    //            yield return new WaitForSeconds(.25f);
    //            break;
    //        }
    //        yield return new WaitUntil(controller.InputGiven);
    //        if (controller.userInput == "yes" || controller.userInput == "no")
    //        {
    //            controller.escToContinue = true;
    //            controller.currentActiveInput = "inventory";
    //            controller.popUpText.text = null;
    //            controller.popUpBox.SetActive(false);
    //            if (controller.userInput == "yes")
    //            {
    //                ClearDeeds();
    //                while (true)
    //                {
    //                    yield return new WaitForSeconds(.25f);
    //                    break;
    //                }
    //                yield return new WaitUntil(controller.EnterPressed);
    //                controller.interactableItems.inventoryStats.SetActive(true);
    //            }
    //            else if (controller.userInput == "no")
    //            {
    //                controller.DisplayNarratorResponse("You're right. What's the point of doing good deeds if no one knows about it?");
    //                while (true)
    //                {
    //                    yield return new WaitForSeconds(.25f);
    //                    break;
    //                }
    //                yield return new WaitUntil(controller.EnterPressed);
    //                controller.interactableItems.inventoryStats.SetActive(true);
    //            }
    //            controller.userInput = null;
    //            break;
    //        }
    //        else if (controller.userInput != null) { controller.AddToMainWindow($"\n\nYes. {controller.userInput}! No?"); }
    //    }
    //}
    public void ClearDeeds()
    {
        doneDeeds.Clear();
        toDoDeeds.Clear();
        for (int i = 0; i < allDeeds.Length; i++)
        {
            allDeeds[i].achieved = false;
            toDoDeeds.Add(allDeeds[i]);
        }
        controller.OpenPopUpWindow("", "", "The deed is done. Your deeds are gone.", "", "", "", "", "Press ESC to return");
        //controller.DisplayNarratorResponse("The deed is done. Your deeds are gone.");
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
