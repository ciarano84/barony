using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum UIState { standard, confirmation, noInput };
    public static UIState uiState = UIState.standard;
    Queue<PopUp> popUps = new Queue<PopUp>();
    public static UIManager ui;
    bool messageDelayNeeded = false;

    private void Awake()
    {
        ui = this;
    }

    public void CheckForMessages()
    {
        if (uiState == UIState.standard)
        {
            if (popUps.Count > 0)
            {
                ui.StartCoroutine(SetPopUp(popUps.Dequeue(), messageDelayNeeded));
                if (popUps.Count > 1) messageDelayNeeded = true;
                else messageDelayNeeded = false;
            }
        }
    }

    public static void RequestConfirmation(string question, string yes, string no)
    {
        PopUp p = new PopUp { mainMessage = question, confirmMessage = yes, cancelMessage = no, popUpType = PopUp.PopUpType.CONFIRMATION };
        ui.AddPopUpToQueue(p);
    }

    public static void RequestAlert(string statement, string ok)
    {
        PopUp p = new PopUp { mainMessage = statement, confirmMessage = ok, popUpType = PopUp.PopUpType.ALERT };
        ui.AddPopUpToQueue(p);
    }

    void AddPopUpToQueue(PopUp popUp)
    {
        popUps.Enqueue(popUp);
    }

    IEnumerator SetPopUp(PopUp popUp, bool delay = false)
    {
        uiState = UIState.confirmation; 
        yield return new WaitForSeconds(0.75f);
        switch (popUp.popUpType)
        {
            case PopUp.PopUpType.ALERT:
                AlertPopUp alert = GameObject.Find("AlertPopUp").GetComponent<AlertPopUp>();
                alert.SetAlert(popUp.mainMessage, popUp.confirmMessage);
                break;
            case PopUp.PopUpType.CONFIRMATION:
                ConfirmationPopUp confirmationPopUp = GameObject.Find("ConfirmationPopUp").GetComponent<ConfirmationPopUp>();
                confirmationPopUp.GetConfirmation(popUp.mainMessage, popUp.confirmMessage, popUp.cancelMessage);
                break;
            default:
                break;
        }
        yield break;
    }
}

public class PopUp
{
    public string mainMessage;
    public string confirmMessage;
    public string cancelMessage;
    public enum PopUpType { ALERT, CONFIRMATION }
    public PopUpType popUpType;
}
