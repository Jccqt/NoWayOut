using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Settings")]
    public bool enableBob = true;

    [Header("Bobbing Intensity")]
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    public float sideAmount = 0.05f; 

    [Header("Smoothness")]
    public float smooth = 10f;

    private float defaultPosY = 0;
    private float defaultPosX = 0;
    private float timer = 0;

    void Start()
    {
        defaultPosY = transform.localPosition.y;
        defaultPosX = transform.localPosition.x;
    }

    void Update()
    {
        if (!enableBob) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 targetPosition = new Vector3(defaultPosX, defaultPosY, transform.localPosition.z);

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            timer += Time.deltaTime * walkingBobbingSpeed;

            float newY = defaultPosY + Mathf.Sin(timer) * bobbingAmount;

            float newX = defaultPosX + Mathf.Cos(timer / 2) * sideAmount;

            targetPosition = new Vector3(newX, newY, transform.localPosition.z);
        }
        else
        {
            timer = 0;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * smooth);
    }
}
