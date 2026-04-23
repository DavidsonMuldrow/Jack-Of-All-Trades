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

    [Header("Victory UI")]
    public GameObject WinPanel;

    [Header("Card Setup")]
    public List<CardData> AllPossibleCards;

    private List<CardData> _currentOptions = new List<CardData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (LevelModifierManager.ChosenCards != null)
        {

            Debug.Log("Scene Started. Applying " + LevelModifierManager.ChosenCards.Count + " cards.");

            foreach (CardData card in LevelModifierManager.ChosenCards)
            {
                ApplyCardToggles(card);
            }
        }
    }

    public void SelectCard(int index)
    {
        if (index < 0 || index >= _currentOptions.Count) return;

        CardData chosenCard = _currentOptions[index];

        if (chosenCard.Effect is StatModifierEffect statEffect)
        {
            LevelModifierManager.MaxRunTime -= statEffect.TimePenalty;
            Debug.Log($"<color=orange>TIME PENALTY:</color> {statEffect.TimePenalty}s removed. New Max: {LevelModifierManager.MaxRunTime}");
        }

        LevelModifierManager.AddCard(chosenCard);

        if (!LevelModifierManager.RemovedFromPool.Contains(chosenCard.CardName))
        {
            LevelModifierManager.RemovedFromPool.Add(chosenCard.CardName);
        }

        if (HealthManager.Instance != null)
        {
            HealthManager.Instance.timeRemaining = LevelModifierManager.MaxRunTime;
        }

        SelectionPanel.SetActive(false);
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ApplyCardToggles(CardData card)
    {
        foreach (ToggleItem item in card.itemsToToggle)
        {
            GameObject target = FindGameObjectByID(item.ObjectID);
            if (target != null)
            {
                target.SetActive(item.ShouldEnable);

                if (LevelModifierManager.StickyObjectStates.ContainsKey(item.ObjectID))
                {
                    LevelModifierManager.StickyObjectStates[item.ObjectID] = item.ShouldEnable;
                }
                else
                {
                    LevelModifierManager.StickyObjectStates.Add(item.ObjectID, item.ShouldEnable);
                }
            }
        }
    }

    public void ShowCardSelection()
    {
        List<CardData> availablePool = new List<CardData>(AllPossibleCards);

        for (int i = availablePool.Count - 1; i >= 0; i--)
        {
            if (LevelModifierManager.RemovedFromPool.Contains(availablePool[i].CardName))
            {
                availablePool.RemoveAt(i);
            }
        }

        if (availablePool.Count <= 0)
        {
            TriggerVictory();
            return;
        }

    _currentOptions.Clear();

        Time.timeScale = 0f;

        if (SelectionPanel != null)
        {
            SelectionPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("SelectionPanel is MISSING in the Inspector!");
        }

        int cardsToShow = Mathf.Min(CardSlots.Length, availablePool.Count);

        for (int i = 0; i < cardsToShow; i++)
        {
            int randomIndex = Random.Range(0, availablePool.Count);
            CardData selected = availablePool[randomIndex];
            _currentOptions.Add(selected);

            if (i < CardTexts.Length && i < CardSlots.Length)
            {
                CardTexts[i].text = $"<b>{selected.CardName}</b>\n{selected.Description}";
                CardSlots[i].SetActive(true);
                Debug.Log($"Card Slot {i} enabled with card: {selected.CardName}");
            }

            availablePool.RemoveAt(randomIndex);
        }

        for (int i = cardsToShow; i < CardSlots.Length; i++)
        {
            CardSlots[i].SetActive(false);
        }
    }

    void TriggerVictory()
    {
        Time.timeScale = 0f;
        if (SelectionPanel != null) SelectionPanel.SetActive(false);
        if (WinPanel != null) WinPanel.SetActive(true);
    }

    private GameObject FindGameObjectByID(string id)
    {
        CardTarget[] targets = Resources.FindObjectsOfTypeAll<CardTarget>();
        foreach (CardTarget t in targets)
        {
            if (t.ObjectID == id) return t.gameObject;
        }

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.name == id) return go;
        }
        return null;
    }
}