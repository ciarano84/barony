using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public TacticsMovement owner;

    private void Start()
    {
        owner = GetComponent<TacticsMovement>();
    }

    public IEnumerator Attack()
    {
        owner.unitAnim.SetTrigger("melee");
        owner.remainingActions--;
        yield return new WaitForSeconds(1f);
        Initiative.CheckForTurnEnd(owner);
        yield break;
    }
}
