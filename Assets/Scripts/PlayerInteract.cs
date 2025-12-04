using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 3f;
    public float aimRadius = 0.5f;
    public LayerMask interactLayer;

    [Header("References")]
    public Transform cameraTransform;

    private GlowObjectEffect currentGlowTarget;

    void Update()
    {
        HandleInteraction();
    }

    void HandleInteraction()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        // Using SphereCast to make finding objects easier
        if (Physics.SphereCast(ray, aimRadius, out hit, interactRange, interactLayer))
        {
            // ------------------ HIGHLIGHTING LOGIC ------------------
            GlowObjectEffect glowScript = hit.collider.GetComponent<GlowObjectEffect>();

            if (glowScript != null)
            {
                if (currentGlowTarget != glowScript)
                {
                    if (currentGlowTarget != null) currentGlowTarget.ToggleHighlight(false);
                    currentGlowTarget = glowScript;
                    currentGlowTarget.ToggleHighlight(true);
                }
            }
            // ---------------------------------------------------------

            // ------------------ INTERACTION INPUT --------------------
            if (Input.GetKeyDown(KeyCode.F))
            {
                // 1. Check if it is a Pickup Item
                PickupItem itemScript = hit.collider.GetComponent<PickupItem>();
                if (itemScript != null)
                {
                    itemScript.Interact();
                    currentGlowTarget = null; // Item picked up, clear highlight
                    return; // Stop here
                }

                // 2. Check if it is the Gramophone (NEW CODE)
                GramophoneController gramophone = hit.collider.GetComponent<GramophoneController>();
                if (gramophone != null)
                {
                    gramophone.Interact();
                    // We don't clear currentGlowTarget here immediately because the object still exists,
                    // but the GramophoneController will handle disabling the glow if needed.
                }
            }
            // ---------------------------------------------------------
        }
        else
        {
            // Raycast hit nothing, turn off current highlight
            if (currentGlowTarget != null)
            {
                currentGlowTarget.ToggleHighlight(false);
                currentGlowTarget = null;
            }
        }
    }
}
