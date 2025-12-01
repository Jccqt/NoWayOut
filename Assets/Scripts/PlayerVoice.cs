using System.Collections;
using UnityEngine;

public class PlayerVoice : MonoBehaviour
{
    private AudioSource voiceSource;
    private float nextDelay = 0f; // Stores the delay for the NEXT spoken line
    private Coroutine currentRoutine; // Tracks the currently running speech

    void Awake()
    {
        voiceSource = GetComponent<AudioSource>();
    }

    // 1. Helper Function: Set the delay for the NEXT line only
    public void SetDelay(float seconds)
    {
        nextDelay = seconds;
    }

    // 2. The Main Function
    public void SayLine(AudioClip clipToPlay)
    {
        if (clipToPlay != null)
        {
            // Stop any voice that is currently talking OR waiting to talk
            if (currentRoutine != null) StopCoroutine(currentRoutine);

            // Start the new speech process
            currentRoutine = StartCoroutine(PlayRoutine(clipToPlay, nextDelay));
        }
    }

    IEnumerator PlayRoutine(AudioClip clip, float delay)
    {
        // If a delay was set, wait for it
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        // Play the audio
        voiceSource.Stop();
        voiceSource.PlayOneShot(clip);

        // Reset delay back to 0 so future lines are instant by default
        nextDelay = 0f;
    }
}
