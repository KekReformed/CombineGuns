using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
	[SerializeField] private float attackCooldown;
	[SerializeField] private float burstInterval;
	[SerializeField] private float recoil;
	[SerializeField] private float recoilTime;
	[SerializeField] private float damage;
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Rigidbody2D playerBody;
	[SerializeField] private PlayerMovement player;
	[SerializeField] private bool multishot;
	[SerializeField] private bool burst;
	[SerializeField] private bool automatic;
	

	private Transform firePoint;
	private float cooldownTimer;
	private float recoilTimer;
	private bool resetAccelerations = true;

	private void Awake()
	{
		firePoint = transform.Find("FirePoint");
		if (firePoint == null)
		{
			Debug.Log("You need an object called FirePoint!!!");
		}
	}

	private void Update()
	{
		if (cooldownTimer > attackCooldown)
		{
			if (Input.GetMouseButtonDown(0) && automatic == false)
			{
				Shoot(recoil,burst,multishot);
			}

			if (Input.GetMouseButton(0) && automatic == true)
			{
				Shoot(recoil,burst,multishot);
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
	
	private void Shoot(float recoil,bool burst, bool multishot)
	{
		Vector2 fireTargetPosition = new Vector2(GameObject.Find("FireTarget").transform.position.x, GameObject.Find("FireTarget").transform.position.y);
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
			StartCoroutine(Burst("FireTarget"));
		}

		if (multishot)
		{
			if (burst)
			{
				StartCoroutine(Burst("FireTargetShotgunUppist"));
				StartCoroutine(Burst("FireTargetShotgunUpper"));
				StartCoroutine(Burst("FireTargetShotgunLower"));
				StartCoroutine(Burst("FireTargetShotgunLowist"));
			}
            else
            {
				pew("FireTargetShotgunUppist");
				pew("FireTargetShotgunUpper");
				pew("FireTarget");
				pew("FireTargetShotgunLower");
				pew("FireTargetShotgunLowist");
			}
		}

		else
		{
			GameObject bulletClone = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
			bulletClone.GetComponent<BasicProjectile>().fireTargetName = "FireTarget";
		}

	}


	private void pew(string fireTarget)
    {
		GameObject bulletClone_1 = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		bulletClone_1.GetComponent<BasicProjectile>().fireTargetName = fireTarget;

	}

	IEnumerator Burst(string targetName)
	{
		for (int i = 0; i < 3; i++)
		{
			GameObject bulletClone = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
			bulletClone.GetComponent<BasicProjectile>().fireTargetName = targetName;
			yield return new WaitForSeconds(burstInterval);
		}
	}
}
