using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Exit Triggered by Player!");

            LevelManager manager = Object.FindFirstObjectByType<LevelManager>();

            if (manager != null)
            {
                manager.ShowMenu();
            }
            else
            {
                Debug.LogError("TriggerObject: Could not find a LevelManager in the scene!");
            }
        }
    }*/
}
