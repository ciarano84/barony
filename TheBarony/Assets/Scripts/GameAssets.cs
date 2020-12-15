using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.Animations;
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

    //map UI
    public GameObject company;
    public GameObject encounterButton;
    
    //encounter UI
    public Transform damagePopUp;
    public Texture2D Sword_Cursor;
    public Texture2D Eye_Cursor;
    public GameObject Bless;
    public GameObject TurnFocus;
    public GameObject OutOfTurnFocus;

    //Units and unit visuals
    public GameObject PlayerUnit;
    public GameObject PriestVisual;
    public GameObject DefenderVisual;
    public GameObject ScoutVisual;
    public GameObject GoblinHunter;
    public GameObject GoblinScout;
    public GameObject OrcDefender;
    public GameObject OrcBrute;
    public GameObject EliteOrcBrute;
    public GameObject BanditAxeman;
    public GameObject BanditHunter;
    public GameObject BanditCuthroat;
    public GameObject EliteBanditHunter;
    public Material cubeTest;

    //Item Sprites
    public Sprite LeatherArmour;
    public Sprite ChainShirtArmour;
    public Sprite ShortSword;
    public Sprite ShortBow;
    public Sprite Shield;
    public Sprite BlankItem;
    public Sprite Mace;
    public Sprite Longbow;
    public Sprite Greataxe;
    public Sprite Dagger;

    //Action Sprites
    public Sprite Reload;
    public Sprite Dash;
    public Sprite Defend;
    public Sprite BlockToggle;
    public Sprite DodgeToggle;

    //Effect Sprites
    public Sprite Defending;
    public Sprite BlessIcon;
    public Sprite PrimingIcon;
    public Sprite ExposedIcon;
    public Sprite CrippledIcon;

    //Item models
    public GameObject DaggerModel;
    public GameObject ShortswordModel;
    public GameObject ShortbowModel;
    public GameObject LongbowModel;
    public GameObject ShieldModel;
    public GameObject MaceModel;
    public GameObject GreataxeModel;
    public GameObject ArrowModel;
    public Mesh MediumArmourMesh;
    public Mesh LightArmourMesh;
    public Mesh PriestRobesMesh;

    //Animators
    public RuntimeAnimatorController TwoHanded;
    public RuntimeAnimatorController OneHanded;

    //Arena Blocks
    public GameObject ArenaBarns;
    public GameObject ArenaBog;
    public GameObject ArenaChapel;
    public GameObject ArenaHills;
    public GameObject ArenaMarket;
    public GameObject ArenaTavern;

    //Music
    public AudioClip QueensFuneral;

    //Debug
    public GameObject TargetMarker;
}
