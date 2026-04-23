using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject canvas2;

    public void Start()
    {
        canvas.SetActive(false);
        canvas2.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("JackOfAllTrades");
    }

    public void ShowHowToPlay()
    {
        canvas.SetActive(true);
        canvas2.SetActive(false);
    }

    public void HideHowToPlay()
    {
        canvas.SetActive(false);
        canvas2.SetActive(true);
    }
}
