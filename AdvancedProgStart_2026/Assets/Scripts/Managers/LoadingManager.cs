using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class LoadingManager
{
    public static bool isLoading = false;
    public static int currentLevel = 0;
    public static string currentLevelName;

    public static IEnumerator LoadLevel(string levelName)
    {
        isLoading = true;

        GameManager.Instance.ManFisherGo.SetActive(false);

        if (!string.IsNullOrEmpty(currentLevelName))
        {
            AsyncOperation unload = SceneManager.UnloadSceneAsync(currentLevelName);

            while (!unload.isDone)
            {
                yield return null;
            }
        }

        AsyncOperation load = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        while (!load.isDone)
            yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName));

        GameManager.Instance.ManFisherGo.transform.position = Vector3.zero;
        GameManager.Instance.ManFisherGo.SetActive(true);

        currentLevelName = levelName;
        isLoading = true;
    }
}
