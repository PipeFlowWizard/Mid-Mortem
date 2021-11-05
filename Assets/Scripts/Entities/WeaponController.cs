using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// from https://youtu.be/rZAnnyensgs
public class WeaponController : MonoBehaviour
{

    [SerializeField] private Transform weaponHold;
    [SerializeField] private Weapon defaultWeapon;
    private Weapon _attachedWeapon;

    private void Start()
    {
        if (defaultWeapon != null)
        {
            AttachWeapon(defaultWeapon);
        }
    }

    private void AttachWeapon(Weapon weapon)
    {
        _attachedWeapon = Instantiate(weapon, weaponHold.position, weaponHold.rotation) as Weapon;
        _attachedWeapon.transform.parent = weaponHold;
    }


    

}
