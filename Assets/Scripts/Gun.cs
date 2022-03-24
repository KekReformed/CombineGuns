using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPun
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
	

	private Transform firePoint;
	private float cooldownTimer;
	private float chargeAmount = 0;
	private float recoilTimer;
	private float attackCooldownModifier = 1;
	private float damageModifier = 1;
	private float recoilModifier = 1;
	private bool resetAccelerations = true;
	private bool boltAction;
	private bool multishot;
	private bool semiAuto;
	private bool burst;
	private bool automatic;
	private string element;

	private void Start()
	{
		cooldownTimer = attackCooldown;
		firePoint = transform.Find("FirePoint");

		if (firePoint == null)
		{
			Debug.Log("You need an object called FirePoint!!!");
		}

		element = PlayerPrefs.GetString("Element","Fire");
		automatic = PlayerPrefs.GetInt("Auto", 0) == 1;
		multishot = PlayerPrefs.GetInt("Multishot", 0) == 1;
		semiAuto = PlayerPrefs.GetInt("SemiAuto", 1) == 1;
		burst = PlayerPrefs.GetInt("Burst", 0) == 1;
		boltAction = PlayerPrefs.GetInt("BoltAction", 0) == 1;

		SetProjectile(PlayerPrefs.GetString("Projectile","Orbital"));
		SetModifiers();
	}

	private void Update()
	{

		if (cooldownTimer > attackCooldown)
		{
			if (Input.GetButtonDown("Shoot") && automatic == false)
			{
				Shoot(recoil, burst, multishot, false);
			}

			if (Input.GetButton("Shoot") && automatic == true)
			{
				Shoot(recoil, burst, multishot, false);
			}

			if (Input.GetButton("Charged Shot"))
			{
				chargeAmount += Time.deltaTime;
				Debug.Log("Chargin! " + chargeAmount);
			}

			if (Input.GetButtonUp("Charged Shot"))
			{
				if (chargeAmount >= attackCooldown * 3)
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
		Vector2 fireTargetPosition = transform.Find("FireTarget").position;
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
			StartCoroutine(Burst(transform.Find("FireTarget").position, charged));
		}

        else
        {
			photonView.RPC("Pew", RpcTarget.All,transform.Find("FireTarget").position, charged);
		}

		if (multishot)
		{
			if (burst)
			{
				StartCoroutine(Burst(transform.Find("FireTargetShotgunUppist").position, charged));
				StartCoroutine(Burst(transform.Find("FireTargetShotgunUpper").position, charged));
				StartCoroutine(Burst(transform.Find("FireTargetShotgunLower").position, charged));
				StartCoroutine(Burst(transform.Find("FireTargetShotgunLowist").position, charged));
			}
            else
            {
				photonView.RPC("Pew", RpcTarget.All, transform.Find("FireTargetShotgunUppist").position,charged);
				photonView.RPC("Pew", RpcTarget.All, transform.Find("FireTargetShotgunUpper").position, charged);
				photonView.RPC("Pew", RpcTarget.All, transform.Find("FireTargetShotgunLower").position, charged);
				photonView.RPC("Pew", RpcTarget.All, transform.Find("FireTargetShotgunLowist").position, charged);
			}
		}
	}


	[PunRPC]
	private void Pew(Vector3 fireTarget, bool charged)
    {
        if (charged)
        {
            GameObject bulletClone = Instantiate(chargedBulletPrefab, firePoint.position, firePoint.rotation);
			SetParameters(bulletClone, fireTarget, bulletSpeed);
		}
        else
        {
            GameObject bulletClone = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
			SetParameters(bulletClone, fireTarget, bulletSpeed);
		}
    }

	private void SetParameters(GameObject bulletClone, Vector3 fireTarget, float bulletSpeed)
    {
		bulletClone.GetComponent<BasicProjectile>().fireTarget = fireTarget;
		bulletClone.GetComponent<BasicProjectile>().bulletSpeed = bulletSpeed;
		bulletClone.GetComponent<BasicProjectile>().element = element;
		bulletClone.GetComponent<BasicProjectile>().bounceLimit = bounceLimit;
	}

	private void SetModifiers()
    {
		//Stat changes depending on different things
		//FireMode
		if (boltAction)
		{
			attackCooldownModifier += 1.5f;
			damageModifier += 1f;
			recoilModifier += 0.05f;
		}

		if (multishot)
		{
			attackCooldownModifier += 0.5f;
			damageModifier += -0.75f;
			recoilModifier += 0.2f;
		}

		if (semiAuto)
		{
			attackCooldown += 0f;
			damageModifier += 0f;
			recoilModifier += 0f;
		}

		if (burst)
		{
			attackCooldown += 0f;
			damageModifier += -0.6f;
			recoilModifier += -0.4f;
		}

		if (automatic)
		{
			attackCooldownModifier += -0.25f;
			damageModifier += -0.2f;
			recoilModifier += 0f;
		}

		attackCooldown *= attackCooldownModifier;
		damage *= damageModifier;
		recoil *= recoilModifier;
		recoilTime *= recoilModifier * 2;
	}

	//DON't COPY THIS OVER IT DOESN'T WORK
	private void SetProjectile(string projectile)
    {
		if (projectile == "Orbital")
        {
			bulletPrefab = Resources.Load<GameObject>("OrbiterShot");
			chargedBulletPrefab = Resources.Load<GameObject>("ChargedOrbiterShot");
		}

		else if(projectile == "Rebound")
        {
			bulletPrefab = Resources.Load<GameObject>("ReboundShot");
			chargedBulletPrefab = Resources.Load<GameObject>("ChargedReboundShot");
		}

		else if (projectile == "Sniper")
		{
			bulletPrefab = Resources.Load<GameObject>("SniperShot");
			chargedBulletPrefab = Resources.Load<GameObject>("ChargedSniperShot");
		}

		else if (projectile == "Snake")
		{
			bulletPrefab = Resources.Load<GameObject>("Snek Head");
			chargedBulletPrefab = Resources.Load<GameObject>("Charged Snek Head");
		}

        else
        {
			Debug.Log("I've been given the wrong projectile!");
        }
	}

	[PunRPC]
	IEnumerator Burst(Vector3 fireTarget, bool charged)
	{
		if (charged)
        {
			for (int i = 0; i < 3; i++)
			{
				GameObject bulletClone = PhotonNetwork.Instantiate(chargedBulletPrefab.name, firePoint.position, firePoint.rotation);
				SetParameters(bulletClone, fireTarget, bulletSpeed);
				yield return new WaitForSeconds(burstInterval);
			}
		}

        else
        {
			for (int i = 0; i < 3; i++)
			{
				GameObject bulletClone = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
				SetParameters(bulletClone, fireTarget, bulletSpeed);
				yield return new WaitForSeconds(burstInterval);
			}
		}
	}
}
