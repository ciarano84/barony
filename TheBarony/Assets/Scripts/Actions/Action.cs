using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionCost { main, move };

public abstract class Action
{   
    protected Unit actioningUnit;
    public string buttonText;
    public ActionCost actionCost;

    public abstract void SetActionButtonData(Unit unit);

    public abstract void ExecuteAction(ActionCost actionCost);

    public abstract bool CheckAvailable();

    public abstract Sprite SetImage();
}
