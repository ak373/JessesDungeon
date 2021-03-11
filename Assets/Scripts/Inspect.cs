using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/InputActions/Inspect")]
public class Inspect : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        //controller.escToContinue = false;
        //if (controller.currentActiveInput == "inventory" && separatedInputWords.Length >= 2)
        //{
        //    string itemName = "";
        //    for (int i = 1; i < separatedInputWords.Length; i++)
        //    {
        //        itemName += separatedInputWords[i] + " ";
        //    }
        //    itemName = itemName.Trim();
        //    controller.InspectItem(itemName);
        //}
        //else if (controller.currentActiveInput == "inventory" && separatedInputWords.Length == 1) { controller.DisplayNarratorResponse("-or gadget DOO DOO DOO DOO DOO DOOOO DOOOOOOO!"); }
        //else { controller.DisplayNarratorResponse("That didn't do anything useful."); }
    }
}
