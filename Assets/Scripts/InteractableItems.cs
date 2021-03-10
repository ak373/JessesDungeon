using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using TMPro;

public class InteractableItems : MonoBehaviour
{
    public TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
    [HideInInspector] public List<Item> inventory = new List<Item>();
    [HideInInspector] public List<string> notYetSearched = new List<string>();
    [HideInInspector] public List<string> alreadySearched = new List<string>();
    //public GameObject inventoryStats;
    //public TMP_Text potion0, potion1, potion2;
    public Ego ego;
    GameController controller;
    [HideInInspector] public bool traySearch = false;

    //new inventory display
    public GameObject invDisplay, invDisplayBorder, invOptions, invOptionsBorder;
    public TMP_Text invText;
    public TMP_Text[] invActions;
    public AudioSource cursorMove, cursorCancel, cursorSelect;

    GameObject invStat, invDamage, invCritMultiplier, invToHitMod, invArmorClass, invCritResist, invDmgReduction;
    TMP_Text invDamageText, invCritMultiplierText, invToHitModText, invArmorClassText, invCritResistText, invDmgReductionText;
    
    Color darkGrey = new Color(0.09411765f, 0.09411765f, 0.09411765f);

    GameObject potionBelt, equipment, equippedWeapon, weapon, equippedArmor, armor, equippedShield, shield, potion0, potion1, potion2, viewDeeds;
    TMP_Text potion0Text, potion1Text, potion2Text, weaponText, armorText, shieldText, viewDeedsText;

    //
    public Dictionary<string, string[]> lookAtDictionary = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> searchDictionary = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> listenToDictionary = new Dictionary<string, string[]>();

