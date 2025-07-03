using UnityEngine;

public class Pet : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;
    public float followSpeed = 5f;
    public float followDistance = 1.5f;
    public float fireRate = 1f;
    public int damage = 15;

    private float fireTimer;

    void Update()
    {
        if (player == null)
            return;

        // Follow the player at a short distance
        Vector3 targetPosition = player.position + (transform.position - player.position).normalized * followDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Fire at nearest enemy
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Vector3 target = FindClosestEnemy();
            if (target != Vector3.zero)
            {
                GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().linearVelocity = (target - transform.position).normalized * 8f;
                bullet.GetComponent<Projectile>().damage = damage;
            }

            fireTimer = fireRate;
        }
    }

    Vector3 FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        float closestDist = float.MaxValue;
        Vector3 closest = Vector3.zero;

        foreach (Enemy enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemy.transform.position;
            }
        }

        return closest;
    }

    void OnDestroy()
    {
        if (PetManager.Instance != null)
        {
            // Not strictly necessary with List.Clear(), but good to ensure no stale refs
            PetManager.Instance.RemovePet(this.gameObject);
        }
    }
}
