using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPopUpManager : MonoBehaviour
{
    //Attached to each unit, and handles all the info that then gets spat out as popups over their heads. 

    public Queue<PopUpInfo> popUps = new Queue<PopUpInfo>();
    float PopUpTimer = 0;
    float PopUpDelay = 0.25f;

    void Update()
    {
        string info;

        PopUpTimer -= Time.deltaTime;

        if (popUps.Count > 0)
        {
            if (PopUpTimer <= 0)
            {
                PopUpInfo popUpInfo = popUps.Dequeue();
                info = popUpInfo.info;
                DamagePopUp.Create(gameObject.transform.position + new Vector3(0, (gameObject.GetComponent<TacticsMovement>().halfHeight) + 0.5f), info, popUpInfo.highlight);
                PopUpTimer = PopUpDelay;
            }
        }
    }

    public void AddPopUpInfo(string info, bool highlight = false)
    {
        PopUpInfo p = new PopUpInfo();
        p.highlight = highlight;
        p.info = info;
        popUps.Enqueue(p);
    }

    public class PopUpInfo
    {
        public string info;
        public bool highlight;
    }
}
