using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private GameObject[] slotPrefabs;

    private GameObject wheel;
    private GameObject[] instantiatedObjects;
    private float spaceBetweenObjects = 2f;
    private float spinSpeed = 8f;

    //spin index, random float between 0 and spaceBetweenObjects
    private float spinIndex;
    private int numObjects;

    void Awake()
    {
        wheel = gameObject;
        spinIndex = Random.Range(0f, spaceBetweenObjects);
    }

    // Start is called before the first frame update
    void Start()
    {
        instantiatedObjects = new GameObject[slotPrefabs.Length];

        //shuffle the slotPrefabs array
        for (int i = 0; i < slotPrefabs.Length - 1; i++)
        {
            int j = Random.Range(i, slotPrefabs.Length);
            (slotPrefabs[i], slotPrefabs[j]) = (slotPrefabs[j], slotPrefabs[i]);
        }


        var prefabIndex = 0;
        foreach (GameObject prefab in slotPrefabs)
        {
            GameObject instance = Instantiate(prefab, wheel.transform);
            // Position the instance based on the index
            instance.transform.localPosition = new Vector3(0, -100, 0);

            instantiatedObjects[prefabIndex] = instance;
            prefabIndex++;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        numObjects = instantiatedObjects.Length;

        float spacing = spaceBetweenObjects;
        float totalHeight = numObjects * spacing;
        float minY = -3f * spacing;     // your chosen start offset

        // advance the scroll (positive = move down visually; flip sign if you want the other way)
        spinIndex = Mathf.Repeat(spinIndex + spinSpeed * Time.deltaTime, totalHeight);

        float loop = 0f;
        for (int i = 0; i < numObjects; i++)
        {
            // Wrap each item within [minY, minY + totalHeight)
            float y = minY + Mathf.Repeat(loop - spinIndex, totalHeight);
            instantiatedObjects[i].transform.localPosition = new Vector3(0f, y, 0f);
            loop += spacing;
        }
    }
}
