using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/Deed")]
public class Deed : ScriptableObject
{
    public string title;
    [TextArea(1,1)]
    public string blurb;
    [TextArea(3,20)]
    public string description;
    public bool achieved;
    public Deed parent;
}
