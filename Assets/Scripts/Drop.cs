using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Jesse's Dungeon/InputActions/Drop")]
public class Drop : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        //controller.escToContinue = false;
        //if (controller.currentActiveInput == "inventory" && separatedInputWords.Length == 2 && separatedInputWords[1] == "it")
        //{
        //    controller.DisplayNarratorResponse("Dead or alive you're coming with me!");
        //}
        //else if (controller.currentActiveInput == "inventory" && separatedInputWords.Length >= 2)
        //{
        //    string itemName = "";
        //    for (int i = 1; i < separatedInputWords.Length; i++)
        //    {
        //        itemName += separatedInputWords[i] + " ";
        //    }
        //    itemName = itemName.Trim();
        //    Item itemToDrop = controller.ExtractItem(itemName);
        //    if (itemToDrop != null && !(itemToDrop is Undroppable))
        //    {                
        //        controller.InitiateDropItem(itemToDrop);
        //        //controller.DropItem(itemToDrop);
        //    }
        //    else if (itemToDrop is Undroppable)
        //    {
        //        string[] responses = { "OK, you drop it.", "Then you think better of it and pick it back up." };
        //        float[] pauses = { 0, 0 };
        //        controller.InitiateMultiLineResponse(responses, pauses);
        //        //controller.inputBox.SetActive(false);
        //        //controller.displayText.text += "\n\n---------------------------\n\nOK, you drop it.";
        //        //controller.timeDelay = Time.time;
        //        //controller.pauseIt = true;
        //        //controller.pauseItForUndroppable = true;
        //        }
        //    else { controller.DisplayNarratorResponse("You must be quite a content person, as you don't want what you don't have.\n\nBut you can't drop what you don't have, so you don't drop it."); }
        //}
        //else if (controller.currentActiveInput == "inventory" && separatedInputWords.Length == 1)
        //{
        //    string[] responses = { "\"Get down!\"\n\nYou instinctively obey the command, just avoiding the tip of a\npoisoned dagger as it flies through the air above you.", "Wait. What game is this?" };
        //    float[] pauses = { 4, 0 };
        //    controller.InitiateMultiLineResponse(responses, pauses);
        //    //controller.timeDelay = Time.time;
        //    //controller.pauseIt = true;
        //    //controller.pauseItForDrop = true;
        //    //controller.inputBox.SetActive(false);
        //    //controller.displayText.text += "\n\n---------------------------\n\n\"Get down!\"\n\nYou instinctively obey the command, just avoiding the tip of a poisoned dagger as it flies through the air above you.";
        //}
        //else { controller.DisplayNarratorResponse("That didn't do anything useful."); }
    }
}
