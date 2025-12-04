using UnityEngine;

public class ForwardMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2.0f;
    public float destroyAfterSeconds = 5.0f;

    void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
    }
}
