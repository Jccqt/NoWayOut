using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("UI Groups")]
    public CanvasGroup warningGroup;     // Drag Seizure Image here
    public CanvasGroup headphoneGroup;   // Drag Headphone Image here

    [Header("Timing Settings")]
    public float fadeDuration = 1.0f;    // How long the fade takes
    public float displayDuration = 2.5f; // How long it stays fully visible
    public string menuSceneName = "MainMenu";

    void Start()
    {
        // Ensure everything is invisible at the start
        warningGroup.alpha = 0;
        headphoneGroup.alpha = 0;

        StartCoroutine(BootSequence());
    }

    IEnumerator BootSequence()
    {
        // --- PHASE 1: WARNING SCREEN ---
        // Fade In
        yield return StartCoroutine(FadeCanvasGroup(warningGroup, 0, 1));

        // Wait (Read time)
        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        yield return StartCoroutine(FadeCanvasGroup(warningGroup, 1, 0));

        // Optional: Small pause between screens
        yield return new WaitForSeconds(0.5f);


        // --- PHASE 2: HEADPHONE SCREEN ---
        // Fade In
        yield return StartCoroutine(FadeCanvasGroup(headphoneGroup, 0, 1));

        // Wait (Read time)
        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        yield return StartCoroutine(FadeCanvasGroup(headphoneGroup, 1, 0));


        // --- PHASE 3: LOAD GAME ---
        SceneManager.LoadScene(menuSceneName);
    }

    // This is a reusable helper function that handles the math
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Lerp calculates the smooth transition between start and end
            cg.alpha = Mathf.Lerp(start, end, elapsedTime / fadeDuration);
            yield return null; // Wait for the next frame
        }

        // Ensure it is exactly the target value at the end
        cg.alpha = end;
    }
}
