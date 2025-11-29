using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvent : MonoBehaviour
{
    [Header("Settings")]
    public float delaySeconds = 2.0f; // How long to wait?

    [Header("Action")]
    public UnityEvent action;

    // Call this function to start the timer
    public void StartDelay()
    {
        StartCoroutine(RunDelay());
    }

    IEnumerator RunDelay()
    {
        yield return new WaitForSeconds(delaySeconds);
        action.Invoke();
    }
}
