using UnityEngine;
public class OrbiterProjectile : BasicProjectile
{
    [SerializeField] private float rotationSpeed;

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        transform.Rotate(gameObject.transform.rotation.x, gameObject.transform.rotation.y, bulletSpeed/2);
    }
}
