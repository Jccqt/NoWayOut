using UnityEngine;
using UnityEngine.AI;

public class WeepingAngel : MonoBehaviour
{
    [Header("Setup")]
    public Transform player;
    public Camera playerCamera;
    public float movementSpeed = 3.5f;

    [Header("Settings")]
    public float startDelay = 3.0f; // The 3-second safety window at start
    public float lookAwayDelay = 3.0f; // --- NEW: 3 seconds wait after looking away

    [Header("Audio (Optional)")]
    public AudioSource movementSound;

    private NavMeshAgent agent;
    private Animator animator;
    private Renderer myRenderer;
    private float lookAwayTimer = 0f; // --- NEW: Internal timer

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        // Ensure we get the renderer even if it's on a child object
        myRenderer = GetComponentInChildren<Renderer>();

        agent.speed = movementSpeed;

        // Force freeze immediately on start
        Freeze();
    }

    void Update()
    {
        if (player == null) return;

        // 1. CHECK THE GLOBAL START TIMER
        if (Time.timeSinceLevelLoad < startDelay)
        {
            return;
        }

        // 2. CHECK VISIBILITY
        if (IsVisibleToPlayer())
        {
            // IF SEEN:
            Freeze();
            lookAwayTimer = 0f; // --- NEW: Reset timer immediately when seen
        }
        else
        {
            // IF NOT SEEN:
            // --- NEW: Count up
            lookAwayTimer += Time.deltaTime;

            // --- NEW: Only move if timer is higher than 3 seconds
            if (lookAwayTimer >= lookAwayDelay)
            {
                MoveTowardsPlayer();
            }
            else
            {
                Freeze();
            }
        }
    }

    // THIS IS YOUR ORIGINAL FUNCTION (UNCHANGED)
    bool IsVisibleToPlayer()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        if (GeometryUtility.TestPlanesAABB(planes, myRenderer.bounds))
        {
            RaycastHit hit;
            Vector3 direction = myRenderer.bounds.center - playerCamera.transform.position;

            // Check for walls
            if (Physics.Raycast(playerCamera.transform.position, direction, out hit))
            {
                if (hit.collider.CompareTag("DoorFrame"))
                {
                    return false;
                }

                // If we hit the mannequin (or something that isn't a wall)
                if (hit.transform.root == transform || !hit.collider.CompareTag("Wall"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void Freeze()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        animator.speed = 0;

        if (movementSound != null && movementSound.isPlaying)
            movementSound.Stop();
    }

    void MoveTowardsPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.speed = 1;

        if (movementSound != null && !movementSound.isPlaying)
            movementSound.Play();
    }
}
