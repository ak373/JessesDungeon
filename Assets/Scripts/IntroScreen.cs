using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroScreen : MonoBehaviour
{
    public TMP_Text firstStartText, secondStartText, firstQuestGreeting, secondQuestGreeting, secondQuestGreetingPartTwo;
    public GameObject introScreen, loopHouse, introTextHouse, greetingTextHouse, wholeScreenFadeToBlackTwo, wholeScreenFadeFromBlackOne;
    public AudioSource introClick;

    GameController controller;
    bool introInSession = true;
    bool messageComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
        StartCoroutine(IntroScript());
        firstQuestGreeting.maxVisibleCharacters = 0;
    }
    public void SnatchInput(string fromTextInput)
    {
        if (introInSession)
        {
            controller.textInput.textIsGood = true;
            if (fromTextInput == "ZELDA")
            {
                controller.secondQuestActive = true;
                secondQuestGreeting.text = "It's dangerous to go alone! Take this.";
                secondQuestGreeting.maxVisibleCharacters = 0;
                secondQuestGreetingPartTwo.text = "You receive the mace.";
                secondQuestGreetingPartTwo.maxVisibleCharacters = 0;
                firstQuestGreeting.text = "";
            }
        }
    }
    IEnumerator IntroScript()
    {
        yield return new WaitForSeconds(5f);
        loopHouse.SetActive(true);
        firstStartText.text = "Press <color=black>ENTER</color> to start";
        StartCoroutine(StartListener());
    }
    IEnumerator StartListener()
    {
        yield return new WaitUntil(EnterPressed);
        introClick.Play();
        controller.inputBox.SetActive(false);
        introInSession = false;
        wholeScreenFadeToBlackTwo.SetActive(true);
        yield return new WaitForSeconds(2f);
        introTextHouse.SetActive(false);
        greetingTextHouse.SetActive(true);
        yield return new WaitForSeconds(2f);
        wholeScreenFadeToBlackTwo.SetActive(false);
        if (!controller.secondQuestActive) { StartCoroutine(Teletype(firstQuestGreeting)); }
        else if (controller.secondQuestActive)
        {
            StartCoroutine(Teletype(secondQuestGreeting));
            yield return new WaitUntil(MessageComplete);
            messageComplete = false;
            StartCoroutine(Teletype(secondQuestGreetingPartTwo));
        }
        yield return new WaitUntil(MessageComplete);
        yield return new WaitForSeconds(3f);
        wholeScreenFadeToBlackTwo.SetActive(true);
        yield return new WaitForSeconds(2f);
        introScreen.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        wholeScreenFadeToBlackTwo.SetActive(false);
        controller.registerRooms.mainTheme.Play();
        wholeScreenFadeFromBlackOne.SetActive(true);
        controller.UnlockUserInput();
        yield return new WaitForSeconds(1f);
        wholeScreenFadeFromBlackOne.SetActive(false);
    }
    IEnumerator Teletype(TMP_Text text)
    {
        int totalVisibleCharacters = text.textInfo.characterCount;
        int counter = 0;
        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            text.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters) { break; }
            counter += 1;

            yield return new WaitForSeconds(.03f);
        }
        messageComplete = true;
    }
    bool EnterPressed() { return Input.GetKeyDown(KeyCode.Return); }
    bool MessageComplete() { return messageComplete; }

    // Update is called once per frame
    void Update()
    {
        
    }
}
