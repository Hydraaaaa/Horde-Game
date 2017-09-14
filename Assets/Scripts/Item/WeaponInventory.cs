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

    public GameObject rifleModel;
    public GameObject shotgunModel;
    public GameObject fireworkModel;

    void Start ()
    {
        movScript = GetComponent<PlayerMovScript>();

        weaponScripts = new Weapon[]
        {
            GetComponent<Rifle>(),
            GetComponent<Shotgun>(),
            GetComponent<FireworkShooter>()
        };

        activeWeapon = weapon1;
        weaponScripts[(int)activeWeapon].enabled = true;
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
    }

    public void PickUp(GameObject pickup, Weapons weapon)
    {
        weaponScripts[(int)activeWeapon].enabled = false;
        if (activeWeapon == weapon1)
        {
            weapon1 = weapon;
            if (pickup1 != null)
            {
                pickup1.SetActive(true);
                pickup1.transform.position = transform.position;
            }
            pickup1 = pickup;
            pickup1.SetActive(false);
        }
        else
        {
            weapon2 = weapon;
            if (pickup2 != null)
            {
                pickup2.SetActive(true);
                pickup2.transform.position = transform.position;
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
