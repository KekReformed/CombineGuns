using UnityEngine;

public class ProjectileElements : MonoBehaviour
{

    [SerializeField] private GameObject explosionSprite;

    public string element;
    public float explosionRadius;
    public float damage;
    public int bounceLimit;
    public Rigidbody2D body;
    public CircleCollider2D circleCollider;


    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void FixedUpdate()
    {
        if (element == "Bounce")
        {
            body.gravityScale = 1;
            circleCollider.sharedMaterial.bounciness = 1;
            circleCollider.isTrigger = false;
        }


        else if (element == "Fire")
        {
            //Refer to the OnCollisionEnter2D method
        }


        else if (element == "Ice")
        {
            //Refer to the OnCollisionEnter2D method
        }


        else if (element == "Acid")
        {

        }


        if (element != "Bounce")
        {
            bounceLimit = 0;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (element == "Fire")
        {
            LayerMask mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Platform");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, mask);
            Instantiate(explosionSprite,transform.position,transform.rotation);
            foreach (Collider2D hit in colliders)
            {
                hit.GetComponent<PlayerMovement>().hp -= damage;
            }
            Destroy(gameObject);
        }

        if (element == "Ice")
        {
            LayerMask mask = LayerMask.GetMask("Player");
            collision.collider.GetComponent<PlayerMovement>().movementReduction += 0.05f;
        }
    }
}
