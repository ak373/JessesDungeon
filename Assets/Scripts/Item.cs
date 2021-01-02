using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string noun;
    public string nome;
    public int price;
    [TextArea(3,5)]
    public string description;
    public bool unlocked;
}
