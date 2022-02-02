using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSuicideScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!transform.root.gameObject.GetComponent<BasicProjectile>().bouncy)
        {
            Destroy(gameObject);
        }
    }
}
