using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriPommePoire : MonoBehaviour
{
    public GameManager gameManager;
    
    public GameObject fruitSpawner;

    [SerializeField] public GameObject fruit1Prefab;
    [SerializeField] public GameObject fruit2Prefab;

    [SerializeField] public string currentFruitName;

    public int fruitsATrouver = 6;
    public TMP_Text fruitsATrouverText;

    private GameObject lastSpawnedFruit;

    public float timerDuration = 5f;

    private bool hasFailed = false;

    void Start()
    {
        SpawnRandomFruit(Vector3.zero);
        gameManager.StartTimer(timerDuration);
    }

    void Update()
    {
        if (gameManager.RemainingTime <= 0f && fruitsATrouver > 0)
        {
            Debug.Log("Temps écoulé, fruit manqué");
            if (hasFailed == false)
            {
                gameManager.LoseLife();
                hasFailed = true;
            }

        }
        
        fruitsATrouverText.text = "Fruits à trier: " + fruitsATrouver;
        if (fruitsATrouver > 0 && gameManager.RemainingTime > 0f)
        {
            if (Input.GetButtonDown("P1_B1"))
            {
                if (currentFruitName == "red")
                {
                    fruitsATrouver--;
                    Debug.Log("Fruit correct");
                    SpawnRandomFruit(Vector3.left);
                }
            }
            else if (Input.GetButtonDown("P1_B2"))
            {
                if (currentFruitName == "blue")
                {
                    fruitsATrouver--;
                    Debug.Log("Fruit correct");
                    SpawnRandomFruit(Vector3.right);
                }
            }
        }
        
    }

    void SpawnRandomFruit(Vector3 moveDirection)
    {
        if (lastSpawnedFruit != null)
        {
            StartCoroutine(MoveAndDestroyFruit(lastSpawnedFruit, moveDirection));
        }

        GameObject prefabToSpawn = Random.Range(0, 2) == 0 ? fruit1Prefab : fruit2Prefab;
        lastSpawnedFruit = Instantiate(prefabToSpawn, fruitSpawner.transform);

        if (prefabToSpawn == fruit1Prefab)
            currentFruitName = "red";
        else
            currentFruitName = "blue";
    }

    IEnumerator MoveAndDestroyFruit(GameObject fruit, Vector3 direction)
    {
        float duration = 0.1f;
        float elapsed = 0f;
        Vector3 startPos = fruit.transform.position;
        Vector3 endPos = startPos + direction * 300f;

        while (elapsed < duration)
        {
            fruit.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        fruit.transform.position = endPos;
        Destroy(fruit);
    }
}