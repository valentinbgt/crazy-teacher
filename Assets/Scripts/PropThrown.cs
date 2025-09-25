using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropThrown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * 20f * Time.deltaTime);
    }
}
