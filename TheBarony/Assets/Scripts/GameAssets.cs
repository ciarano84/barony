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
    public Texture2D Eye_Cursor;
    public GameObject PlayerUnit;
    public GameObject PriestVisual;
    public GameObject DefenderVisual;
    public GameObject ScoutVisual;
    public GameObject EnemyVisual;
    public GameObject GoblinHunter;
    public GameObject GoblinScout;
    public GameObject OrcDefender;
    public GameObject Bless;
    public GameObject TurnFocus;
    public GameObject OutOfTurnFocus;
    public Sprite LeatherArmour;
    public Sprite ShortSword;
    public Sprite ShortBow;
    public Sprite Shield;
    public Sprite BlankItem;
    public Sprite Mace;
    public Sprite Reload;
    public Sprite Longbow;
    public Sprite Greataxe;
    public Sprite Dagger;
    public GameObject DaggerModel;
    public GameObject ShortswordModel;
    public GameObject ShortbowModel;
    public GameObject LongbowModel;
    public GameObject ShieldModel;
    public GameObject MaceModel;
    public GameObject GreataxeModel;
    public Mesh MediumArmourMesh;
    public Mesh LightArmourMesh;
    public Mesh PriestRobesMesh;
}
