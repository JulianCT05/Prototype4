using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 25;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);  // ✅ destroy projectile on hit
        }
    }

    void Start()
    {
        Destroy(gameObject, 5f); // Cleanup
    }
}
