using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Visual Settings")]
    public float hoverScale = 1.1f; // How big it gets (1.1 = 110%)
    public float transitionSpeed = 10f; // How fast it grows

    [Header("Audio Settings")]
    public AudioSource audioSource; // Drag your audio source here
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Start()
    {
        // Remember the starting size
        originalScale = transform.localScale;
        targetScale = originalScale;

        // If we forgot to assign an AudioSource, try to find one
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Smoothly animate the size change
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * transitionSpeed);
    }

    // Triggered when mouse enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale; // Set target to bigger size
        PlaySound(hoverSound);
    }

    // Triggered when mouse leaves the button
    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale; // Set target back to normal
    }

    // Triggered when clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(clickSound);
        // Note: The actual button logic (Start/Exit) is still handled by the Button component
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
