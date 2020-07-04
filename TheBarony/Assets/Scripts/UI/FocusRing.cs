using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusRing : MonoBehaviour
{
    public LineRenderer line;
    public GameObject ring;
    Vector3 hidePoint = new Vector3(0,-100,0);

    private void Update()
    {
        if (Initiative.currentUnit != null && Initiative.currentUnit.focus != null)
        {
            line.SetPosition(0, Initiative.currentUnit.transform.position + new Vector3(0, -Initiative.currentUnit.halfHeight + 0.2f));
            line.SetPosition(1, Initiative.currentUnit.focus.transform.position + new Vector3(0, -Initiative.currentUnit.focus.GetComponent<TacticsMovement>().halfHeight + 0.2f));
            ring.transform.position = Initiative.currentUnit.focus.transform.position + new Vector3(0, -Initiative.currentUnit.focus.GetComponent<TacticsMovement>().halfHeight);
        }
        else
        {
            line.SetPosition(0, hidePoint);
            line.SetPosition(1, hidePoint);
            ring.transform.position = hidePoint;
        }
    }
}

