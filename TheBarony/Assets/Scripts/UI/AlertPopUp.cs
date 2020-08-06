using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertPopUp : MonoBehaviour
{
    public Text statement;
    public Text okText;
    public GameObject UI;

    public void SetAlert(string _question, string _okText)
    {
        UI.SetActive(true);
        statement.text = _question;
        okText.text = _okText;
        MapUIManager.uiState = MapUIManager.UIState.confirmation;
    }

    public void OK()
    {
        UI.SetActive(false);
        MapUIManager.uiState = MapUIManager.UIState.standard;
    }
}
