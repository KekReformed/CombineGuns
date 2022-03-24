using UnityEngine;

public class BasicProjectile : ProjectileElements
{
	public float bulletSpeed;
	private int collisionCount = 0;
	public Vector3 fireTarget;

    private new void Start()
    {
		base.Start();
		body.velocity = (fireTarget - transform.position) * bulletSpeed;
	}

    public void Update()
	{
		//Deletes the projectile if its off screen
		if (!GetComponent<Renderer>().isVisible)
		{
			Destroy(gameObject);
		}
	}

    public new void OnTriggerEnter2D(Collider2D collision)
    {
		base.OnTriggerEnter2D(collision);
		this.collisionCount += 1;

		if (collisionCount > bounceLimit)
        {
			Destroy(gameObject);
        }
		
		if (collision.gameObject.layer == 3)
        {
			Destroy(gameObject);
        }
    }
}
