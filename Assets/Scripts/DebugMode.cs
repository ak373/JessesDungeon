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
            //controller.DisplayNarratorResponse("I sure won't be answering my door tonight.");
            StartCoroutine(controller.Narrator("I sure won't be answering my door tonight."));
        }
        else if (empireBusiness)
        {
            controller.textInput.textIsGood = true;
            empireBusiness = false;
            bool success = int.TryParse(userInput, out int crystals);
            if (success && crystals >= 0)
            {
                controller.ego.blueCrystals = crystals;
                //controller.DisplayNarratorResponse("You got it, boss.");
                StartCoroutine(controller.Narrator("You got it, boss."));
            }
            //else { controller.DisplayNarratorResponse("Hey, oh, heee-y. We sent that figure out to the number guys, and they're telling us it doesn't quite work out."); }
            else { StartCoroutine(controller.Narrator("Hey, oh, heee-y. We sent that figure out to the number guys, and they're telling us it doesn't quite work out.")); }
        }
        else if (userInput == "i did it for me" && controller.debugMode)
        {
            controller.textInput.textIsGood = true;
            //controller.DisplayNarratorResponse("I'm in the empire business     Set total blue crystals\nSay my name                    God mode\nTread lightly                  Reveal map\nI'm a knight!                  Endgame equipment set\nRazzle Dazzle Root Beer        Choose equipment\nSuck blue frogs                Choose quest items\nAlt k                          Alter state flags and hero stats\nTeleport to ##                 Teleport to room");
            StartCoroutine(controller.Narrator("I'm in the empire business     Set total blue crystals\nSay my name                    God mode\nTread lightly                  Reveal map\nI'm a knight!                  Endgame equipment set\nRazzle Dazzle Root Beer        Choose equipment\nSuck blue frogs                Choose quest items\nAlt k                          Alter state flags and hero stats\nTeleport to ##                 Teleport to room"));
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
            controller.ego.allStats[2].value = 125000;
            controller.ego.allStats[0].value = 125000;
            controller.ego.allStats[3].value = 125000;
            controller.ego.allStats[7].value = 125000;
            //controller.DisplayNarratorResponse("Beetlejuice Beetlejuice BEETLEJUICE!");
            StartCoroutine(controller.Narrator("Beetlejuice Beetlejuice BEETLEJUICE!"));
        }
        else if (userInput == "tread lightly" && controller.debugMode)
        {
            controller.textInput.textIsGood = true;
            for (int i = 0; i < controller.registerRooms.allRooms.Length; i++) { controller.registerRooms.allRooms[i].visited = true; }
            //controller.DisplayNarratorResponse("Black sheep wall!");
            StartCoroutine(controller.Narrator("Black sheep wall!"));
        }
        else if (userInput == "i'm a knight!" && controller.debugMode)
        {
            controller.textInput.textIsGood = true;
            StartCoroutine(SuitUp());

            IEnumerator SuitUp()
            {
                controller.inputBox.SetActive(false);
                controller.AddToMainWindowWithLine("All right, Heath, settle down.");
                yield return new WaitForSeconds(1.25f);
                controller.AddToMainWindow("\n.");
                yield return new WaitForSeconds(.35f);
                controller.AddToMainWindow(".");
                yield return new WaitForSeconds(.35f);
                controller.AddToMainWindow(".");
                yield return new WaitForSeconds(.5f);
                controller.AddToMainWindow("\n.");
                yield return new WaitForSeconds(.35f);
                controller.AddToMainWindow(".");
                yield return new WaitForSeconds(.35f);
                controller.AddToMainWindow(".");
                yield return new WaitForSeconds(.5f);
                controller.AddToMainWindow("\n.");
                yield return new WaitForSeconds(.35f);
                controller.AddToMainWindow(".");
                yield return new WaitForSeconds(.35f);
                controller.AddToMainWindow(".");
                yield return new WaitForSeconds(1f);
                controller.GetEquipped(Instantiate((Weapon)controller.registerObjects.allItems[7]));
                controller.GetDressed(Instantiate((Armor)controller.registerObjects.allItems[5]));
                controller.GetStrapped(Instantiate((Shield)controller.registerObjects.allItems[6]));
                StartCoroutine(controller.Narrator("All set! Sorry no horse and stick."));
            }
        }
    }






    // Update is called once per frame
    void Update()
    {
        
    }
}
