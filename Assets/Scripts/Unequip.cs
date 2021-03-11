using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

[CreateAssetMenu(menuName = "Jesse's Dungeon/InputActions/Unequip")]
public class Unequip : InputAction
{
    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
    //    controller.escToContinue = false;
    //    if (controller.currentActiveInput == "inventory" && separatedInputWords.Length >= 2)
    //    {
    //        string itemName = "";
    //        for (int i = 1; i < separatedInputWords.Length; i++)
    //        {
    //            itemName += separatedInputWords[i] + " ";
    //        }
    //        itemName = itemName.Trim();
    //        Item itemToUnequip = controller.ExtractItem(itemName);
    //        if (itemToUnequip is Weapon)
    //        {
    //            if (controller.ego.equippedWeapon == null || !(controller.ego.equippedWeapon.nome == itemName)) { controller.DisplayNarratorResponse("You aren't using the " + myTI.ToTitleCase(itemName) + ", silly."); }
    //            else
    //            {
    //                controller.GetUnEquipped();
    //                controller.DisplayNarratorResponse("You remove the " + myTI.ToTitleCase(itemToUnequip.nome) + ".");
    //            }
    //        }
    //        else if (itemToUnequip is Armor)
    //        {
    //            if (controller.ego.equippedArmor == null || !(controller.ego.equippedArmor.nome == itemName)) { controller.DisplayNarratorResponse("You aren't using the " + myTI.ToTitleCase(itemName) + ", silly."); }
    //            else
    //            {
    //                controller.GetUnDressed();
    //                controller.DisplayNarratorResponse("You remove the " + myTI.ToTitleCase(itemToUnequip.nome) + ".");
    //            }
    //        }
    //        else if (itemToUnequip is Shield)
    //        {
    //            if (controller.ego.equippedShield == null || !(controller.ego.equippedShield.nome == itemName)) { controller.DisplayNarratorResponse("You aren't using the " + myTI.ToTitleCase(itemName) + ", silly."); }
    //            else
    //            {
    //                controller.GetUnStrapped();
    //                controller.DisplayNarratorResponse("You remove the " + myTI.ToTitleCase(itemToUnequip.nome) + ".");
    //            }
    //        }
    //        else { controller.DisplayNarratorResponse("No, you don't."); }

    //    }
    //    else if (controller.currentActiveInput == "inventory" && separatedInputWords.Length == 1)
    //    {
    //        controller.DisplayNarratorResponse("You are now unequipped. Like my cat.");
    //    }
    //    else { controller.DisplayNarratorResponse("That didn't do anything useful."); }
    }
}