using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniMap : MonoBehaviour
{
    GameController controller;
    public GameObject mapWindow;
    public TMP_Text legend;
    public GameObject[] tiles;
    public TMP_Text[] labels;
    // tiles, labels, and registerRooms must all have aligned elements

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
    }
    public void GetMap()
    {
        controller.inputBox.SetActive(false);
        legend.text = "X: You are here!\n";

        for (int i = 0; i < controller.registerRooms.allRooms.Length; i++)
        {
            string iRoomName = controller.registerRooms.allRooms[i].roomName;
            bool iRoomVisited = controller.registerRooms.allRooms[i].visited;

            if (iRoomVisited) { tiles[i].SetActive(true); }
            else { tiles[i].SetActive(false); }            

            if (iRoomName == controller.roomNavigation.currentRoom.roomName) { labels[i].text = "X"; }
            else if (iRoomName == "F7") { labels[i].text = "T"; }
            else if (iRoomName == "J4") { labels[i].text = "M"; }
            else if (iRoomName == "J6") { labels[i].text = "L"; }
            else if (iRoomName == "H1") { labels[i].text = "S"; }
            else if (iRoomName == "H9") { labels[i].text = "H"; }
            else if (iRoomName == "G4") { labels[i].text = "C"; }
            else if (iRoomName == "G1") { labels[i].text = "P"; }
            else if (iRoomName == "F8") { labels[i].text = "B"; }
            else if (iRoomName == "E11") { labels[i].text = "F"; }
            else if (iRoomName == "E3") { labels[i].text = "W"; }
            else if (iRoomName == "C6") { labels[i].text = "!"; }
            else if (iRoomName == "C4") { labels[i].text = "?"; }
            else if (iRoomName == "A7") { labels[i].text = "J"; }
            else { labels[i].text = ""; }
            
            if (iRoomVisited && iRoomName == "F7") { legend.text += "\nT: Town"; }
            if (iRoomVisited && iRoomName == "F8") { legend.text += "\nB: Bowling Alley"; }
            if (iRoomVisited && iRoomName == "H9") { legend.text += "\nH: Hermit"; }
            if (iRoomVisited && iRoomName == "C6") { legend.text += "\n!: Fight Club"; }
            if (iRoomVisited && iRoomName == "G1") { legend.text += "\nP: Peaceful Sanctuary"; }
            if (iRoomVisited && iRoomName == "H1") { legend.text += "\nS: Salamanca's Territory"; }
            if (iRoomVisited && iRoomName == "G4") { legend.text += "\nC: Cartel"; }
            if (iRoomVisited && iRoomName == "J4") { legend.text += "\nM: Mike"; }
            if (iRoomVisited && iRoomName == "J6") { legend.text += "\nL: Lydia"; }
            if (iRoomVisited && iRoomName == "A7") { legend.text += "\nJ: Jimmy"; }
            if (iRoomVisited && iRoomName == "E3") { legend.text += "\nW: Winery"; }
            if (iRoomVisited && iRoomName == "E11") { legend.text += "\nF: Fortress"; }
        }
        if (legend.text.Contains("\nF: Fortress"))
        {
            legend.text = legend.text.Replace("\nF: Fortress", "");
            legend.text += "\nF: Fortress";
        }
        mapWindow.SetActive(true);
        controller.escToContinue = true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
