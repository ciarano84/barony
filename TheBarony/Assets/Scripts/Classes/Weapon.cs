using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public PlayerCharacter owner;
    int attackModifier = 0;
    int damageModifier = 2;

    private void Start()
    {
        owner = GetComponent<PlayerCharacter>();
        attackModifier += owner.attackModifier;
        damageModifier += owner.damageModifier;
    }

    public IEnumerator Attack()
    {
        owner.unitAnim.SetTrigger("melee");
        owner.remainingActions--;
        yield return new WaitForSeconds(0.3f);

        //proxy attack vs 10 in here.
        AbilityChecker.CheckAbility(attackModifier, 10);

        yield return new WaitForSeconds(1f);
        Initiative.CheckForTurnEnd(owner);
        yield break;
    }
}
