using UnityEngine;

public class PickupFlashlight : MonoBehaviour
{

    [Header("Settings")]
    public float pickupRange = 3f;

    [Header("References")]
    public GameObject flashlight; // The one in your hand (initially disabled)

    // Variable to remember what we are currently looking at
    private GlowObjectEffect currentTarget;

    void Update()
    {
        HandleLooking();
        HandleInput();
    }

    void HandleLooking()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // 1. Check if we hit something within range
        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            // 2. Is it the flashlight?
            if (hit.collider.CompareTag("FlashlightPickup"))
            {
                // Get the glow script from the object we hit
                GlowObjectEffect glowScript = hit.collider.GetComponent<GlowObjectEffect>();

                if (glowScript != null)
                {
                    // If we weren't looking at this before, highlight it!
                    if (currentTarget != glowScript)
                    {
                        // If we were looking at a DIFFERENT object, turn that one off first
                        if (currentTarget != null) currentTarget.ToggleHighlight(false);

                        currentTarget = glowScript;
                        currentTarget.ToggleHighlight(true);
                    }
                    return; // Stop here, we found it.
                }
            }
        }

        // 3. If the ray hit NOTHING, or hit a wall (not the flashlight)
        if (currentTarget != null)
        {
            // Turn off the glow on the last thing we looked at
            currentTarget.ToggleHighlight(false);
            currentTarget = null;
        }
    }

    void HandleInput()
    {
        // We only allow pickup if we have a valid target (currentTarget is not null)
        if (Input.GetMouseButtonDown(0) && currentTarget != null)
        {
            PickupObject();
        }
    }

    void PickupObject()
    {
        // Activate the hand flashlight
        flashlight.SetActive(true);

        // Destroy the floor flashlight (which is currentTarget.gameObject)
        Destroy(currentTarget.gameObject);

        // Clear the target variable so we don't try to access a destroyed object
        currentTarget = null;

        Debug.Log("Flashlight Picked Up!");
    }
}
