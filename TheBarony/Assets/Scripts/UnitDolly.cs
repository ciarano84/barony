using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDolly : MonoBehaviour
{
    public TacticsMovement unit;
    
    // Start is called before the first frame update
    void Start()
    {
        unit = gameObject.transform.parent.GetComponent<TacticsMovement>();
        gameObject.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = unit.transform.position;
    }
}
