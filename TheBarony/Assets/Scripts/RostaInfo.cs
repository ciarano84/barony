using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RostaInfo : MonoBehaviour
{
    public List<UnitInfo> rosta = new List<UnitInfo>();
    public List<UnitInfo> squad = new List<UnitInfo>();
    public int currentUnitShown = 0;
    public int companyPosition;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
