using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class RoomNavigation : MonoBehaviour
{
    public Room currentRoom;
    GameController controller;

    Dictionary<string, Room> exitDictionary = new Dictionary<string, Room>();

    private void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public void UnpackExitsInRoom()
    {
        for (int i = 0; i < currentRoom.exits.Length; i++)
        {
            exitDictionary.Add(currentRoom.exits[i].keyString, currentRoom.exits[i].valueRoom);
        }
    }

    public void AttemptToChangeRooms(string direction)
    {
        if (exitDictionary.ContainsKey(direction))
        {
            currentRoom = exitDictionary[direction];
            controller.additionalNarrations.SnatchRoom(currentRoom);
            exitDictionary.Clear();
            controller.DisplayNarratorResponse("You head to the " + direction + ".");
        }
        else
        {
            controller.DisplayNarratorResponse("Try as you might, but you can't.");
        }
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }
}
