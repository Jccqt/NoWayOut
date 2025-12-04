using UnityEngine;
using UnityEngine.Events;

public class GramophoneController : MonoBehaviour
{
    [Header("Requirements")]
    public ItemData requiredItem;

    [Header("Success Settings (Has Vinyl)")]
    [Tooltip("If true, the Success Event only happens once. Afterward, the object becomes non-interactive.")]
    public bool triggerSuccessOnce = true;
    public UnityEvent onVinylPlaced;

    [Header("Missing Settings (No Vinyl)")]
    [Tooltip("If true, the 'Missing Item' event only happens once. Subsequent clicks without the item will do nothing.")]
    public bool triggerMissingOnce = false;
    public UnityEvent onMissingItem;

    // Private flags to track state
    private bool successTriggered = false;
    private bool missingTriggered = false;

    private GlowObjectEffect glowEffect;

    private void Start()
    {
        glowEffect = GetComponent<GlowObjectEffect>();
    }

    public void Interact()
    {
        // Check if we have the vinyl in inventory
        bool hasItem = false;
        if (InventoryManager.Instance != null)
        {
            hasItem = InventoryManager.Instance.HasItem(requiredItem);
        }

        if (hasItem)
        {
            HandleSuccess();
        }
        else
        {
            HandleMissing();
        }
    }

    private void HandleSuccess()
    {
        // If we only want this to run once, and it already ran, stop.
        if (triggerSuccessOnce && successTriggered) return;

        // Trigger the event (Play music, animation, etc.)
        onVinylPlaced.Invoke();

        // Mark as done
        if (triggerSuccessOnce)
        {
            successTriggered = true;

            // If success is done, we usually want to stop interacting entirely (remove the glow)
            DisableInteraction();
        }

        // OPTIONAL: Remove item from inventory
        // InventoryManager.Instance.RemoveItem(requiredItem);
    }

    private void HandleMissing()
    {
        // If we only want the "I need a key" line to play once, and it already ran, stop.
        if (triggerMissingOnce && missingTriggered) return;

        // Trigger the missing event (Dialogue, text, sound)
        onMissingItem.Invoke();

        // Mark as done
        if (triggerMissingOnce)
        {
            missingTriggered = true;
        }
    }

    private void DisableInteraction()
    {
        if (glowEffect != null)
        {
            glowEffect.ToggleHighlight(false);
            Destroy(glowEffect); // Removes the prompt UI so player can't click F anymore
        }
    }
}
