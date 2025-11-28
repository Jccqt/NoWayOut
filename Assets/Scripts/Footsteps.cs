using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class Footsteps : MonoBehaviour
{
    [Header("Assign Components")]
    public AudioSource footstepSource;

    [Header("Step Sounds")]
    public AudioClip[] stepSounds;

    [Header("Settings")]
    public float stepInterval = 0.5f;

    private float stepTimer;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // If moving...
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            HandleSteps();
        }
        else
        {
            stepTimer = 0;
        }
    }

    void HandleSteps()
    {
        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0)
        {
            PlayRandomSound();
            stepTimer = stepInterval;
        }
    }

    void PlayRandomSound()
    {
        // Safety check: Do we have sounds?
        if (stepSounds.Length > 0 && footstepSource != null)
        {
            // Pick a random number between 0 and the number of sounds you have
            int index = Random.Range(0, stepSounds.Length);

            // Randomize pitch slightly for extra realism
            footstepSource.pitch = Random.Range(0.9f, 1.1f);
            footstepSource.volume = Random.Range(0.8f, 1.0f);

            // Play the chosen sound
            footstepSource.PlayOneShot(stepSounds[index]);
        }
    }
}