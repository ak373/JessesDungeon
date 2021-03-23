using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jesse's Dungeon/InputActions/TalkTo")]
public class TalkTo : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if (/*controller.currentActiveInput == "main" && */separatedInputWords[1] == "to" && separatedInputWords.Length == 2) { controller.InitiateNarrator("You have a nice conversation with the wall. You bear a surprising brunt of the load considering its one job."); }
        else if (/*controller.currentActiveInput == "main" && */separatedInputWords[1] == "to" && separatedInputWords.Length > 2)
        {
            string NPCName = "";
            for (int i = 2; i < separatedInputWords.Length; i++)
            {
                NPCName += separatedInputWords[i] + " ";
            }
            NPCName = NPCName.Trim();
            NPC npc = null;
            for (int i = 0; i < controller.npcInteraction.allNPCs.Length; i++)
            {
                if (controller.npcInteraction.myTI.ToTitleCase(NPCName) == controller.npcInteraction.allNPCs[i].nome) { npc = controller.npcInteraction.allNPCs[i]; }
            }
            if (npc != null) { controller.npcInteraction.StartInitiateDialogue(npc); }
            else { controller.InitiateNarrator("No, you don't."); }

            //enter all npcs
            //if (NPCName == "badger")
            //{
                //controller.npcInteraction.InitiateDialogue(npc);
                //controller.currentActiveInput = "badger";
                //controller.npcTalk.BadgerMain();
            //}
            //else if (NPCName == "skinny pete")
            //{
                //controller.currentActiveInput = "skinny pete";
                //controller.npcTalk.SkinnyPeteMain();
            //}
            //else if (NPCName == "geoff")
            //{
                //controller.currentActiveInput = "geoff";
                //controller.npcTalk.GeoffMain();
            //}
            //enter npcs above
            //else { controller.InitiateNarrator("No, you don't."); }           
        }
        else { controller.InitiateNarrator("That didn't do anything useful."); }
    }
}