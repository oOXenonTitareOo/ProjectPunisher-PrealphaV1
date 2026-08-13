using UnityEngine;

public class WeaponShoot : MonoBehaviour
{   
    private Gun gun;
    private WeaponReload weaponReloadScript;
    private WeaponRecoil weaponRecoilScript;
    private WeaponShellEjection weaponShellEjectionScript;
    public AudioClip shootingSFX;
    public float muzzleFlashSize = 0.1f;
    void Start()
    {
        gun = GetComponent<Gun>();
        weaponReloadScript = GetComponent<WeaponReload>();
        weaponRecoilScript = GetComponent<WeaponRecoil>();
        weaponShellEjectionScript = GetComponent<WeaponShellEjection>();
        }
    public void Shoot()
    {
        if(weaponReloadScript.isRealoading) return;
        if(Time.time < gun.nextTimeToFire) return;

        if(gun.currentAmmo <= 0)
        {
            StartCoroutine(weaponReloadScript.Reload());
            return;
        }

        gun.nextTimeToFire = Time.time + gun.fireRate;
        gun.currentAmmo--;

        // Rotate MuzzleFlash to align with barell. By default it's 90 degrees, facing sideways.
        Quaternion muzzleFlashRotationOffset = Quaternion.Euler(0,90,0);

        Instantiate(gun.bullet, gun.bulletSpawnPoint.position, gun.bulletSpawnPoint.rotation);
        GameObject spawnedWeaponFlash = Instantiate(gun.weaponFlash, gun.bulletSpawnPoint.position, gun.bulletSpawnPoint.rotation, gun.bulletSpawnPoint);
        GameObject spawnedMuzzleFlash = Instantiate(gun.muzzleFlash, gun.bulletSpawnPoint.position, gun.bulletSpawnPoint.rotation * muzzleFlashRotationOffset, gun.bulletSpawnPoint);
        spawnedMuzzleFlash.transform.localScale = new Vector3(muzzleFlashSize, muzzleFlashSize, muzzleFlashSize);

        weaponShellEjectionScript.EjectShell();

        weaponRecoilScript.StopAllCoroutines();
        weaponRecoilScript.StartCoroutine("Recoil");

        AudioManager.Instance.PlaySFX(shootingSFX, gun.bulletSpawnPoint.position, 0.25f);
    }
}