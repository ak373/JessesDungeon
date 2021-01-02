using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/InputActions/Go")]
public class Go : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if(controller.currentActiveInput == "main" && separatedInputWords.Length == 2) { controller.roomNavigation.AttemptToChangeRooms(separatedInputWords[1]); }
        else if (controller.currentActiveInput == "main" && separatedInputWords.Length == 1) { controller.DisplayNarratorResponse("Is that #1 or #2?"); }
        else { controller.DisplayNarratorResponse("That didn't do anything useful."); }
    }
}
