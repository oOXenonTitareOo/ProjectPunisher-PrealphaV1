using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public AudioClip hitSFX;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            DecreaseHealth(10);

            Destroy(collision.gameObject);
        }
    }

    private void DecreaseHealth(int decreaseAmount)
    {
        health -= decreaseAmount;
        PlayerLook.Instance.AddShake(0.1f, 0.25f);
        UIManager.Instance.InstantiateHitUI();
        AudioManager.Instance.PlaySFX(hitSFX, transform.position);

        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Time.timeScale = 0f;
    }
}
