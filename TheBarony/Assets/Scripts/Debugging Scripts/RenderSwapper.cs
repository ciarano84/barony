using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RenderSwapper : MonoBehaviour
{
    public GameObject character;
    public Mesh replacement;

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("fired");
            character.GetComponent<SkinnedMeshRenderer>().sharedMesh = replacement;
        }
    }
}
