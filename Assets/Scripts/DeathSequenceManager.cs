using System.Collections;
using UnityEngine;

public class DeathSequenceManager : MonoBehaviour
{
    [Header("Monster Setup")]
    public GameObject monster;          // The actual monster object
    public Animator monsterAnimator;    // The monster's animator
    public string biteTriggerName = "Kill"; // The name of your parameter in Animator

    [Header("Player Setup")]
    public Transform playerCamera;      // Your Main Camera
    public MonoBehaviour playerMovement; // Your Player Controller Script (to disable walking)
    public MonoBehaviour playerLook;     // Your Mouse Look Script (to disable looking)

    [Header("Effects")]
    public AudioSource screamAudio;     // The AudioSource with the scream
    public CanvasGroup blackScreenParams; // The Panel with the Canvas Group you made
    public float monsterDistance = 0.8f; // How close she spawns to your face

    // This is the function we call from your GameEventTrigger
    public void StartDeath()
    {
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        // 1. FREEZE PLAYER
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerLook != null) playerLook.enabled = false;

        // 2. POSITION MONSTER
        // Move monster directly in front of the camera
        monster.SetActive(true);
        Vector3 spawnPos = playerCamera.position + (playerCamera.forward * monsterDistance);
        spawnPos.y = monster.transform.position.y; // Keep her feet on the floor
        monster.transform.position = spawnPos;

        // Make monster look at player
        monster.transform.LookAt(new Vector3(playerCamera.position.x, monster.transform.position.y, playerCamera.position.z));

        // 3. FORCE CAMERA LOOK
        // Make the camera look at the monster's head (approx height)
        playerCamera.LookAt(monster.transform.position + Vector3.up * 1.5f);

        // 4. PLAY ANIMATION & SOUND
        monsterAnimator.SetTrigger(biteTriggerName);
        screamAudio.Play();

        // 5. BLINKING EFFECT
        // Blink 1 (Close)
        yield return StartCoroutine(FadeEyes(1, 0.1f)); // Fast close
        yield return new WaitForSeconds(0.1f);
        // Blink 1 (Open)
        yield return StartCoroutine(FadeEyes(0, 0.1f)); // Fast open

        yield return new WaitForSeconds(0.3f); // Wait a tiny bit

        // Blink 2 (Close)
        yield return StartCoroutine(FadeEyes(1, 0.1f));
        yield return new WaitForSeconds(0.1f);
        // Blink 2 (Open)
        yield return StartCoroutine(FadeEyes(0, 0.1f));

        yield return new WaitForSeconds(0.5f); // Wait for the bite impact

        // 6. FINAL DEATH (Close Eyes Permanently)
        yield return StartCoroutine(FadeEyes(1, 0.2f));

        Debug.Log("Player is Dead.");
        // Here you would load the "Game Over" scene
    }

    IEnumerator FadeEyes(float targetAlpha, float duration)
    {
        float startAlpha = blackScreenParams.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            blackScreenParams.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }
        blackScreenParams.alpha = targetAlpha;
    }
}
