using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    private Gun gun;
    public float swayAmount = 2f;
    public float swaySmoothness = 10f;
    public float maxSwayAmount = 5f;
    private WeaponReload weaponReloadScript;
    private Quaternion initialRotation;
    void Start()
    {
        gun = GetComponent<Gun>();
        weaponReloadScript = GetComponent<WeaponReload>();
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        WeaponSwayByMouse();
    }

    private void WeaponSwayByMouse()
    {
        if (weaponReloadScript != null && weaponReloadScript.isRealoading) return;

        float mouseX = -Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = -Input.GetAxis("Mouse Y") * swayAmount;

        mouseX = Mathf.Clamp(mouseX, -maxSwayAmount, maxSwayAmount);
        mouseY = Mathf.Clamp(mouseY, -maxSwayAmount, maxSwayAmount);

        Quaternion swayRotation = Quaternion.Euler(mouseY, mouseX, 0f);
        Quaternion targetRotation = initialRotation * swayRotation;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime / swaySmoothness);
    }
}
