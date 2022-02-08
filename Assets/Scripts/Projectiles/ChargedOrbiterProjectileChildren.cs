using UnityEngine;

public class ChargedOrbiterProjectileChildren : MonoBehaviour
{
    private float bulletSpeed;

    private void Start()
    {
        bulletSpeed = GetComponentInParent<BasicProjectile>().bulletSpeed;
    }
    void FixedUpdate()
    {
        transform.Rotate(gameObject.transform.rotation.x, gameObject.transform.rotation.y, -bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.root.gameObject.GetComponent<BasicProjectile>().element != "Bouncy")
        {
            Destroy(gameObject);
        }
    }
}
