using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class InteractableItems : MonoBehaviour
{
    public TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
    [HideInInspector] public List<Item> inventory = new List<Item>();
    [HideInInspector] public List<string> notYetSearched = new List<string>();
    [HideInInspector] public List<string> alreadySearched = new List<string>();
    public GameObject inventoryStats;
    GameController controller;
    [HideInInspector] public bool traySearch = false;

    public Dictionary<string, string[]> lookAtDictionary = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> searchDictionary = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> listenToDictionary = new Dictionary<string, string[]>();

    private void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public void UnpackInteractables(Room currentRoom, int i)
    {
        InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i];

        if (!interactableInRoom.searched)
        {
            notYetSearched.Add(interactableInRoom.noun);
        }
        else
        {
            alreadySearched.Add(interactableInRoom.noun);
        }
    }
    public void ClearInteractablesInRoom()
    {
        lookAtDictionary.Clear();
        searchDictionary.Clear();
        notYetSearched.Clear();
        alreadySearched.Clear();
        listenToDictionary.Clear();
    }

    public void DisplayInventory()
    {
        string toPassIn;
        List<Item> alreadyListed = new List<Item>();

        controller.currentActiveInput = "inventory";
        inventoryStats.SetActive(true);
        alreadyListed.Clear();
        toPassIn = "";
        if (inventory.Count == 0) { toPassIn += "Your inventory is empty! How sad.\n"; }
        else
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (alreadyListed.Contains(inventory[i])) { continue; }
                else
                {
                    int counter = 0;
                    for (int j = i; j < inventory.Count; j++)
                    {
                        if (inventory[i] == inventory[j]) { counter++; }
                    }
                    alreadyListed.Add(inventory[i]);
                    string total = counter.ToString();
                    toPassIn += total + " " + myTI.ToTitleCase(inventory[i].nome) + "\n";
                }                
            }
        }
        toPassIn += "\n\n-------------------------------------\nInspect\nEquip\nDrop\nUse\n\nAchievements\n\nPress ESC to return";
        controller.escToContinue = true;
        controller.OverwriteMainWindow(toPassIn);        
    }

    public Dictionary<string, string[]> Search (string[] separatedInputWords)
    {
        string noun = separatedInputWords[1];
        Room currentRoom = controller.roomNavigation.currentRoom;

        if (controller.currentActiveInput == "main")
        {
            if (alreadySearched.Contains(noun)) { return searchDictionary; }
            else if (notYetSearched.Contains(noun))
            {
                if (noun == "tray" && currentRoom.roomName == "I7") { traySearch = true; }
                controller.FlipSearchedText(currentRoom, noun);
                notYetSearched.Remove(noun);
                alreadySearched.Add(noun);
                return searchDictionary;
            }
            else
            {
                controller.DisplayNarratorResponse("Unfortunately, searching that did not aid you in your quest. A waste of time even to describe it!");
                return null;
            }
        }
        else { return null; }
    }
}
