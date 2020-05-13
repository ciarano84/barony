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

    public void Attack()
    {
        Gizmos.DrawIcon(transform.position, "Light Gizmo.tiff", true);
        owner.remainingActions--;
        Initiative.CheckForTurnEnd(owner);
    }
}
