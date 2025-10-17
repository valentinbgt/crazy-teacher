using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScenesLoader : MonoBehaviour
{
    public GameObject sceneContainer;

    public void LoadMiniGame(string sceneName)
    {
        StartCoroutine(LoadMiniGameSequence(sceneName));
    }

    private IEnumerator LoadMiniGameSequence(string sceneName)
    {
        // Étape 1 : Charger la scène de transition
        yield return StartCoroutine(LoadTransitionSceneCoroutine("LoadingScene"));

        // Étape 2 : Petite pause
        yield return new WaitForSeconds(1f);

        // Étape 3 : Décharger la scène de transition
        yield return StartCoroutine(UnloadTransitionSceneCoroutine("LoadingScene"));

        // Étape 5 : Charger la scène du mini-jeu
        yield return StartCoroutine(LoadMiniGameCoroutine(sceneName));
    }

    private IEnumerator LoadTransitionSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        Scene transitionScene = SceneManager.GetSceneByName(sceneName);
        foreach (GameObject go in transitionScene.GetRootGameObjects())
        {
            go.transform.SetParent(sceneContainer.transform, false);
        }
    }

    private IEnumerator UnloadTransitionSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone)
            yield return null;

        foreach (Transform child in sceneContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator LoadMiniGameCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        Scene miniScene = SceneManager.GetSceneByName(sceneName);
        foreach (GameObject go in miniScene.GetRootGameObjects())
        {
            if (go.name == "Main Camera" || go.name == "EventSystem")
            {
                Destroy(go);
                continue;
            }
            go.transform.SetParent(sceneContainer.transform, false);
        }
    }

    public void UnloadMiniGame(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
        foreach (Transform child in sceneContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
