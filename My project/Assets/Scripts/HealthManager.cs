using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public GameObject GameOverPanel;
    public PlayerStats MoveStats;
    public GameObject Player;

    private float healthAmount = 100f;
    private bool isDead = false;

    void Start()
    {
        if (MoveStats == null) MoveStats = GetComponent<PlayerStats>();

        healthBar.type = Image.Type.Filled;
        healthBar.fillMethod = Image.FillMethod.Horizontal;

        if (GameOverPanel != null)
            GameOverPanel.SetActive(false);
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
        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);
    }

    public void FullRestart()
    {
        LevelModifierManager.RemovedFromPool.Clear();

        LevelModifierManager.ChosenCards.Clear();

        if (MoveStats != null)
        {
            MoveStats.ResetToDefaults();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("JackofAllTrades");
    }

    public void Heal(float healingAmount)
    {
        if (isDead) return;
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        healthBar.fillAmount = healthAmount / 100f;
    }
}
