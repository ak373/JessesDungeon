using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/InputActions/Saerch")]
public class Saerch : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {   
        if (controller.currentActiveInput == "main" && separatedInputWords.Length == 2)
        {
            Dictionary<string, string[]> searchDictionary = controller.interactableItems.Search(separatedInputWords);
            if (searchDictionary != null)
            {
                if (separatedInputWords[1] == "tray" && controller.interactableItems.traySearch && controller.roomNavigation.currentRoom.roomName == "I7")
                {
                    controller.interactableItems.traySearch = false;
                    controller.InitiateScriptedResponse(controller.TestVerbDictionaryWithNoun(searchDictionary, separatedInputWords[0], separatedInputWords[1]), "tray");
                }
                else { controller.InitiateInputActionResponse(controller.TestVerbDictionaryWithNoun(searchDictionary, separatedInputWords[0], separatedInputWords[1])); }
            }
        }
        //else if (controller.currentActiveInput == "main" && separatedInputWords.Length == 1) { controller.DisplayNarratorResponse("Clowns to the left of me, jokers to the right -- here I am stuck in the middle with you!"); }
        //else { controller.DisplayNarratorResponse("That didn't do anything useful."); }
        else if (/*controller.currentActiveInput == "main" && */separatedInputWords.Length == 1) { controller.InitiateNarrator("Clowns to the left of me, jokers to the right -- here I am stuck in the middle with you!"); }
        else { controller.InitiateNarrator("That didn't do anything useful."); }
    }
}
