using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Jesse's Dungeon/InputActions/LookAt")]
public class LookAt : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        //if (controller.currentActiveInput == "main" && separatedInputWords[1] == "at" && separatedInputWords.Length == 2) { controller.DisplayNarratorResponse("Look at? Look at WHAT?!"); }
        if (/*controller.currentActiveInput == "main" && */separatedInputWords[1] == "at" && separatedInputWords.Length == 2) { controller.InitiateNarrator("Look at? Look at WHAT?!"); }
        else if (/*controller.currentActiveInput == "main" && */separatedInputWords[1] == "at" && separatedInputWords.Length == 3) { controller.InitiateInputActionResponse(controller.TestVerbDictionaryWithNoun(controller.interactableItems.lookAtDictionary, separatedInputWords[0], separatedInputWords[2])); }
        //else { controller.DisplayNarratorResponse("That didn't do anything useful."); }
        else { controller.InitiateNarrator("That didn't do anything useful."); }
    }
}
