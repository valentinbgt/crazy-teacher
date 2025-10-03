using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
    private string[] scenesList = [
        "SlotMachine",
        "BallDropper",
        "TriPommePoire"
    ];
    private string loadingScene = "LoadingScene";

    public void LoadMiniGame(string sceneName)
    {
        Debug.Log("Trying to load " + sceneName);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadMiniGame(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void NextMiniGameStep()
    {

    }

    void Update()
    {
        if (Input.GetButton("P1_B6"))
        {
            LoadMiniGame("SlotMachine");
        }
    }
}