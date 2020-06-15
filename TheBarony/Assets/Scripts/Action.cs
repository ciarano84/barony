using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{   
    protected Unit actioningUnit;
    public string buttonText;
    //image for button.
    //tooltip text.

    public abstract void SetActionButtonData(Unit unit);

    public abstract void ExecuteAction();

    public abstract bool CheckAvailable();
}
