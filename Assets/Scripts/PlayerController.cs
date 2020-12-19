using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Vector3 moveDirection;
    private float speed = 5f;

    private new Camera camera;

    private Vector3 lastMousePosition;
    //Unity lifecycle
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        //register Inputs
        InputManager.OnPressedFire += RegisterShooting;
        InputManager.OnMovement += RegisterMove;
    }

    // Update is called once per frame
    void Update()
    {
        //HandleMove();
        HandleRotion();
        //HandleShooting();
    }

    private void FixedUpdate()
    {
        transform.position += moveDirection * Time.fixedDeltaTime * speed;
    }


    //Private methods
    private void HandleRotion()
    {
        //early return
        if (lastMousePosition.Equals(Input.mousePosition))
        {
            return;
        }

        lastMousePosition = Input.mousePosition;

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

    private void RegisterShooting()
    {
        var bullet = AssetProvider.GetAsset(GameAsset.Bullet);

        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
    }

    private void RegisterMove(Vector3 direction)
    {
        moveDirection = direction;
    }  
}