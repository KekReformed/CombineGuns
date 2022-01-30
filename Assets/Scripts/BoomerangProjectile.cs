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

        timeCount += Time.deltaTime;
    }
}
