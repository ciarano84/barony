using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCamera
{
    static Unit unitCurrentlyFollowed;

    public static void FollowUnit(TacticsMovement unit)
    {
        if (unitCurrentlyFollowed != null)
        {
            unit.vcam.transform.position = unitCurrentlyFollowed.transform.position;
        }
        unit.vcam.MoveToTopOfPrioritySubqueue();
        unitCurrentlyFollowed = unit;
    }
}
