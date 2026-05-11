using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_ShipPlayerController : SC_ShipController
{

    void Update()
    {
        Rigidbody2D rigidBody = gameObject.GetComponent<Rigidbody2D>();

        if (Input.GetKey(KeyCode.Space))
        {
            _ShipManager.RequestShoot();
        }
        if (Input.GetKey(KeyCode.Z))
        {
            rigidBody.AddRelativeForce(new Vector2(0, 1));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rigidBody.AddTorque(1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidBody.AddTorque(-1);
        }
    }
}
