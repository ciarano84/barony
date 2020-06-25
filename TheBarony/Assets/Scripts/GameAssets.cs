using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    public static GameAssets i 
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    public Transform damagePopUp;
    public Texture2D Sword_Cursor;
    public GameObject PriestVisual;
    public GameObject DefenderVisual;
    public GameObject ScoutVisual;
    public GameObject EnemyVisual;
    public GameObject Bless;
    public Sprite LeatherArmour;
    public Sprite ShortSword;
    public Sprite ShortBow;
    public Sprite Shield;
    public Sprite BlankItem;
    public Sprite Mace;
}
