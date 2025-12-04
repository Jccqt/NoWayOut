using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("UI Settings")]
    public Image fadePanel; // Drag your Black Panel here
    public float fadeDuration = 2.0f;

    [Header("Scene Settings")]
    public string mainSceneName = "MainLevel"; // The name of your gameplay scene

    // This function will be called by the Timeline when it finishes
    public void StartGameSequence()
    {
        StartCoroutine(TransitionToNextScene());
    }

    IEnumerator TransitionToNextScene()
    {
        // 1. Fade Out (Make screen black)
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            var color = fadePanel.color;
            color.a = alpha;
            fadePanel.color = color;
            yield return null;
        }

        // Ensure it is fully black
        var finalColor = fadePanel.color;
        finalColor.a = 1f;
        fadePanel.color = finalColor;

        // 2. Load the Main Scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(mainSceneName);

        // This prevents the scene from activating immediately
        operation.allowSceneActivation = false;

        // OPTIONAL: Fake "Loading..." wait time (e.g., 3 seconds)
        yield return new WaitForSeconds(3f);

        // 3. Allow the scene to switch
        operation.allowSceneActivation = true;
    }
}
