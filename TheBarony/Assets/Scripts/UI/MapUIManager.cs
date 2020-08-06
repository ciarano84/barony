using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUIManager : MonoBehaviour
{
    public enum UIState { standard, confirmation, noInput };
    public static UIState uiState = UIState.standard;

    public static void RequestConfirmation(string question, string yes, string no)
    {
        ConfirmationPopUp confirmationPopUp = GameObject.Find("ConfirmationPopUp").GetComponent<ConfirmationPopUp>();
        confirmationPopUp.GetConfirmation(question, yes, no);
    }

    public static void RequestAlert(string statement, string ok)
    {
        AlertPopUp alert = GameObject.Find("AlertPopUp").GetComponent<AlertPopUp>();
        alert.SetAlert(statement, ok);
    }

}
