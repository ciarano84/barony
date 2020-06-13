using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour, IEquipable<Shield>
{
    public PlayerCharacter owner;

    public void Equip()
    {
        owner.unitInfo.defendModifier += 2;
    }

    public void Unequip()
    {
        owner.unitInfo.defendModifier -= 2;
    }
}
