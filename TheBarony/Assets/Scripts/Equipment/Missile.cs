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
    Vector3 dir;
    public Vector3 missVariance = new Vector3(1, 1, 0);

    public Result hit = Result.FAIL;
    public Unit targetUnit;
    public RangedWeapon firingWeapon;
    bool onTarget = false;

    //Debug
    //GameObject marker;

    public void Launch(Result onTarget)
    {
        hit = onTarget;

        //Debug
        //Destroy(GameObject.Find("Cube"));
        //marker = Instantiate(GameAssets.i.TargetMarker, target, Quaternion.identity);

        if (hit >= Result.PARTIAL || AttackManager.defenceType == DefenceType.DODGE)
        {
            dir = target;
        }
        else
        {
            dir = target + missVariance;
        }

        transform.LookAt(dir);
        dir = (dir - transform.position).normalized;
        Invoke("DestroyMissile", 2);
    }

    private void Update()
    {
        //Debugging
        //marker.transform.position = target;
        
        step = speed * Time.deltaTime;
        transform.Translate(dir * step, Space.World);
        if (hit >= Result.PARTIAL)
        {
            if (Vector3.Distance(transform.position, target) < 0.25)
            {
                if (AttackManager.defenceType != DefenceType.DODGE || hit == Result.SUCCESS)
                {
                    onTarget = true;
                    DestroyMissile();
                }    
            }
        }
        transform.forward = dir;
    }

    void DestroyMissile()
    {
        if (onTarget) firingWeapon.DamageEvent(targetUnit, hit);
        Destroy(gameObject);
    }
}
