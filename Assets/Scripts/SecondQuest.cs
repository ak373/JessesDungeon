using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondQuest : MonoBehaviour
{
    GameController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
    }

    public void InitiateSecondQuest()
    {
        for (int i = 0; i < controller.registerObjects.allObjects.Length; i++)
        {
            for (int j = 0; j < controller.registerObjects.allObjects[i].interactions.Length; j++)
            {
                if (controller.registerObjects.allObjects[i].interactions[j].secondQuestTextResponse != null)
                {

                }
            }            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
