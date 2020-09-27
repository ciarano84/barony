using System.Collections;
using System.Collections.Generic;
// System.Numerics;
using UnityEngine;
using UnityEngine.Animations;

public class Missile : MonoBehaviour
{
    readonly float speed = 16;
    float step;
    public Vector3 target;
    public Vector3 missVariance = new Vector3(1, 1, 0);
    public bool hit = false;
    Vector3 dir;
    public Unit targetUnit;
    public RangedWeapon firingWeapon;

    public void Launch(bool onTarget)
    {
        if (onTarget)
        {
            dir = target - transform.position;
            hit = true;
        }
        else
        {
            dir = (target + missVariance) - transform.position;
        }
        transform.LookAt(dir);
        Invoke("DestroyMissile", 2);
    }

    private void Update()
    {
        step = speed * Time.deltaTime;
        transform.Translate(dir.normalized * step, Space.World);
        if (Vector3.Distance(this.transform.position, target) < 0.25) DestroyMissile();
        transform.forward = dir;
    }

    void DestroyMissile()
    {
        if (hit) firingWeapon.DamageEvent(targetUnit);
        Destroy(this.gameObject);
    }
}
