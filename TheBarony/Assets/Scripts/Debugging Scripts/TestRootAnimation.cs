using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRootAnimation : MonoBehaviour
{
    public Animator anim;

    public void OnMouseDown()
    {
        Debug.Log("onmousedown triggered");
        anim.SetTrigger("turnRight");
    }
}
