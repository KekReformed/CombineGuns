using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject bulletPrefab;

    private Transform firePoint;
    private float cooldownTimer;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.Log("You need an object called FirePoint!!!");
        }
    }

    private void Update()
    {
        if (cooldownTimer > attackCooldown)
        {
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);
        cooldownTimer = 0;
    }
}
