using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem : MonoBehaviour
{
    // Arrault Rifle, Light Machine Gun, Shotgun, Sniper Rifle, Flamethrower
    public enum WeaponType { AR, LMG, SG, SR, FT }

    public GameObject RifleModel;
    public GameObject ShotgunModel;

    // Scripts
    public Rifle LocalRifleScript;
    public Shotgun LocalShotgunScript;

    public WeaponType WeaponSlot1;
    public WeaponType WeaponSlot2;
    public WeaponType CurrentWeapon;

    private PlayerMovScript movement;
    private string AxisCombo;
    public bool SwappingWeapon = false;

    // Use this for initialization
    void Start ()
    {
        movement = GetComponent<PlayerMovScript>();
        AxisCombo = movement.playerBeginning;

        LocalRifleScript = GetComponent<Rifle>();
        LocalShotgunScript = GetComponent<Shotgun>();
        enabled = true;

        // Enable Starter Weapon
        TurnAllOff();
        LocalRifleScript.enabled = true;
        RifleModel.SetActive(true);
    }

    // Update is called once per frame
    void Update ()
    {
        if (!SwappingWeapon)
        {

            if (Input.GetAxis(AxisCombo + "DHorizontal") < -0.9f)
            {
                Debug.Log("Left!");
                SwappingWeapon = true;

                switch (WeaponSlot1)
                {
                    case WeaponType.AR:
                        TurnAllOff();
                        LocalRifleScript.enabled = true;
                        movement.playerAttack = LocalRifleScript.Attack;
                        RifleModel.SetActive(true);
                        CurrentWeapon = WeaponType.AR;
                        break;
                    case WeaponType.LMG:
                        break;
                    case WeaponType.SG:
                        TurnAllOff();
                        LocalShotgunScript.enabled = true;
                        movement.playerAttack = LocalShotgunScript.Attack;
                        ShotgunModel.SetActive(true);
                        CurrentWeapon = WeaponType.SG;
                        break;
                    case WeaponType.SR:
                        break;
                    case WeaponType.FT:
                        break;
                    default:
                        break;
                }
            }

            if (Input.GetAxis(AxisCombo + "DHorizontal") > 0.9f)
            {
                Debug.Log("Right!");
                SwappingWeapon = true;

                switch (WeaponSlot2)
                {
                    case WeaponType.AR:
                        TurnAllOff();
                        LocalRifleScript.enabled = true;
                        movement.playerAttack = LocalRifleScript.Attack;
                        RifleModel.SetActive(true);
                        CurrentWeapon = WeaponType.AR;
                        break;
                    case WeaponType.LMG:
                        break;
                    case WeaponType.SG:
                        TurnAllOff();
                        LocalShotgunScript.enabled = true;
                        movement.playerAttack = LocalShotgunScript.Attack;
                        ShotgunModel.SetActive(true);
                        CurrentWeapon = WeaponType.SG;
                        break;
                    case WeaponType.SR:
                        break;
                    case WeaponType.FT:
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            if (Input.GetAxis(AxisCombo + "DHorizontal") < 0.9f && Input.GetAxis(AxisCombo + "DHorizontal") > -0.9f)
            {
                SwappingWeapon = false;
            }
        }
    }

    void TurnAllOff()
    {
        LocalRifleScript.enabled = false;
        RifleModel.SetActive(false);

        LocalShotgunScript.enabled = false;
        ShotgunModel.SetActive(false);
    }
}
