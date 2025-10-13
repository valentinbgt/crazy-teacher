using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScenesLoader : MonoBehaviour
{
    // [SerializeField] private GameObject sceneContainer;
    public GameObject sceneContainer; // Ton GameObject vide
    public Vector3 targetScale = new Vector3(0.5f, 0.5f, 1f);

    private bool SceneLoaded = false;

    private string[] scenesList = {
        "SlotMachine",
        "BallDropper",
        "TriPommePoire"
    };

    public void LoadMiniGame(string sceneName)
    {
        StartCoroutine(LoadMiniGameCoroutine(sceneName));
    }

    IEnumerator LoadMiniGameCoroutine(string sceneName)
    {
        // Charge la scène additive
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        // Récupère la scène
        Scene miniScene = SceneManager.GetSceneByName("SlotMachine");

        // Parent tous les objets racines au container
        foreach (GameObject go in miniScene.GetRootGameObjects())
        {
            if (go.name == "Main Camera")
            {
                Destroy(go);
                continue;
            }
            ; // Ignore la caméra principale
            go.transform.SetParent(sceneContainer.transform, false);
        }
        sceneContainer.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    }

    public void UnloadMiniGame(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }















    // public void LoadMiniGame(string sceneName)
    // {
    //     Debug.Log("Trying to load " + sceneName);
    //     SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    // }

    // public void UnloadMiniGame(string sceneName)
    // {
    //     SceneManager.UnloadSceneAsync(sceneName);
    // }

    // public void NextMiniGameStep()
    // {
    // }

    // void Update()
    // {
    //     if (Input.GetButton("P1_B6"))
    //     {
    //         LoadMiniGame("SlotMachine");
    //     }
    // }
}