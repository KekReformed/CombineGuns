using UnityEngine;

public class SniperShot : BasicProjectile
{
    [SerializeField] private float timeToSpeedUp;

    private float counter = 0;
    private float fastBulletSpeed;
    private bool spedUp = false;

    private new void Start()
    {
        fastBulletSpeed = bulletSpeed*2.5f;
        bulletSpeed *= 0.1f;
        base.Start();

        if (bouncy)
        {
            body.gravityScale *= 0.1f;
        }
    }

    private void FixedUpdate()
    {
        if (counter > timeToSpeedUp && !spedUp)
        {
            Transform fireTarget = transform.Find("FireTarget");
            Vector2 fireVector = new Vector2(fireTarget.position.x, fireTarget.position.y);

            body.velocity = (fireVector - body.position) * fastBulletSpeed;
            if (bouncy)
            {
                body.gravityScale = 1;
            }
            spedUp = true;
        }

        counter += Time.deltaTime;
    }
}
