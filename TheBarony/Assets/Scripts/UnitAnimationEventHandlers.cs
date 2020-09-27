using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEventHandlers : MonoBehaviour
{
    public Unit unit;

    public void AttackCalled()
    {
        unit.mainWeapon.AttackEvent();
    }
}
