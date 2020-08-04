using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSave 
{
    public float x;
    public float y;
    public float z;

    public static TransformSave StoreTransform(GameObject go)
    {
        TransformSave t = new TransformSave();
        t.x = go.transform.position.x;
        t.y = go.transform.position.y;
        t.z = go.transform.position.z;
        return t;
    }

    public static Transform ReturnTransform(TransformSave save)
    {
        GameObject go = new GameObject();
        go.transform.position = new Vector3(save.x, save.y, save.z);
        return go.transform;
    }
}
