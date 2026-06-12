using UnityEngine;
using System.Collections;

public class WeaponReload : MonoBehaviour
{
    private Gun gun;
    public float reloadTime = 1f;
    public bool isRealoading = false;
    private Vector3 reloadRotationOffset = new Vector3(66, 50, 50);

    void Start()
    {
        gun = GetComponent<Gun>();
    }
    public IEnumerator Reload()
    {
        isRealoading = true;
        Quaternion targetRotation = Quaternion.Euler(gun.initialRotation.eulerAngles + reloadRotationOffset);
        float halfReload = reloadTime / 2f;
        float t = 0f;

        while(t < halfReload)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(gun.initialRotation, targetRotation, t / halfReload);

            yield return null;
        }

        t = 0f;

        while(t < halfReload)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(targetRotation, gun.initialRotation, t / halfReload);

            yield return null;
        }

        gun.currentAmmo = gun.magSize;
        isRealoading = false;  
    }
    public void TryReload()
    {
        if (isRealoading) return;
        if (gun.currentAmmo == gun.magSize) return;

        StartCoroutine(Reload());
    }
}
