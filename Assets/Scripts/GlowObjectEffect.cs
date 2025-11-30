using UnityEngine;

public class GlowObjectEffect : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject uiPromptPrefab; // Drag your 'InteractionPrompt' prefab here
    public float promptHeight = 0.5f; // How high above the object the 'F' floats

    private GameObject currentPrompt; // The actual instance of the UI
    private bool isHovered = false;

    void Start()
    {
        // Instantiate the UI but hide it immediately
        if (uiPromptPrefab != null)
        {
            currentPrompt = Instantiate(uiPromptPrefab, transform.position + Vector3.up * promptHeight, Quaternion.identity);
            currentPrompt.transform.SetParent(this.transform); // Make it follow the object
            currentPrompt.SetActive(false);
        }
    }

    public void ToggleHighlight(bool state)
    {
        isHovered = state;

        if (currentPrompt != null)
        {
            currentPrompt.SetActive(state);
        }
    }

    // Ensure UI is destroyed if object is picked up
    void OnDestroy()
    {
        if (currentPrompt != null) Destroy(currentPrompt);
    }
}
