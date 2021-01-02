﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/InputActions/Map")]
public class OpenMap : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if (controller.currentActiveInput == "main" && separatedInputWords.Length == 1) { controller.map.GetMap(); }
        else { controller.DisplayNarratorResponse("That didn't do anything useful."); }
    }
}
