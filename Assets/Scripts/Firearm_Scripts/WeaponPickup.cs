using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;

public class WeaponPickup : MonoBehaviour
{
    public Material highlightMaterial;
    private Material[] originalMaterials;
    private Renderer[] renderers;
    public GameObject weaponPrefab;
    private PlayerShooting player;
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];
        for(int i =  0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }

        player = FindAnyObjectByType<PlayerShooting>();
    }

    public void SetLookedAt(bool lookedAt)
    {
        Debug.Log("item in sight" + lookedAt);
        if(lookedAt)
        {
            foreach(Renderer mr in renderers)
            {
                mr.material = highlightMaterial;
            }
        }
        else
        {
            for(int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = originalMaterials[i];
            }
        }
    }
    public void PickupThisWeapon()
    {
        player.OnWeaponDrop();

        GameObject newWeapon = Instantiate(weaponPrefab, player.gunHolder);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

        player.gun = newWeapon.GetComponent<Gun>();
        player.weaponReloadScript = newWeapon.GetComponent<WeaponReload>();
        player.weaponShootScript = newWeapon.GetComponent<WeaponShoot>();
        player.weaponSwayScript = newWeapon.GetComponent<WeaponSway>();
        
        Destroy(gameObject);
    }
}
