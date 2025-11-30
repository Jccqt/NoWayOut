using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public bool open;
        public Transform Player;
        public bool isLocked = false;

        [Header("Audio Settings")]
        public AudioSource doorSource;
        public AudioClip openSound;
        public AudioClip closeSound;
        public AudioClip lockedSound;

        [Header("Event Settings")]
        [Tooltip("If true, the Locked Event will only happen the first time you click.")]
        public bool triggerLockedOnce = true;
        private bool hasTriggeredLocked = false;

        [Tooltip("If true, the Open Event will only happen the first time.")]
        public bool triggerOpenOnce = true;
        private bool hasTriggeredOpen = false;

        // --- NEW: Close Once Settings ---
        [Tooltip("If true, the Close Event will only happen the first time.")]
        public bool triggerCloseOnce = true;
        private bool hasTriggeredClose = false;
        // --------------------------------

        [Header("Interaction Events")]
        public UnityEvent OnOpenEvent;
        public UnityEvent OnCloseEvent;
        public UnityEvent OnLockedEvent;

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
                                PlayLockedSound();

                                if (!triggerLockedOnce || !hasTriggeredLocked)
                                {
                                    OnLockedEvent.Invoke();
                                    hasTriggeredLocked = true;
                                }
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

        void PlayLockedSound()
        {
            if (doorSource != null && lockedSound != null)
            {
                doorSource.PlayOneShot(lockedSound);
            }
        }

        IEnumerator opening()
        {
            print("you are opening the door");
            openandclose.Play("Opening");

            if (doorSource != null && openSound != null)
            {
                doorSource.PlayOneShot(openSound);
            }

            if (!triggerOpenOnce || !hasTriggeredOpen)
            {
                OnOpenEvent.Invoke();
                hasTriggeredOpen = true;
            }

            open = true;
            yield return new WaitForSeconds(.5f);
        }

        IEnumerator closing()
        {
            print("you are closing the door");
            openandclose.Play("Closing");

            if (doorSource != null && closeSound != null)
            {
                doorSource.PlayOneShot(closeSound);
            }

            // --- NEW: Check Close Event Logic ---
            if (!triggerCloseOnce || !hasTriggeredClose)
            {
                OnCloseEvent.Invoke();
                hasTriggeredClose = true;
            }
            // ------------------------------------

            open = false;
            yield return new WaitForSeconds(.5f);
        }

        public void ForceOpen()
        {
            if (!open)
            {
                StartCoroutine(opening());
            }
        }
        public void ForceClose()
        {
            if (open)
            {
                StartCoroutine(closing());
            }
        }
        public void ForceLock() { isLocked = true; }
    }
}