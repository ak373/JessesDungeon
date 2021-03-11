using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/InputActions/ListenTo")]
public class ListenTo : InputAction
{    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        //if (controller.currentActiveInput == "main" && separatedInputWords[1] == "to" && separatedInputWords.Length == 2) { controller.DisplayNarratorResponse("The only thing you hear is the beating of your own heart."); }
        if (/*controller.currentActiveInput == "main" && */separatedInputWords[1] == "to" && separatedInputWords.Length == 2) { controller.InitiateNarrator("The only thing you hear is the beating of your own heart."); }
        else if (/*controller.currentActiveInput == "main" && */ separatedInputWords[1] == "to" && separatedInputWords.Length == 3) { controller.InitiateInputActionResponse(controller.TestVerbDictionaryWithNoun(controller.interactableItems.lookAtDictionary, separatedInputWords[0], separatedInputWords[2])); }
        //else { controller.DisplayNarratorResponse("That didn't do anything useful."); }
        else { controller.InitiateNarrator("That didn't do anything useful."); }
    }
}
