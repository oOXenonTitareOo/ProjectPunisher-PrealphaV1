using UnityEngine;
using System.Collections;

public class WeaponRecoil : MonoBehaviour
{
    private Gun gun;
    public float recoilBackward = 0.1f;
    public float recoilUpward = 0.5f;
    public float recoilRotation = 5f;
    public float recoilSpeed = 15f;
    public AnimationCurve backwardKickCurve;
    public AnimationCurve upwardKickCurve;
    public AnimationCurve rotationCurve;

    [HideInInspector] public Vector3 recoilPositionOffset;
    [HideInInspector] public Quaternion recoilRotationOffset = Quaternion.identity;
    void Start()
    {
        gun = GetComponent<Gun>();
    }
    public IEnumerator Recoil()
    {
        float t = 0f;
        while(t < 1f)
        {
            float backValue = backwardKickCurve.Evaluate(t);
            float upValue = upwardKickCurve.Evaluate(t);
            float rotationValue = rotationCurve.Evaluate(t);

            recoilPositionOffset =  new Vector3(backValue * recoilBackward, upValue * recoilUpward, 0f);
            recoilRotationOffset = Quaternion.Euler(rotationValue * recoilRotation, 0, 0);

            t += Time.deltaTime * recoilSpeed;

            yield return null;
        }

        recoilPositionOffset = Vector3.zero;
        recoilRotationOffset = Quaternion.identity; 
    }
}
