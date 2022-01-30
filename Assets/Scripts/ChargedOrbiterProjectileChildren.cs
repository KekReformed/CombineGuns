using UnityEngine;

public class ChargedOrbiterProjectileChildren : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    private float bulletSpeed;

    private void Start()
    {
        bulletSpeed = GetComponentInParent<BasicProjectile>().bulletSpeed;
    }
    void FixedUpdate()
    {
        transform.Rotate(gameObject.transform.rotation.x, gameObject.transform.rotation.y, -bulletSpeed);
    }
}
