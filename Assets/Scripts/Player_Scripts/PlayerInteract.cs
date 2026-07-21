using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public Camera playerCam;
    public float lookRange = 3f;
    public float sphereRadius = 0.5f;
    public InputActionReference interactAction;
    private WeaponPickup currentTarget;
    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.Enable();
            interactAction.action.performed += OnInteract;
        }
    }
     private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.Disable();
            interactAction.action.performed -= OnInteract;
        }
    }
    // Update is called once per frame
    void Update()
    {
        CheckForInteractables();
    }
    private void CheckForInteractables()
    {
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, lookRange))
        {
            WeaponPickup pickup = hit.collider.GetComponentInParent<WeaponPickup>();

            if (pickup != null)
            {
                if (currentTarget != pickup)
                {
                    ClearTarget();

                    currentTarget = pickup;
                    currentTarget.SetLookedAt(true);
                }
            }
            else
            {
                ClearTarget();
            }
        }
        else
        {
            ClearTarget();
        }
    }
    private void ClearTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.SetLookedAt(false);
            currentTarget = null;
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && currentTarget != null)
        {
            currentTarget.PickupThisWeapon();
            currentTarget = null;
        }
    }
}
