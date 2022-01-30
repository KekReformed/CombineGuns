using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
	[SerializeField] private float attackCooldown;
	[SerializeField] private float burstInterval;
	[SerializeField] private float bulletSpeed;
	[SerializeField] private float recoil;
	[SerializeField] private float recoilTime;
	[SerializeField] private float damage;
	[SerializeField] private int bounceLimit;
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private GameObject chargedBulletPrefab;
	[SerializeField] private Rigidbody2D playerBody;
	[SerializeField] private PlayerMovement player;
	[SerializeField] private bool boltAction;
	[SerializeField] private bool multishot;
	[SerializeField] private bool semiAuto;
	[SerializeField] private bool burst;
	[SerializeField] private bool automatic;
	[SerializeField] public bool bouncy;
	

	private Transform firePoint;
	private float cooldownTimer;
	private float chargeAmount = 0;
	private float recoilTimer;
	private bool resetAccelerations = true;

	private void Awake()
	{
		cooldownTimer = attackCooldown;
		firePoint = transform.Find("FirePoint");

		if (firePoint == null)
		{
			Debug.Log("You need an object called FirePoint!!!");
		}

		//Stat changes depending on different things
		//FireMode
		if (boltAction)
        {
			attackCooldown *= 2.5f;
			damage *= 2f;
			recoil *= 1.05f;
		}

		if (multishot)
		{
			attackCooldown *= 1.5f;
			damage *= 0.25f;
			recoil *= 1.2f;
		}

		if (semiAuto)
		{
			attackCooldown *= 1f;
			damage *= 1f;
			recoil *= 1f;
		}

		if (burst)
		{
			attackCooldown *= 1f;
			damage *= 0.4f;
			recoil *= 0.6f;
		}

		if (automatic)
        {
			attackCooldown *= 0.75f;
			damage *= 0.8f;
			recoil *= 1;
        }
	}

	private void Update()
	{


		if (cooldownTimer > attackCooldown)
		{
			if (Input.GetButtonDown("Shoot") && automatic == false)
			{
				Shoot(recoil,burst,multishot,false);
			}

			if (Input.GetButton("Shoot") && automatic == true)
			{
				Shoot(recoil,burst,multishot,false);
			}

			if (Input.GetButton("Charged Shot"))
            {
				chargeAmount += Time.deltaTime;
			}

            if (Input.GetButtonUp("Charged Shot"))
            {
				if (chargeAmount >= attackCooldown*3)
                {
					Shoot(recoil * 2, burst, multishot, true);
				}
                else
                {
					Shoot(recoil, burst, multishot, false);
				}
				chargeAmount = 0;
			}
		}

		cooldownTimer += Time.deltaTime;
		recoilTimer += Time.deltaTime;

		if (recoilTimer > recoilTime && !resetAccelerations)
		{
			player.acceleration *= 10f;
			player.decceleration *= 10f;
			resetAccelerations = true;
		}

	}
	
	private void Shoot(float recoil,bool burst, bool multishot, bool charged)
	{
		Vector2 fireTargetPosition = new Vector2(transform.Find("FireTarget").position.x, transform.Find("FireTarget").position.y);
		playerBody.velocity += (fireTargetPosition - playerBody.position) * -recoil;
		cooldownTimer = 0;
		recoilTimer = 0;

		if (resetAccelerations)
		{
			player.acceleration *= 0.1f;
			player.decceleration *= 0.1f;
			resetAccelerations = false;
		}

		if (burst)
		{
			StartCoroutine(Burst(transform.Find("FireTarget"), charged));
		}

        else
        {
			Pew(transform.Find("FireTarget"), charged);
		}

		if (multishot)
		{
			if (burst)
			{
				StartCoroutine(Burst(transform.Find("FireTargetShotgunUppist"), charged));
				StartCoroutine(Burst(transform.Find("FireTargetShotgunUpper"), charged));
				StartCoroutine(Burst(transform.Find("FireTargetShotgunLower"), charged));
				StartCoroutine(Burst(transform.Find("FireTargetShotgunLowist"), charged));
			}
            else
            {
				Pew(transform.Find("FireTargetShotgunUppist"), charged);
				Pew(transform.Find("FireTargetShotgunUpper"), charged);
				Pew(transform.Find("FireTargetShotgunLower"), charged);
				Pew(transform.Find("FireTargetShotgunLowist"), charged);
			}
		}
	}


	private void Pew(Transform fireTarget, bool charged)
    {
		if (charged)
        {
			GameObject bulletClone = Instantiate(chargedBulletPrefab, firePoint.position, firePoint.rotation);
			SetParameters(bulletClone, fireTarget, bulletSpeed, bouncy);
		}
        else
        {
			GameObject bulletClone = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
			SetParameters(bulletClone, fireTarget, bulletSpeed, bouncy);
		}
	}


	private void SetParameters(GameObject bulletClone, Transform fireTarget, float bulletSpeed, bool bouncy)
    {
		bulletClone.GetComponent<BasicProjectile>().fireTarget = fireTarget;
		bulletClone.GetComponent<BasicProjectile>().bulletSpeed = bulletSpeed;
		bulletClone.GetComponent<BasicProjectile>().bouncy = bouncy;
		bulletClone.GetComponent<BasicProjectile>().bounceLimit = bounceLimit;
	}


	IEnumerator Burst(Transform fireTarget, bool charged)
	{
		if (charged)
        {
			for (int i = 0; i < 3; i++)
			{
				GameObject bulletClone = Instantiate(chargedBulletPrefab, firePoint.position, firePoint.rotation);
				SetParameters(bulletClone, fireTarget, bulletSpeed, bouncy);
				yield return new WaitForSeconds(burstInterval);
			}
		}

        else
        {
			for (int i = 0; i < 3; i++)
			{
				GameObject bulletClone = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
				SetParameters(bulletClone, fireTarget, bulletSpeed, bouncy);
				yield return new WaitForSeconds(burstInterval);
			}
		}
	}
}
