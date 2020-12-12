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
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
        HandleRotion();
        HandleShooting();
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

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = AssetProvider.GetAsset(GameAsset.Bullet);

            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
        }
    }

    private void HandleMove()
    {
        var inputX = Input.GetAxis(InputStrings.axisX);
        var inputY = Input.GetAxis(InputStrings.axixY);

        moveDirection = new Vector3(inputX, 0, inputY).normalized;
    }  
}

public struct InputStrings
{
    public static string axisX = "Horizontal";
    public static string axixY = "Vertical";
} 