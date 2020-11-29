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

    public void EndActionCalled()
    {
        
        StartCoroutine(EndAction());
    }

    IEnumerator EndAction()
    {
        yield return new WaitForSeconds(1f);
        unit.mainWeapon.EndAction();
        yield break;
    }
}
