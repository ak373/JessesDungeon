using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Room")]
public class Room : ScriptableObject
{
    [TextArea (3, 20)]
    public string description1;
    [TextArea(3, 20)]
    public string description2;
    [TextArea (3, 20)]
    public string description3;
    [HideInInspector] public int currentDescription;
    public string roomName;
    [HideInInspector] public bool visited;
    public Exit[] exits;
    public InteractableObject[] interactableObjectsInRoom;
}
