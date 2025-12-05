using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Groups (Drag Objects Here)")]
    public CanvasGroup fadePanel;    // The Black Panel
    public CanvasGroup titleText;    // The Game Title
    public CanvasGroup buttonsGroup; // The Parent of Start/Exit buttons

    [Header("Audio Sources (Drag AudioSources Here)")]
    public AudioSource sfxSource;    // For the "Bang" sound
    public AudioSource musicSource;  // For the looping menu music

    [Header("Audio Clips (Drag Files Here)")]
    public AudioClip startSound;     // The "Big Bang"
    public AudioClip loopMusic;      // The background drone/music

    [Header("Settings")]
    public float fadeSpeed = 0.8f;   // Lower is slower/spookier
    public string gameSceneName = "CondominiumDaylight.unity";

    void Start()
    {
        // 1. Setup Initial State (Black Screen, Invisible UI)
        fadePanel.alpha = 1;
        titleText.alpha = 0;
        buttonsGroup.alpha = 0;

        // Disable buttons so you can't click invisible things
        buttonsGroup.interactable = false;
        buttonsGroup.blocksRaycasts = false;

        // 2. Start Music (Looping)
        if (musicSource != null && loopMusic != null)
        {
            musicSource.clip = loopMusic;
            musicSource.loop = true;
            musicSource.volume = 0; // Start silent for fade in
            musicSource.Play();
        }

        // 3. Begin the Intro Cinematic
        StartCoroutine(IntroSequence());
    }

    // --- CINEMATIC INTRO ---
    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(0.5f); // Short black pause

        // Fade OUT Black Panel (Screen reveals) + Fade IN Music
        StartCoroutine(FadeAudio(musicSource, 1.0f, 2.0f));
        yield return StartCoroutine(FadeCanvasGroup(fadePanel, 1, 0));

        // Fade IN Title
        yield return StartCoroutine(FadeCanvasGroup(titleText, 0, 1));

        yield return new WaitForSeconds(0.5f); // Pause before buttons

        // Fade IN Buttons
        yield return StartCoroutine(FadeCanvasGroup(buttonsGroup, 0, 1));

        // Enable Clicking
        buttonsGroup.interactable = true;
        buttonsGroup.blocksRaycasts = true;
    }

    // --- BUTTONS ---
    public void StartGame()
    {
        StartCoroutine(OutroSequence(true));
    }

    public void QuitGame()
    {
        StartCoroutine(OutroSequence(false));
    }

    // --- CINEMATIC OUTRO ---
    IEnumerator OutroSequence(bool isStartingGame)
    {
        // Lock buttons instantly
        buttonsGroup.interactable = false;

        // Play the "Big Bang" Impact
        if (isStartingGame && sfxSource != null && startSound != null)
        {
            sfxSource.PlayOneShot(startSound);
        }

        // Fade OUT Music
        StartCoroutine(FadeAudio(musicSource, 0f, 1.5f));

        // Fade IN Black Panel (Screen goes dark)
        yield return StartCoroutine(FadeCanvasGroup(fadePanel, 0, 1));

        // Optional: Wait for the Echo of the bang to finish
        yield return new WaitForSeconds(1.0f);

        if (isStartingGame)
        {
            SceneManager.LoadScene("CondominiumDaylight");
        }
        else
        {
            Debug.Log("Quitting...");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    // --- HELPERS ---
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end)
    {
        float counter = 0f;
        float duration = 1f / fadeSpeed;
        cg.alpha = start;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, counter / duration);
            yield return null;
        }
        cg.alpha = end;
    }

    IEnumerator FadeAudio(AudioSource source, float targetVolume, float duration)
    {
        if (source == null) yield break;

        float startVolume = source.volume;
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, counter / duration);
            yield return null;
        }
        source.volume = targetVolume;
    }
}
