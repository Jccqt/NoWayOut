using UnityEngine;

public class PickupFlashlight : MonoBehaviour
{
    [Header("Settings")]
    public float pickupRange = 3f;
    public float aimRadius = 0.5f; // How "thick" the aim is (0.5 is generous)

    [Header("References")]
    public GameObject handFlashlight;

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

        // use SphereCast instead of Raycast
        // Think of this as firing a tennis ball instead of a laser pointer
        if (Physics.SphereCast(ray, aimRadius, out hit, pickupRange))
        {
            // Check specifically for the Interactable script, not just the tag
            GlowObjectEffect item = hit.collider.GetComponent<GlowObjectEffect>();

            if (item != null)
            {
                if (currentTarget != item)
                {
                    if (currentTarget != null) currentTarget.ToggleHighlight(false);
                    currentTarget = item;
                    currentTarget.ToggleHighlight(true);
                }
                return;
            }
        }

        // If we hit nothing interactable
        if (currentTarget != null)
        {
            currentTarget.ToggleHighlight(false);
            currentTarget = null;
        }
    }

    void HandleInput()
    {
        // Press F to interact (Standard RE controls)
        if (Input.GetKeyDown(KeyCode.F) && currentTarget != null)
        {
            // Check if it's actually the flashlight by tag or name
            if (currentTarget.CompareTag("FlashlightPickup"))
            {
                handFlashlight.SetActive(true);
                Destroy(currentTarget.gameObject);
                currentTarget = null; // Important: Clear the target immediately
            }
        }
    }
}
