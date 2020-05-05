using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE {OPEN, OCCUPIED, BLOCKED, DIFFICULT};

public class Tile : MonoBehaviour
{
    public Vector3 basepoint;
    public Troop occupyingTroop = null;

    private void Start()
    {
        basepoint = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
    }
}
