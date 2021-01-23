using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

[CreateAssetMenu(menuName = "Jesse's Dungeon/InputActions/Equip")]
public class Equip : InputAction
{
    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        controller.escToContinue = false;
        if (controller.currentActiveInput == "inventory" && separatedInputWords.Length >= 2)
        {
            string itemName = "";
            for (int i = 1; i < separatedInputWords.Length; i++)
            {
                itemName += separatedInputWords[i] + " ";
            }
            itemName = itemName.Trim();
            if (itemName == "bubble lead") { itemName = "not Bubble Lead sorry"; }
            Item itemToEquip = controller.ExtractItem(itemName);
            if (itemToEquip is Weapon)
            {
                if (controller.ego.equippedWeapon != null && controller.ego.equippedWeapon.nome == itemName) { controller.DisplayNarratorResponse("You're already using the " + myTI.ToTitleCase(itemName) + ", silly."); }
                else
                {
                    if (controller.ego.equippedShield != null)
                    {
                        Weapon equippingWeapon = (Weapon)itemToEquip;
                        if (equippingWeapon.twoHanded) { controller.DisplayNarratorResponse($"You can't equip the {myTI.ToTitleCase(itemToEquip.nome)} while using a shield."); }
                        else
                        {
                            controller.GetEquipped((Weapon)itemToEquip);
                            controller.DisplayNarratorResponse("You equip the " + myTI.ToTitleCase(itemToEquip.nome) + ".");
                        }
                    }
                    else
                    {
                        controller.GetEquipped((Weapon)itemToEquip);
                        controller.DisplayNarratorResponse("You equip the " + myTI.ToTitleCase(itemToEquip.nome) + ".");
                    }                    
                }
            }
            else if (itemToEquip is Armor)
            {
                if (controller.ego.equippedArmor != null && controller.ego.equippedArmor.nome == itemName) { controller.DisplayNarratorResponse("You're already using the " + myTI.ToTitleCase(itemName) + ", silly."); }
                else
                {
                    controller.GetDressed((Armor)itemToEquip);
                    controller.DisplayNarratorResponse("You equip the " + myTI.ToTitleCase(itemToEquip.nome) + ".");
                }
            }
            else if (itemToEquip is Shield)
            {
                if (controller.ego.equippedShield != null && controller.ego.equippedShield.nome == itemName) { controller.DisplayNarratorResponse("You're already using the " + myTI.ToTitleCase(itemName) + ", silly."); }
                else
                {
                    if (controller.ego.equippedWeapon != null)
                    {
                        if (controller.ego.equippedWeapon.twoHanded) { controller.DisplayNarratorResponse($"You can't equip the {myTI.ToTitleCase(itemToEquip.nome)} while wielding a two-handed weapon."); }
                        else
                        {
                            controller.GetStrapped((Shield)itemToEquip);
                            controller.DisplayNarratorResponse("You equip the " + myTI.ToTitleCase(itemToEquip.nome) + ".");
                        }
                    }
                    else
                    {
                        controller.GetStrapped((Shield)itemToEquip);
                        controller.DisplayNarratorResponse("You equip the " + myTI.ToTitleCase(itemToEquip.nome) + ".");
                    }
                    
                }
            }
            else { controller.DisplayNarratorResponse("No, you don't."); }
            
        }
        else if (controller.currentActiveInput == "inventory" && separatedInputWords.Length == 1 && !controller.hasBubbleLead)
        {
            string itemName = "bubble lead";
            Item itemToEquip = controller.ExtractItem(itemName);
            controller.GetEquipped((Weapon)itemToEquip);
            controller.DisplayNarratorResponse("Get equipped with Bubble Lead.");
        }
        else { controller.DisplayNarratorResponse("That didn't do anything useful."); }
    }
}
