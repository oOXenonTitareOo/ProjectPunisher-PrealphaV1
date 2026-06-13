using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    private Gun gun;
    public float swayAmount = 2f;
    public float maxSwayAmount = 5f;
    public float positionalSwayAmount = 0.2f;
    public float maxPositionalSwayAmount = 0.06f;
    public float positionalSwaySmoothing = 1f;
    private float mouseX;
    private float mouseY;
    private float smoothX;
    private float smoothY;
    public float smoothPosSwayX;
    public float smoothPosSwayY;

    private WeaponReload weaponReloadScript;
    private WeaponRecoil weaponRecoilScript;
    private Quaternion initialRotation;
    public AnimationCurve swayCurve;
    void Start()
    {
        gun = GetComponent<Gun>();
        weaponReloadScript = GetComponent<WeaponReload>();
        weaponRecoilScript = GetComponent<WeaponRecoil>();
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (weaponReloadScript != null && weaponReloadScript.isRealoading) return;

        if (Mouse.current != null)
        {
            mouseX = -Mouse.current.delta.x.ReadValue() * (swayAmount * 0.2f);
            mouseY = -Mouse.current.delta.y.ReadValue() * (swayAmount * 0.2f);
        }

        mouseX = Mathf.Clamp(mouseX, -maxSwayAmount, maxSwayAmount);
        mouseY = Mathf.Clamp(mouseY, -maxSwayAmount, maxSwayAmount);

        float distanceRatioX = Mathf.Clamp01(Mathf.Abs(mouseX - smoothX) / maxSwayAmount);
        float distanceRatioY = Mathf.Clamp01(Mathf.Abs(mouseY - smoothY) / maxSwayAmount);

        float curveSpeedModifierX = swayCurve.Evaluate(distanceRatioX);
        float curveSpeedModifierY = swayCurve.Evaluate(distanceRatioY);

        smoothX = Mathf.Lerp(smoothX, mouseX, Time.deltaTime * curveSpeedModifierX);
        smoothY = Mathf.Lerp(smoothY, mouseY, Time.deltaTime * curveSpeedModifierY);

        float targetPosSwayX = (mouseX / swayAmount) * positionalSwayAmount;
        float targetPosSwayY = (mouseY / swayAmount) * positionalSwayAmount;

        targetPosSwayX = Mathf.Clamp(targetPosSwayX, -maxPositionalSwayAmount, maxPositionalSwayAmount);
        targetPosSwayY = Mathf.Clamp(targetPosSwayY, -maxPositionalSwayAmount, maxPositionalSwayAmount);

        smoothPosSwayX = Mathf.Lerp(smoothPosSwayX, targetPosSwayX, Time.deltaTime * positionalSwaySmoothing);
        smoothPosSwayY = Mathf.Lerp(smoothPosSwayY, targetPosSwayY, Time.deltaTime * positionalSwaySmoothing);

    }
    void LateUpdate()
    {
        if (weaponReloadScript != null && weaponReloadScript.isRealoading) return;

        smoothX = Mathf.Clamp(smoothX, -maxSwayAmount, maxSwayAmount);
        smoothY = Mathf.Clamp(smoothY, -maxSwayAmount, maxSwayAmount);

        Quaternion swayRotation = Quaternion.Euler(smoothY, smoothX, 0f);

        Vector3 swayPosition = new Vector3(smoothPosSwayX, smoothPosSwayY, 0f);

        transform.localPosition = gun.initialPosition + swayPosition + weaponRecoilScript.recoilPositionOffset;
        transform.localRotation = initialRotation * swayRotation * weaponRecoilScript.recoilRotationOffset;
    }

    void OnEnable()
    {
        smoothX = 0f;
        smoothY = 0f;
        smoothPosSwayX = 0f;
        smoothPosSwayY = 0f;
    }
}
