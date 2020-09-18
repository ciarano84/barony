﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatScript : MonoBehaviour
{
    public EncounterManager encounterManager;

    public void Update()
    {
        //Campaign cheats

        //Cheat for player victory
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.P))
        {
            RostaInfo.currentEncounter.completionState = Encounter.CompletionState.VICTORY;
            StartCoroutine(encounterManager.EncounterEnd(Factions.players));
        }

        //Cheat for enemy victory
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.E))
        {
            encounterManager.playerSquad.Clear();
            RostaInfo.currentEncounter.completionState = Encounter.CompletionState.DEFEAT;
            StartCoroutine(encounterManager.EncounterEnd(Factions.enemies));
        }

        //Equipment Cheats

        //Cheat for greataxe
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.G))
        {
            foreach (Transform child in Initiative.currentUnit.mainHandSlot)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in Initiative.currentUnit.offHandSlot)
            {
                Destroy(child.gameObject);
            }



            //Destroy(Initiative.currentUnit.mainWeapon.itemData.itemModel);
            GreataxeData axe = new GreataxeData();
            axe.SetData(Initiative.currentUnit.unitInfo, Slot.mainHand);
            axe.EquipItem(Initiative.currentUnit);
            BlankItemData blank = new BlankItemData();
            blank.SetData(Initiative.currentUnit.unitInfo, Slot.offHand);
        }

        //Stat Cheats
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.D))
        {
            Initiative.currentUnit.unitInfo.currentDefence += 30;
        }
    }
}
