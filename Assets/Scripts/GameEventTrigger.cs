using UnityEngine;
using UnityEngine.Events;

public class GameEventTrigger : MonoBehaviour
{
    [Header("Settings")]
    public bool triggerOnce = true;
    private bool hasTriggered = false;

    [Header("Actions")]
    // This creates a list in the Inspector where you can drag ANYTHING
    public UnityEvent onEnterTrigger;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerOnce && hasTriggered) return;

            // Execute everything in the list
            onEnterTrigger.Invoke();

            hasTriggered = true;

            // Optional debug to help you see it works
            // print("Event Triggered: " + gameObject.name);
        }
    }
}
