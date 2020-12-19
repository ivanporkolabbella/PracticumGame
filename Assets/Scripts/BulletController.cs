using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 10f;
    private Vector3 originPosition;

    private PoolableObject poolable;

    private void Awake()
    {
        originPosition = transform.position;
    }

    private void Start()
    {
        poolable = GetComponent<PoolableObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;

        if (Vector3.Distance(transform.position, originPosition) > 10)
        {
            if (poolable != null)
            {
                poolable.ReturnToPool();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
