using UnityEngine;

public class ChargedReboundShot : BasicProjectile
{

    [SerializeField] private GameObject bulletPrefab;
    private bool fired = false;

    private new void Update()
    {
        base.Update();
        if (Input.GetButton("AltFire") && !fired)
        {
            Vector3 lookAt = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float AngleRad = Mathf.Atan2(lookAt.y - this.transform.position.y, lookAt.x - this.transform.position.x);

            float AngleDeg = (180 / Mathf.PI) * AngleRad;

            this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 180);

            GameObject bulletClone = Instantiate(bulletPrefab,transform.position,transform.rotation);
            bulletClone.GetComponent<BasicProjectile>().fireTarget = transform.Find("ReturnPoint").position;
            bulletClone.GetComponent<BasicProjectile>().bulletSpeed = bulletSpeed*0.33f;

            fired = true;
        }
    }
}
