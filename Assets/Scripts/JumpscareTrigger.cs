using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JumpscareTrigger : MonoBehaviour
{
    [Header("Monster Settings")]
    [Tooltip("The actual monster object (DeathRig)")]
    public GameObject scareObject;
    public Animator scareAnimator;
    public string animationTriggerName = "PlayScare";

    [Header("Audio Settings")]
    [Tooltip("The AudioSource on the Monster")]
    public AudioSource scareAudioSource;
    public AudioClip screamSound;

    [Header("Ending Settings")]
    [Tooltip("The black panel in your Canvas for fading out")]
    public Image fadePanel;
    [Tooltip("How long to wait after scare before fading starts")]
    public float waitBeforeFade = 2.0f;
    [Tooltip("How fast the screen turns black")]
    public float fadeSpeed = 1.0f;

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            hasPlayed = true;
            StartCoroutine(PlayJumpscareSequence());
        }
    }

    IEnumerator PlayJumpscareSequence()
    {
        // 1. Reveal Monster
        if (scareObject != null) scareObject.SetActive(true);

        // 2. Play Animation
        if (scareAnimator != null) scareAnimator.SetTrigger(animationTriggerName);

        // 3. Play Sound (from the monster's AudioSource)
        if (scareAudioSource != null && screamSound != null)
        {
            scareAudioSource.PlayOneShot(screamSound);
        }

        // 4. Wait while the player gets scared
        yield return new WaitForSeconds(waitBeforeFade);

        // 5. Fade Out
        if (fadePanel != null)
        {
            float alpha = 0;
            while (alpha < 1)
            {
                alpha += Time.deltaTime * fadeSpeed;
                Color c = fadePanel.color;
                c.a = alpha;
                fadePanel.color = c;
                yield return null;
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 6. Load Credits
        SceneManager.LoadScene("Credits");
    }
}
