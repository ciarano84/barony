using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusRing : MonoBehaviour
{
    public LineRenderer line;
    public GameObject ring;
    TacticsMovement startingCharacter;
    TacticsMovement finishingCharacter;
    bool focusSet = false;
    bool hidden = false;
    Vector3 hidePoint = new Vector3(0,-100,0);

    private void Update()
    {
        if (focusSet)
        {
            if (startingCharacter != null && finishingCharacter != null)
            {
                line.SetPosition(0, startingCharacter.transform.position + new Vector3(0, -startingCharacter.halfHeight + 0.2f));
                line.SetPosition(1, finishingCharacter.transform.position + new Vector3(0, -finishingCharacter.halfHeight + 0.2f));
                ring.transform.position = finishingCharacter.transform.position + new Vector3(0, -finishingCharacter.halfHeight);
            }
        }
        else
        {
            if (hidden)
            {
                return;
            }
            else
            {
                line.SetPosition(0, hidePoint);
                line.SetPosition(1, hidePoint);
                ring.transform.position = hidePoint;
                hidden = true;
            }
        }
    }

    public void SetFocus(TacticsMovement _startingCharacter, TacticsMovement _finishingCharacter)
    {
        hidden = false;
        focusSet = true;
        startingCharacter = _startingCharacter;
        finishingCharacter = _finishingCharacter;
    }

    public void NoFocus()
    {
        hidden = false;
        focusSet = false;
        startingCharacter = null;
        finishingCharacter = null;
    }
}

