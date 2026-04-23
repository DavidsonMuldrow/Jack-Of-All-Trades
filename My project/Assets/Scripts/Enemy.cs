using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Detection & Combat")]
    public float shootRange = 10f;
    public float fireRate = 1f;
    public LayerMask obstacleLayers;
    private float nextFireTime;

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootRange)
        {
            if (HasLineOfSight())
            {
                Vector2 direction = player.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);

                if (Time.time >= nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + fireRate;
                }
            }
        }
    }

    bool HasLineOfSight()
    {
        Vector2 direction = player.position - firePoint.position;
        float distance = Vector2.Distance(firePoint.position, player.position);

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, distance, obstacleLayers);

        Debug.DrawRay(firePoint.position, direction, Color.blue);

        if (hit.collider != null)
        {
            if (!hit.collider.CompareTag("Player"))
            {
                return false;
            }
        }

        return true;
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
    private void OnDrawGizmos()
    {
        if (player != null && firePoint != null)
        {
            Gizmos.color = HasLineOfSight() ? Color.green : Color.red;
            Gizmos.DrawLine(firePoint.position, player.position);
        }
    }
}