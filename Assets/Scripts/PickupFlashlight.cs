using UnityEngine;

public class PickupFlashlight : MonoBehaviour
{

    [Header("Settings")]
    public float pickupRange = 3f;

    [Header("References")]
    public GameObject flashlight;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPickup();
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Shoot the laser!
        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            // Did we hit the flashlight?
            if (hit.collider.CompareTag("FlashlightPickup"))
            {
                flashlight.SetActive(true);

                Destroy(hit.collider.gameObject);

                // Debug log to confirm it worked
                Debug.Log("Flashlight Picked Up!");
            }
        }
    }
}