    private void Awake()
    {
        controller = GetComponent<GameController>();

        invStat = invDisplay.transform.Find("InventoryStat").gameObject;
        invDamage = invStat.transform.Find("DamageNumber").gameObject;
        invDamageText = invDamage.transform.GetComponent<TMP_Text>();
        invCritMultiplier = invStat.transform.Find("CritMultiplierNumber").gameObject;
        invCritMultiplierText = invCritMultiplier.transform.GetComponent<TMP_Text>();
        invToHitMod = invStat.transform.Find("ToHitNumber").gameObject;
        invToHitModText = invToHitMod.transform.GetComponent<TMP_Text>();
        invArmorClass = invStat.transform.Find("ArmorClassNumber").gameObject;
        invArmorClassText = invArmorClass.transform.GetComponent<TMP_Text>();
        invCritResist = invStat.transform.Find("CritResistNumber").gameObject;
        invCritResistText = invCritResist.transform.GetComponent<TMP_Text>();
        invDmgReduction = invStat.transform.Find("DmgReductionNumber").gameObject;
        invDmgReductionText = invDmgReduction.transform.GetComponent<TMP_Text>();

        equipment = invDisplay.transform.Find("Equipment").gameObject;
        equippedWeapon = equipment.transform.Find("EquippedWeapon").gameObject;
        weapon = equippedWeapon.transform.Find("Weapon").gameObject;
        weaponText = weapon.transform.GetComponent<TMP_Text>();
        equippedArmor = equipment.transform.Find("EquippedArmor").gameObject;
        armor = equippedArmor.transform.Find("Armor").gameObject;
        armorText = armor.transform.GetComponent<TMP_Text>();
        equippedShield = equipment.transform.Find("EquippedShield").gameObject;
        shield = equippedShield.transform.Find("Shield").gameObject;
        shieldText = shield.transform.GetComponent<TMP_Text>();

        potionBelt = invDisplay.transform.Find("PotionBelt").gameObject;
        potion0 = potionBelt.transform.Find("Potion0").gameObject;
        potion0Text = potion0.transform.GetComponent<TMP_Text>();
        potion1 = potionBelt.transform.Find("Potion1").gameObject;
        potion1Text = potion1.transform.GetComponent<TMP_Text>();
        potion2 = potionBelt.transform.Find("Potion2").gameObject;
        potion2Text = potion2.transform.GetComponent<TMP_Text>();

        viewDeeds = invDisplay.transform.Find("viewDeeds").gameObject;
        viewDeedsText = viewDeeds.transform.GetComponent<TMP_Text>();
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
    public IEnumerator DisplayInventory()
    {
        string toPassIn;
        List<Item> alreadyListed = new List<Item>();

        controller.inputBox.SetActive(false);
        invDisplay.SetActive(true);
        invDisplayBorder.SetActive(true);
        DisplayPotionBelt();
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
        invText.text = toPassIn;
        void DisplayPotionBelt()
        {
            potion0Text.text = "";
            potion1Text.text = "";
            potion2Text.text = "";
            if (ego.potionBelt.Count > 0) { potion0Text.text = myTI.ToTitleCase(ego.potionBelt[0].nome); }
            if (ego.potionBelt.Count > 1) { potion1Text.text = myTI.ToTitleCase(ego.potionBelt[1].nome); }
            if (ego.potionBelt.Count > 2) { potion2Text.text = myTI.ToTitleCase(ego.potionBelt[2].nome); }
        }


        if (inventory.Count > 0 || ego.equippedWeapon != null || ego.equippedArmor != null || ego.equippedShield != null || ego.potionBelt.Count > 0)
        {
            bool skipToEquipment = false;
            if (inventory.Count <= 0) { skipToEquipment = true; }
            string normalInvText = invText.text;
            Item selectedItem = alreadyListed[0];
            int selectedElement = 0;
            int memoryElement = 0;
            bool itemUsed = false;
            bool dropUsed = false;
            bool doubleBreak = false;
            while (true)
            {
                if (!skipToEquipment)
                {
                    invText.text = normalInvText;
                    if (selectedElement < 0) { selectedElement = alreadyListed.Count - 1; }
                    if (selectedElement > alreadyListed.Count - 1) { selectedElement = 0; }
                    int itemLength = alreadyListed[selectedElement].nome.Length;
                    int invIndex = 0;
                    invIndex = invText.text.IndexOf(myTI.ToTitleCase(alreadyListed[selectedElement].nome));

                    string newText = "";

                    for (int i = 0; i < invIndex; i++) { newText += invText.text[i]; }

                    newText += "<color=yellow>";

                    for (int i = invIndex; i < invIndex + itemLength; i++) { newText += invText.text[i]; }

                    newText += "</color>";

                    for (int i = invIndex + itemLength; i < invText.text.Length; i++) { newText += invText.text[i]; }

                    invText.text = newText;

                    InvStats(alreadyListed[selectedElement]);

                    yield return new WaitUntil(controller.RightUpDownEnterEscPressed);
                }
                
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    cursorMove.Play();
                    selectedElement--;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    cursorMove.Play();
                    selectedElement++;
                }
                //Right Side
                else if (Input.GetKeyDown(KeyCode.RightArrow) || skipToEquipment)
                {
                    if (skipToEquipment) { skipToEquipment = false; }
                    else { cursorMove.Play(); }
                    invText.text = normalInvText;
                    memoryElement = selectedElement;
                    int equipmentElement = 0;
                    string plainPotion0 = potion0Text.text;
                    string plainPotion1 = potion1Text.text;
                    string plainPotion2 = potion2Text.text;
                    string plainWeapon = weaponText.text;
                    string plainArmor = armorText.text;
                    string plainShield = shieldText.text;
                    string plainDeeds = viewDeedsText.text;

                    while (true)
                    {
                        if (equipmentElement < 0) { equipmentElement = 6; }
                        if (equipmentElement > 6) { equipmentElement = 0; }

                        potion0Text.text = plainPotion0;
                        potion1Text.text = plainPotion1;
                        potion2Text.text = plainPotion2;
                        weaponText.text = plainWeapon;
                        armorText.text = plainArmor;
                        shieldText.text = plainShield;
                        viewDeedsText.text = plainDeeds;

                        if (equipmentElement == 0) { weaponText.text = $"<color=yellow>{weaponText.text}</color>"; }
                        else if (equipmentElement == 1) { armorText.text = $"<color=yellow>{armorText.text}</color>"; }
                        else if (equipmentElement == 2) { shieldText.text = $"<color=yellow>{shieldText.text}</color>"; }
                        else if (equipmentElement == 3) { potion0Text.text = $"<color=yellow>{potion0Text.text}</color>"; }
                        else if (equipmentElement == 4) { potion1Text.text = $"<color=yellow>{potion1Text.text}</color>"; }
                        else if (equipmentElement == 5) { potion2Text.text = $"<color=yellow>{potion2Text.text}</color>"; }
                        else if (equipmentElement == 6) { viewDeedsText.text = $"<color=yellow>{viewDeedsText.text}</color>"; }

                        yield return new WaitUntil(controller.LeftUpDownEnterEscPressed);
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            cursorMove.Play();
                            equipmentElement--;
                            if (ego.potionBelt.Count <= 2 && equipmentElement == 5) { equipmentElement = 2; }
                            if (ego.potionBelt.Count <= 1 && equipmentElement == 4) { equipmentElement = 2; }
                            if (ego.potionBelt.Count == 0 && equipmentElement == 3) { equipmentElement = 2; }
                            if (ego.equippedShield == null && equipmentElement == 2) { equipmentElement--; }
                            if (ego.equippedArmor == null && equipmentElement == 1) { equipmentElement--; }
                            if (ego.equippedWeapon == null && equipmentElement == 0) { equipmentElement--; }
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            cursorMove.Play();
                            equipmentElement++;
                            if (ego.equippedWeapon == null && equipmentElement == 0) { equipmentElement++; }
                            if (ego.equippedArmor == null && equipmentElement == 1) { equipmentElement++; }
                            if (ego.equippedShield == null && equipmentElement == 2) { equipmentElement++; }
                            if (ego.potionBelt.Count == 0 && equipmentElement == 3) { equipmentElement = 6; }
                            if (ego.potionBelt.Count == 1 && equipmentElement == 4) { equipmentElement = 6; }
                            if (ego.potionBelt.Count == 2 && equipmentElement == 5) { equipmentElement = 6; }
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            cursorMove.Play();
                            if (inventory.Count > 0)
                            {
                                potion0Text.text = plainPotion0;
                                potion1Text.text = plainPotion1;
                                potion2Text.text = plainPotion2;
                                weaponText.text = plainWeapon;
                                armorText.text = plainArmor;
                                shieldText.text = plainShield;
                                viewDeedsText.text = plainDeeds;
                                selectedElement = memoryElement;
                                break;
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            cursorCancel.Play();
                            potion0Text.text = plainPotion0;
                            potion1Text.text = plainPotion1;
                            potion2Text.text = plainPotion2;
                            weaponText.text = plainWeapon;
                            armorText.text = plainArmor;
                            shieldText.text = plainShield;
                            viewDeedsText.text = plainDeeds;
                            invDisplay.SetActive(false);
                            invDisplayBorder.SetActive(false);
                            controller.inputBox.SetActive(true);
                            controller.textInput.inputField.ActivateInputField();
                            controller.textInput.inputField.text = null;
                            doubleBreak = true;
                            break;
                        }
                        else if (Input.GetKeyDown(KeyCode.Return))
                        {
                            invOptions.SetActive(true);
                            invOptionsBorder.SetActive(true);
                            cursorSelect.Play();
                            if (equipmentElement == 0) { selectedItem = ego.equippedWeapon; }
                            else if (equipmentElement == 1) { selectedItem = ego.equippedArmor; }
                            else if (equipmentElement == 2) { selectedItem = ego.equippedShield; }
                            else if (equipmentElement == 6) { controller.achievements.InitiateDisplayAchievements(); }
                            else { selectedItem = ego.potionBelt[equipmentElement]; }

                            int option = 0;
                            if (selectedItem is Potion) { invActions[1].color = darkGrey; }
                            else { invActions[1].text = "Unequip"; }
                            bool useUsed = false;
                            while (true)
                            {
                                invOptions.SetActive(true);
                                invOptionsBorder.SetActive(true);
                                if (option < 0) { option = 3; }
                                if (option > 3) { option = 0; }
                                yield return new WaitForSeconds(.01f);
                                invActions[option].color = Color.yellow;
                                yield return new WaitUntil(controller.UpDownEnterEscPressed);
                                if (Input.GetKeyDown(KeyCode.UpArrow))
                                {
                                    cursorMove.Play();
                                    invActions[option].color = Color.white;
                                    option--;
                                    if (selectedItem is Potion && option == 1) { option = 0; }
                                }
                                else if (Input.GetKeyDown(KeyCode.DownArrow))
                                {
                                    cursorMove.Play();
                                    invActions[option].color = Color.white;
                                    option++;
                                    if (selectedItem is Potion && option == 1) { option = 2; }
                                }
                                else if (Input.GetKeyDown(KeyCode.Escape))
                                {
                                    cursorCancel.Play();
                                    invActions[option].color = Color.white;
                                    invOptions.SetActive(false);
                                    invOptionsBorder.SetActive(false);
                                    invActions[1].text = "Equip";
                                    break;
                                }
                                else if (Input.GetKeyDown(KeyCode.Return))
                                {
                                    cursorSelect.Play();
                                    invActions[option].color = Color.white;
                                    //Inspect
                                    if (option == 0)
                                    {
                                        Inspect(selectedItem);
                                    }
                                    //Unequip
                                    if (option == 1)
                                    {
                                        invActions[1].text = "Equip";
                                        invOptions.SetActive(false);
                                        invOptionsBorder.SetActive(false);
                                        if (selectedItem is Weapon) { controller.GetUnEquipped(); }
                                        if (selectedItem is Armor) { controller.GetUnDressed(); }
                                        if (selectedItem is Shield) { controller.GetUnStrapped(); }
                                        controller.OpenPopUpWindow($"", "", $"You unequip the {myTI.ToTitleCase(selectedItem.nome)}.", "", "", "", "", "Press ESC to return");
                                        controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                        controller.popUpMessage.font = controller.achievements.originalFont;
                                        yield return new WaitUntil(controller.EscPressed);
                                        cursorCancel.Play();
                                        controller.ClosePopUpWindow();
                                        break;
                                    }
                                    //Use
                                    if (option == 2)
                                    {
                                        invActions[1].text = "Equip";
                                        invDisplay.SetActive(false);
                                        invDisplayBorder.SetActive(false);
                                        invOptions.SetActive(false);
                                        invOptionsBorder.SetActive(false);
                                        Use(selectedItem);
                                        break;
                                    }
                                    //Drop
                                    else if (option == 3)
                                    {
                                        invActions[1].text = "Equip";
                                        bool yesSelected = false;
                                        while (true)
                                        {
                                            if (yesSelected) { controller.OpenPopUpWindow($"Drop {selectedItem.nome}?", "", "This action cannot be undone.", "", "<b>[Yes]</b><color=white>. I'm not afraid.</color>", "", "<color=white>No! Take me back!</color>", ""); }
                                            else { controller.OpenPopUpWindow($"Drop {selectedItem.nome}?", "", "This action cannot be undone.", "", "<color=white>Yes. I'm not afraid.</color>", "", "<b>[No]</b><color=white>! Take me back!</color>", ""); }
                                            yield return new WaitUntil(controller.LeftRightEnterPressed);
                                            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                                            {
                                                cursorMove.Play();
                                                yesSelected = !yesSelected;
                                            }
                                            else if (Input.GetKeyDown(KeyCode.Return))
                                            {
                                                cursorSelect.Play();
                                                if (yesSelected)
                                                {
                                                    controller.OpenPopUpWindow($"", "", $"You drop the {myTI.ToTitleCase(selectedItem.nome)}.", "", "", "", "", "Press ESC to return");
                                                    controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                    if (selectedItem is Potion) { ego.potionBelt.Remove((Potion)selectedItem); }
                                                    else { controller.interactableItems.inventory.Remove(selectedItem); }
                                                    controller.popUpMessage.font = controller.achievements.originalFont;
                                                    dropUsed = true;
                                                    yield return new WaitUntil(controller.EscPressed);
                                                    cursorCancel.Play();
                                                }
                                                controller.ClosePopUpWindow();
                                                break;
                                            }
                                        }
                                    }
                                }
                                //boolean gatekeepers
                                if (useUsed)
                                {
                                    useUsed = false;
                                    itemUsed = true;
                                    break;
                                }
                                if (dropUsed)
                                {
                                    itemUsed = true;
                                    break;
                                }
                            }
                        }
                        if (doubleBreak)
                        {
                            doubleBreak = false;
                            break;
                        }
                    }
                }
                //End Right Side
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    cursorCancel.Play();
                    invDisplay.SetActive(false);
                    invDisplayBorder.SetActive(false);
                    controller.inputBox.SetActive(true);
                    controller.textInput.inputField.ActivateInputField();
                    controller.textInput.inputField.text = null;
                    break;
                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    cursorSelect.Play();
                    selectedItem = alreadyListed[selectedElement];
                    int option = 0;
                    invActions[0].color = Color.white;
                    if (selectedItem is Undroppable || selectedItem is Potion)
                    {
                        invActions[1].color = darkGrey;
                    }
                    bool useUsed = false;
                    while (true)
                    {
                        invOptions.SetActive(true);
                        invOptionsBorder.SetActive(true);
                        if (option < 0) { option = 3; }
                        if (option > 3) { option = 0; }
                        yield return new WaitForSeconds(.01f);
                        invActions[option].color = Color.yellow;
                        yield return new WaitUntil(controller.UpDownEnterEscPressed);
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            cursorMove.Play();
                            invActions[option].color = Color.white;
                            option--;
                            if (selectedItem is Undroppable) { if (option == 1) { option = 0; } }
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            cursorMove.Play();
                            invActions[option].color = Color.white;
                            option++;
                            if (selectedItem is Undroppable) { if (option == 1) { option = 2; } }
                        }
                        else if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            cursorCancel.Play();
                            invActions[option].color = Color.white;
                            invOptions.SetActive(false);
                            invOptionsBorder.SetActive(false);
                            break;
                        }
                        else if (Input.GetKeyDown(KeyCode.Return))
                        {
                            cursorSelect.Play();
                            invActions[option].color = Color.white;
                            invOptions.SetActive(false);
                            invOptionsBorder.SetActive(false);
                            //Inspect
                            if (option == 0)
                            {
                                Inspect(selectedItem);
                            }
                            //Equip
                            if (option == 1)
                            {
                                //exceptions for shield/two-handed weapons
                                if (ego.equippedWeapon != null)
                                {
                                    //two handed weapon blocking shield
                                    if (selectedItem is Shield && ego.equippedWeapon.twoHanded)
                                    {
                                        controller.OpenPopUpWindow("", "", $"You can't equip the {myTI.ToTitleCase(selectedItem.nome)} while wielding a two-handed weapon.", "", "", "", "", "Press ESC to return");
                                        controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                        yield return new WaitUntil(controller.EscPressed);
                                        cursorCancel.Play();
                                        controller.ClosePopUpWindow();
                                        break;
                                    }
                                }
                                if (ego.equippedShield != null && selectedItem is Weapon)
                                {
                                    //two handed weapon unequiping both slots
                                    Weapon equippingWeapon = (Weapon)selectedItem;
                                    if (equippingWeapon.twoHanded)
                                    {
                                        bool yesSelected = false;
                                        while (true)
                                        {
                                            //failedEquip = false;
                                            if (yesSelected) { controller.OpenPopUpWindow("", "", "This weapon is two-handed, so your shield will also be unequipped.", "", "<b>[Yeah]</b><color=white>, duh.</color>", "", "<color=white>No! As if!</color>", ""); }
                                            else { controller.OpenPopUpWindow("", "", "This weapon is two-handed, so your shield will also be unequipped.", "", "<color=white>Yeah, duh.</color>", "", "<b>[No]</b><color=white>! As if!</color>", ""); }
                                            yield return new WaitUntil(controller.LeftRightEnterPressed);
                                            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                                            {
                                                cursorMove.Play();
                                                yesSelected = !yesSelected;
                                            }
                                            else if (Input.GetKeyDown(KeyCode.Return))
                                            {
                                                cursorSelect.Play();
                                                controller.ClosePopUpWindow();
                                                if (yesSelected) { controller.GetUnStrapped(); }
                                                //else { failedEquip = true; }
                                                break;
                                            }
                                        }
                                    }
                                }
                                //if (!failedEquip)
                                //{
                                //    itemUsed = true;
                                //}
                                controller.GetEquipped((Weapon)selectedItem);
                                break;
                            }
                            //Use
                            else if (option == 2)
                            {
                                invDisplay.SetActive(false);
                                invDisplayBorder.SetActive(false);
                                Use(selectedItem);
                                break;
                            }
                            //Drop
                            else if (option == 3)
                            {
                                bool yesSelected = false;
                                while (true)
                                {
                                    if (yesSelected) { controller.OpenPopUpWindow($"Drop {selectedItem.nome}?", "", "This action cannot be undone.", "", "<b>[Yes]</b><color=white>. I'm not afraid.</color>", "", "<color=white>No! Take me back!</color>", ""); }
                                    else { controller.OpenPopUpWindow($"Drop {selectedItem.nome}?", "", "This action cannot be undone.", "", "<color=white>Yes. I'm not afraid.</color>", "", "<b>[No]</b><color=white>! Take me back!</color>", ""); }
                                    yield return new WaitUntil(controller.LeftRightEnterPressed);
                                    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                                    {
                                        cursorMove.Play();
                                        yesSelected = !yesSelected;
                                    }
                                    else if (Input.GetKeyDown(KeyCode.Return))
                                    {
                                        cursorSelect.Play();
                                        if (yesSelected)
                                        {
                                            if (selectedItem is Undroppable)
                                            {
                                                controller.OpenPopUpWindow($"Oops!", "", "You dropped something that we weren't expecting. Whatever it was, you needed it to finish the game.\n\nError 4. SCI Version 1.001.050", "", "", "", "", "Press ESC to exit");
                                                controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                yield return new WaitUntil(controller.EscPressed);
                                                controller.OpenPopUpWindow($"Just Joshin'", "", $"Okay not really, but you know you shouldn't drop that. If you can't figure out what to do with it, try taking a different approach to the situation.", "", "", "", "", "Press ESC to return");
                                                controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                yield return new WaitForSeconds(.25f);
                                            }
                                            else
                                            {
                                                controller.OpenPopUpWindow($"", "", $"You drop the {myTI.ToTitleCase(selectedItem.nome)}.", "", "", "", "", "Press ESC to return");
                                                controller.popUpMessage.font = controller.achievements.deedDescriptionFont;
                                                controller.interactableItems.inventory.Remove(selectedItem);
                                            }
                                            controller.popUpMessage.font = controller.achievements.originalFont;
                                            dropUsed = true;
                                            yield return new WaitUntil(controller.EscPressed);
                                        }
                                        controller.ClosePopUpWindow();
                                        break;
                                    }
                                }
                            }
                        }
                        //boolean gatekeepers
                        if (useUsed)
                        {
                            useUsed = false;
                            itemUsed = true;
                            break;
                        }
                        if (dropUsed)
                        {
                            itemUsed = true;
                            break;
                        }
                    }
                }
                if (doubleBreak)
                {
                    doubleBreak = false;
                    break;
                }
                if (itemUsed)
                {
                    itemUsed = false;
                    if (dropUsed)
                    {
                        dropUsed = false;
                        StartCoroutine(DisplayInventory());
                    }
                    break;
                }
            }
        }
        else
        {
            yield return new WaitUntil(controller.EscPressed);
            cursorCancel.Play();
            invDisplay.SetActive(false);
            invDisplayBorder.SetActive(false);
            controller.inputBox.SetActive(true);
            controller.textInput.inputField.ActivateInputField();
            controller.textInput.inputField.text = null;
        }

        IEnumerator Inspect(Item itemInspected)
        {
            controller.OpenPopUpWindow();
            controller.WriteItemToPopUpWindow(itemInspected);
            yield return new WaitUntil(controller.EscPressed);
            cursorCancel.Play();
            controller.ClosePopUpWindow();
        }
        IEnumerator Use(Item itemUsed)
        {
            string verb = "use";
            if (itemUsed is Potion && itemUsed.beneficial) { verb = "drink"; }

            controller.AddToMainWindowWithLine($"You {verb} the {myTI.ToTitleCase(itemUsed.nome)}, and should look up the effects.");
            yield return new WaitUntil(controller.EnterPressed);
        }
    }
    void InvStats(Item itemSelected)
    {        
        //damage die & damage
        int egoDamageDie = (int)(ego.allStats[6].value + ego.allStats[6].effectValue);
        int withItemDamageDie = egoDamageDie;
        int egoDamage = (int)(ego.allStats[7].value + ego.allStats[7].effectValue);
        int withItemDamage = egoDamage;
        int egoCurrentWeaponDamage = 0;
        if (ego.equippedWeapon != null) { egoCurrentWeaponDamage = controller.ego.equippedWeapon.damage; }
        if (itemSelected is Weapon)
        {
            Weapon weaponSelected = (Weapon)itemSelected;
            withItemDamage = egoDamage - egoCurrentWeaponDamage + weaponSelected.damage;
            withItemDamageDie = (int)ego.allStats[6].effectValue + weaponSelected.damageDie;
        }
        string dieColor = "white";
        string damageColor = "white";
        string damageSign = "+";
        if (egoDamageDie < withItemDamageDie) { dieColor = "green"; }
        if (egoDamageDie > withItemDamageDie) { dieColor = "red"; }
        if (egoDamage < withItemDamage) { damageColor = "green"; }
        if (egoDamage > withItemDamage) { damageColor = "red"; }
        if (withItemDamage < 0) { damageSign = ""; }
        invDamageText.text = $"<color={dieColor}>1d{withItemDamageDie}</color> <color={damageColor}>{damageSign}{withItemDamage}</color>";

        //critMultiplier
        float egoCritMultiplier = ego.allStats[8].value + ego.allStats[8].effectValue;
        float withItemCritMultiplier = egoCritMultiplier;
        if (itemSelected is Weapon)
        {
            Weapon weaponSelected = (Weapon)itemSelected;
            withItemCritMultiplier = ego.allStats[8].effectValue + weaponSelected.critMultiplier;
        }
        string critMultiplierColor = "white";
        if (egoCritMultiplier < withItemCritMultiplier) { critMultiplierColor = "green"; }
        if (egoCritMultiplier > withItemCritMultiplier) { critMultiplierColor = "red"; }

        invCritMultiplierText.text = $"<color={critMultiplierColor}>x{withItemCritMultiplier}</color>";

        //toHitMod
        int egoToHitMod = (int)(ego.allStats[9].value + ego.allStats[9].effectValue);
        int withItemToHitMod = egoToHitMod;
        int egoCurrentWeaponToHitMod = 0;
        if (ego.equippedWeapon != null) { egoCurrentWeaponToHitMod = controller.ego.equippedWeapon.toHitMod; }
        if (itemSelected is Weapon)
        {
            Weapon weaponSelected = (Weapon)itemSelected;
            withItemToHitMod = egoToHitMod - egoCurrentWeaponToHitMod + weaponSelected.toHitMod;
        }
        string toHitModColor = "white";
        if (egoToHitMod < withItemToHitMod) { toHitModColor = "green"; }
        if (egoToHitMod > withItemToHitMod) { toHitModColor = "red"; }

        if (withItemToHitMod < 0) { invToHitModText.text = $"<color={toHitModColor}>{withItemToHitMod}</color>"; }
        else { invToHitModText.text = $"<color={toHitModColor}>+{withItemToHitMod}</color>"; }



        //armorClass
        int egoArmorClass = (int)(ego.allStats[3].value + ego.allStats[3].effectValue);
        int withItemArmorClass = (int)egoArmorClass;
        int egoCurrentShieldArmorClass = 0;
        if (ego.equippedShield != null) { egoCurrentShieldArmorClass = controller.ego.equippedShield.armorClass; }
        if (itemSelected is Shield)
        {
            Shield shieldSelected = (Shield)itemSelected;
            withItemArmorClass = egoArmorClass - egoCurrentShieldArmorClass + shieldSelected.armorClass;
        }
        string armorClassColor = "white";
        if (egoArmorClass < withItemArmorClass) { armorClassColor = "green"; }
        if (egoArmorClass > withItemArmorClass) { armorClassColor = "red"; }

        invArmorClassText.text = $"<color={armorClassColor}>{withItemArmorClass}</color>";

        //critResist
        float egoCritResist = ego.allStats[4].value - ego.allStats[4].effectValue;
        float withItemCritResist = egoCritResist;
        float egoCurrentArmorCritResist = 1;
        float egoCurrentShieldCritResist = 1;
        if (ego.equippedArmor != null) { egoCurrentArmorCritResist = controller.ego.equippedArmor.critResist; }
        if (ego.equippedShield != null) { egoCurrentShieldCritResist = controller.ego.equippedShield.critResist; }
        if (itemSelected is Armor)
        {
            Armor armorSelected = (Armor)itemSelected;
            withItemCritResist = armorSelected.critResist - (1 - egoCurrentShieldCritResist) - ego.allStats[4].effectValue;
        }
        else if (itemSelected is Shield)
        {
            Shield shieldSelected = (Shield)itemSelected;
            withItemCritResist = shieldSelected.critResist - (1 - egoCurrentArmorCritResist) - ego.allStats[4].effectValue;
        }
        string critResistColor = "white";
        if (egoCritResist > withItemCritResist) { critResistColor = "green"; }
        if (egoCritResist < withItemCritResist) { critResistColor = "red"; }

        invCritResistText.text = $"<color={critResistColor}>x{withItemCritResist}</color>";

        //dmgReduction
        int egoDmgReduction = (int)(ego.allStats[5].value + ego.allStats[5].effectValue);
        int withItemDmgReduction = (int)egoDmgReduction;
        int egoCurrentArmorDmgReduction = 0;
        if (ego.equippedArmor != null) { egoCurrentArmorDmgReduction = controller.ego.equippedArmor.damageReduction; }
        if (itemSelected is Armor)
        {
            Armor armorSelected = (Armor)itemSelected;
            withItemDmgReduction = egoDmgReduction - egoCurrentArmorDmgReduction + armorSelected.damageReduction;
        }
        string dmgReductionColor = "white";
        if (egoDmgReduction < withItemDmgReduction) { dmgReductionColor = "green"; }
        if (egoDmgReduction > withItemDmgReduction) { dmgReductionColor = "red"; }

        if (withItemDmgReduction >= 0) { invDmgReductionText.text = $"<color={dmgReductionColor}>-{withItemDmgReduction}</color>"; }
        else { invDmgReductionText.text = $"<color={dmgReductionColor}>+{Mathf.Abs(withItemDmgReduction)}</color>"; }
    }
    //
    //
    //
    //old version
    //public void DisplayInventory()
    //{
    //    string toPassIn;
    //    List<Item> alreadyListed = new List<Item>();

    //    controller.currentActiveInput = "inventory";
    //    inventoryStats.SetActive(true);
    //    DisplayPotionBelt();
    //    alreadyListed.Clear();
    //    toPassIn = "";
    //    if (inventory.Count == 0) { toPassIn += "Your inventory is empty! How sad.\n"; }
    //    else
    //    {
    //        for (int i = 0; i < inventory.Count; i++)
    //        {
    //            if (alreadyListed.Contains(inventory[i])) { continue; }
    //            else
    //            {
    //                int counter = 0;
    //                for (int j = i; j < inventory.Count; j++)
    //                {
    //                    if (inventory[i] == inventory[j]) { counter++; }
    //                }
    //                alreadyListed.Add(inventory[i]);
    //                string total = counter.ToString();
    //                toPassIn += total + " " + myTI.ToTitleCase(inventory[i].nome) + "\n";
    //            }                
    //        }
    //    }
    //    toPassIn += "\n\n-------------------------------------\nInspect\nEquip\nDrop\nUse\n\nAchievements\n\nPress ESC to return";
    //    controller.escToContinue = true;
    //    controller.OverwriteMainWindow(toPassIn);        
    //}
    //public void DisplayPotionBelt()
    //{
    //    potion0.text = "";
    //    potion1.text = "";
    //    potion2.text = "";
    //    if (ego.potionBelt[0] != null) { potion0.text = ego.potionBelt[0].nome; }
    //    if (ego.potionBelt[1] != null) { potion1.text = ego.potionBelt[1].nome; }
    //    if (ego.potionBelt[2] != null) { potion2.text = ego.potionBelt[2].nome; }
    //}
    string ChangeColor(string text, Color color)
    {
        return $"<color={color}>{text}</color>";
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
                StartCoroutine(controller.Narrator("Unfortunately, searching that did not aid you in your quest. A waste of time even to describe it!"));
                return null;
            }
        }
        else { return null; }
    }
}
