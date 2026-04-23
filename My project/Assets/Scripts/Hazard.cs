using UnityEngine;
using UnityEngine.UI;

public class Hazard : MonoBehaviour
{
    public Image HealthBar;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("hit");
            HealthBar.GetComponent<HealthManager>().TakeDamage(100f);
        }
    }
}