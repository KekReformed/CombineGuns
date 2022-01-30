using UnityEngine;

public class BoomerangProjectile : BasicProjectile
{
    [SerializeField] private float returnTime;

    private float timeCount = 0;
    private bool returning = false;
    private Vector2 returnPoint;

    private void FixedUpdate()
    {
        if (timeCount > returnTime && returning == false)
        {
            Transform returnPointTransform = gameObject.transform.Find("ReturnPoint");
            returnPoint = new Vector2(returnPointTransform.position.x, returnPointTransform.position.y);
            body.velocity = (returnPoint - body.position) * bulletSpeed;
            returning = true;
        }

        Vector3 lookAt = firedFrom;

        float AngleRad = Mathf.Atan2(lookAt.y - this.transform.position.y, lookAt.x - this.transform.position.x);

        float AngleDeg = (180 / Mathf.PI) * AngleRad;

        this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg-180);
        timeCount += Time.deltaTime;
    }
}
