using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPopUp : MonoBehaviour
{
    public Text question;
    public Text yesText;
    public Text noText;
    public GameObject UI;

    public delegate void OnConfirmDelegate();
    public static OnConfirmDelegate onConfirm;

    public delegate void OnCancelDelegate();
    public static OnCancelDelegate onCancel;

    public void GetConfirmation(string _question, string _yesText, string _noText)
    {
        UI.SetActive(true);
        question.text = _question;
        yesText.text = _yesText;
        noText.text = _noText;
    }

    public void ReturnConfirmation()
    {
        UI.SetActive(false);
        UIManager.uiState = UIManager.UIState.standard;
        onConfirm();
    }

    public void Cancel()
    {
        UI.SetActive(false);
        UIManager.uiState = UIManager.UIState.standard;
        onCancel();
    }
}
