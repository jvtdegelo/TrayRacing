using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public void PlayGame () {
        SceneManager.LoadScene (2);
    }

    public void PlayOptions () {
        SceneManager.LoadScene (1);
    }

    public void MainMenu () {
        SceneManager.LoadScene (0);
    }
}