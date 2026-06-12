using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 1f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = -transform.right * speed;
        Destroy(gameObject, lifeTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
