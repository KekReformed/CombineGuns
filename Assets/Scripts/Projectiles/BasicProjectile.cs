using UnityEngine;

public class BasicProjectile : ProjectileElements
{
	public float bulletSpeed;
	private int collisionCount = 0;
	public Transform fireTarget;

	public new void Start()
	{
		base.Start();

		Vector2 fireTargetPosition = new Vector2(fireTarget.position.x, fireTarget.position.y);

		body.velocity = (fireTargetPosition - body.position) * bulletSpeed;

		float AngleRad = Mathf.Atan2(fireTargetPosition.y - transform.position.y, fireTargetPosition.x - transform.position.x);

		float AngleDeg = (180 / Mathf.PI) * AngleRad;

		transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
	}

	public void Update()
	{
		//Deletes the projectile if its off screen
		if (!GetComponent<Renderer>().isVisible)
		{
			Destroy(gameObject);
		}
	}

    private new void OnCollisionEnter2D(Collision2D collision)
    {
		base.OnCollisionEnter2D(collision);
		if (collision.gameObject.tag == "Platform")
        {
			this.collisionCount += 1;
		}

		if (collisionCount > bounceLimit)
        {
			Destroy(gameObject);
        }
    }
}
