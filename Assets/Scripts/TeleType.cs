using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeleType : MonoBehaviour
{
    TMP_Text teleTypeText;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        teleTypeText = gameObject.GetComponent<TMP_Text>() ?? gameObject.AddComponent<TMP_Text>();

        int totalVisibleCharacters = teleTypeText.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            teleTypeText.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters) { yield return new WaitForSeconds(1.0f); }
            counter += 1;

            yield return new WaitForSeconds(0.05f);
        }
    }
}
