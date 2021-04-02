using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class RoomNavigation : MonoBehaviour
{
    public Room currentRoom;
    public AudioSource currentMusic;
    [HideInInspector] public Room lastRoom;
    GameController controller;

    Dictionary<string, Room> exitDictionary = new Dictionary<string, Room>();

    private void Awake()
    {
        controller = GetComponent<GameController>();
        lastRoom = currentRoom;
    }

    public void UnpackExitsInRoom()
    {
        for (int i = 0; i < currentRoom.exits.Length; i++)
        {
            exitDictionary.Add(currentRoom.exits[i].keyString, currentRoom.exits[i].valueRoom);
        }
    }
    public void MusicListener(Room last, Room current)
    {
        if (last.music != current.music)
        {
            StartCoroutine(FadeAudioOut(last.music, .25f));
            StartCoroutine(FadeAudioIn(current.music, .25f));
            currentMusic = current.music;
        }
        lastRoom = currentRoom;
    }
    public IEnumerator FadeAudioOut(AudioSource audio, float fadeTime)
    {
        float startVolume = audio.volume;

        while (audio.volume > 0)
        {
            audio.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audio.Stop();
        audio.volume = startVolume;
    }
    public IEnumerator FadeAudioIn(AudioSource audio, float fadeTime)
    {
        float startVolume = audio.volume;
        audio.volume = 0;
        audio.Play();
        while (audio.volume < startVolume)
        {
            audio.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }
    public void AttemptToChangeRooms(string direction)
    {
        if (exitDictionary.ContainsKey(direction))
        {
            lastRoom = currentRoom;
            currentRoom = exitDictionary[direction];
            controller.additionalNarrations.SnatchRoom(currentRoom);
            exitDictionary.Clear();
            //controller.DisplayNarratorResponse("You head to the " + direction + ".");
            StartCoroutine(controller.Narrator("You head to the " + direction + "."));
        }
        else
        {
            //controller.DisplayNarratorResponse("Try as you might, but you can't.");
            StartCoroutine(controller.Narrator("Try as you might, but you can't."));
        }
    }
    public void RandomBattleCheck(Room newRoom)
    {
        if (newRoom.battleRoom)
        {
            //roll some randoms
            //if roll does whatever
            //controller.combat.InitiateCombat(enemy, number);
        }
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }
}
