using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateauController : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    GameManager gameManager;
    public float maxTilt; 

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        float angle = transform.rotation.eulerAngles.z;
        angle = (angle > 180) ? angle - 360 : angle;

        if (angle > maxTilt) {
            gameManager.GameOver();
            // rigidbody2D.MoveRotation((maxTilt+0.1f));
            // transform.Rotate(new Vector3(0f, 0f, maxTilt-angle));
        } else if (angle < -maxTilt) {
            gameManager.GameOver();
            // rigidbody2D.MoveRotation(-(maxTilt+0.1f));
            // transform.Rotate(new Vector3(0f, 0f, -angle-maxTilt));
        }

        float torque = angle + rigidbody2D.angularVelocity;

        // Force to try to balance the plateau
        rigidbody2D.AddTorque(-(angle+rigidbody2D.angularVelocity));

        // Debug.Log(angle);
    }

    public void Fall()
    {
        rigidbody2D.AddForce(Vector2.up*5f, ForceMode2D.Impulse);
        rigidbody2D.AddTorque(80f, ForceMode2D.Impulse);
    }
}
