using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusRing : MonoBehaviour
{
    public LineRenderer line;
    public GameObject ring;
    public Unit startingCharacter;
    public Unit finishingCharacter;

    void Start()
    {
        ring.transform.position = finishingCharacter.transform.position + new Vector3(0, -finishingCharacter.GetComponent<TacticsMovement>().halfHeight);
        line.SetPosition(0, startingCharacter.transform.position + new Vector3(0, -startingCharacter.GetComponent<TacticsMovement>().halfHeight + 0.2f));
        line.SetPosition(1, finishingCharacter.transform.position + new Vector3(0, -finishingCharacter.GetComponent<TacticsMovement>().halfHeight + 0.2f));
    }
}
