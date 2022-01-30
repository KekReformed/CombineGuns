using UnityEngine;
public class OrbiterProjectile : BasicProjectile
{
    private float rotation;
    [SerializeField] private float rotationSpeed;

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().rotation += rotationSpeed;
    }

}
