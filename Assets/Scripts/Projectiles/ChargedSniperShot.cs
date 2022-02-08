using UnityEngine;

public class ChargedSniperShot : BasicProjectile
{
    [SerializeField] private float timeOnScreenMax;

    private Vector3 fireTargetVector;
    private LineRenderer lineRender;
    private float timeOnScreen;
    private int layerMask;


    private new void Start()
    {
        layerMask = LayerMask.GetMask("Platform","Player");
        Vector3 fireTargetPosition = new Vector2(fireTarget.position.x, fireTarget.position.y);
        fireTargetVector = -(transform.position - fireTargetPosition);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, fireTargetVector, Mathf.Infinity, layerMask);


        if (hit)
        {
            Debug.Log("I've hit something!");
            lineRender = GetComponent<LineRenderer>();
            lineRender.SetPositions(new Vector3[] { transform.position, hit.point });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        if(timeOnScreen > timeOnScreenMax)
        {
            Destroy(gameObject);
        }

        timeOnScreen += Time.deltaTime;
    }
}
