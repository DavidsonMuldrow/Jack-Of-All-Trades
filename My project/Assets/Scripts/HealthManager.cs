using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Added for TextMeshPro support

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public GameObject GameOverPanel;
    public PlayerStats MoveStats;
    public GameObject Player;

    [Header("Timer Settings")]
    public TMP_Text timerText;
    public float timeRemaining = 60f;
    private bool timerIsRunning = false;

    private float healthAmount = 100f;
    private bool isDead = false;

    public static HealthManager Instance;

    void Start()
    {
        if (Instance == null) Instance = this;

        if (MoveStats == null) MoveStats = GetComponent<PlayerStats>();

        healthBar.type = Image.Type.Filled;
        healthBar.fillMethod = Image.FillMethod.Horizontal;

        if (GameOverPanel != null)
            GameOverPanel.SetActive(false);
        timeRemaining = LevelModifierManager.MaxRunTime;

        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning && !isDead)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                Die();
            }
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        healthBar.fillAmount = healthAmount / 100f;

        if (healthAmount <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        timerIsRunning = false;
        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);
    }

    public void FullRestart()
    {
        LevelModifierManager.RemovedFromPool.Clear();
        LevelModifierManager.ChosenCards.Clear();

        if (LevelModifierManager.StickyObjectStates != null)
        {
            LevelModifierManager.StickyObjectStates.Clear();
        }

        if (MoveStats != null) MoveStats.ResetToDefaults();

        LevelModifierManager.MaxRunTime = 45f;

        Time.timeScale = 1f;
        SceneManager.LoadScene("JackofAllTrades");
    }

    public void BackToStart()
    {
        SceneManager.LoadScene("TitleScene");
    }
}