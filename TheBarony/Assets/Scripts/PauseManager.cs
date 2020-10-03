using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool paused = false;
    public static bool actionPaused = false;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            PauseGame();
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            PauseAction();
        }
    }

    public void PauseGame()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
        }
        else if (!paused)
        {
            Time.timeScale = 1;
        }
    }

    public static void PauseAction()
    {
        actionPaused = !actionPaused;

        //Unit unit = Initiative.currentUnit.GetComponent<Unit>();

        if (actionPaused)
        {
            foreach (TacticsMovement t in Initiative.order)
            {
                Unit unit = t.GetComponent<Unit>();
                unit.enabled = false;
                unit.unitAnim.enabled = false;
            } 
        }
        else if (!actionPaused)
        {
            foreach (TacticsMovement t in Initiative.order)
            {
                Unit unit = t.GetComponent<Unit>();
                unit.enabled = true;
                unit.unitAnim.enabled = true;
            }
        }
    }
}
