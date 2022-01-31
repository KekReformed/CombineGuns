using UnityEngine;

public class ReboundShot : BasicProjectile
{
    private bool rebounding = false;
    private Vector2 reboundPoint;

    private new void Update()
    {
        base.Update();

        if (Input.GetButton("AltFire") && !rebounding)
        {
            Vector3 lookAt = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float AngleRad = Mathf.Atan2(lookAt.y - this.transform.position.y, lookAt.x - this.transform.position.x);

            float AngleDeg = (180 / Mathf.PI) * AngleRad;

            this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 180);

            Transform reboundPointTransform = gameObject.transform.Find("ReturnPoint");
            reboundPoint = new Vector2(reboundPointTransform.position.x, reboundPointTransform.position.y);

            body.velocity = (reboundPoint - body.position) * bulletSpeed;
            rebounding = true;
        }
    }
}
