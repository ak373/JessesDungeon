using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    string userInput;
    GameController controller;
    [HideInInspector] public bool empireBusiness = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
    }
    public void SnatchInput(string fromTextInput)
    {
        userInput = fromTextInput;
        if (userInput == "i am the one who knocks" && !controller.debugMode)
        {
            controller.textInput.textIsGood = true;
            controller.debugMode = true;
            controller.DisplayNarratorResponse("I sure won't be answering my door tonight.");
        }
        else if (empireBusiness)
        {
            controller.textInput.textIsGood = true;
            empireBusiness = false;
            bool success = int.TryParse(userInput, out int crystals);
            if (success && crystals >= 0)
            {
                controller.ego.blueCrystals = crystals;
                controller.DisplayNarratorResponse("You got it, boss.");
            }
            else { controller.DisplayNarratorResponse("Hey, oh, heee-y. We sent that figure out to the number guys, and they're telling us it doesn't quite work out."); }
        }
        else if (userInput == "i did it for me" && controller.debugMode)
        {
            controller.textInput.textIsGood = true;
            controller.DisplayNarratorResponse("I'm in the empire business     Set total blue crystals\nSay my name                    God mode\nTread lightly                  Reveal map\nI'm a knight!                  Endgame equipment set\nRazzle Dazzle Root Beer        Choose equipment\nSuck blue frogs                Choose quest items\nAlt k                          Alter state flags and hero stats\nTeleport to ##                 Teleport to room");
        }
        else if (userInput == "i'm in the empire business" && controller.debugMode)
        {
            controller.textInput.textIsGood = true;
            empireBusiness = true;
            controller.OverwriteMainWindow("And just how much is your empire worth nowadays?");
        }
        else if (userInput == "say my name" && controller.debugMode)
        {
            controller.textInput.textIsGood = true;
            controller.ego.maxHitPoints = 125000;
            controller.ego.currentHitPoints = 125000;
            controller.ego.armorClass = 125000;
            controller.ego.damage = 125000;
            controller.DisplayNarratorResponse("Beetlejuice Beetlejuice BEETLEJUICE!");
        }
        else if (userInput == "tread lightly" && controller.debugMode)
        {
            controller.textInput.textIsGood = true;
            for (int i = 0; i < controller.registerRooms.allRooms.Length; i++) { controller.registerRooms.allRooms[i].visited = true; }
            controller.DisplayNarratorResponse("Black sheep wall!");
        }
    }






    // Update is called once per frame
    void Update()
    {
        
    }
}
