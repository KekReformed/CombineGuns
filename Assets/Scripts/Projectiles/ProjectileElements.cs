using UnityEngine;
using Unity.Netcode;
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
        else
        {
            bounceLimit = 2;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Touched a wall! my element is {element}");
        if (element == "Fire")
        {
            LayerMask mask = 3 | 6;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, mask);
            Instantiate(explosionSprite,transform.position,transform.rotation);
            foreach (Collider2D hit in colliders)
            {
                if (hit.gameObject.layer == 3)
                {
                    hit.GetComponent<PlayerMovement>().hp -= damage;
                }
            }
            Destroy(gameObject);
        }

        else if (element == "Ice")
        {
            if (collision.gameObject.layer == 3){
                collision.GetComponent<PlayerMovement>().movementReduction += 0.05f;
            }
        }

        else if (element == "Acid")
        {
            if (collision.GetComponent<PlayerMovement>())
            {
                collision.GetComponent<PlayerMovement>().hitbyacid = true;
            }
        }
    }
}
