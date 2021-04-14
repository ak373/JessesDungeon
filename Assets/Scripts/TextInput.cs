using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextInput : MonoBehaviour
{
    public TMP_InputField inputField;
    GameController controller;
    AdditionalNarrations additionalNarrations;
    public float keyPressDelay;
    public bool textIsGood = false;

    private void Awake()
    {
        controller = GetComponent<GameController>();
        additionalNarrations = GetComponent<AdditionalNarrations>();
        //inputField.onEndEdit.AddListener(AcceptStringInput);
        inputField.onEndEdit.AddListener(val =>
        {
            if (Input.GetKeyDown(KeyCode.Return))
                AcceptStringInput(val);
        });
    }

    public void AcceptStringInput(string userInput)
    {
        keyPressDelay = Time.time;
        controller.introScreen.SnatchInput(userInput);
        userInput = userInput.Trim();
        controller.AddToActionLog(userInput);
        controller.lastActionHero = 1;
        userInput = userInput.ToLower();
        controller.SnatchInput(userInput);
        additionalNarrations.SnatchInput(userInput);
        controller.debugClass.SnatchInput(userInput);
        controller.demoScript.SnatchInput(userInput);
        //controller.npcTalk.SnatchInput(userInput);
        //controller.achievements.SnatchInput(userInput);
        if (userInput == "quit")
        {
            textIsGood = true;
            StartCoroutine(controller.QuitGame());
        }

        char[] delimiterCharacters = { ' ' };
        string[] separatedInputWords = userInput.Split(delimiterCharacters);
        //if (userInput.StartsWith("test ")) { controller.ego.conversation = System.Convert.ToInt32(userInput.Substring(5)); }
        if (!controller.debugClass.empireBusiness/* && controller.currentActiveInput != "yesno"*/ && !textIsGood)
        {
            for (int i = 0; i < controller.inputActions.Length; i++)
            {
                InputAction inputAction = controller.inputActions[i];
                if (inputAction.keyWord == separatedInputWords[0])
                {
                    inputAction.RespondToInput(controller, separatedInputWords);
                    textIsGood = true;
                }
            }
        }
        
        //cleaned up town voices to additionalnarrations class

        //cleaned up debug input to debug class        
        //else if (controller.debugClass.empireBusiness)
        //{
        //    textIsGood = true;
        //    controller.debugClass.empireBusiness = false;
        //    bool success = int.TryParse(userInput, out int crystals);
        //    if (success && crystals >= 0)
        //    {
        //        controller.ego.blueCrystals = crystals;
        //        controller.DisplayNarratorResponse("You got it, boss.");
        //    }
        //    else { controller.DisplayNarratorResponse("Hey, oh, heee-y. We sent that figure out to the number guys, and they're telling us it doesn't quite work out."); }
        //}
        
        //error input
        //if (!textIsGood && controller.currentActiveInput != "yesno") { controller.DisplayNarratorResponse("That didn't do anything useful."); }
        if (!textIsGood/* && controller.currentActiveInput != "yesno"*/) { StartCoroutine(controller.Narrator("That didn't do anything useful.")); }
        InputComplete();
    }

    void InputComplete()
    {
        textIsGood = false;
        inputField.ActivateInputField();
        inputField.text = null;
        StartCoroutine(controller.ForceTextWindowDown());
    }
}
