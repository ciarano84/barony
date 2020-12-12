using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatScript : MonoBehaviour
{
    public EncounterManager encounterManager;

    public void Update()
    {
        //Campaign cheats (Right shift)

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

        //Equipment Cheats (left shift)

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
            GreataxeData axe = new GreataxeData();
            axe.SetData(Initiative.currentUnit.unitInfo, Slot.mainHand);
            axe.EquipItem(Initiative.currentUnit);
            BlankItemData blank = new BlankItemData();
            blank.SetData(Initiative.currentUnit.unitInfo, Slot.offHand);
        }

            //Cheat for Mace
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.M))
        {
            foreach (Transform child in Initiative.currentUnit.mainHandSlot)
            {
                Destroy(child.gameObject);
            }
            MaceData mace = new MaceData();
            mace.SetData(Initiative.currentUnit.unitInfo, Slot.mainHand);
            mace.EquipItem(Initiative.currentUnit);
        }

            //Cheat for Shortbow 
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.B))
        {
            foreach (Transform child in Initiative.currentUnit.mainHandSlot)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in Initiative.currentUnit.offHandSlot)
            {
                Destroy(child.gameObject);
            }
            ShortbowData bow = new ShortbowData();
            bow.SetData(Initiative.currentUnit.unitInfo, Slot.offHand);
            bow.EquipItem(Initiative.currentUnit);
        }


        //Stat Cheats (button1)

            //Defence
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.D))
        {
            Initiative.currentUnit.unitInfo.currentDefence += 30;
        }

        //auto-crit - all attacks will now automatically score at least one crit.
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("autocrit enabled");
            AttackManager.OnAttack += SetAutoCrit;
        }
    }

    void SetAutoCrit(Unit a, Unit d)
    {
        AttackManager.autocrit = true;
    }
}
