using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeedDegreesPerSecond = 70f;
    public Vector3 localRotationAxis = Vector3.up;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {

    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ball"))
        {
            gameManager.LoseLife();
            Debug.Log("loose");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationSpeedDegreesPerSecond == 0f)
        {
            return;
        }
        float deltaDegrees = rotationSpeedDegreesPerSecond * Time.deltaTime;
        transform.Rotate(localRotationAxis.normalized, deltaDegrees, Space.Self);
    }
}
