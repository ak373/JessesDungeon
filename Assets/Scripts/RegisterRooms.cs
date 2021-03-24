using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterRooms : MonoBehaviour
{
    public AudioSource mainTheme;
    public AudioSource townTheme;
    public Room[] allRooms;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < allRooms.Length; i++)
        {
            allRooms[i].visited = false;
            allRooms[i].currentDescription = 1;
            allRooms[i].music = mainTheme;
            if (allRooms[i].roomName == "F7") { allRooms[i].music = townTheme; }
        }
        allRooms[2].visited = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
