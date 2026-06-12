using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{   
    public static Gun Instance;
    public float fireRate = 0.15f;
    public float nextTimeToFire = 0f;
    public int magSize = 15;
    public int currentAmmo;
    public Transform bulletSpawnPoint;
    public Quaternion initialRotation;
    public Vector3 initialPosition;
    public GameObject bullet;
    public GameObject weaponFlash;
    public GameObject muzzleFlash;
    public GameObject droppedWeapon;
    public Rigidbody playerRb;

    void Start()
    {
        currentAmmo = magSize;
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;

        PlayerShooting player = Object.FindAnyObjectByType<PlayerShooting>();

        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody>();
        }
    }

    void Awake()
    {
        Instance = this;
    }

    public void Drop()
    {
        Instantiate(droppedWeapon, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
