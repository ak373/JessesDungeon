using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RegisterObjects : MonoBehaviour
{
    //public Shield soupBowl;
    //public Weapon rustyWoodenSpoon;
    //public Weapon bubbleLead;
    public Potion[] allPotions;
    public Item[] allItems;
    //5 = chainmail
    //6 = shield
    //7 = sword
    public InteractableObject[] allObjects;

    private void Awake()
    {
        //soupBowl = Instantiate(soupBowl);
        //rustyWoodenSpoon = Instantiate(rustyWoodenSpoon);
        //bubbleLead = Instantiate(bubbleLead);
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < allObjects.Length; i++)
        {
            allObjects[i].searched = false;
        }
    }

    //void ConstructItems(Shield soupBowl, Weapon rustyWoodenSpoon)
    //{
    //    soupBowl = soupBowl.InitializeShield(soupBowl, "bowl", 1, "soup bowl", 1, 0.95f, false, "Not very slick.");
    //    rustyWoodenSpoon = rustyWoodenSpoon.InitializeWeapon(rustyWoodenSpoon, "spoon", 1, "rusty wooden spoon", "SlashBash", 1, 2, 0, false, false, "A miracle of natural law.");
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
