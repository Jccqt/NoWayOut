using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SojaExiles

{
    public class opencloseDoor1 : MonoBehaviour
    {
        public Animator openandclose;
        public bool open;
        public Transform Player;
        public bool isLocked = false;

        [Header("Key Settings")]
        public ItemData requiredKey;
        [Tooltip("If true, the key will be removed from inventory after unlocking.")]
        public AudioClip unlockSound;

        [Header("Audio Settings")]
        public AudioSource doorSource;
        public AudioClip openSound;
        public AudioClip closeSound;
        public AudioClip lockedSound;

        [Header("Event Settings")]
        public bool triggerLockedOnce = true;
        private bool hasTriggeredLocked = false;

        public bool triggerOpenOnce = true;
        private bool hasTriggeredOpen = false;

        public bool triggerCloseOnce = true;
        private bool hasTriggeredClose = false;

        [Header("Auto Close Settings")]
        public bool autoClose = false;
        public float autoCloseDelay = 5.0f;
        private Coroutine autoCloseRoutine;

        [Header("Interaction Events")]
        public UnityEvent OnOpenEvent;
        public UnityEvent OnCloseEvent;
        public UnityEvent OnLockedEvent;
        public UnityEvent OnUnlockEvent; // New Event for when you unlock it

        void Start()
        {
            open = false;
        }

        void OnMouseOver()
        {
            if (Player)
            {
                float dist = Vector3.Distance(Player.position, transform.position);
                if (dist < 3)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (open == false)
                        {
                            if (isLocked)
                            {
                                // --- NEW LOGIC STARTS HERE ---
                                // 1. Check if the door requires a key and if the player has it
                                if (requiredKey != null && InventoryManager.Instance.HasItem(requiredKey))
                                {
                                    UnlockDoor();
                                }
                                // 2. If no key is needed, or player doesn't have it, do the standard locked behavior
                                else
                                {
                                    HandleLockedBehavior();
                                }
                                // --- NEW LOGIC ENDS HERE ---
                            }
                            else
                            {
                                StartCoroutine(opening());
                            }
                        }
                        else
                        {
                            StartCoroutine(closing());
                        }
                    }
                }
            }
        }

        void UnlockDoor()
        {
            isLocked = false;
            print("Door Unlocked!");

            // Play unlock sound
            if (doorSource != null && unlockSound != null)
            {
                doorSource.PlayOneShot(unlockSound);
            }

            OnUnlockEvent.Invoke();

            // Automatically open the door immediately after unlocking
            StartCoroutine(opening());
        }

        void HandleLockedBehavior()
        {
            PlayLockedSound();

            if (!triggerLockedOnce || !hasTriggeredLocked)
            {
                OnLockedEvent.Invoke();
                hasTriggeredLocked = true;
            }
        }

        void PlayLockedSound()
        {
            if (doorSource != null && lockedSound != null)
            {
                doorSource.PlayOneShot(lockedSound);
            }
        }

        // ... (Keep existing AutoCloseTimer, opening, closing, ForceOpen, etc.) ...

        IEnumerator AutoCloseTimer()
        {
            yield return new WaitForSeconds(autoCloseDelay);
            if (open) StartCoroutine(closing());
        }

        IEnumerator opening()
        {
            print("you are opening the door");
            openandclose.Play("OpeningReverse");

            if (doorSource != null && openSound != null) doorSource.PlayOneShot(openSound);

            if (!triggerOpenOnce || !hasTriggeredOpen)
            {
                OnOpenEvent.Invoke();
                hasTriggeredOpen = true;
            }

            open = true;

            if (autoClose)
            {
                if (autoCloseRoutine != null) StopCoroutine(autoCloseRoutine);
                autoCloseRoutine = StartCoroutine(AutoCloseTimer());
            }

            yield return new WaitForSeconds(.5f);
        }

        IEnumerator closing()
        {
            print("you are closing the door");
            openandclose.Play("ClosingReverse");

            if (autoCloseRoutine != null)
            {
                StopCoroutine(autoCloseRoutine);
                autoCloseRoutine = null;
            }

            if (doorSource != null && closeSound != null) doorSource.PlayOneShot(closeSound);

            if (!triggerCloseOnce || !hasTriggeredClose)
            {
                OnCloseEvent.Invoke();
                hasTriggeredClose = true;
            }

            open = false;
            yield return new WaitForSeconds(.5f);
        }

        // Keep your Force functions
        public void ForceOpen() { if (!open) StartCoroutine(opening()); }
        public void ForceClose() { if (open) StartCoroutine(closing()); }
        public void ForceLock() { isLocked = true; }
        public void ForceUnlock() { isLocked = false; }
        public void ForceRemoveAutoClose() { autoClose = false; }
    }
}