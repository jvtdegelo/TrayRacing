using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject audioParent;
    private bool isPaused = false;
    private AudioSource[] allAudioSources;

    void Start()
    {
        Debug.Log("Game Start");
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        if (audioParent != null)
            {
                allAudioSources = audioParent.GetComponentsInChildren<AudioSource>();
            }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
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
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        foreach (AudioSource audio in allAudioSources)
        {
            audio.UnPause();
        }
        isPaused = false;
        Debug.Log("Game Resumed");

    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Pause();
        }
        isPaused = true;
        Debug.Log("Game Paused");

    }

    public void MainMenu () {
        Time.timeScale = 1f;
        SceneManager.LoadScene (0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
