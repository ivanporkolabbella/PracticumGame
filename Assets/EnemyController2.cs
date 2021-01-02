using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController2 : MonoBehaviour
{

    //Behavoiur:
    //Wait after spawn (roam around/ go to waypoint...) -> Idle state
    //if distance from player < 10 -> chaseState
    //if distance from player < 5 -> attackState

    public Transform target;

    public float Speed = 3f;
    public EnemyType type;

    private int health = 10;

    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var distanceFromTarget = Vector3.Distance(transform.position, target.position);
        anim.SetFloat("Distance", distanceFromTarget);

        anim.SetInteger("Health", health);
    }

    public void Shoot()
    {
        var bullet = AssetProvider.GetAsset(GameAsset.Bullet);
        bullet.GetComponent<BulletController>().Activate(transform);
    }
}