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

        if (Physics.SphereCast(ray, aimRadius, out hit, interactRange, interactLayer))
        {
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

            if (Input.GetKeyDown(KeyCode.F))
            {
                PickupItem itemScript = hit.collider.GetComponent<PickupItem>();

                if (itemScript != null)
                {
                    itemScript.Interact();
                    currentGlowTarget = null;
                }
            }
        }
        else
        {
            if (currentGlowTarget != null)
            {
                currentGlowTarget.ToggleHighlight(false);
                currentGlowTarget = null;
            }
        }
    }
}
