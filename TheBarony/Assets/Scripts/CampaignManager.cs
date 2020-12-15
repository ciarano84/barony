using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    public int reclaimsForVictory;
    public string completionMessage;

    public bool CheckForCampaignCompletion()
    {
        if (RostaInfo.ReclaimedSites >= reclaimsForVictory)
        {
            MapUIManager.RequestAlert(completionMessage ,"ok");
            return true;
        } else return false;
    }
}
