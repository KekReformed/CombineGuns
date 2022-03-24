using Photon.Pun;
using UnityEngine;

public class NewGun : MonoBehaviourPun
{
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject chargedProjectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform fireTarget;

    private float cooldownTimer;
    private float chargeAmount;
    private string element;

    private void Start()
    {
        element = PlayerPrefs.GetString("Element", "Fire");
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (cooldownTimer > attackCooldown)
        {
            if (Input.GetButtonDown("Shoot"))
            {
                photonView.RPC("Pew", RpcTarget.All, false);
            }

            if (Input.GetButtonDown("Charged Shot"))
            {
                chargeAmount += Time.deltaTime;
                Debug.Log("Chargin! " + chargeAmount);
            }

            if (Input.GetButtonUp("Charged Shot"))
            {
                if (chargeAmount >= attackCooldown * 3)
                {
                    photonView.RPC("Pew", RpcTarget.All, true);
                }
                else
                {
                    photonView.RPC("Pew", RpcTarget.All, false);
                }
                chargeAmount = 0;
            }
        }

        cooldownTimer += Time.deltaTime;
    }

    [PunRPC]
    private void Pew(bool charged)
    {
        if (charged)
        {
            GameObject bulletClone = Instantiate(chargedProjectile, firePoint.position, firePoint.rotation);
            SetParameters(bulletClone, fireTarget.position, projectileSpeed);
        }
        else
        {
            GameObject bulletClone = Instantiate(projectile, firePoint.position, firePoint.rotation);
            SetParameters(bulletClone, fireTarget.position, projectileSpeed);
        }
        cooldownTimer = 0;
    }

    private void SetParameters(GameObject bulletClone, Vector3 fireTarget, float bulletSpeed)
    {
        bulletClone.GetComponent<BasicProjectile>().fireTarget = fireTarget;
        bulletClone.GetComponent<BasicProjectile>().bulletSpeed = bulletSpeed;
        bulletClone.GetComponent<BasicProjectile>().element = element;
    }
}
