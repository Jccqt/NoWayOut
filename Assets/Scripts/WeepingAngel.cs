using UnityEngine;
using UnityEngine.AI;

public class WeepingAngel : MonoBehaviour
{
    [Header("Setup")]
    public Transform player;
    public Camera playerCamera;
    public float movementSpeed = 3.5f;

    [Header("Settings")]
    public float startDelay = 3.0f; // The 3-second safety window

    [Header("Audio (Optional)")]
    public AudioSource movementSound;

    private NavMeshAgent agent;
    private Animator animator;
    private Renderer myRenderer;
    private bool isActive = false; // Tracks if the 3 seconds are over

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        // Ensure we get the renderer even if it's on a child object (like Mixamo models often are)
        myRenderer = GetComponentInChildren<Renderer>();

        agent.speed = movementSpeed;

        // Force freeze immediately on start
        Freeze();
    }

    void Update()
    {
        if (player == null) return;

        // 1. CHECK THE 3-SECOND TIMER
        // If the game has been running for less than 3 seconds, do nothing.
        if (Time.timeSinceLevelLoad < startDelay)
        {
            return;
        }

        // 2. NORMAL BEHAVIOR
        if (IsVisibleToPlayer())
        {
            Freeze();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

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
