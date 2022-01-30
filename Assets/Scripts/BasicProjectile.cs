using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
	public float bulletSpeed;
	public string fireTargetName;
	public bool bouncy = false;

	[SerializeField] private int bounceLimit;

	private Rigidbody2D body;
	private int collisionCount = 0;
	private CircleCollider2D circleCollider;

	private void Start()
	{
		body = this.GetComponent<Rigidbody2D>();
		circleCollider = GetComponent<CircleCollider2D>();
		Vector2 fireTargetPosition = new Vector2(GameObject.Find(fireTargetName).transform.position.x, GameObject.Find(fireTargetName).transform.position.y);
		body.velocity = (fireTargetPosition - body.position)*bulletSpeed;

		if (bouncy)
		{
			body.gravityScale = 1;
			circleCollider.sharedMaterial.bounciness = 1;
			circleCollider.isTrigger = false;
		}
	}

	private void Update()
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
    }
}
