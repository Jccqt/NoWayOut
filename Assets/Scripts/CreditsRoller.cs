using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsRoller : MonoBehaviour
{
    [Header("Settings")]
    public float scrollSpeed = 50f;

    [Tooltip("If true, player can press any key to skip credits")]
    public bool enableSkip = true;

    [Header("Audio")]
    [Tooltip("Assign an AudioSource with your credits music here")]
    public AudioSource musicSource;

    [Header("References")]
    public RectTransform creditsTextTransform;

    // Internal variables
    private float textHeight;
    private float canvasHeight;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Force the layout to rebuild immediately to get accurate height
        // This prevents issues where height is 0 on the first frame
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(creditsTextTransform);

        // 1. Calculate dimensions
        textHeight = creditsTextTransform.rect.height;

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        }
        else
        {
            canvasHeight = Screen.height;
        }

        // 2. Reset position
        // We need to move the text down by (Half Screen Height) + (Half Text Height)
        // This ensures the TOP of the text is just below the screen
        float startY = -(canvasHeight / 2) - (textHeight / 2) - 100f;
        creditsTextTransform.anchoredPosition = new Vector2(0, startY);

        // 3. Play Music
        if (musicSource != null)
        {
            musicSource.loop = false; // Credits usually play once, but set true if desired
            musicSource.Play();
        }
    }

    void Update()
    {
        // 1. Scroll Upwards
        creditsTextTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.unscaledDeltaTime);

        // 2. Check for Completion
        // We check if the BOTTOM of the text has passed the TOP of the screen
        // Position Y is the center. So Center - HalfHeight = Bottom. 
        // We want Bottom > Top of Screen (canvasHeight/2)
        if ((creditsTextTransform.anchoredPosition.y - textHeight / 2) > (canvasHeight / 2))
        {
            ReturnToMenu();
        }

        // 3. Skip functionality
        if (enableSkip && Input.anyKeyDown)
        {
            ReturnToMenu();
        }
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
