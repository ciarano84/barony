using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;

public class DamagePopUp : MonoBehaviour
{
    TextMeshPro textMesh;
    float dissappearTimer = 1f;
    Color textColor;
    float moveSpeed = 2;
    float dissappearTime = 2.5f;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    //Create a damage popup
    public static DamagePopUp Create(Vector3 position, string effect, bool isWounding)
    {
        Transform damagePopUpTransform = Instantiate(GameAssets.i.damagePopUp, position, Camera.main.transform.rotation);
        DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
        damagePopUp.Setup(effect, isWounding);
        return damagePopUp;
    }

    //Set up the the pop up.
    public void Setup(string effect, bool isWounding)
    {
        textMesh.SetText(effect);
        if (!isWounding)
        {
            //for basic breath loss
            textMesh.fontSize = 4;
            textColor = Color.white;
        }
        else if (isWounding)
        {
            //for wounding hits
            textMesh.fontSize = 5;
            textColor = Color.red;
            transform.position += new Vector3(0, 0.5f, 0);
        }

        textMesh.color = textColor;
    }

    //Animate the pop up
    private void Update()
    {
        transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;
        transform.rotation = Camera.main.transform.rotation;

        dissappearTimer -= Time.deltaTime;
        if (dissappearTimer < 0)
        {
            textColor.a -= (dissappearTime * Time.deltaTime);
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
