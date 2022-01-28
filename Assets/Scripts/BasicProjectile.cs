using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    private Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        Vector2 fireTargetPosition = new Vector2(GameObject.Find("FireTarget").transform.position.x, GameObject.Find("FireTarget").transform.position.y);
        body.velocity = (fireTargetPosition - body.position)*bulletSpeed;
    }

    private void Update()
    {
        //Deletes the projectile if its off screen
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }
}
