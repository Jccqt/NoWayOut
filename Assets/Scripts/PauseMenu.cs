using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("UI References")]
    public GameObject pauseMenuUI;

    [Header("Music Settings")]
    public AudioSource musicSource;
    public AudioClip pauseMusicLoop;

    void Start()
    {
        // 1. Force the menu to be hidden on start
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        // 2. Lock and hide cursor on start since we are in gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 3. Setup the music source
        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            // IMPORTANT: This allows the pause menu music to play even when AudioListener.pause is true
            musicSource.ignoreListenerPause = true;

            if (musicSource.clip == null) musicSource.clip = pauseMusicLoop;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Unpause all gameplay sounds
        AudioListener.pause = false;

        if (musicSource != null)
        {
            musicSource.Stop();
        }

        // Lock and hide the cursor when returning to gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Pause all gameplay sounds (voices, effects, background music)
        AudioListener.pause = true;

        if (musicSource != null && pauseMusicLoop != null)
        {
            musicSource.clip = pauseMusicLoop;
            musicSource.Play();
        }

        // Unlock and show cursor so player can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false; // Reset audio before changing scenes
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
