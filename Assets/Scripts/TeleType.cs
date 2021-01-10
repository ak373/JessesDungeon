using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeleType : MonoBehaviour
{
    public TMP_Text battleText;
    public bool messageComplete;
    public int endingCharacter;
    public IEnumerator BattleMessage(int startingCharacter)
    {
        int totalVisibleCharacters = battleText.textInfo.characterCount;
        int counter = startingCharacter;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            battleText.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters) { break; }
            counter += 1;

            yield return new WaitForSeconds(0.05f);
        }
        endingCharacter = counter;
        messageComplete = true;
    }
}
