using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CardUIManager : MonoBehaviour
{
    public static CardUIManager Instance;

    [Header("UI References")]
    public GameObject SelectionPanel;
    public GameObject[] CardSlots;
    public TMP_Text[] CardTexts;

    [Header("Card Setup")]
    public List<CardData> AllPossibleCards;

    private List<CardData> _currentOptions = new List<CardData>();

    private void Awake() => Instance = this;

    public void ShowCardSelection()
    {
        Time.timeScale = 0f;
        SelectionPanel.SetActive(true);
        _currentOptions.Clear();

        List<CardData> availablePool = new List<CardData>(AllPossibleCards);

        foreach (CardData removedCard in LevelModifierManager.RemovedFromPool)
        {
            if (availablePool.Contains(removedCard))
            {
                availablePool.Remove(removedCard);
            }
        }

        if (availablePool.Count < 3)
        {
            Debug.LogWarning("Not enough cards left in pool! Showing what's left.");
        }

        int cardsToShow = Mathf.Min(3, availablePool.Count);
        for (int i = 0; i < cardsToShow; i++)
        {
            int randomIndex = Random.Range(0, availablePool.Count);
            CardData selected = availablePool[randomIndex];

            _currentOptions.Add(selected);

            if (i < CardTexts.Length)
            {
                CardTexts[i].text = $"<b>{selected.CardName}</b>\n{selected.Description}";
                CardTexts[i].transform.parent.gameObject.SetActive(true);
            }

            availablePool.RemoveAt(randomIndex);
        }

        for (int i = cardsToShow; i < CardTexts.Length; i++)
        {
            CardTexts[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public void SelectCard(int index)
    {
        CardData chosenCard = _currentOptions[index];

        LevelModifierManager.AddCard(chosenCard);

        Debug.Log("Selection confirmed. Loading next level...");

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}