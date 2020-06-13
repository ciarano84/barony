using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{
    TextMeshPro textMesh;
    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public static DamagePopUp Create(Vector3 position, int damageAmount)
    {
        Debug.Log("called");
        Transform damagePopUpTransform = Instantiate(GameAssets.i.damagePopUp, position, Quaternion.identity);
        DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
        damagePopUp.Setup(damageAmount);

        return damagePopUp;
    }



    public void Setup(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
    }
}
