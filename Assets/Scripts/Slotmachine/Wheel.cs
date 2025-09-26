using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private GameObject[] slotPrefabs;

    private GameObject wheel;
    private GameObject[] instantiatedObjects;
    private float spaceBetweenObjects = 2f;
    private float spinSpeed = 0.05f;
    private float spinIndex = 0f;
    private int numObjects;

    void Awake()
    {
        wheel = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        instantiatedObjects = new GameObject[slotPrefabs.Length];

        //shuffle the slotPrefabs array
        for (int i = 0; i < slotPrefabs.Length; i++)
        {
            int randomIndex = Random.Range(0, slotPrefabs.Length);
            (slotPrefabs[randomIndex], slotPrefabs[i]) = (slotPrefabs[i], slotPrefabs[randomIndex]);
        }


        var index = -spaceBetweenObjects * 3;
        var prefabIndex = 0;
        foreach (GameObject prefab in slotPrefabs)
        {
            GameObject instance = Instantiate(prefab, wheel.transform);
            // Position the instance based on the index
            instance.transform.localPosition = new Vector3(0, index, 0);
            index += spaceBetweenObjects;

            instantiatedObjects[prefabIndex] = instance;
            prefabIndex++;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var loop = -spaceBetweenObjects * 3;
        spinIndex += spinSpeed;
        numObjects = instantiatedObjects.Length;

        foreach (GameObject element in instantiatedObjects)
        {
            float yPosition = spinIndex + loop;

            if (yPosition >= (numObjects - 3) * spaceBetweenObjects)
            {
                yPosition += numObjects * spaceBetweenObjects;
            }

            element.transform.localPosition = new Vector3(0, yPosition, 0);
            loop += spaceBetweenObjects;
        }
    }
}
