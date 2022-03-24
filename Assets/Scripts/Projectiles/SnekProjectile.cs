using UnityEngine;

public class SnekProjectile : BasicProjectile
{
    private Vector2 mouseVector;
    private BoxCollider2D boxCollider;

    private new void Start()
    {
        base.Start();
        boxCollider = GetComponent<BoxCollider2D>();

        body.gravityScale = 0.5f;
        if (element != "Bouncy")
        {
            bounceLimit = 999999;
        }
        bounceLimit = 999999;
    }

    private new void Update()
    {
        base.Update();
        //Look at mouse
        if (Input.GetButtonDown("AltFire"))
        {
            mouseVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float AngleRad = Mathf.Atan2(mouseVector.y - transform.position.y, mouseVector.x - transform.position.x);

            float AngleDeg = (180 / Mathf.PI) * AngleRad;

            transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 180);
        }

        float acceleration = 0.2f;
        Vector2 direction = (mouseVector - body.position).normalized * bulletSpeed;

        if (Vector3.Dot(body.velocity, direction) < 0)
        {
            return;
        }

        if (OnGround())
        {
            body.AddForce(body.mass * (acceleration * direction));
        }
    }

    private bool OnGround()
    {
        Vector2 hitboxSize = boxCollider.bounds.size;
        hitboxSize.x *= 0.75f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, hitboxSize, 0, Vector2.down, 0.1f);

        return raycastHit.collider != null;
    }
}
