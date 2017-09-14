using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapons
{
    rifle = 0,
    shotgun = 1,
    firework = 2
}

public class WeaponInventory : MonoBehaviour
{
    public Weapons weapon1, weapon2;
    Weapons activeWeapon;

    GameObject pickup1, pickup2;

    Weapon[] weaponScripts;

    PlayerMovScript movScript;
    ReviveSystem reviveScript;
    
    [Header("Pickup prefabs (For dropping initial weapons)")]
    public GameObject riflePickup;
    public GameObject shotgunPickup;
    public GameObject fireworkPickup;
    
    [Header("Weapon models")]
    public GameObject rifleModel;
    public GameObject shotgunModel;
    public GameObject fireworkModel;

    private string AxisCombo;
    public bool SwappingWeapon = false;

    void Start ()
    {
        movScript = GetComponent<PlayerMovScript>();
        reviveScript = GetComponent<ReviveSystem>();
        AxisCombo = movScript.playerBeginning;

        weaponScripts = new Weapon[]
        {
            GetComponent<Rifle>(),
            GetComponent<Shotgun>(),
            GetComponent<FireworkShooter>()
        };

        activeWeapon = weapon1;
        weaponScripts[(int)activeWeapon].enabled = true;
        SelectWeapon(1);
    }
    void Update ()
    {
        if (!movScript.useController)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SelectWeapon(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                SelectWeapon(2);
        }
        else
        {
            if (!SwappingWeapon)
            {
                if (Input.GetAxis(AxisCombo + "DHorizontal") < -0.9f || (!movScript.useController && Input.GetKeyDown(KeyCode.Alpha1)))
                {
                    Debug.Log("Left!");
                    SwappingWeapon = true;
                    SelectWeapon(1);
                }

                if (Input.GetAxis(AxisCombo + "DHorizontal") > 0.9f || (!movScript.useController && Input.GetKeyDown(KeyCode.Alpha2)))
                {
                    Debug.Log("Right!");
                    SwappingWeapon = true;

                    SelectWeapon(2);
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
    }

    public void PickUp(GameObject pickup, Weapons weapon)
    {
        if (reviveScript.NeedRes)
            return;

        weaponScripts[(int)activeWeapon].enabled = false;
        if (activeWeapon == weapon1)
        {
            weapon1 = weapon;
            if (pickup1 != null) // If a pickup object has been stored from last PickUp()
            {
                pickup1.SetActive(true);
                pickup1.transform.position = transform.position;
            }
            else switch (activeWeapon) // If no pickup object has been stored from last PickUp() (Probably the items you start with)
                {
                case Weapons.rifle:     Instantiate(riflePickup, transform.position, Quaternion.identity);      break;
                case Weapons.shotgun:   Instantiate(shotgunPickup, transform.position, Quaternion.identity);    break;
                case Weapons.firework:  Instantiate(fireworkPickup, transform.position, Quaternion.identity);   break;
            }

            pickup1 = pickup;
            pickup1.SetActive(false);
        }
        else // if (activeWeapon == weapon2)
        {
            weapon2 = weapon;
            if (pickup2 != null) // If a pickup object has been stored from last PickUp()
            {
                pickup2.SetActive(true);
                pickup2.transform.position = transform.position;
            }
            else switch (activeWeapon) // If no pickup object has been stored from last PickUp() (Probably the items you start with)
            {
                case Weapons.rifle: Instantiate(riflePickup, transform.position, Quaternion.identity); break;
                case Weapons.shotgun: Instantiate(shotgunPickup, transform.position, Quaternion.identity); break;
                case Weapons.firework: Instantiate(fireworkPickup, transform.position, Quaternion.identity); break;
            }
            pickup2 = pickup;
            pickup2.SetActive(false);
        }

        activeWeapon = weapon;
        weaponScripts[(int)activeWeapon].enabled = true;
        movScript.playerAttack = weaponScripts[(int)activeWeapon].Attack;

        DeactivateModels();
        switch (activeWeapon)
        {
            case Weapons.rifle:         rifleModel.SetActive(true);     break;
            case Weapons.shotgun:       shotgunModel.SetActive(true);   break;
            case Weapons.firework:      fireworkModel.SetActive(true);  break;
        }
    }

    public void SelectWeapon(int weapon) // 1 and 2
    {
        weaponScripts[(int)activeWeapon].enabled = false;
        if (weapon == 1)
            activeWeapon = weapon1;
        else if (weapon == 2)
            activeWeapon = weapon2;
        weaponScripts[(int)activeWeapon].enabled = true;
        movScript.playerAttack = weaponScripts[(int)activeWeapon].Attack;

        DeactivateModels();
        switch (activeWeapon)
        {
            case Weapons.rifle: rifleModel.SetActive(true); break;
            case Weapons.shotgun: shotgunModel.SetActive(true); break;
            case Weapons.firework: fireworkModel.SetActive(true); break;
        }
    }

    public void SwitchWeapon()
    {
        weaponScripts[(int)activeWeapon].enabled = false;
        if (activeWeapon == weapon1)
            activeWeapon = weapon2;
        else
            activeWeapon = weapon1;
        weaponScripts[(int)activeWeapon].enabled = true;
        movScript.playerAttack = weaponScripts[(int)activeWeapon].Attack;

        DeactivateModels();
        switch (activeWeapon)
        {
            case Weapons.rifle: rifleModel.SetActive(true); break;
            case Weapons.shotgun: shotgunModel.SetActive(true); break;
            case Weapons.firework: fireworkModel.SetActive(true); break;
        }
    }

    void DeactivateModels()
    {
        rifleModel.SetActive(false);
        shotgunModel.SetActive(false);
        fireworkModel.SetActive(false);
    }
}
