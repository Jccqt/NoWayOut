using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    public bool reverseFace = true;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;

        // 2. Apply the rotation
        if (reverseFace)
        {
            // Look AWAY from camera (Fixes backward text)
            transform.rotation = Quaternion.LookRotation(-directionToCamera);
        }
        else
        {
            // Look AT camera
            transform.rotation = Quaternion.LookRotation(directionToCamera);
        }
    }
}
