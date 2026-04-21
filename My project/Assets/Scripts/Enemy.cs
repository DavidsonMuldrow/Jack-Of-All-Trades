using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Image HealthBar;
    
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("hit");
            HealthBar.GetComponent<HealthManager>().TakeDamage(20f);
        }
    }
}
