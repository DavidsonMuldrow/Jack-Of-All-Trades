using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject pauseMenu;

    public void ShowMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // freezes the game world
        Cursor.lockState = CursorLockMode.None; // unlocks mouse for ui
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
