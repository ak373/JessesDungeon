using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/InputActions/Inventory")]
public class Inventory : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if (/*controller.currentActiveInput == "main" && */separatedInputWords.Length == 1) { controller.interactableItems.ActivateDisplayInventory(); }
        //else { controller.DisplayNarratorResponse("That didn't do anything useful."); }
        else { controller.InitiateNarrator("That didn't do anything useful."); }
    }
}
