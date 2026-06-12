using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public Gun gun;
    public WeaponReload weaponReloadScript;
    public WeaponShoot weaponShootScript;
    public WeaponSway weaponSwayScript;
    private bool isHoldingShoot = false;
    public Transform gunHolder;
    void OnShoot()
    {
        isHoldingShoot = true;
    }
    void OnShootRelease()
    {
        isHoldingShoot = false;
    }
    void OnReload()
    {
        if(gun != null)
        {
            weaponReloadScript.TryReload();
        }
    }
    void Update()
    {
        if(isHoldingShoot && gun != null)
        {
            weaponShootScript.Shoot();
        }
    }
}
