using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private bool _hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_hasTriggered)
        {
            _hasTriggered = true;
            CardUIManager.Instance.ShowCardSelection();
        }
    }
}