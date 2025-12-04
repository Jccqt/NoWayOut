using System.Collections;
using TMPro;
using UnityEngine;

public class ObjectiveDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private CanvasGroup uiCanvasGroup;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float displayDuration = 4.0f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip updateSound;

    private Coroutine currentRoutine;

    private void Start()
    {
        // Ensure text is invisible when game starts
        if (uiCanvasGroup != null)
        {
            uiCanvasGroup.alpha = 0f;
        }
    }

    // This is the function we will call from your GameEventTrigger
    public void ShowObjective(string message)
    {
        objectiveText.text = message;

        // Play sound if assigned
        if (audioSource != null && updateSound != null)
        {
            audioSource.PlayOneShot(updateSound);
        }

        // Handle overlapping objectives (if player triggers two quickly)
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Fade In
        float timer = 0f;
        while (timer < fadeDuration)
        {
            uiCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        uiCanvasGroup.alpha = 1f;

        // Stay visible
        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        timer = 0f;
        while (timer < fadeDuration)
        {
            uiCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        uiCanvasGroup.alpha = 0f;
    }
}
