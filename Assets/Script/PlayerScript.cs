using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxHealth = 100;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootInterval = 0.5f;
    public Image healthBar;

    private int currentHealth;
    private float shootTimer;

    void Start()
    {
        currentHealth = maxHealth;
        shootTimer = shootInterval;
    }

    void Update()
    {
        Move();
        AimAndShoot();
        RotateFirePoint();  // New line
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(h, v).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    void AimAndShoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDir = (mousePos - transform.position).normalized;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = aimDir * 10f;
            shootTimer = shootInterval;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
            Destroy(gameObject);

        PetManager.Instance.ResetPet();

        Debug.Log("Player taking damage. Current HP: " + currentHealth);
        if (PetManager.Instance != null)
        {
            Debug.Log("Calling PetManager.Instance.ResetPet()");
            PetManager.Instance.ResetPet();
        }
        else
        {
            Debug.LogWarning("PetManager.Instance is NULL when trying to call ResetPet!");
        }

    }

    void RotateFirePoint()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        // Optional: reposition firePoint to be a little in front of the player
        firePoint.position = transform.position + direction * 0.6f; // Adjust 0.6 to fit your collider size
    }

}
