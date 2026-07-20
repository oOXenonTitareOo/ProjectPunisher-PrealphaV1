using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Damage")
        {
            DecreaseHealth(10);
        }
    }

    private void DecreaseHealth(int decreaseAmount)
    {
        health -= decreaseAmount;
        PlayerLook.Instance.AddShake(0.1f, 0.25f);
        UIManager.Instance.InstantiateHitUI();

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
