using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfTurnFocusRing : MonoBehaviour
{
    public LineRenderer line;
    public GameObject ring;
    Vector3 hidePoint = new Vector3(0, -100, 0);

    private void Update()
    {
        if (Unit.mousedOverUnit != null && Unit.mousedOverUnit.focus != null)
        {
            line.SetPosition(0, Unit.mousedOverUnit.transform.position + new Vector3(0, 0.2f));
            line.SetPosition(1, Unit.mousedOverUnit.focus.transform.position + new Vector3(0, 0.2f));
            ring.transform.position = Unit.mousedOverUnit.focus.transform.position;
        }
        else
        {
            line.SetPosition(0, hidePoint);
            line.SetPosition(1, hidePoint);
            ring.transform.position = hidePoint;
        }
    }
}
