using UnityEngine;
public class OrbiterProjectile : BasicProjectile
{
    [SerializeField] private float rotationSpeed;

    void FixedUpdate()
    {
        transform.Rotate(gameObject.transform.rotation.x, gameObject.transform.rotation.y, bulletSpeed/2);
    }
}
