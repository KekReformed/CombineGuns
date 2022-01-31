using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
	public float bulletSpeed;
	public int bounceLimit;
	private int collisionCount = 0;
	public bool bouncy = false;
	public Rigidbody2D body;
	public Transform fireTarget;
	private CircleCollider2D circleCollider;

	public void Start()
	{
		body = GetComponent<Rigidbody2D>();
		circleCollider = GetComponent<CircleCollider2D>();

		Vector2 fireTargetPosition = new Vector2(fireTarget.position.x, fireTarget.position.y);

		body.velocity = (fireTargetPosition - body.position) * bulletSpeed;

		if (bouncy)
		{
			body.gravityScale = 1;
			circleCollider.sharedMaterial.bounciness = 1;
			circleCollider.isTrigger = false;
		}
	}

	public void Update()
	{
		//Deletes the projectile if its off screen
		if (!GetComponent<Renderer>().isVisible)
		{
			Destroy(gameObject);
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.tag == "Platform")
        {
			this.collisionCount += 1;
		}

		if (collisionCount > bounceLimit && bouncy)
        {
			Destroy(gameObject);
        }

		if (!bouncy)
        {
			Destroy(gameObject);
        }
    }
}
