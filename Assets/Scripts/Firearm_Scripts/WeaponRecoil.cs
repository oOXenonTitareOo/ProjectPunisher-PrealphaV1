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

            Vector3 currentRecoilPunchOffset =  new Vector3(backValue * recoilBackward, upValue * recoilUpward, 0f);
            Quaternion currentRecoilRotationOffset = Quaternion.Euler(rotationValue * recoilRotation, 0, 0);

            transform.localPosition = gun.initialPosition + currentRecoilPunchOffset;
            transform.localRotation = gun.initialRotation * currentRecoilRotationOffset;

            t += Time.deltaTime * recoilSpeed;

            yield return null;
        }

        transform.localPosition = gun.initialPosition;
        transform.localRotation = gun.initialRotation;
    }
}
