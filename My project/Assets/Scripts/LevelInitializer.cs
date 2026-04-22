using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    public PlayerStats MoveStats;
    public GameObject Player;

    void Start()
    {
        Debug.Log("LevelInitializer checking for cards...");

        MoveStats.ResetToDefaults();

        if (LevelModifierManager.ChosenCards.Count == 0)
        {
            Debug.LogWarning("The ChosenCards list is EMPTY!");
        }

        foreach (CardData card in LevelModifierManager.ChosenCards)
        {
            Debug.Log("Found a card to apply: " + card.CardName);
            if (card.Effect != null)
            {
                card.Effect.Apply(MoveStats, Player);
            }
        }
    }
}