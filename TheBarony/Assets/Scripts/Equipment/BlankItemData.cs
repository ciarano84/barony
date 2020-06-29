using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankItem : Item
{
    public override void GetItemData()
    {
        itemData = new BlankItemData();
    }
}
